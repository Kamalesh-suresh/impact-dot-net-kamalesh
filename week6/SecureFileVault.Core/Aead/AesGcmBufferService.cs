using System.Security.Cryptography;
using SecureFileVault.Core.Kdf;

namespace SecureFileVault.Core.Aead;

// Tasks 6.8-6.9: AES-GCM — authenticated encryption. Unlike CBC, GCM produces
// a 16-byte authentication tag alongside the ciphertext. Decrypt verifies the
// tag BEFORE returning any plaintext: any change to the ciphertext, the tag,
// or the associated data makes AesGcm.Decrypt throw AuthenticationTagMismatchException
// (a CryptographicException) instead of silently returning garbage.
//
// Wire format for a single buffer: salt (16) || nonce (12) || tag (16) || ciphertext.
// The salt lets DecryptWithPassword re-derive the same PBKDF2 key; nonce and
// tag are not secret and travel with the ciphertext by design.
public static class AesGcmBufferService
{
    private const int NonceSizeBytes = 12;
    private const int TagSizeBytes = 16;

    public static byte[] EncryptWithPassword(byte[] plaintext, string password)
    {
        var salt = Pbkdf2KeyDerivation.GenerateSalt();
        var key = Pbkdf2KeyDerivation.DeriveKey(password, salt);

        var nonce = RandomNumberGenerator.GetBytes(NonceSizeBytes);
        var ciphertext = new byte[plaintext.Length];
        var tag = new byte[TagSizeBytes];

        using (var aesGcm = new AesGcm(key, TagSizeBytes))
        {
            aesGcm.Encrypt(nonce, plaintext, ciphertext, tag);
        }

        var packed = new byte[salt.Length + nonce.Length + tag.Length + ciphertext.Length];
        var offset = 0;
        Buffer.BlockCopy(salt, 0, packed, offset, salt.Length); offset += salt.Length;
        Buffer.BlockCopy(nonce, 0, packed, offset, nonce.Length); offset += nonce.Length;
        Buffer.BlockCopy(tag, 0, packed, offset, tag.Length); offset += tag.Length;
        Buffer.BlockCopy(ciphertext, 0, packed, offset, ciphertext.Length);

        return packed;
    }

    // Throws CryptographicException if the password is wrong OR the packed
    // buffer was tampered with anywhere (salt aside) — that's Task 6.9's
    // "flip one byte and confirm it throws" proven at the API boundary.
    public static byte[] DecryptWithPassword(byte[] packed, string password)
    {
        var saltLength = Pbkdf2KeyDerivation.DefaultSaltLengthBytes;
        if (packed.Length < saltLength + NonceSizeBytes + TagSizeBytes)
        {
            throw new CryptographicException("Packed buffer is too short to contain salt, nonce and tag.");
        }

        var salt = packed[..saltLength];
        var nonce = packed[saltLength..(saltLength + NonceSizeBytes)];
        var tag = packed[(saltLength + NonceSizeBytes)..(saltLength + NonceSizeBytes + TagSizeBytes)];
        var ciphertext = packed[(saltLength + NonceSizeBytes + TagSizeBytes)..];

        var key = Pbkdf2KeyDerivation.DeriveKey(password, salt);
        var plaintext = new byte[ciphertext.Length];

        using var aesGcm = new AesGcm(key, TagSizeBytes);
        aesGcm.Decrypt(nonce, ciphertext, tag, plaintext); // throws on tamper or wrong key
        return plaintext;
    }
}
