namespace SecureFileVault.Core.FileHandling;

// Task 6.1: FileStream I/O without ReadAllBytes. Reads the source in fixed-size
// chunks and writes each chunk immediately, so memory use stays O(bufferSize)
// regardless of file size — the same shape every streaming cipher in this
// project (CryptoStreamFileCipher, ChunkedGcmVaultService) reuses.
public static class ChunkedFileCopier
{
    public const int DefaultBufferSize = 4096;

    public static void Copy(string sourcePath, string destinationPath, int bufferSize = DefaultBufferSize)
    {
        using var source = new FileStream(sourcePath, FileMode.Open, FileAccess.Read);
        using var destination = new FileStream(destinationPath, FileMode.Create, FileAccess.Write);

        var buffer = new byte[bufferSize];
        int bytesRead;
        while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
        {
            destination.Write(buffer, 0, bytesRead);
        }
    }

    public static void Append(string path, string text)
    {
        using var stream = new FileStream(path, FileMode.Append, FileAccess.Write);
        using var writer = new StreamWriter(stream);
        writer.Write(text);
    }

    public static string ReadAll(string path)
    {
        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    // Generates a file of pseudo-random bytes without ever holding the whole
    // thing in memory — used to produce the 100 MB+ demo file for Task 6.12.
    public static void GenerateRandomFile(string path, long sizeInBytes, int bufferSize = 1024 * 1024)
    {
        using var destination = new FileStream(path, FileMode.Create, FileAccess.Write);
        var buffer = new byte[bufferSize];
        var rng = Random.Shared;

        long remaining = sizeInBytes;
        while (remaining > 0)
        {
            int toWrite = (int)Math.Min(bufferSize, remaining);
            rng.NextBytes(buffer.AsSpan(0, toWrite));
            destination.Write(buffer, 0, toWrite);
            remaining -= toWrite;
        }
    }
}
