using SecureFileVault.App.Demos;

namespace SecureFileVault.App;

public static class Program
{
    public static int Main(string[] args)
    {
        // Direct CLI usage for real encrypt/decrypt runs, bypassing the menu:
        //   dotnet run -- encrypt <source> <destination.sfv> <password>
        //   dotnet run -- decrypt <source.sfv> <destination> <password>
        if (args.Length > 0 && (args[0] == "encrypt" || args[0] == "decrypt"))
        {
            return VaultRunner.RunFromArgs(args);
        }

        var sampleDataDir = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "SampleData");
        Directory.CreateDirectory(sampleDataDir);

        RunMenu(sampleDataDir);
        return 0;
    }

    private static void RunMenu(string sampleDataDir)
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== SecureFileVault -- Week 6 ===");
            Console.WriteLine(" 1. File handling: chunked copy + append          (Task 6.1)");
            Console.WriteLine(" 2. Fundamentals: encoding vs hashing, cipher modes (Tasks 6.2, 6.3)");
            Console.WriteLine(" 3. AES-CBC: round trip, IV rule, wrong key        (Tasks 6.4-6.6)");
            Console.WriteLine(" 4. PBKDF2 key derivation                          (Task 6.7)");
            Console.WriteLine(" 5. AES-GCM: round trip + tamper rejection         (Tasks 6.8, 6.9)");
            Console.WriteLine(" 6. Hashing, HMAC, constant-time compare           (Tasks 6.10, 6.11)");
            Console.WriteLine(" 7. Stream-encrypt a 100 MB+ file (CryptoStream)   (Task 6.12)");
            Console.WriteLine(" 8. RSA concept demo                               (Task 6.13)");
            Console.WriteLine(" 9. Vault demo: encrypt/decrypt/tamper (automatic)");
            Console.WriteLine("10. Vault: encrypt a file (interactive)");
            Console.WriteLine("11. Vault: decrypt a file (interactive)");
            Console.WriteLine(" 0. Exit");
            Console.Write("Choose: ");

            var choice = Console.ReadLine();
            Console.WriteLine();

            try
            {
                switch (choice)
                {
                    case "1": FileHandlingDemo.Run(sampleDataDir); break;
                    case "2": FundamentalsDemo.Run(); break;
                    case "3": AesCbcDemo.Run(); break;
                    case "4": Pbkdf2Demo.Run(); break;
                    case "5": AesGcmDemo.Run(); break;
                    case "6": HashingDemo.Run(sampleDataDir); break;
                    case "7": StreamingDemo.Run(sampleDataDir); break;
                    case "8": RsaDemoRunner.Run(); break;
                    case "9": VaultRunner.RunAutoDemo(sampleDataDir); break;
                    case "10": VaultRunner.EncryptInteractive(); break;
                    case "11": VaultRunner.DecryptInteractive(); break;
                    case "0": return;
                    default: Console.WriteLine("Unknown choice."); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] {ex.GetType().Name}: {ex.Message}");
            }
        }
    }
}
