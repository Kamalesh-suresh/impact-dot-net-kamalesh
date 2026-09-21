using System.Security.Cryptography;
using System.Text;
using SecureFileVault.Core.Aead;

namespace SecureFileVault.Tests;

public class AesGcmBufferServiceTests
{
    [Fact]
    public void EncryptDecrypt_WithCorrectPassword_RoundTrips()
    {
        var plaintext = Encoding.UTF8.GetBytes("top secret payload");

        var packed = AesGcmBufferService.EncryptWithPassword(plaintext, "correct-password");
        var decrypted = AesGcmBufferService.DecryptWithPassword(packed, "correct-password");

        Assert.Equal(plaintext, decrypted);
    }

    [Fact]
    public void Decrypt_WrongPassword_Throws()
    {
        var plaintext = Encoding.UTF8.GetBytes("top secret payload");
        var packed = AesGcmBufferService.EncryptWithPassword(plaintext, "correct-password");

        Assert.ThrowsAny<CryptographicException>(() => AesGcmBufferService.DecryptWithPassword(packed, "wrong-password"));
    }

    [Fact]
    public void Decrypt_SingleByteTamper_Throws()
    {
        var plaintext = Encoding.UTF8.GetBytes("top secret payload");
        var packed = AesGcmBufferService.EncryptWithPassword(plaintext, "correct-password");

        var tampered = (byte[])packed.Clone();
        tampered[^1] ^= 0x01; // flip the last byte of the ciphertext

        Assert.ThrowsAny<CryptographicException>(() => AesGcmBufferService.DecryptWithPassword(tampered, "correct-password"));
    }
}
