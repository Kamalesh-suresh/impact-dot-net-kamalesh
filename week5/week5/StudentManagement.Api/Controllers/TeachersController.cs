using Microsoft.AspNetCore.Mvc;
using StudentManagement.Api.Dtos;
using StudentManagement.Api.Models;
using StudentManagement.Api.Services;

namespace StudentManagement.Api.Controllers;

// Task 5.10 (stretch): a second, thin controller — same shape as Students,
// proving the pattern generalizes to a new entity without new architecture.
[ApiController]
[Route("api/[controller]")]
public class TeachersController : ControllerBase
{
    private readonly ITeacherService _service;
    private readonly ILogger<TeachersController> _logger;

    public TeachersController(ITeacherService service, ILogger<TeachersController> logger)
    {
        _service = service;
        _logger = logger;
    }

    private static TeacherReadDto ToDto(Teacher t) => new()
    {
        Id = t.Id, Name = t.Name, Email = t.Email, Designation = t.Designation
    };

    [HttpGet]
    public ActionResult<IEnumerable<TeacherReadDto>> GetAll() =>
        Ok(_service.GetAll().Select(ToDto)); // 200

    [HttpGet("{id:int}")]
    public ActionResult<TeacherReadDto> GetById(int id)
    {
        var teacher = _service.GetById(id);
        if (teacher is null) { _logger.LogWarning("Teacher {Id} not found.", id); return NotFound(); } // 404
        return Ok(ToDto(teacher)); // 200
    }

    [HttpPost]
    public ActionResult<TeacherReadDto> Create([FromBody] TeacherCreateDto dto)
    {
        Teacher teacher;
        try
        {
            teacher = new Teacher { Name = dto.Name, Email = dto.Email, Designation = dto.Designation };
        }
        catch (Exception ex) { return BadRequest(ex.Message); } // 400

        var result = _service.AddTeacher(teacher);
        _logger.LogInformation("Teacher {Id} created.", teacher.Id);
        return CreatedAtAction(nameof(GetById), new { id = teacher.Id }, ToDto(teacher)); // 201
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] TeacherCreateDto dto)
    {
        var existing = _service.GetById(id);
        if (existing is null) { _logger.LogWarning("Update failed — teacher {Id} not found.", id); return NotFound(); } // 404

        Teacher updated;
        try
        {
            updated = new Teacher { Id = id, Name = dto.Name, Email = dto.Email, Designation = dto.Designation };
        }
        catch (Exception ex) { return BadRequest(ex.Message); } // 400

        _service.UpdateTeacher(updated);
        return NoContent(); // 204
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var removed = _service.DeleteTeacher(id);
        if (!removed) { _logger.LogWarning("Delete failed — teacher {Id} not found.", id); return NotFound(); } // 404
        return NoContent(); // 204
    }
}
