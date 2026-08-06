//var account = new Account(1000m);
//account.Withdraw(1500m); // triggers InsufficientFundsException, then finally logs

//var calc = new Calculator();
//calc.ParseAndDivide("abc", "2"); // FormatException path


using (var manager = new TempFileManager())
{
    Console.WriteLine($"Exists during block: {File.Exists(manager.FilePath)}"); // True
} // Dispose() runs automatically right here

// after the block: file is gone


var sw = System.Diagnostics.Stopwatch.StartNew();
await UserDataFetcher.FetchUserDataAsync("Asha");
await UserDataFetcher.FetchUserDataAsync("Ravi");
await UserDataFetcher.FetchUserDataAsync("Meera");
sw.Stop();
Console.WriteLine($"Sequential took {sw.ElapsedMilliseconds} ms"); 

sw.Restart();
var results = await Task.WhenAll(
    UserDataFetcher.FetchUserDataAsync("Asha"),
    UserDataFetcher.FetchUserDataAsync("Ravi"),
    UserDataFetcher.FetchUserDataAsync("Meera"));
sw.Stop();
Console.WriteLine($"Concurrent took {sw.ElapsedMilliseconds} ms"); 


Console.ReadKey();


public class Calculator
{
    public void ParseAndDivide(string numerator, string denominator)
    {
        try
        {
            int n = int.Parse(numerator);
            int d = int.Parse(denominator);
            int result = checked(n / d);
            Console.WriteLine(result);
        }
        catch (FormatException)
        {
            Console.WriteLine("One of the inputs wasn't a valid number.");
        }
        catch (OverflowException)
        {
            Console.WriteLine("The result overflowed.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

