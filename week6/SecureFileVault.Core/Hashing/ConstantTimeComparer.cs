using System.Security.Cryptography;

namespace SecureFileVault.Core.Hashing;

// Task 6.11: `==`/SequenceEqual on byte arrays compare left-to-right and
// return as soon as they find a mismatching byte. For a secret comparison
// (a digest, an HMAC, a token) that means how LONG the comparison takes
// leaks how many leading bytes were correct — an attacker who can measure
// timing (even over a network, with enough samples) can recover the secret
// one byte at a time. CryptographicOperations.FixedTimeEquals always walks
// the full length regardless of where the first mismatch is, so the timing
// reveals nothing.
public static class ConstantTimeComparer
{
    public static bool Equals(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right) =>
        CryptographicOperations.FixedTimeEquals(left, right);
}
