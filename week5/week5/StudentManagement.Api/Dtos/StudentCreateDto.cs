using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Api.Dtos;

// The INPUT shape (Task 5.7). This is what a client is allowed to send.
// Task 5.6: these attributes are what makes [ApiController] auto-return 400
// for bad input, with NO manual "if" check written anywhere in the controller.
public class StudentCreateDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Range(5, 100, ErrorMessage = "Age must be between 5 and 100.")]
    public int Age { get; set; }

    [Required]
    public string RollNumber { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}
