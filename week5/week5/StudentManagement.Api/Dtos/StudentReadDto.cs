namespace StudentManagement.Api.Dtos;

// The OUTPUT shape (Task 5.7). Deliberately does NOT include
// Student.InternalAuditNote — that omission IS the test.
public class StudentReadDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string RollNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
