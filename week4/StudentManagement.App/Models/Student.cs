namespace StudentManagement.App.Models;

// MODEL = DATA ONLY.
// The only logic allowed here is "property validation" — invariants that must
// ALWAYS be true for a Student to be a valid Student (e.g. age range).
// Business rules that depend on OTHER data (like "no duplicate roll numbers")
// do NOT live here — those belong to the Service layer.
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

    // Age invariant: must be between 5 and 100 (inclusive).
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

    public override string ToString() =>
        $"{Id,-3} {Name,-18} {Age,-4} {RollNumber,-10} {Email}";
}
