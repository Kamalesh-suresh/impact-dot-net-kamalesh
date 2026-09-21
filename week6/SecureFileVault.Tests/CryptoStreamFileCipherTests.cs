using SecureFileVault.Core.Streaming;
using SecureFileVault.Core.Symmetric;

namespace SecureFileVault.Tests;

public class CryptoStreamFileCipherTests : IDisposable
{
    private readonly string _dir = Directory.CreateDirectory(
        Path.Combine(Path.GetTempPath(), "sfv-cryptostream-" + Guid.NewGuid())).FullName;

    [Fact]
    public void EncryptDecrypt_RoundTrips_ByteIdentical()
    {
        var original = Path.Combine(_dir, "original.bin");
        var encrypted = Path.Combine(_dir, "encrypted.bin");
        var decrypted = Path.Combine(_dir, "decrypted.bin");

        var bytes = new byte[500_000]; // several MB-sized buffer's worth of chunk cycles
        Random.Shared.NextBytes(bytes);
        File.WriteAllBytes(original, bytes);

        var key = AesCbcService.GenerateKey256();
        CryptoStreamFileCipher.EncryptFile(original, encrypted, key);
        CryptoStreamFileCipher.DecryptFile(encrypted, decrypted, key);

        Assert.Equal(bytes, File.ReadAllBytes(decrypted));
    }

    public void Dispose()
    {
        if (Directory.Exists(_dir))
        {
            Directory.Delete(_dir, recursive: true);
        }
    }
}
