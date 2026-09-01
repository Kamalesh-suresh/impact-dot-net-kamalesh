using StudentManagement.Api.Dtos;
using StudentManagement.Api.Models;

namespace StudentManagement.Api.Formatters;

// Strategy 1: every public field. The default view.
public class DetailedStudentViewFormatter : IStudentViewFormatter
{
    public StudentReadDto Format(Student student) => new()
    {
        Id = student.Id,
        Name = student.Name,
        Age = student.Age,
        RollNumber = student.RollNumber,
        Email = student.Email
    };
}
