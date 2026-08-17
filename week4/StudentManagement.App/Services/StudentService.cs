using StudentManagement.App.Data;
using StudentManagement.App.Logging;
using StudentManagement.App.Models;

namespace StudentManagement.App.Services;

// All orchestration + rules for students. Depends only on ABSTRACTIONS
// (IRepository, ITransactionLog) so it is fully unit-testable with mocks.
public class StudentService : IStudentService
{
    private readonly IRepository<Student> _repository;
    private readonly ITransactionLog _log;

    public StudentService(IRepository<Student> repository, ITransactionLog log)
    {
        _repository = repository;
        _log = log;
    }

    public OperationResult AddStudent(Student student)
    {
        // Rule 1: name must not be empty (the model also guards this).
        if (string.IsNullOrWhiteSpace(student.Name))
            return OperationResult.Fail("Name cannot be empty.");

        // Rule 2: age must be valid. The model enforces 5-100 on assignment,
        // so by the time we get a Student here it's already valid — but we keep
        // a defensive check to show where a service-level rule would go.
        if (student.Age < 5 || student.Age > 100)
            return OperationResult.Fail("Age must be between 5 and 100.");

        // Rule 3: roll numbers must be unique across ALL students.
        // This needs to look at other records, so it can only live in the service.
        var duplicate = _repository.GetAll()
            .Any(s => s.RollNumber.Equals(student.RollNumber, StringComparison.OrdinalIgnoreCase));
        if (duplicate)
            return OperationResult.Fail($"Roll number '{student.RollNumber}' already exists.");

        _repository.Add(student);
        _log.Record($"ADD    Student #{student.Id} ({student.Name}, roll {student.RollNumber})");
        return OperationResult.Ok($"Student '{student.Name}' added with Id {student.Id}.");
    }

    public IEnumerable<Student> GetAll() => _repository.GetAll();

    public Student? GetById(int id) => _repository.GetById(id);

    public OperationResult UpdateStudent(Student student)
    {
        var existing = _repository.GetById(student.Id);
        if (existing is null)
            return OperationResult.Fail($"No student found with Id {student.Id}.");

        // Uniqueness rule again, but ignore the student's own record.
        var clash = _repository.GetAll().Any(s =>
            s.Id != student.Id &&
            s.RollNumber.Equals(student.RollNumber, StringComparison.OrdinalIgnoreCase));
        if (clash)
            return OperationResult.Fail($"Roll number '{student.RollNumber}' already exists.");

        _repository.Update(student);
        _log.Record($"UPDATE Student #{student.Id} ({student.Name})");
        return OperationResult.Ok($"Student {student.Id} updated.");
    }

    public OperationResult DeleteStudent(int id)
    {
        var removed = _repository.Delete(id);
        if (!removed)
            return OperationResult.Fail($"No student found with Id {id}.");

        _log.Record($"DELETE Student #{id}");
        return OperationResult.Ok($"Student {id} deleted.");
    }
}
