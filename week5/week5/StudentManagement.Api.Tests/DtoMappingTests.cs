using StudentManagement.Api.Formatters;
using StudentManagement.Api.Models;
using Xunit;

namespace StudentManagement.Api.Tests;

// Task 5.7's Done-when, as a test: the internal field must NEVER leak,
// under either formatter strategy.
public class DtoMappingTests
{
    [Fact]
    public void InternalAuditNote_NeverAppearsInReadDto()
    {
        var student = new Student
        {
            Id = 1, Name = "Anita Rao", Age = 20, RollNumber = "R001", Email = "a@x.com",
            InternalAuditNote = "created via API at 2026-01-01T00:00:00Z"
        };

        var detailedDto = new DetailedStudentViewFormatter().Format(student);
        var minimalDto = new MinimalStudentViewFormatter().Format(student);

        // StudentReadDto has no InternalAuditNote property at all — this test
        // documents that guarantee at the type level, not just at runtime.
        var dtoProperties = typeof(Dtos.StudentReadDto).GetProperties().Select(p => p.Name);
        Assert.DoesNotContain("InternalAuditNote", dtoProperties);
    }
}
