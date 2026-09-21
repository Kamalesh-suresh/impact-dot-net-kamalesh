using SecureFileVault.Core.Rsa;

namespace SecureFileVault.Tests;

public class RsaDemoTests
{
    [Fact]
    public void EncryptDecryptShortMessage_RoundTrips()
    {
        using var rsa = RsaDemo.CreateKeyPair(2048);

        var ciphertext = RsaDemo.EncryptShortMessage(rsa, "short message");
        var plaintext = RsaDemo.DecryptShortMessage(rsa, ciphertext);

        Assert.Equal("short message", plaintext);
    }

    [Fact]
    public void MaxOaepSha256PlaintextBytes_MatchesKnownFormula()
    {
        // 2048-bit key -> 256 bytes; OAEP-SHA256 overhead is 2*32 + 2 = 66 bytes.
        Assert.Equal(190, RsaDemo.MaxOaepSha256PlaintextBytes(2048));
    }
}
