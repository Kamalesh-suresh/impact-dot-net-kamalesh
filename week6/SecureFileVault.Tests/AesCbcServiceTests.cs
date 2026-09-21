using System.Security.Cryptography;
using SecureFileVault.Core.Symmetric;

namespace SecureFileVault.Tests;

public class AesCbcServiceTests
{
    [Fact]
    public void EncryptDecrypt_RoundTrips()
    {
        var key = AesCbcService.GenerateKey256();
        var (ciphertext, iv) = AesCbcService.Encrypt("hello vault", key);

        var plaintext = AesCbcService.Decrypt(ciphertext, key, iv);

        Assert.Equal("hello vault", plaintext);
    }

    [Fact]
    public void Encrypt_SamePlaintextTwice_ProducesDifferentCiphertextButBothDecrypt()
    {
        var key = AesCbcService.GenerateKey256();

        var (ciphertext1, iv1) = AesCbcService.Encrypt("same message", key);
        var (ciphertext2, iv2) = AesCbcService.Encrypt("same message", key);

        Assert.NotEqual(ciphertext1, ciphertext2);
        Assert.Equal("same message", AesCbcService.Decrypt(ciphertext1, key, iv1));
        Assert.Equal("same message", AesCbcService.Decrypt(ciphertext2, key, iv2));
    }

    [Fact]
    public void Decrypt_WrongKey_FailsSafely()
    {
        const string plaintext = "secret message for the CBC wrong-key test";
        var key = AesCbcService.GenerateKey256();
        var wrongKey = AesCbcService.GenerateKey256();
        var (ciphertext, iv) = AesCbcService.Encrypt(plaintext, key);

        string? result = null;
        Exception? thrown = null;
        try
        {
            result = AesCbcService.Decrypt(ciphertext, wrongKey, iv);
        }
        catch (Exception ex)
        {
            thrown = ex;
        }

        // CBC has no authentication tag: decrypting with the wrong key
        // throws a CryptographicException from broken PKCS7 padding the
        // overwhelming majority of the time, but padding can coincidentally
        // look valid (~1/256 chance). Either way it must NOT reproduce the
        // original plaintext -- that "silent garbage on the rare non-throw"
        // case is exactly the integrity gap this task highlights.
        Assert.True(thrown is CryptographicException || result != plaintext);
    }
}
