using StudentManagement.App.Models;

namespace StudentManagement.App.Services;

// The Service is the ONLY place business rules live. This is the primary unit
// we test (Task 4.3 / testing focus).
public interface IStudentService
{
    OperationResult AddStudent(Student student);
    IEnumerable<Student> GetAll();
    Student? GetById(int id);
    OperationResult UpdateStudent(Student student);
    OperationResult DeleteStudent(int id);
}
