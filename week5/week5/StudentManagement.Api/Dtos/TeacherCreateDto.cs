using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Api.Dtos;

public class TeacherCreateDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Designation { get; set; } = string.Empty;
}
