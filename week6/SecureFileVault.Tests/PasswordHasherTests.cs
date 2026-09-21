using SecureFileVault.Core.Auth;

namespace SecureFileVault.Tests;

public class PasswordHasherTests
{
    [Fact]
    public void Verify_CorrectPassword_ReturnsTrue()
    {
        var stored = PasswordHasher.Hash("correct-horse-battery-staple");

        Assert.True(PasswordHasher.Verify("correct-horse-battery-staple", stored));
    }

    [Fact]
    public void Verify_WrongPassword_ReturnsFalse()
    {
        var stored = PasswordHasher.Hash("correct-horse-battery-staple");

        Assert.False(PasswordHasher.Verify("wrong-password", stored));
    }

    [Fact]
    public void Hash_SamePasswordTwice_ProducesDifferentStoredValues()
    {
        // Different random salt each call -> different stored string, even
        // for the same password (this is what stops two users with the
        // same password from having identical stored hashes).
        var storedA = PasswordHasher.Hash("same-password");
        var storedB = PasswordHasher.Hash("same-password");

        Assert.NotEqual(storedA, storedB);
        Assert.True(PasswordHasher.Verify("same-password", storedA));
        Assert.True(PasswordHasher.Verify("same-password", storedB));
    }

    [Fact]
    public void Verify_MalformedStoredValue_ReturnsFalse()
    {
        Assert.False(PasswordHasher.Verify("anything", "not-a-valid-stored-hash"));
    }
}
