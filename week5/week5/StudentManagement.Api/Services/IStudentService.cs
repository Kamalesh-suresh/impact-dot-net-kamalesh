using StudentManagement.Api.Models;

namespace StudentManagement.Api.Services;

public interface IStudentService
{
    OperationResult<Student> AddStudent(Student student);
    IEnumerable<Student> GetAll();
    Student? GetById(int id);
    OperationResult<Student> UpdateStudent(Student student);
    bool DeleteStudent(int id);
    IEnumerable<Student> SearchByName(string name);
}
