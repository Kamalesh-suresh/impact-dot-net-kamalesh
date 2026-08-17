namespace StudentManagement.App.Logging;

// ENCAPSULATION in action: the history list is PRIVATE. Nobody can reach in and
// edit or clear it. They can only append through Record() or read a safe copy.
public class TransactionLog : ITransactionLog
{
    private readonly List<string> _history = new();

    public void Record(string entry)
    {
        var stamped = $"{DateTime.Now:HH:mm:ss}  {entry}";
        _history.Add(stamped);
    }

    // Return a read-only view so callers cannot mutate our private list.
    public IReadOnlyList<string> GetHistory() => _history.AsReadOnly();
}
