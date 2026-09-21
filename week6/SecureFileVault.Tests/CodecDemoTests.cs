using SecureFileVault.Core.Fundamentals;

namespace SecureFileVault.Tests;

public class CodecDemoTests
{
    [Fact]
    public void Base64_RoundTrips()
    {
        var encoded = CodecDemo.Base64Encode("hello world");
        Assert.Equal("hello world", CodecDemo.Base64Decode(encoded));
    }

    [Fact]
    public void Sha256HexOf_IsStableAndFixedLength()
    {
        var hashA = CodecDemo.Sha256HexOf("hello world");
        var hashB = CodecDemo.Sha256HexOf("hello world");

        Assert.Equal(hashA, hashB);
        Assert.Equal(64, hashA.Length); // 32 bytes, hex-encoded
    }

    [Fact]
    public void Sha256HexOf_DifferentInput_DifferentHash()
    {
        Assert.NotEqual(CodecDemo.Sha256HexOf("hello"), CodecDemo.Sha256HexOf("world"));
    }
}
