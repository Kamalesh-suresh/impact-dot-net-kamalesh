public class EmployeeSortingTests
{
    private static List<Employee> TenEmployees() => new()
    {
        new("Meera", 95000),
        new("Ravi", 62000),
        new("Asha", 78000),
        new("Karthik", 54000),
        new("Divya", 110000),
        new("Naveen", 71000),
        new("Priya", 88000),
        new("Suresh", 47000),
        new("Anjali", 130000),
        new("Vikram", 66000),
    };

    [Fact]
    public void Sort_WithNoComparer_OrdersBySalaryAscending()
    {
        var employees = TenEmployees();
        employees.Sort();

        Assert.Equal(employees.OrderBy(e => e.Salary).Select(e => e.Name), employees.Select(e => e.Name));
        Assert.True(employees.Zip(employees.Skip(1), (a, b) => a.Salary <= b.Salary).All(ok => ok));
    }

    [Fact]
    public void Sort_WithEmployeeNameComparer_OrdersByNameOrdinal()
    {
        var employees = TenEmployees();
        employees.Sort(new EmployeeNameComparer());

        var expectedNames = TenEmployees().Select(e => e.Name).OrderBy(n => n, StringComparer.Ordinal);
        Assert.Equal(expectedNames, employees.Select(e => e.Name));
    }

    [Fact]
    public void ToString_IncludesNameAndSalary()
    {
        var employee = new Employee("Asha", 78000);
        var text = employee.ToString();

        Assert.Contains("Asha", text);
    }

    [Fact]
    public void CompareTo_NullOther_ReturnsPositive()
    {
        var employee = new Employee("Asha", 78000);
        Assert.True(employee.CompareTo(null) > 0);
    }

    [Fact]
    public void Comparer_Compare_HandlesNulls()
    {
        var comparer = new EmployeeNameComparer();
        var employee = new Employee("Asha", 78000);

        Assert.Equal(0, comparer.Compare(null, null));
        Assert.True(comparer.Compare(null, employee) < 0);
        Assert.True(comparer.Compare(employee, null) > 0);
    }
}
