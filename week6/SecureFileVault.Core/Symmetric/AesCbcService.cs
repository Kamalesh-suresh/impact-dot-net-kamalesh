using System.Security.Cryptography;
using System.Text;

namespace SecureFileVault.Core.Symmetric;

// Tasks 6.4-6.6: AES-256-CBC via CryptoStream over a MemoryStream.
//
// Task 6.6's key point: CBC gives confidentiality only. Decrypting with the
// wrong key (or a tampered ciphertext) usually surfaces as a
// CryptographicException from bad PKCS7 padding — but a tampered ciphertext
// that happens to unpad cleanly decrypts to silent garbage with NO error at
// all. CBC has no built-in way to tell "wrong key" apart from "tampered
// data" apart from "corrupted, still valid-looking padding by chance".
// Contrast with AesGcmBufferService, where any single-byte change always
// throws (Task 6.9).
public static class AesCbcService
{
    public static byte[] GenerateKey256()
    {
        using var aes = Aes.Create();
        aes.KeySize = 256;
        aes.GenerateKey();
        return aes.Key;
    }

    public static (byte[] Ciphertext, byte[] Iv) Encrypt(string plainText, byte[] key)
    {
        using var aes = Aes.Create();
        aes.Key = key;
        aes.GenerateIV(); // Task 6.5: a fresh, random IV every call — never reused under the same key.

        using var encryptor = aes.CreateEncryptor();
        using var memoryStream = new MemoryStream();
        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
        using (var writer = new StreamWriter(cryptoStream))
        {
            writer.Write(plainText);
        }

        return (memoryStream.ToArray(), aes.IV);
    }

    public static string Decrypt(byte[] ciphertext, byte[] key, byte[] iv)
    {
        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor();
        using var memoryStream = new MemoryStream(ciphertext);
        using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        using var reader = new StreamReader(cryptoStream, Encoding.UTF8);
        return reader.ReadToEnd();
    }
}
