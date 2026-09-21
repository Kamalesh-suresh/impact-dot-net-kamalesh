using System.Security.Cryptography;
using SecureFileVault.Core.Hashing;

namespace SecureFileVault.App.Demos;

// Tasks 6.10, 6.11
public static class HashingDemo
{
    public static void Run(string sampleDataDir)
    {
        var path = Path.Combine(sampleDataDir, "6.10-file.txt");
        File.WriteAllText(path, "Contents that will be hashed and HMAC'd for Task 6.10.");

        var sha256 = FileHasher.Sha256OfFile(path);
        var sha512 = FileHasher.Sha512OfFile(path);
        var sha256Again = FileHasher.Sha256OfFile(path);

        Console.WriteLine($"  SHA-256 : {FileHasher.ToHex(sha256)}");
        Console.WriteLine($"  SHA-512 : {FileHasher.ToHex(sha512)}");
        Console.WriteLine($"  [OK] SHA-256 stable across runs: {sha256.AsSpan().SequenceEqual(sha256Again)}");

        var keyA = RandomNumberGenerator.GetBytes(32);
        var keyB = RandomNumberGenerator.GetBytes(32);
        var hmacA = FileHasher.HmacSha256OfFile(path, keyA);
        var hmacA2 = FileHasher.HmacSha256OfFile(path, keyA);
        var hmacB = FileHasher.HmacSha256OfFile(path, keyB);

        Console.WriteLine($"  HMAC-SHA256 (key A) : {FileHasher.ToHex(hmacA)}");
        Console.WriteLine($"  HMAC-SHA256 (key B) : {FileHasher.ToHex(hmacB)}");
        Console.WriteLine($"  [OK] same key -> same HMAC: {hmacA.AsSpan().SequenceEqual(hmacA2)}");
        Console.WriteLine($"  [OK] different key -> different HMAC: {!hmacA.AsSpan().SequenceEqual(hmacB)}");
        Console.WriteLine("  A plain hash proves \"what\" (the file wasn't changed); HMAC additionally");
        Console.WriteLine("  proves \"who\" (only someone holding the key could have produced this tag).");
        Console.WriteLine();

        // 6.11: constant-time comparison.
        Console.WriteLine("  6.11 comparing two digests with CryptographicOperations.FixedTimeEquals:");
        Console.WriteLine($"    equal digests   -> {ConstantTimeComparer.Equals(sha256, sha256Again)}");
        Console.WriteLine($"    different digest -> {ConstantTimeComparer.Equals(sha256, sha512[..32])}");
        Console.WriteLine("  `==`/SequenceEqual short-circuit at the first mismatching byte, so comparison");
        Console.WriteLine("  time leaks how many leading bytes were correct -- exploitable in a timing");
        Console.WriteLine("  attack against a token/HMAC check. FixedTimeEquals always walks the full");
        Console.WriteLine("  length, so the elapsed time reveals nothing about where (or whether) it matched.");
    }
}
