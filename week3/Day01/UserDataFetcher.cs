public static class UserDataFetcher
{
    public static async Task<string> FetchUserDataAsync(string user)
    {
        Console.WriteLine($"Starting fetch for {user}...");
        await Task.Delay(3000);
        Console.WriteLine($"Finished fetch for {user}.");
        return $"{user}'s data";
    }
}