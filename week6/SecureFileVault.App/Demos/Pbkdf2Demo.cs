using SecureFileVault.Core.Kdf;

namespace SecureFileVault.App.Demos;

// Task 6.7
public static class Pbkdf2Demo
{
    public static void Run()
    {
        const string password = "hunter2-but-longer-and-better";
        var salt = Pbkdf2KeyDerivation.GenerateSalt();

        var key1 = Pbkdf2KeyDerivation.DeriveKey(password, salt);
        var key2 = Pbkdf2KeyDerivation.DeriveKey(password, salt);
        var sameSaltSameKey = key1.AsSpan().SequenceEqual(key2);

        var differentSalt = Pbkdf2KeyDerivation.GenerateSalt();
        var key3 = Pbkdf2KeyDerivation.DeriveKey(password, differentSalt);
        var differentSaltDifferentKey = !key1.AsSpan().SequenceEqual(key3);

        Console.WriteLine($"  password              : {password}");
        Console.WriteLine($"  salt (b64)            : {Convert.ToBase64String(salt)}");
        Console.WriteLine($"  iterations            : {Pbkdf2KeyDerivation.DefaultIterations:N0}");
        Console.WriteLine($"  derived key (b64)     : {Convert.ToBase64String(key1)}");
        Console.WriteLine($"  same password+salt -> same key: {sameSaltSameKey}");
        Console.WriteLine($"  different salt -> different key: {differentSaltDifferentKey}");
        Console.WriteLine($"  [{(sameSaltSameKey && differentSaltDifferentKey ? "OK" : "FAIL")}] PBKDF2 determinism holds.");
        Console.WriteLine();
        Console.WriteLine("  Salt stops two users with the same password from sharing a derived key (and");
        Console.WriteLine("  defeats precomputed rainbow-table attacks); it isn't secret, it just has to");
        Console.WriteLine("  be unique per password. Iterations deliberately slow down each guess, so a");
        Console.WriteLine("  stolen salt+hash still costs the attacker 100,000 rounds of SHA-256 per try.");
    }
}
