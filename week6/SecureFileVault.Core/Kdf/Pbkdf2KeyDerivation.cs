using System.Security.Cryptography;

namespace SecureFileVault.Core.Kdf;

// Task 6.7: PBKDF2 (Rfc2898DeriveBytes) turns a human password into a
// fixed-size key. Two things make this safe to store alongside the data it
// protects:
//   - Salt: a random per-secret value so two users with the same password
//     never derive the same key, and pre-computed ("rainbow table") attacks
//     don't work — the salt isn't secret, it just needs to be unique.
//   - Iterations: deliberately slows down each guess. A brute-force
//     attacker who steals the salt+hash still has to redo >=100,000 rounds
//     of SHA-256 per candidate password, turning a cheap offline attack
//     into an expensive one.
public static class Pbkdf2KeyDerivation
{
    public const int DefaultIterations = 100_000;
    public const int DefaultKeyLengthBytes = 32; // 256-bit key
    public const int DefaultSaltLengthBytes = 16;

    public static byte[] GenerateSalt(int lengthBytes = DefaultSaltLengthBytes) =>
        RandomNumberGenerator.GetBytes(lengthBytes);

    public static byte[] DeriveKey(
        string password,
        byte[] salt,
        int iterations = DefaultIterations,
        int keyLengthBytes = DefaultKeyLengthBytes)
    {
        return Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, keyLengthBytes);
    }
}
