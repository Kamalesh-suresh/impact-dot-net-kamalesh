using StudentManagement.Api.Data;
using StudentManagement.Api.Models;

namespace StudentManagement.Api.Services;

// Identical business rules to Week 4's StudentService — the point of Task 5.3
// is proving this logic is front-end-agnostic. A console app and a Web API
// controller can both drive the exact same service unchanged.
public class StudentService : IStudentService
{
    private readonly IRepository<Student> _repository;

    public StudentService(IRepository<Student> repository)
    {
        _repository = repository;
    }

    public OperationResult<Student> AddStudent(Student student)
    {
        var duplicate = _repository.GetAll()
            .Any(s => s.RollNumber.Equals(student.RollNumber, StringComparison.OrdinalIgnoreCase));
        if (duplicate)
            return OperationResult<Student>.Fail($"Roll number '{student.RollNumber}' already exists.");

        _repository.Add(student);
        return OperationResult<Student>.Ok(student, "Student added.");
    }

    public IEnumerable<Student> GetAll() => _repository.GetAll();

    public Student? GetById(int id) => _repository.GetById(id);

    public OperationResult<Student> UpdateStudent(Student student)
    {
        var existing = _repository.GetById(student.Id);
        if (existing is null)
            return OperationResult<Student>.Fail($"No student found with Id {student.Id}.");

        var clash = _repository.GetAll().Any(s =>
            s.Id != student.Id &&
            s.RollNumber.Equals(student.RollNumber, StringComparison.OrdinalIgnoreCase));
        if (clash)
            return OperationResult<Student>.Fail($"Roll number '{student.RollNumber}' already exists.");

        _repository.Update(student);
        return OperationResult<Student>.Ok(student, "Student updated.");
    }

    public bool DeleteStudent(int id) => _repository.Delete(id);

    // Task 5.9: case-insensitive search via LINQ, in the service (not the controller).
    public IEnumerable<Student> SearchByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Enumerable.Empty<Student>();

        return _repository.GetAll()
            .Where(s => s.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
    }
}
