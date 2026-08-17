namespace StudentManagement.App.Logging;

// Records every mutating operation (add/update/delete) so we have an audit trail.
public interface ITransactionLog
{
    void Record(string entry);
    IReadOnlyList<string> GetHistory();
}
