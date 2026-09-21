using System.Security.Cryptography;
using System.Text;
using SecureFileVault.Core.Aead;

namespace SecureFileVault.App.Demos;

// Tasks 6.8, 6.9
public static class AesGcmDemo
{
    public static void Run()
    {
        const string password = "vault-demo-password";
        const string plaintext = "Q3 numbers are not public yet.";
        var plainBytes = Encoding.UTF8.GetBytes(plaintext);

        var packed = AesGcmBufferService.EncryptWithPassword(plainBytes, password);
        Console.WriteLine($"  plaintext         : {plaintext}");
        Console.WriteLine($"  packed (b64)      : {Convert.ToBase64String(packed)}");
        Console.WriteLine("    format: salt(16) || nonce(12) || tag(16) || ciphertext");

        var decrypted = Encoding.UTF8.GetString(AesGcmBufferService.DecryptWithPassword(packed, password));
        Console.WriteLine($"  decrypted         : {decrypted}");
        Console.WriteLine($"  [{(decrypted == plaintext ? "OK" : "FAIL")}] round trip + tag verified.");
        Console.WriteLine();

        // 6.9: flip one byte of the ciphertext and confirm decrypt throws.
        var tampered = (byte[])packed.Clone();
        var tamperIndex = tampered.Length - 1; // last byte of ciphertext
        tampered[tamperIndex] ^= 0x01;

        Console.WriteLine("  6.9 flipping one byte of the ciphertext, then decrypting:");
        try
        {
            AesGcmBufferService.DecryptWithPassword(tampered, password);
            Console.WriteLine("  [FAIL] expected a CryptographicException but decryption succeeded.");
        }
        catch (CryptographicException ex)
        {
            Console.WriteLine($"  [OK] rejected: {ex.GetType().Name}: {ex.Message}");
        }
        Console.WriteLine("  Contrast with Task 6.6 (AES-CBC): CBC could only detect SOME tampering, by");
        Console.WriteLine("  accident, via broken padding. GCM's tag makes EVERY single-bit change detectable.");
    }
}
