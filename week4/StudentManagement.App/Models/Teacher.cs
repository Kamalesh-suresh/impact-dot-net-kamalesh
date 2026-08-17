namespace StudentManagement.App.Models;

// Second entity for the Task 4.10 end-to-end slice.
public class Teacher : IEntity
{
    private string _name = string.Empty;
    private string _email = string.Empty;
    private string _designation = string.Empty;

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

    public string Designation
    {
        get => _designation;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Designation cannot be empty.");
            _designation = value.Trim();
        }
    }

    public override string ToString() =>
        $"{Id,-3} {Name,-18} {Designation,-16} {Email}";
}
