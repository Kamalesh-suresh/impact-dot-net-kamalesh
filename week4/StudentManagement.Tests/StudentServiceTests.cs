using Moq;
using StudentManagement.App.Data;
using StudentManagement.App.Logging;
using StudentManagement.App.Models;
using StudentManagement.App.Services;
using Xunit;

namespace StudentManagement.Tests;

// We test the SERVICE in isolation by MOCKING its dependencies (IRepository and
// ITransactionLog) with Moq. That way a test failure can only mean the service's
// own logic is wrong - never the storage.
public class StudentServiceTests
{
    private static StudentService BuildService(
        out Mock<IRepository<Student>> repo,
        out Mock<ITransactionLog> log,
        List<Student>? existing = null)
    {
        existing ??= new List<Student>();
        repo = new Mock<IRepository<Student>>();
        log = new Mock<ITransactionLog>();
        repo.Setup(r => r.GetAll()).Returns(existing);
        return new StudentService(repo.Object, log.Object);
    }

    [Fact]
    public void AddStudent_ValidStudent_Succeeds()
    {
        var service = BuildService(out var repo, out var log);
        var result = service.AddStudent(new Student { Name = "Ravi", Age = 20, RollNumber = "R010", Email = "r@x.com" });

        Assert.True(result.Success);
        repo.Verify(r => r.Add(It.IsAny<Student>()), Times.Once);
        log.Verify(l => l.Record(It.IsAny<string>()), Times.Once); // log records the mutation
    }

    [Fact]
    public void AddStudent_DuplicateRollNumber_IsRejected()
    {
        var existing = new List<Student> { new() { Id = 1, Name = "A", Age = 20, RollNumber = "R001", Email = "a@x.com" } };
        var service = BuildService(out var repo, out _, existing);

        var result = service.AddStudent(new Student { Name = "B", Age = 21, RollNumber = "R001", Email = "b@x.com" });

        Assert.False(result.Success);
        Assert.Contains("already exists", result.Message);
        repo.Verify(r => r.Add(It.IsAny<Student>()), Times.Never); // never stored
    }

    [Fact]
    public void AddStudent_InvalidAge_IsRejectedByModel()
    {
        // The model guards the 5-100 invariant, so building the Student throws.
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Student { Name = "C", Age = 3, RollNumber = "R011", Email = "c@x.com" });
    }

    [Fact]
    public void AddStudent_EmptyName_IsRejectedByModel()
    {
        Assert.Throws<ArgumentException>(() =>
            new Student { Name = "", Age = 20, RollNumber = "R012", Email = "d@x.com" });
    }

    [Fact]
    public void UpdateStudent_MissingId_FailsCleanly()
    {
        var service = BuildService(out var repo, out _);
        repo.Setup(r => r.GetById(99)).Returns((Student?)null);

        var result = service.UpdateStudent(new Student { Id = 99, Name = "X", Age = 20, RollNumber = "R099", Email = "x@x.com" });

        Assert.False(result.Success);
        repo.Verify(r => r.Update(It.IsAny<Student>()), Times.Never);
    }

    [Fact]
    public void DeleteStudent_MissingId_FailsCleanly()
    {
        var service = BuildService(out var repo, out _);
        repo.Setup(r => r.Delete(99)).Returns(false);

        var result = service.DeleteStudent(99);

        Assert.False(result.Success);
    }

    [Fact]
    public void AddStudent_RecordsTransaction()
    {
        var service = BuildService(out _, out var log);
        service.AddStudent(new Student { Name = "Ravi", Age = 20, RollNumber = "R013", Email = "r@x.com" });
        log.Verify(l => l.Record(It.Is<string>(s => s.Contains("ADD"))), Times.Once);
    }
}
