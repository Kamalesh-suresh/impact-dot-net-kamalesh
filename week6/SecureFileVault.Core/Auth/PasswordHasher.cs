using SecureFileVault.Core.Hashing;
using SecureFileVault.Core.Kdf;

namespace SecureFileVault.Core.Auth;

// Task 6.15's storage half: never store a password, store a salted, slow
// PBKDF2 hash of it (reusing Pbkdf2KeyDerivation from Task 6.7). Stored
// format is "iterations.saltBase64.hashBase64" so the iteration count and
// salt travel with the hash — needed to re-derive and compare on Verify,
// and lets the iteration count be raised later without invalidating hashes
// created under the old count.
public static class PasswordHasher
{
    public static string Hash(string password, int iterations = Pbkdf2KeyDerivation.DefaultIterations)
    {
        var salt = Pbkdf2KeyDerivation.GenerateSalt();
        var derived = Pbkdf2KeyDerivation.DeriveKey(password, salt, iterations);
        return $"{iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(derived)}";
    }

    public static bool Verify(string password, string stored)
    {
        var parts = stored.Split('.', 3);
        if (parts.Length != 3 || !int.TryParse(parts[0], out var iterations))
        {
            return false;
        }

        var salt = Convert.FromBase64String(parts[1]);
        var expected = Convert.FromBase64String(parts[2]);
        var actual = Pbkdf2KeyDerivation.DeriveKey(password, salt, iterations, expected.Length);

        // Constant-time compare (Task 6.11) — a login endpoint is exactly
        // the kind of secret comparison a timing attack targets.
        return ConstantTimeComparer.Equals(actual, expected);
    }
}
