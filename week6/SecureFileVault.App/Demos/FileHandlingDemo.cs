using SecureFileVault.Core.FileHandling;
using SecureFileVault.Core.Hashing;

namespace SecureFileVault.App.Demos;

// Task 6.1
public static class FileHandlingDemo
{
    public static void Run(string sampleDataDir)
    {
        var sourcePath = Path.Combine(sampleDataDir, "6.1-source.txt");
        var copyPath = Path.Combine(sampleDataDir, "6.1-copy.txt");

        File.WriteAllText(sourcePath, string.Concat(Enumerable.Repeat("The quick brown fox jumps over the lazy dog.\n", 500)));

        Console.WriteLine($"Copying {sourcePath} -> {copyPath} in {ChunkedFileCopier.DefaultBufferSize}-byte chunks (no ReadAllBytes)...");
        ChunkedFileCopier.Copy(sourcePath, copyPath);

        var sourceHash = FileHasher.ToHex(FileHasher.Sha256OfFile(sourcePath));
        var copyHash = FileHasher.ToHex(FileHasher.Sha256OfFile(copyPath));
        var identical = sourceHash == copyHash;

        Console.WriteLine($"  source SHA-256: {sourceHash}");
        Console.WriteLine($"  copy   SHA-256: {copyHash}");
        Console.WriteLine(identical ? "  [OK] copy is byte-for-byte identical." : "  [FAIL] copy differs from source.");

        ChunkedFileCopier.Append(copyPath, "\n-- appended via Task 6.1 --\n");
        Console.WriteLine("  Appended a line to the copy via FileMode.Append.");
    }
}
