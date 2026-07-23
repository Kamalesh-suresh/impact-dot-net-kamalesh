public partial class Employee
{
    partial void OnNameChanged()
    {
        Console.WriteLine($"{Name}");
    }
}