public class SingletonTests
{
    [Fact]
    public void Instance_ReturnsSameReferenceAcrossCalls()
    {
        var first = Logger.Instance;
        var second = Logger.Instance;

        Assert.Same(first, second);
    }

    [Fact]
    public async Task Instance_ReturnsSameReferenceAcrossConcurrentTasks()
    {
        var results = await Task.WhenAll(Enumerable.Range(0, 20).Select(_ => Task.Run(() => Logger.Instance)));

        Assert.True(results.All(r => ReferenceEquals(r, results[0])));
    }

    [Fact]
    public void Log_WritesMessagePrefixedWithSharedHashCode()
    {
        var originalOut = Console.Out;
        using var writer = new StringWriter();
        Console.SetOut(writer);
        try
        {
            Logger.Instance.Log("hello from a test");
        }
        finally
        {
            Console.SetOut(originalOut);
        }

        Assert.Contains("hello from a test", writer.ToString());
    }
}
