using Moq;
using StudentManagement.Api.Data;
using StudentManagement.Api.Models;
using StudentManagement.Api.Services;
using Xunit;

namespace StudentManagement.Api.Tests;

// Same shape as Week 4's service tests — the repository is mocked so a
// failing test can only mean the service's own logic is wrong.
public class StudentServiceTests
{
    private static StudentService BuildService(out Mock<IRepository<Student>> repo, List<Student>? existing = null)
    {
        existing ??= new List<Student>();
        repo = new Mock<IRepository<Student>>();
        repo.Setup(r => r.GetAll()).Returns(existing);
        return new StudentService(repo.Object);
    }

    [Fact]
    public void AddStudent_ValidStudent_Succeeds()
    {
        var service = BuildService(out var repo);
        var result = service.AddStudent(new Student { Name = "Ravi", Age = 20, RollNumber = "R010", Email = "r@x.com" });

        Assert.True(result.Success);
        repo.Verify(r => r.Add(It.IsAny<Student>()), Times.Once);
    }

    [Fact]
    public void AddStudent_DuplicateRollNumber_IsRejected()
    {
        var existing = new List<Student> { new() { Id = 1, Name = "A", Age = 20, RollNumber = "R001", Email = "a@x.com" } };
        var service = BuildService(out var repo, existing);

        var result = service.AddStudent(new Student { Name = "B", Age = 21, RollNumber = "R001", Email = "b@x.com" });

        Assert.False(result.Success);
        repo.Verify(r => r.Add(It.IsAny<Student>()), Times.Never);
    }

    [Fact]
    public void UpdateStudent_MissingId_Fails()
    {
        var service = BuildService(out var repo);
        repo.Setup(r => r.GetById(99)).Returns((Student?)null);

        var result = service.UpdateStudent(new Student { Id = 99, Name = "X", Age = 20, RollNumber = "R099", Email = "x@x.com" });

        Assert.False(result.Success);
    }

    [Fact]
    public void DeleteStudent_MissingId_ReturnsFalse()
    {
        var service = BuildService(out var repo);
        repo.Setup(r => r.Delete(99)).Returns(false);

        Assert.False(service.DeleteStudent(99));
    }

    [Fact]
    public void SearchByName_CaseInsensitiveMatch_ReturnsHit()
    {
        var existing = new List<Student> { new() { Id = 1, Name = "Anita Rao", Age = 20, RollNumber = "R001", Email = "a@x.com" } };
        var service = BuildService(out _, existing);

        var results = service.SearchByName("ANI").ToList();

        Assert.Single(results);
        Assert.Equal("Anita Rao", results[0].Name);
    }

    [Fact]
    public void SearchByName_NoMatch_ReturnsEmptyNotNull()
    {
        var existing = new List<Student> { new() { Id = 1, Name = "Anita Rao", Age = 20, RollNumber = "R001", Email = "a@x.com" } };
        var service = BuildService(out _, existing);

        var results = service.SearchByName("zzz").ToList();

        Assert.Empty(results); // hit vs miss vs empty-query, per the Testing Focus
    }

    [Fact]
    public void SearchByName_EmptyQuery_ReturnsEmpty()
    {
        var service = BuildService(out _);
        Assert.Empty(service.SearchByName(""));
    }
}
