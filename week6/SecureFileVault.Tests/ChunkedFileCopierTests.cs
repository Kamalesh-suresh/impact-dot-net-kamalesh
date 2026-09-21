using SecureFileVault.Core.FileHandling;

namespace SecureFileVault.Tests;

public class ChunkedFileCopierTests : IDisposable
{
    private readonly string _dir = Directory.CreateDirectory(
        Path.Combine(Path.GetTempPath(), "sfv-copy-" + Guid.NewGuid())).FullName;

    [Fact]
    public void Copy_ProducesByteIdenticalFile_UsingSmallBuffer()
    {
        var source = Path.Combine(_dir, "source.bin");
        var destination = Path.Combine(_dir, "destination.bin");
        var bytes = new byte[10_000];
        Random.Shared.NextBytes(bytes);
        File.WriteAllBytes(source, bytes);

        // A buffer far smaller than the file forces several read/write
        // cycles, exercising the chunking loop rather than a single pass.
        ChunkedFileCopier.Copy(source, destination, bufferSize: 512);

        Assert.Equal(bytes, File.ReadAllBytes(destination));
    }

    [Fact]
    public void Append_AddsTextToEndOfExistingFile()
    {
        var path = Path.Combine(_dir, "appendable.txt");
        File.WriteAllText(path, "line one\n");

        ChunkedFileCopier.Append(path, "line two\n");

        Assert.Equal("line one\nline two\n", ChunkedFileCopier.ReadAll(path));
    }

    [Fact]
    public void GenerateRandomFile_ProducesExactRequestedSize()
    {
        var path = Path.Combine(_dir, "random.bin");

        ChunkedFileCopier.GenerateRandomFile(path, sizeInBytes: 12_345, bufferSize: 4096);

        Assert.Equal(12_345, new FileInfo(path).Length);
    }

    public void Dispose()
    {
        if (Directory.Exists(_dir))
        {
            Directory.Delete(_dir, recursive: true);
        }
    }
}
