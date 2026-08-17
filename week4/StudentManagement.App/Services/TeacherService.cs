using StudentManagement.App.Data;
using StudentManagement.App.Logging;
using StudentManagement.App.Models;

namespace StudentManagement.App.Services;

public class TeacherService : ITeacherService
{
    private readonly IRepository<Teacher> _repository;
    private readonly ITransactionLog _log;

    public TeacherService(IRepository<Teacher> repository, ITransactionLog log)
    {
        _repository = repository;
        _log = log;
    }

    public OperationResult AddTeacher(Teacher teacher)
    {
        if (string.IsNullOrWhiteSpace(teacher.Name))
            return OperationResult.Fail("Name cannot be empty.");

        _repository.Add(teacher);
        _log.Record($"ADD    Teacher #{teacher.Id} ({teacher.Name})");
        return OperationResult.Ok($"Teacher '{teacher.Name}' added with Id {teacher.Id}.");
    }

    public IEnumerable<Teacher> GetAll() => _repository.GetAll();
    public Teacher? GetById(int id) => _repository.GetById(id);

    public OperationResult DeleteTeacher(int id)
    {
        var removed = _repository.Delete(id);
        if (!removed) return OperationResult.Fail($"No teacher found with Id {id}.");
        _log.Record($"DELETE Teacher #{id}");
        return OperationResult.Ok($"Teacher {id} deleted.");
    }
}
