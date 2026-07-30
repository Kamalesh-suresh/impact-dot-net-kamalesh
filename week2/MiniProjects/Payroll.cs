using System.Collections.Generic;
using System.Linq;
 
// ── Q4 · Employee Payroll ─────────────────────────────────────

public abstract class Employee
{
    public string Name { get; init; } = "";
    public string Department { get; init; } = "";
    public abstract decimal CalculateSalary();
}

public interface ITaxable
{
    decimal CalculateTax();
}

public class FullTimeEmployee : Employee, ITaxable   // only full-timers are taxable
{
    public decimal MonthlyBase { get; init; }
    public override decimal CalculateSalary() => MonthlyBase;
    public decimal CalculateTax() => MonthlyBase * 0.2m;
}

public class PartTimeEmployee : Employee
{
    public decimal HourlyRate { get; init; }
    public int Hours { get; init; }
    public override decimal CalculateSalary() => HourlyRate * Hours;
}

public class ContractEmployee : Employee
{
    public decimal ContractAmount { get; init; }
    public override decimal CalculateSalary() => ContractAmount;
}

// A named type so the grouping can be RETURNED and asserted on.
// (An anonymous type couldn't leave the method — Task 2.14.)
public record DepartmentTotal(string Department, int Count, decimal Total);

public static class Payroll
{
    // total payroll via polymorphism — one call, right formula per type
    public static decimal Total(IEnumerable<Employee> staff) =>
        staff.Sum(e => e.CalculateSalary());

    // only the ITaxable employees contribute tax
    public static decimal TotalTax(IEnumerable<Employee> staff) =>
        staff.OfType<ITaxable>().Sum(t => t.CalculateTax());

    // group by department: count + total, sorted for stable output
    public static IReadOnlyList<DepartmentTotal> ByDepartment(IEnumerable<Employee> staff) =>
        staff.GroupBy(e => e.Department)
             .Select(g => new DepartmentTotal(g.Key, g.Count(), g.Sum(e => e.CalculateSalary())))
             .OrderBy(d => d.Department)
             .ToList();
}