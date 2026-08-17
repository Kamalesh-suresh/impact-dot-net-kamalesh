namespace StudentManagement.App.Services;

// Task 4.4: a clear, explicit success/failure signal the caller can inspect.
// WHY a result object (instead of throwing, or returning a bare bool)?
//  - A bool alone can't explain *why* something failed.
//  - Exceptions are for the unexpected; a duplicate roll number is an expected,
//    recoverable outcome, so we model it as data, not as a thrown error.
public class OperationResult
{
    public bool Success { get; }
    public string Message { get; }

    protected OperationResult(bool success, string message)
    {
        Success = success;
        Message = message;
    }

    public static OperationResult Ok(string message = "Success") => new(true, message);
    public static OperationResult Fail(string message) => new(false, message);
}
