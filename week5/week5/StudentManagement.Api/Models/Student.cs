namespace StudentManagement.Api.Models;

// The ENTITY — this is the internal, storage-shaped truth about a student.
// Note InternalAuditNote below: Task 5.7 requires an internal-only field that
// must NEVER appear in an API response. This is that field.
public class Student : IEntity
{
    private string _name = string.Empty;
    private int _age;
    private string _rollNumber = string.Empty;
    private string _email = string.Empty;

    public int Id { get; set; }

    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Name cannot be empty.");
            _name = value.Trim();
        }
    }

    public int Age
    {
        get => _age;
        set
        {
            if (value < 5 || value > 100)
                throw new ArgumentOutOfRangeException(nameof(Age), "Age must be between 5 and 100.");
            _age = value;
        }
    }

    public string RollNumber
    {
        get => _rollNumber;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Roll number cannot be empty.");
            _rollNumber = value.Trim();
        }
    }

    public string Email
    {
        get => _email;
        set
        {
            if (string.IsNullOrWhiteSpace(value) || !value.Contains('@'))
                throw new ArgumentException("Email must be a valid address.");
            _email = value.Trim();
        }
    }

    // INTERNAL-ONLY (Task 5.7). Populated by the system, never by a client,
    // and — this is the point of the task — never mapped into StudentReadDto.
    public string InternalAuditNote { get; set; } = string.Empty;
}
