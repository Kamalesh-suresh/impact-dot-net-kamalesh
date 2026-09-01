using StudentManagement.Api.Dtos;
using StudentManagement.Api.Models;

namespace StudentManagement.Api.Formatters;

// Strategy 2: a lightweight shape for list views where the client only
// needs enough to render a picker/dropdown — Email and RollNumber omitted.
public class MinimalStudentViewFormatter : IStudentViewFormatter
{
    public StudentReadDto Format(Student student) => new()
    {
        Id = student.Id,
        Name = student.Name,
        Age = student.Age
        // RollNumber and Email deliberately left at their default ("")
    };
}
