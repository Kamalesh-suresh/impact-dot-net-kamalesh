// ===================================================================
// Task 3.14 — IComparable<T> (natural/default order) vs IComparer<T>
// (an alternate, pluggable order).
// ===================================================================

public class Employee : IComparable<Employee>
{
    public string Name { get; }
    public decimal Salary { get; }

    public Employee(string name, decimal salary)
    {
        Name = name;
        Salary = salary;
    }

    // The "natural" order for an Employee: List<T>.Sort() and Array.Sort()
    // use this automatically when no comparer is supplied.
    public int CompareTo(Employee? other)
    {
        if (other is null) return 1;
        return Salary.CompareTo(other.Salary);
    }

    public override string ToString() => $"{Name,-10} {Salary:C0}";
}

// An alternate order, kept separate from Employee itself: sorting by
// name doesn't belong on the class (it's not "the" natural order), so
// it lives in its own IComparer<T> that callers opt into explicitly.
public class EmployeeNameComparer : IComparer<Employee>
{
    public int Compare(Employee? x, Employee? y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (x is null) return -1;
        if (y is null) return 1;
        return string.Compare(x.Name, y.Name, StringComparison.Ordinal);
    }
}
