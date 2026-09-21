using SecureFileVault.Core.Rsa;

namespace SecureFileVault.App.Demos;

// Task 6.13
public static class RsaDemoRunner
{
    public static void Run()
    {
        const int keySizeBits = 2048;
        const string message = "The vault key rotates at midnight.";

        using var rsa = RsaDemo.CreateKeyPair(keySizeBits);

        var ciphertext = RsaDemo.EncryptShortMessage(rsa, message);
        var decrypted = RsaDemo.DecryptShortMessage(rsa, ciphertext);

        Console.WriteLine($"  message           : {message}");
        Console.WriteLine($"  RSA-2048/OAEP-SHA256 ciphertext (b64): {Convert.ToBase64String(ciphertext)}");
        Console.WriteLine($"  decrypted         : {decrypted}");
        Console.WriteLine($"  [{(decrypted == message ? "OK" : "FAIL")}] round trip matches.");
        Console.WriteLine();

        var maxBytes = RsaDemo.MaxOaepSha256PlaintextBytes(keySizeBits);
        Console.WriteLine($"  Max OAEP-SHA256 plaintext for a {keySizeBits}-bit key: {maxBytes} bytes.");
        Console.WriteLine("  That's why RSA never encrypts bulk data directly: real protocols (TLS");
        Console.WriteLine("  included) use hybrid encryption -- AES encrypts the actual data (no size");
        Console.WriteLine("  limit), and RSA only encrypts that small AES key. The public key encrypts");
        Console.WriteLine("  (anyone can send you a secret); the private key decrypts -- and signing");
        Console.WriteLine("  works the other way round: the private key signs, the public key verifies.");
        Console.WriteLine("  (Concept-level only -- SecureFileVault itself doesn't use RSA anywhere.)");
    }
}
