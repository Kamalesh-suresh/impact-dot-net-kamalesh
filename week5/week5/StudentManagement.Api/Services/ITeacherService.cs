using StudentManagement.Api.Models;

namespace StudentManagement.Api.Services;

public interface ITeacherService
{
    OperationResult<Teacher> AddTeacher(Teacher teacher);
    IEnumerable<Teacher> GetAll();
    Teacher? GetById(int id);
    OperationResult<Teacher> UpdateTeacher(Teacher teacher);
    bool DeleteTeacher(int id);
}
