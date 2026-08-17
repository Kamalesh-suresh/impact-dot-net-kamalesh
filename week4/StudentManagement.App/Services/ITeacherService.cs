using StudentManagement.App.Models;

namespace StudentManagement.App.Services;

public interface ITeacherService
{
    OperationResult AddTeacher(Teacher teacher);
    IEnumerable<Teacher> GetAll();
    Teacher? GetById(int id);
    OperationResult DeleteTeacher(int id);
}
