public class TempFileManager : IDisposable
{
    public string FilePath { get; }
    private bool disposed = false;

    public TempFileManager()
    {
        FilePath = Path.Combine(Path.GetTempPath(), $"temp_{Guid.NewGuid()}.txt");
        File.WriteAllText(FilePath, "temp content");
        Console.WriteLine($"Created: {FilePath}");
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposed) return;

        if (File.Exists(FilePath))
        {
            File.Delete(FilePath);
            Console.WriteLine($"Deleted: {FilePath}");
        }
        disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this); // cleanup already done, skip the finalizer below
    }

    ~TempFileManager()
    {
        Dispose(false); // safety net if someone forgets `using` or Dispose()
    }
}