using System.Security.Cryptography;

namespace SecureFileVault.Core.Streaming;

// Task 6.12 (literal reading): "chain FileStream -> CryptoStream so the
// whole file never loads into memory." AES-GCM can't do this (see
// ChunkedGcmVaultService's header comment for why), so this class uses
// AES-256-CBC, which — like every block-cipher mode built on
// ICryptoTransform — plugs straight into CryptoStream. This path gives NO
// tamper detection (same caveat as AesCbcService); it exists purely to
// exercise CryptoStreamMode.Write vs Read and FileStream<->CryptoStream
// disposal order on a large file.
//
// Output format: iv (16 bytes, not secret) || ciphertext.
public static class CryptoStreamFileCipher
{
    private const int BufferSize = 1024 * 1024; // 1 MB — large enough to move a 100 MB file quickly, small enough to never approach loading the file whole.

    public static void EncryptFile(string sourcePath, string destinationPath, byte[] key)
    {
        using var aes = Aes.Create();
        aes.Key = key;
        aes.GenerateIV();

        using var source = new FileStream(sourcePath, FileMode.Open, FileAccess.Read);
        using var destination = new FileStream(destinationPath, FileMode.Create, FileAccess.Write);

        destination.Write(aes.IV, 0, aes.IV.Length);

        using var encryptor = aes.CreateEncryptor();
        // CryptoStreamMode.Write: we push plaintext IN, CryptoStream encrypts
        // it and writes ciphertext OUT to `destination` as we go.
        using (var cryptoStream = new CryptoStream(destination, encryptor, CryptoStreamMode.Write))
        {
            CopyChunked(source, cryptoStream);
            // Disposing here (end of this `using` block) flushes the final
            // padded block through the transform BEFORE `destination` is
            // disposed below — that order matters: flush the cipher, then
            // close the file.
        }
    }

    public static void DecryptFile(string sourcePath, string destinationPath, byte[] key)
    {
        using var source = new FileStream(sourcePath, FileMode.Open, FileAccess.Read);

        var iv = new byte[16];
        var ivRead = source.Read(iv, 0, iv.Length);
        if (ivRead != iv.Length)
        {
            throw new InvalidDataException("File is too short to contain an IV header.");
        }

        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;

        using var destination = new FileStream(destinationPath, FileMode.Create, FileAccess.Write);
        using var decryptor = aes.CreateDecryptor();
        // CryptoStreamMode.Read: we pull FROM `source` through CryptoStream,
        // which decrypts on the way out; we then write plaintext to `destination`.
        using var cryptoStream = new CryptoStream(source, decryptor, CryptoStreamMode.Read);
        CopyChunked(cryptoStream, destination);
    }

    private static void CopyChunked(Stream input, Stream output)
    {
        var buffer = new byte[BufferSize];
        int bytesRead;
        while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
        {
            output.Write(buffer, 0, bytesRead);
        }
    }
}
