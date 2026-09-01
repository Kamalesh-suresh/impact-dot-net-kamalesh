using StudentManagement.Api.Dtos;
using StudentManagement.Api.Models;

namespace StudentManagement.Api.Formatters;

// STRATEGY interface (Task 5.8). Each implementation is one way of shaping
// a Student into a StudentReadDto for the wire. The controller never knows
// or cares which strategy it's using — only the factory decides that.
public interface IStudentViewFormatter
{
    StudentReadDto Format(Student student);
}
