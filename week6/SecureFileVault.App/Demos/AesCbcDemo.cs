using System.Security.Cryptography;
using SecureFileVault.Core.Symmetric;

namespace SecureFileVault.App.Demos;

// Tasks 6.4, 6.5, 6.6
public static class AesCbcDemo
{
    public static void Run()
    {
        const string plaintext = "Meet at the old bridge, 9pm.";
        var key = AesCbcService.GenerateKey256();

        // 6.4: round trip.
        var (ciphertext1, iv1) = AesCbcService.Encrypt(plaintext, key);
        var roundTripped = AesCbcService.Decrypt(ciphertext1, key, iv1);
        Console.WriteLine($"  plaintext        : {plaintext}");
        Console.WriteLine($"  ciphertext (b64) : {Convert.ToBase64String(ciphertext1)}");
        Console.WriteLine($"  IV (b64, stored alongside ciphertext -- not secret): {Convert.ToBase64String(iv1)}");
        Console.WriteLine($"  decrypted        : {roundTripped}");
        Console.WriteLine($"  [OK] round trip matches: {roundTripped == plaintext}");
        Console.WriteLine();

        // 6.5: same plaintext, same key, fresh IV each call -> different ciphertext, both still decrypt.
        var (ciphertext2, iv2) = AesCbcService.Encrypt(plaintext, key);
        var sameCiphertext = ciphertext1.AsSpan().SequenceEqual(ciphertext2);
        var bothDecryptCorrectly =
            AesCbcService.Decrypt(ciphertext1, key, iv1) == plaintext &&
            AesCbcService.Decrypt(ciphertext2, key, iv2) == plaintext;

        Console.WriteLine("  6.5 IV rule -- same plaintext encrypted twice with the same key, fresh IV each time:");
        Console.WriteLine($"    ciphertexts identical? {sameCiphertext} (must be false)");
        Console.WriteLine($"    both still decrypt correctly? {bothDecryptCorrectly} (must be true)");
        Console.WriteLine($"  [{(!sameCiphertext && bothDecryptCorrectly ? "OK" : "FAIL")}] IV rule holds.");
        Console.WriteLine();

        // 6.6: wrong key.
        var wrongKey = AesCbcService.GenerateKey256();
        Console.WriteLine("  6.6 decrypting with the WRONG key:");
        try
        {
            AesCbcService.Decrypt(ciphertext1, wrongKey, iv1);
            Console.WriteLine("  [FAIL] expected a CryptographicException but none was thrown.");
        }
        catch (CryptographicException ex)
        {
            Console.WriteLine($"  [OK] caught CryptographicException: {ex.Message}");
        }
        Console.WriteLine("  Note: that exception is just broken PKCS7 padding after decrypting with the");
        Console.WriteLine("  wrong key. CBC has no authentication tag -- a tampered ciphertext that happens");
        Console.WriteLine("  to unpad cleanly would decrypt to silent garbage with NO exception at all.");
        Console.WriteLine("  Compare with the AES-GCM demo, which rejects tampering every time.");
    }
}
