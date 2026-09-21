using SecureFileVault.Core.Fundamentals;

namespace SecureFileVault.App.Demos;

// Tasks 6.2 and 6.3
public static class FundamentalsDemo
{
    public static void Run()
    {
        const string secret = "correct horse battery staple";

        var encoded = CodecDemo.Base64Encode(secret);
        var decoded = CodecDemo.Base64Decode(encoded);
        var hash = CodecDemo.Sha256HexOf(secret);

        Console.WriteLine($"  plaintext : {secret}");
        Console.WriteLine($"  Base64    : {encoded}   (reversible, no key -> anyone can decode this back to \"{decoded}\". NOT protection.)");
        Console.WriteLine($"  SHA-256   : {hash}   (fixed-length, one-way -> cannot be turned back into the plaintext.)");
        Console.WriteLine($"  Base64 round-trip OK: {decoded == secret}");
        Console.WriteLine();
        Console.WriteLine("  6.2 distinction: Base64 encodes (reversible, secret-free); SHA-256 hashes");
        Console.WriteLine("  (one-way, irreversible); AES/RSA elsewhere in this app encrypt (reversible,");
        Console.WriteLine("  but only WITH a key). Using Base64 as \"security\" is the classic wrong-tool");
        Console.WriteLine("  mistake -- it hides data from a casual glance, not from anyone who tries.");
        Console.WriteLine();
        Console.WriteLine("  6.3 cipher modes: ECB encrypts each block independently with the same key,");
        Console.WriteLine("  so identical plaintext blocks produce identical ciphertext blocks -- patterns");
        Console.WriteLine("  in the input (e.g. a flat-color image) leak straight through the ciphertext.");
        Console.WriteLine("  CBC XORs each block with the previous ciphertext block before encrypting, so");
        Console.WriteLine("  identical plaintext blocks no longer match -- but it's still unauthenticated");
        Console.WriteLine("  (see the AES-CBC demo). CTR turns a block cipher into a stream cipher by");
        Console.WriteLine("  encrypting a counter and XORing it with the plaintext -- fast and parallel,");
        Console.WriteLine("  still unauthenticated. GCM is CTR mode plus a built-in authentication tag:");
        Console.WriteLine("  the only one of the four that detects tampering on its own (see the AES-GCM demo).");
    }
}
