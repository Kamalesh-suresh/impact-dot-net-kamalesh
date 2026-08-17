using StudentManagement.App.Models;
using Xunit;

namespace StudentManagement.Tests;

// Smaller, second target: the model's own validation invariants.
public class StudentModelTests
{
    [Theory]
    [InlineData(5)]
    [InlineData(50)]
    [InlineData(100)]
    public void Age_WithinRange_IsAccepted(int age)
    {
        var s = new Student { Name = "OK", Age = age, RollNumber = "R1", Email = "o@x.com" };
        Assert.Equal(age, s.Age);
    }

    [Theory]
    [InlineData(4)]
    [InlineData(0)]
    [InlineData(101)]
    public void Age_OutOfRange_Throws(int age)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Student { Name = "Bad", Age = age, RollNumber = "R1", Email = "o@x.com" });
    }

    [Fact]
    public void Email_WithoutAtSign_Throws()
    {
        Assert.Throws<ArgumentException>(() =>
            new Student { Name = "Bad", Age = 20, RollNumber = "R1", Email = "not-an-email" });
    }
}
