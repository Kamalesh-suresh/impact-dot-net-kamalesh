namespace StudentManagement.Api.Formatters;

// FACTORY (Task 5.8): picks a strategy per request based on a simple string,
// e.g. from a query parameter. Adding a third view later means adding one
// case here — the controller and the two existing formatters never change.
public static class StudentViewFormatterFactory
{
    public static IStudentViewFormatter Create(string? view)
    {
        return (view?.ToLowerInvariant()) switch
        {
            "minimal" => new MinimalStudentViewFormatter(),
            _ => new DetailedStudentViewFormatter() // default
        };
    }
}
