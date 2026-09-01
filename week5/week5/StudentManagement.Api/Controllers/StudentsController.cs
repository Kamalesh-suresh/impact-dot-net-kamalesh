using Microsoft.AspNetCore.Mvc;
using StudentManagement.Api.Dtos;
using StudentManagement.Api.Formatters;
using StudentManagement.Api.Models;
using StudentManagement.Api.Services;

namespace StudentManagement.Api.Controllers;

// [ApiController] turns on automatic 400 responses for invalid model state
// (Task 5.6) and a few other API-friendly conventions (binding source
// inference, problem-details error responses).
[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _service;
    private readonly ILogger<StudentsController> _logger;

    // The controller has NO business logic (Task 5.3's Done-when) — it only
    // orchestrates: read the request, call the service, shape the response.
    public StudentsController(IStudentService service, ILogger<StudentsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    // GET api/students?view=detailed|minimal
    [HttpGet]
    public ActionResult<IEnumerable<StudentReadDto>> GetAll([FromQuery] string? view)
    {
        var formatter = StudentViewFormatterFactory.Create(view); // Strategy selected here
        var dtos = _service.GetAll().Select(formatter.Format);
        return Ok(dtos); // 200
    }

    // GET api/students/5
    [HttpGet("{id:int}")]
    public ActionResult<StudentReadDto> GetById(int id, [FromQuery] string? view)
    {
        var student = _service.GetById(id);
        if (student is null)
        {
            _logger.LogWarning("Student {Id} not found.", id);
            return NotFound(); // 404
        }

        var formatter = StudentViewFormatterFactory.Create(view);
        return Ok(formatter.Format(student)); // 200
    }

    // GET api/students/search?name=ani
    [HttpGet("search")]
    public ActionResult<IEnumerable<StudentReadDto>> Search([FromQuery] string? name)
    {
        var formatter = StudentViewFormatterFactory.Create(null);
        var matches = _service.SearchByName(name ?? string.Empty).Select(formatter.Format);
        return Ok(matches); // 200, even if the list is empty
    }

    // POST api/students
    [HttpPost]
    public ActionResult<StudentReadDto> Create([FromBody] StudentCreateDto dto)
    {
        // No manual validation here — [Required]/[Range]/[EmailAddress] on the
        // DTO plus [ApiController] already returned 400 before this method runs
        // if the body was invalid (Task 5.6).
        Student student;
        try
        {
            student = new Student
            {
                Name = dto.Name,
                Age = dto.Age,
                RollNumber = dto.RollNumber,
                Email = dto.Email,
                InternalAuditNote = $"created via API at {DateTime.UtcNow:O}" // never leaves this method
            };
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message); // 400 — model-level invariant failed
        }

        var result = _service.AddStudent(student);
        if (!result.Success)
            return BadRequest(result.Message); // 400 — business rule failed (duplicate roll)

        _logger.LogInformation("Student {Id} created with roll {Roll}.", student.Id, student.RollNumber);

        var formatter = StudentViewFormatterFactory.Create(null);
        var readDto = formatter.Format(student);

        // 201 + Location header pointing at GET api/students/{id}
        return CreatedAtAction(nameof(GetById), new { id = student.Id }, readDto);
    }

    // PUT api/students/5
    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] StudentCreateDto dto)
    {
        var existing = _service.GetById(id);
        if (existing is null)
        {
            _logger.LogWarning("Update failed — student {Id} not found.", id);
            return NotFound(); // 404
        }

        Student updated;
        try
        {
            updated = new Student
            {
                Id = id,
                Name = dto.Name,
                Age = dto.Age,
                RollNumber = dto.RollNumber,
                Email = dto.Email,
                InternalAuditNote = existing.InternalAuditNote
            };
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message); // 400
        }

        var result = _service.UpdateStudent(updated);
        if (!result.Success)
            return BadRequest(result.Message); // 400 — e.g. roll clash

        return NoContent(); // 204
    }

    // DELETE api/students/5
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var removed = _service.DeleteStudent(id);
        if (!removed)
        {
            _logger.LogWarning("Delete failed — student {Id} not found.", id);
            return NotFound(); // 404
        }
        return NoContent(); // 204
    }
}
