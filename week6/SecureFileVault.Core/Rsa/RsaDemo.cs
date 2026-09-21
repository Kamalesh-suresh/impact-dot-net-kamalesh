using System.Security.Cryptography;
using System.Text;

namespace SecureFileVault.Core.Rsa;

// Task 6.13 (concept only — the project does not use RSA anywhere else).
//
// Asymmetric crypto uses a key PAIR instead of one shared secret:
//   - The PUBLIC key encrypts (anyone can send you a secret) and verifies
//     signatures (anyone can check a signature is genuine).
//   - The PRIVATE key decrypts (only you can read what was encrypted to
//     you) and signs (only you can produce a signature others can verify).
//   Encrypting and signing use the SAME key pair but in opposite directions.
//
// RSA has a hard size limit: with OAEP-SHA256 padding, the maximum
// plaintext is (keySizeBytes - 2*hashSizeBytes - 2). For a 2048-bit key
// that's 256 - 64 - 2 = 190 bytes — nowhere near enough for a file. That's
// why real protocols (TLS included) use HYBRID encryption: generate a
// random AES key, encrypt the actual data with AES (fast, no size limit),
// then RSA-encrypt only that small AES key with the recipient's public key.
// The recipient RSA-decrypts the AES key with their private key, then
// AES-decrypts the data. RSA never touches the bulk data directly.
public static class RsaDemo
{
    public static RSA CreateKeyPair(int keySizeBits = 2048) => RSA.Create(keySizeBits);

    public static byte[] EncryptShortMessage(RSA publicKey, string message) =>
        publicKey.Encrypt(Encoding.UTF8.GetBytes(message), RSAEncryptionPadding.OaepSHA256);

    public static string DecryptShortMessage(RSA privateKey, byte[] ciphertext) =>
        Encoding.UTF8.GetString(privateKey.Decrypt(ciphertext, RSAEncryptionPadding.OaepSHA256));

    public static int MaxOaepSha256PlaintextBytes(int keySizeBits)
    {
        const int hashSizeBytes = 32; // SHA-256
        return keySizeBits / 8 - 2 * hashSizeBytes - 2;
    }
}
