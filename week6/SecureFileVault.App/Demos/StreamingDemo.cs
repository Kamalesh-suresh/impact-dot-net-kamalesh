using System.Diagnostics;
using SecureFileVault.Core.FileHandling;
using SecureFileVault.Core.Hashing;
using SecureFileVault.Core.Streaming;
using SecureFileVault.Core.Symmetric;

namespace SecureFileVault.App.Demos;

// Task 6.12: stream-encrypt a >=100 MB file, chaining FileStream -> CryptoStream
// (AES-CBC — see CryptoStreamFileCipher's header comment for why GCM can't
// do this), and prove the round trip is byte-identical without ever loading
// the whole file into memory (verified here via a streamed SHA-256, which
// itself never buffers the whole file either).
public static class StreamingDemo
{
    private const long FileSizeBytes = 105L * 1024 * 1024; // 105 MB, comfortably over the 100 MB requirement

    public static void Run(string sampleDataDir)
    {
        var originalPath = Path.Combine(sampleDataDir, "6.12-original.bin");
        var encryptedPath = Path.Combine(sampleDataDir, "6.12-encrypted.bin");
        var decryptedPath = Path.Combine(sampleDataDir, "6.12-decrypted.bin");

        Console.WriteLine($"  Generating a {FileSizeBytes / (1024.0 * 1024.0):N0} MB random file (streamed, not held in memory)...");
        ChunkedFileCopier.GenerateRandomFile(originalPath, FileSizeBytes);

        var key = AesCbcService.GenerateKey256();
        var stopwatch = Stopwatch.StartNew();

        Console.WriteLine("  Encrypting: FileStream -> CryptoStream (CryptoStreamMode.Write)...");
        CryptoStreamFileCipher.EncryptFile(originalPath, encryptedPath, key);

        Console.WriteLine("  Decrypting: FileStream -> CryptoStream (CryptoStreamMode.Read)...");
        CryptoStreamFileCipher.DecryptFile(encryptedPath, decryptedPath, key);
        stopwatch.Stop();

        var originalHash = FileHasher.ToHex(FileHasher.Sha256OfFile(originalPath));
        var decryptedHash = FileHasher.ToHex(FileHasher.Sha256OfFile(decryptedPath));
        var identical = originalHash == decryptedHash;

        Console.WriteLine($"  original  SHA-256 : {originalHash}");
        Console.WriteLine($"  decrypted SHA-256 : {decryptedHash}");
        Console.WriteLine($"  encrypt+decrypt took: {stopwatch.ElapsedMilliseconds:N0} ms");
        Console.WriteLine(identical
            ? "  [OK] decrypted file is byte-identical to the original — 100 MB+ round trip verified."
            : "  [FAIL] decrypted file does NOT match the original.");

        Console.WriteLine("  Cleaning up the large demo files...");
        File.Delete(originalPath);
        File.Delete(encryptedPath);
        File.Delete(decryptedPath);
    }
}
