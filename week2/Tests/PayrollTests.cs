using System.Collections.Generic;
using Xunit;

public class PayrollTests
{
    [Fact]
    public void FullTimeEmployee_CalculateSalary_ReturnsMonthlyBase()
    {
        var e = new FullTimeEmployee { Name = "Alice", Department = "Eng", MonthlyBase = 5000m };
        Assert.Equal(5000m, e.CalculateSalary());
    }

    [Fact]
    public void PartTimeEmployee_CalculateSalary_ReturnsRateTimesHours()
    {
        var e = new PartTimeEmployee { Name = "Bob", Department = "Eng", HourlyRate = 20m, Hours = 100 };
        Assert.Equal(2000m, e.CalculateSalary());
    }

    [Fact]
    public void ContractEmployee_CalculateSalary_ReturnsContractAmount()
    {
        var e = new ContractEmployee { Name = "Cara", Department = "Design", ContractAmount = 3000m };
        Assert.Equal(3000m, e.CalculateSalary());
    }

    [Fact]
    public void Total_SumsAcrossAllEmployeeTypes_ViaPolymorphism()
    {
        var staff = new List<Employee>
        {
            new FullTimeEmployee { Name = "Alice", Department = "Eng", MonthlyBase = 5000m },
            new PartTimeEmployee { Name = "Bob", Department = "Eng", HourlyRate = 20m, Hours = 100 },
            new ContractEmployee { Name = "Cara", Department = "Design", ContractAmount = 3000m },
        };

        Assert.Equal(10000m, Payroll.Total(staff));
    }

    [Fact]
    public void Total_EmptyStaff_ReturnsZero()
    {
        Assert.Equal(0m, Payroll.Total(new List<Employee>()));
    }

    [Fact]
    public void TotalTax_OnlyTaxesFullTimeEmployees()
    {
        var staff = new List<Employee>
        {
            new FullTimeEmployee { Name = "Alice", Department = "Eng", MonthlyBase = 5000m }, // tax = 1000
            new PartTimeEmployee { Name = "Bob", Department = "Eng", HourlyRate = 20m, Hours = 100 },
            new ContractEmployee { Name = "Cara", Department = "Design", ContractAmount = 3000m },
        };

        // only the FullTimeEmployee is ITaxable — PartTime/Contract must contribute nothing
        Assert.Equal(1000m, Payroll.TotalTax(staff));
    }

    [Fact]
    public void TotalTax_NoFullTimeEmployees_ReturnsZero()
    {
        var staff = new List<Employee>
        {
            new PartTimeEmployee { Name = "Bob", Department = "Eng", HourlyRate = 20m, Hours = 100 },
            new ContractEmployee { Name = "Cara", Department = "Design", ContractAmount = 3000m },
        };

        Assert.Equal(0m, Payroll.TotalTax(staff));
    }

    [Fact]
    public void ByDepartment_GroupsCountsAndTotalsCorrectly_SortedByDepartment()
    {
        var staff = new List<Employee>
        {
            new FullTimeEmployee { Name = "Alice", Department = "Eng", MonthlyBase = 5000m },
            new PartTimeEmployee { Name = "Bob", Department = "Eng", HourlyRate = 20m, Hours = 100 },
            new ContractEmployee { Name = "Cara", Department = "Design", ContractAmount = 3000m },
        };

        var result = Payroll.ByDepartment(staff);

        Assert.Equal(2, result.Count);
        Assert.Equal("Design", result[0].Department);   // alphabetical order
        Assert.Equal(1, result[0].Count);
        Assert.Equal(3000m, result[0].Total);
        Assert.Equal("Eng", result[1].Department);
        Assert.Equal(2, result[1].Count);
        Assert.Equal(7000m, result[1].Total);
    }

    [Fact]
    public void ByDepartment_EmptyStaff_ReturnsEmptyList()
    {
        Assert.Empty(Payroll.ByDepartment(new List<Employee>()));
    }
}
