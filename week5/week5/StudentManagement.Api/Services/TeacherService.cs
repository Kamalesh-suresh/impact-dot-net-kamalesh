using StudentManagement.Api.Data;
using StudentManagement.Api.Models;

namespace StudentManagement.Api.Services;

public class TeacherService : ITeacherService
{
    private readonly IRepository<Teacher> _repository;

    public TeacherService(IRepository<Teacher> repository)
    {
        _repository = repository;
    }

    public OperationResult<Teacher> AddTeacher(Teacher teacher)
    {
        _repository.Add(teacher);
        return OperationResult<Teacher>.Ok(teacher, "Teacher added.");
    }

    public IEnumerable<Teacher> GetAll() => _repository.GetAll();

    public Teacher? GetById(int id) => _repository.GetById(id);

    public OperationResult<Teacher> UpdateTeacher(Teacher teacher)
    {
        var existing = _repository.GetById(teacher.Id);
        if (existing is null)
            return OperationResult<Teacher>.Fail($"No teacher found with Id {teacher.Id}.");
        _repository.Update(teacher);
        return OperationResult<Teacher>.Ok(teacher, "Teacher updated.");
    }

    public bool DeleteTeacher(int id) => _repository.Delete(id);
}
