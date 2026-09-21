using System.Security.Cryptography;
using System.Text;

namespace SecureFileVault.Core.Fundamentals;

// Task 6.2: the one-line distinction, proven in code rather than just asserted.
//   Encoding (Base64)  -> reversible, no secret involved. Never use it to "protect" data.
//   Hashing  (SHA-256)  -> one-way. Fixed-length output, cannot be reversed back to the input.
//   Encryption (AES/RSA, elsewhere in this project) -> reversible, but ONLY with a secret key.
public static class CodecDemo
{
    public static string Base64Encode(string plainText) =>
        Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));

    public static string Base64Decode(string base64Text) =>
        Encoding.UTF8.GetString(Convert.FromBase64String(base64Text));

    public static string Sha256HexOf(string plainText)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(plainText));
        return Convert.ToHexString(hash).ToLowerInvariant();
    }
}
