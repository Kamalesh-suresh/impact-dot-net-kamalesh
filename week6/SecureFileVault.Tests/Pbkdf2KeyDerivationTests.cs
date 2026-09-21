using SecureFileVault.Core.Kdf;

namespace SecureFileVault.Tests;

public class Pbkdf2KeyDerivationTests
{
    [Fact]
    public void DeriveKey_SamePasswordAndSalt_ProducesSameKey()
    {
        var salt = Pbkdf2KeyDerivation.GenerateSalt();

        var key1 = Pbkdf2KeyDerivation.DeriveKey("hunter2", salt);
        var key2 = Pbkdf2KeyDerivation.DeriveKey("hunter2", salt);

        Assert.Equal(key1, key2);
    }

    [Fact]
    public void DeriveKey_DifferentSalt_ProducesDifferentKey()
    {
        var saltA = Pbkdf2KeyDerivation.GenerateSalt();
        var saltB = Pbkdf2KeyDerivation.GenerateSalt();

        var keyA = Pbkdf2KeyDerivation.DeriveKey("hunter2", saltA);
        var keyB = Pbkdf2KeyDerivation.DeriveKey("hunter2", saltB);

        Assert.NotEqual(keyA, keyB);
    }

    [Fact]
    public void DeriveKey_DifferentPassword_ProducesDifferentKey()
    {
        var salt = Pbkdf2KeyDerivation.GenerateSalt();

        var keyA = Pbkdf2KeyDerivation.DeriveKey("hunter2", salt);
        var keyB = Pbkdf2KeyDerivation.DeriveKey("hunter3", salt);

        Assert.NotEqual(keyA, keyB);
    }

    [Fact]
    public void DeriveKey_RespectsRequestedKeyLength()
    {
        var salt = Pbkdf2KeyDerivation.GenerateSalt();

        var key = Pbkdf2KeyDerivation.DeriveKey("hunter2", salt, keyLengthBytes: 16);

        Assert.Equal(16, key.Length);
    }
}
