using System.Security.Cryptography;

namespace SecureFileVault.Core.Hashing;

// Task 6.10: file digests, streamed (HashData/ComputeHash read the FileStream
// in internal chunks rather than needing the caller to buffer the file).
// SHA-256/SHA-512 prove "this is the same bytes as before" but not "who
// produced them" — anyone can compute the hash of public data. HMAC adds a
// secret key into the computation, so a valid HMAC also proves the sender
// knew the key: it authenticates origin, not just content.
public static class FileHasher
{
    public static byte[] Sha256OfFile(string path)
    {
        using var stream = File.OpenRead(path);
        return SHA256.HashData(stream);
    }

    public static byte[] Sha512OfFile(string path)
    {
        using var stream = File.OpenRead(path);
        return SHA512.HashData(stream);
    }

    public static byte[] HmacSha256OfFile(string path, byte[] key)
    {
        using var stream = File.OpenRead(path);
        using var hmac = new HMACSHA256(key);
        return hmac.ComputeHash(stream);
    }

    public static string ToHex(byte[] bytes) => Convert.ToHexString(bytes).ToLowerInvariant();
}
