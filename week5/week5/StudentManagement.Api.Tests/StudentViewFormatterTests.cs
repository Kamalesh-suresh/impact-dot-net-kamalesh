using StudentManagement.Api.Formatters;
using StudentManagement.Api.Models;
using Xunit;

namespace StudentManagement.Api.Tests;

// Task 5.8's Testing Focus: each strategy's OUTPUT, plus the FACTORY's
// SELECTION logic, tested independently.
public class StudentViewFormatterTests
{
    private static readonly Student Sample = new()
    {
        Id = 1, Name = "Anita Rao", Age = 20, RollNumber = "R001", Email = "anita@x.com"
    };

    [Fact]
    public void DetailedFormatter_IncludesEveryField()
    {
        var dto = new DetailedStudentViewFormatter().Format(Sample);
        Assert.Equal("R001", dto.RollNumber);
        Assert.Equal("anita@x.com", dto.Email);
    }

    [Fact]
    public void MinimalFormatter_OmitsRollNumberAndEmail()
    {
        var dto = new MinimalStudentViewFormatter().Format(Sample);
        Assert.Equal(string.Empty, dto.RollNumber);
        Assert.Equal(string.Empty, dto.Email);
        Assert.Equal("Anita Rao", dto.Name); // still present
    }

    [Theory]
    [InlineData("minimal", typeof(MinimalStudentViewFormatter))]
    [InlineData("MINIMAL", typeof(MinimalStudentViewFormatter))] // case-insensitive
    [InlineData("detailed", typeof(DetailedStudentViewFormatter))]
    [InlineData(null, typeof(DetailedStudentViewFormatter))]      // default
    [InlineData("garbage", typeof(DetailedStudentViewFormatter))] // unknown -> default
    public void Factory_SelectsCorrectStrategy(string? view, Type expected)
    {
        var formatter = StudentViewFormatterFactory.Create(view);
        Assert.IsType(expected, formatter);
    }
}
