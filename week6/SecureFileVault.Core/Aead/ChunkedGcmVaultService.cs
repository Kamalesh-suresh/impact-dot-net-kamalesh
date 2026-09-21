using System.Buffers.Binary;
using System.Security.Cryptography;
using SecureFileVault.Core.Kdf;

namespace SecureFileVault.Core.Aead;

// Thrown when a vault file fails authentication — either the password is
// wrong or the ciphertext was tampered with. Wraps the underlying
// CryptographicException with the chunk index so the failure is easy to
// place (Task 6.9's "flip one byte, confirm it's rejected", applied to a
// whole file instead of one buffer).
public sealed class VaultIntegrityException : CryptographicException
{
    public VaultIntegrityException(int chunkIndex, Exception inner)
        : base($"Vault integrity check failed at chunk {chunkIndex}: wrong password or tampered file.", inner)
    {
    }
}

// The actual "SecureFileVault" deliverable: password -> PBKDF2 -> AES-GCM,
// streamed over a file so a 100 MB+ input never sits fully in memory, with
// every chunk independently authenticated so tampering anywhere is rejected.
//
// Why chunked instead of one AesGcm.Encrypt call over the whole file: .NET's
// AesGcm does not implement ICryptoTransform, so it cannot be threaded
// through CryptoStream at all. That's not a missing feature — AEAD's
// security guarantee depends on verifying the ENTIRE tag before releasing
// ANY plaintext, so a true single-tag "streaming GCM" would either have to
// buffer the whole file (defeating the point) or release unauthenticated
// plaintext incrementally (defeating GCM's whole purpose). The standard fix,
// used by TLS, age, and libsodium's secretstream, is what this class does:
// split the file into fixed-size chunks and give each one its own nonce and
// tag, so encryption/decryption only ever holds one chunk in memory while
// still authenticating every byte of the file.
//
// File format: "SFV1" (4) || salt (16) || noncePrefix (8) ||
//   repeated: chunkLength (4, big-endian) || tag (16) || ciphertext (chunkLength)
// Per-chunk nonce = noncePrefix (8 bytes, random per file) || chunkIndex (4 bytes, big-endian).
// The prefix+counter construction guarantees every chunk in the file uses a
// distinct nonce under the same key, which AES-GCM requires for its
// security proof to hold.
public static class ChunkedGcmVaultService
{
    public const int DefaultChunkSizeBytes = 64 * 1024;

    private static readonly byte[] Magic = "SFV1"u8.ToArray();
    private const int NoncePrefixBytes = 8;
    private const int NonceCounterBytes = 4;
    private const int NonceSizeBytes = NoncePrefixBytes + NonceCounterBytes;
    private const int TagSizeBytes = 16;

    public static void EncryptFile(string sourcePath, string destinationPath, string password, int chunkSize = DefaultChunkSizeBytes)
    {
        var salt = Pbkdf2KeyDerivation.GenerateSalt();
        var key = Pbkdf2KeyDerivation.DeriveKey(password, salt);
        var noncePrefix = RandomNumberGenerator.GetBytes(NoncePrefixBytes);

        using var source = new FileStream(sourcePath, FileMode.Open, FileAccess.Read);
        using var destination = new FileStream(destinationPath, FileMode.Create, FileAccess.Write);
        using var aesGcm = new AesGcm(key, TagSizeBytes);

        destination.Write(Magic);
        destination.Write(salt);
        destination.Write(noncePrefix);

        var plainBuffer = new byte[chunkSize];
        var cipherBuffer = new byte[chunkSize];
        var tag = new byte[TagSizeBytes];
        var lengthPrefix = new byte[NonceCounterBytes];
        uint chunkIndex = 0;

        int bytesRead;
        while ((bytesRead = source.Read(plainBuffer, 0, plainBuffer.Length)) > 0)
        {
            var nonce = BuildNonce(noncePrefix, chunkIndex);
            var plainSpan = plainBuffer.AsSpan(0, bytesRead);
            var cipherSpan = cipherBuffer.AsSpan(0, bytesRead);

            aesGcm.Encrypt(nonce, plainSpan, cipherSpan, tag);

            BinaryPrimitives.WriteInt32BigEndian(lengthPrefix, bytesRead);
            destination.Write(lengthPrefix);
            destination.Write(tag);
            destination.Write(cipherSpan);

            checked { chunkIndex++; }
        }
    }

    public static void DecryptFile(string sourcePath, string destinationPath, string password, int chunkSize = DefaultChunkSizeBytes)
    {
        using var source = new FileStream(sourcePath, FileMode.Open, FileAccess.Read);

        var magic = new byte[Magic.Length];
        ReadExact(source, magic, "vault header (magic)");
        if (!magic.AsSpan().SequenceEqual(Magic))
        {
            throw new InvalidDataException("Not a SecureFileVault (.sfv) file.");
        }

        var salt = new byte[Pbkdf2KeyDerivation.DefaultSaltLengthBytes];
        ReadExact(source, salt, "vault header (salt)");

        var noncePrefix = new byte[NoncePrefixBytes];
        ReadExact(source, noncePrefix, "vault header (nonce prefix)");

        var key = Pbkdf2KeyDerivation.DeriveKey(password, salt);

        using var destination = new FileStream(destinationPath, FileMode.Create, FileAccess.Write);
        using var aesGcm = new AesGcm(key, TagSizeBytes);

        var lengthPrefix = new byte[NonceCounterBytes];
        var tag = new byte[TagSizeBytes];
        var cipherBuffer = new byte[chunkSize];
        var plainBuffer = new byte[chunkSize];
        uint chunkIndex = 0;

        while (true)
        {
            int firstByte = source.ReadByte();
            if (firstByte == -1)
            {
                break; // clean EOF right at a chunk boundary
            }

            lengthPrefix[0] = (byte)firstByte;
            ReadExact(source, lengthPrefix.AsSpan(1), "chunk length prefix");
            var chunkLength = BinaryPrimitives.ReadInt32BigEndian(lengthPrefix);
            if (chunkLength < 0 || chunkLength > chunkSize)
            {
                throw new InvalidDataException($"Corrupt vault file: implausible chunk length {chunkLength}.");
            }

            ReadExact(source, tag, "chunk tag");
            var cipherSpan = cipherBuffer.AsSpan(0, chunkLength);
            ReadExact(source, cipherSpan, "chunk ciphertext");

            var nonce = BuildNonce(noncePrefix, chunkIndex);
            var plainSpan = plainBuffer.AsSpan(0, chunkLength);

            try
            {
                aesGcm.Decrypt(nonce, cipherSpan, tag, plainSpan);
            }
            catch (CryptographicException ex)
            {
                throw new VaultIntegrityException((int)chunkIndex, ex);
            }

            destination.Write(plainSpan);
            checked { chunkIndex++; }
        }
    }

    private static byte[] BuildNonce(byte[] noncePrefix, uint chunkIndex)
    {
        var nonce = new byte[NonceSizeBytes];
        noncePrefix.CopyTo(nonce, 0);
        BinaryPrimitives.WriteUInt32BigEndian(nonce.AsSpan(NoncePrefixBytes), chunkIndex);
        return nonce;
    }

    private static void ReadExact(FileStream stream, byte[] buffer, string what) =>
        ReadExact(stream, buffer.AsSpan(), what);

    private static void ReadExact(FileStream stream, Span<byte> buffer, string what)
    {
        int totalRead = 0;
        while (totalRead < buffer.Length)
        {
            int read = stream.Read(buffer[totalRead..]);
            if (read == 0)
            {
                throw new InvalidDataException($"Corrupt vault file: unexpected end of stream while reading {what}.");
            }
            totalRead += read;
        }
    }
}
