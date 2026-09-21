using System.Security.Cryptography;
using SecureFileVault.Core.Aead;
using SecureFileVault.Core.FileHandling;
using SecureFileVault.Core.Hashing;

namespace SecureFileVault.App.Demos;

// The Weekly Deliverable itself: password -> PBKDF2 -> AES-GCM, streamed,
// tamper-rejected (backed by SecureFileVault.Core.Aead.ChunkedGcmVaultService).
public static class VaultRunner
{
    private const long DemoFileSizeBytes = 3L * 1024 * 1024; // small enough to run instantly, big enough to span several chunks

    // Fully automated: generates a file, encrypts it, decrypts it, proves the
    // round trip is byte-identical, then flips one byte in the vault file and
    // proves decryption is rejected. No prompts, so it's easy to screenshot.
    public static void RunAutoDemo(string sampleDataDir)
    {
        const string password = "correct horse battery staple";

        var originalPath = Path.Combine(sampleDataDir, "vault-demo-original.bin");
        var vaultPath = Path.Combine(sampleDataDir, "vault-demo.sfv");
        var restoredPath = Path.Combine(sampleDataDir, "vault-demo-restored.bin");

        Console.WriteLine($"  Generating a {DemoFileSizeBytes / 1024} KB demo file...");
        ChunkedFileCopier.GenerateRandomFile(originalPath, DemoFileSizeBytes);

        Console.WriteLine($"  Encrypting -> {Path.GetFileName(vaultPath)} (password -> PBKDF2 -> chunked AES-GCM)...");
        ChunkedGcmVaultService.EncryptFile(originalPath, vaultPath, password);

        Console.WriteLine("  Decrypting with the correct password...");
        ChunkedGcmVaultService.DecryptFile(vaultPath, restoredPath, password);

        var originalHash = FileHasher.ToHex(FileHasher.Sha256OfFile(originalPath));
        var restoredHash = FileHasher.ToHex(FileHasher.Sha256OfFile(restoredPath));
        var identical = originalHash == restoredHash;
        Console.WriteLine($"  original SHA-256 : {originalHash}");
        Console.WriteLine($"  restored SHA-256 : {restoredHash}");
        Console.WriteLine(identical ? "  [OK] vault round trip is byte-identical." : "  [FAIL] restored file does not match.");
        Console.WriteLine();

        Console.WriteLine("  Tampering: flipping one byte inside the vault file, then decrypting again...");
        var vaultBytes = File.ReadAllBytes(vaultPath);
        vaultBytes[^1] ^= 0x01; // flip the last byte (inside the final chunk's ciphertext)
        File.WriteAllBytes(vaultPath, vaultBytes);

        try
        {
            ChunkedGcmVaultService.DecryptFile(vaultPath, restoredPath, password);
            Console.WriteLine("  [FAIL] expected the tampered vault to be rejected, but decryption succeeded.");
        }
        catch (VaultIntegrityException ex)
        {
            Console.WriteLine($"  [OK] tampered vault rejected: {ex.Message}");
        }

        File.Delete(originalPath);
        File.Delete(vaultPath);
        File.Delete(restoredPath);
    }

    public static void EncryptInteractive()
    {
        Console.Write("  Source file path to encrypt: ");
        var source = Console.ReadLine() ?? "";
        Console.Write("  Destination .sfv path: ");
        var destination = Console.ReadLine() ?? "";
        var password = ReadPassword("  Password: ");

        if (!File.Exists(source))
        {
            Console.WriteLine($"  [FAIL] source file not found: {source}");
            return;
        }

        ChunkedGcmVaultService.EncryptFile(source, destination, password);
        Console.WriteLine($"  [OK] encrypted -> {destination}");
    }

    public static void DecryptInteractive()
    {
        Console.Write("  Vault (.sfv) path to decrypt: ");
        var source = Console.ReadLine() ?? "";
        Console.Write("  Destination path for the recovered file: ");
        var destination = Console.ReadLine() ?? "";
        var password = ReadPassword("  Password: ");

        if (!File.Exists(source))
        {
            Console.WriteLine($"  [FAIL] vault file not found: {source}");
            return;
        }

        try
        {
            ChunkedGcmVaultService.DecryptFile(source, destination, password);
            Console.WriteLine($"  [OK] decrypted -> {destination}");
        }
        catch (VaultIntegrityException ex)
        {
            Console.WriteLine($"  [REJECTED] {ex.Message}");
        }
        catch (InvalidDataException ex)
        {
            Console.WriteLine($"  [FAIL] {ex.Message}");
        }
    }

    // dotnet run -- encrypt <source> <destination.sfv> <password>
    // dotnet run -- decrypt <source.sfv> <destination> <password>
    public static int RunFromArgs(string[] args)
    {
        if (args.Length != 4 || (args[0] != "encrypt" && args[0] != "decrypt"))
        {
            Console.WriteLine("Usage: dotnet run -- encrypt|decrypt <source> <destination> <password>");
            return 1;
        }

        var (command, source, destination, password) = (args[0], args[1], args[2], args[3]);

        try
        {
            if (command == "encrypt")
            {
                ChunkedGcmVaultService.EncryptFile(source, destination, password);
            }
            else
            {
                ChunkedGcmVaultService.DecryptFile(source, destination, password);
            }

            Console.WriteLine($"[OK] {command}ed -> {destination}");
            return 0;
        }
        catch (VaultIntegrityException ex)
        {
            Console.WriteLine($"[REJECTED] {ex.Message}");
            return 2;
        }
    }

    private static string ReadPassword(string prompt)
    {
        Console.Write(prompt);
        var password = new System.Text.StringBuilder();
        ConsoleKeyInfo key;
        while ((key = Console.ReadKey(intercept: true)).Key != ConsoleKey.Enter)
        {
            if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password.Length--;
            }
            else if (!char.IsControl(key.KeyChar))
            {
                password.Append(key.KeyChar);
            }
        }
        Console.WriteLine();
        return password.ToString();
    }
}
