using System.Text;
using SecureFileVault.Core.Hashing;

namespace SecureFileVault.Tests;

public class ConstantTimeComparerTests
{
    [Fact]
    public void Equals_IdenticalBytes_ReturnsTrue()
    {
        var a = Encoding.UTF8.GetBytes("same-value");
        var b = Encoding.UTF8.GetBytes("same-value");

        Assert.True(ConstantTimeComparer.Equals(a, b));
    }

    [Fact]
    public void Equals_DifferentBytes_ReturnsFalse()
    {
        var a = Encoding.UTF8.GetBytes("value-one");
        var b = Encoding.UTF8.GetBytes("value-two");

        Assert.False(ConstantTimeComparer.Equals(a, b));
    }

    [Fact]
    public void Equals_DifferentLengths_ReturnsFalse()
    {
        var a = Encoding.UTF8.GetBytes("short");
        var b = Encoding.UTF8.GetBytes("a-much-longer-value");

        Assert.False(ConstantTimeComparer.Equals(a, b));
    }
}
