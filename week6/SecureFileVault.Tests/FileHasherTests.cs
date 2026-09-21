using SecureFileVault.Core.Hashing;

namespace SecureFileVault.Tests;

public class FileHasherTests : IDisposable
{
    private readonly string _path = Path.Combine(Path.GetTempPath(), "sfv-hash-" + Guid.NewGuid() + ".txt");

    public FileHasherTests()
    {
        File.WriteAllText(_path, "content to hash");
    }

    [Fact]
    public void Sha256OfFile_IsStableAcrossCalls()
    {
        Assert.Equal(FileHasher.Sha256OfFile(_path), FileHasher.Sha256OfFile(_path));
    }

    [Fact]
    public void Sha512OfFile_DiffersFromSha256()
    {
        var sha256 = FileHasher.Sha256OfFile(_path);
        var sha512 = FileHasher.Sha512OfFile(_path);

        Assert.NotEqual(sha256.Length, sha512.Length);
    }

    [Fact]
    public void HmacSha256OfFile_DifferentKeys_ProduceDifferentTags()
    {
        var keyA = new byte[32];
        var keyB = new byte[32];
        keyB[0] = 1; // ensure the keys differ

        var hmacA = FileHasher.HmacSha256OfFile(_path, keyA);
        var hmacB = FileHasher.HmacSha256OfFile(_path, keyB);

        Assert.NotEqual(hmacA, hmacB);
    }

    [Fact]
    public void ToHex_ProducesLowercaseHexString()
    {
        var hex = FileHasher.ToHex([0xAB, 0xCD]);
        Assert.Equal("abcd", hex);
    }

    public void Dispose()
    {
        if (File.Exists(_path))
        {
            File.Delete(_path);
        }
    }
}
