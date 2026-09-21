using SecureFileVault.Core.Aead;

namespace SecureFileVault.Tests;

public class ChunkedGcmVaultServiceTests : IDisposable
{
    private readonly string _tempDir = Directory.CreateDirectory(
        Path.Combine(Path.GetTempPath(), "sfv-tests-" + Guid.NewGuid())).FullName;

    [Fact]
    public void EncryptDecrypt_RoundTrips_AcrossMultipleChunks()
    {
        var source = Path.Combine(_tempDir, "source.bin");
        var vault = Path.Combine(_tempDir, "vault.sfv");
        var restored = Path.Combine(_tempDir, "restored.bin");

        // Small chunk size + a few-hundred-KB file forces several chunks
        // through the loop without needing an actual 100 MB fixture in the test suite.
        var originalBytes = new byte[300_000];
        Random.Shared.NextBytes(originalBytes);
        File.WriteAllBytes(source, originalBytes);

        ChunkedGcmVaultService.EncryptFile(source, vault, "vault-password", chunkSize: 64 * 1024);
        ChunkedGcmVaultService.DecryptFile(vault, restored, "vault-password", chunkSize: 64 * 1024);

        Assert.Equal(originalBytes, File.ReadAllBytes(restored));
    }

    [Fact]
    public void Decrypt_WrongPassword_ThrowsVaultIntegrityException()
    {
        var source = Path.Combine(_tempDir, "source.bin");
        var vault = Path.Combine(_tempDir, "vault.sfv");
        var restored = Path.Combine(_tempDir, "restored.bin");
        File.WriteAllBytes(source, "hello vault"u8.ToArray());

        ChunkedGcmVaultService.EncryptFile(source, vault, "correct-password");

        Assert.Throws<VaultIntegrityException>(() =>
            ChunkedGcmVaultService.DecryptFile(vault, restored, "wrong-password"));
    }

    [Fact]
    public void Decrypt_TamperedByte_ThrowsVaultIntegrityException()
    {
        var source = Path.Combine(_tempDir, "source.bin");
        var vault = Path.Combine(_tempDir, "vault.sfv");
        var restored = Path.Combine(_tempDir, "restored.bin");
        File.WriteAllBytes(source, "hello vault, please stay intact"u8.ToArray());

        ChunkedGcmVaultService.EncryptFile(source, vault, "correct-password");

        var vaultBytes = File.ReadAllBytes(vault);
        vaultBytes[^1] ^= 0x01; // flip the last byte of the last chunk's ciphertext
        File.WriteAllBytes(vault, vaultBytes);

        Assert.Throws<VaultIntegrityException>(() =>
            ChunkedGcmVaultService.DecryptFile(vault, restored, "correct-password"));
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempDir))
        {
            Directory.Delete(_tempDir, recursive: true);
        }
    }
}
