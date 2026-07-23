public partial class Employee
{
    public string Name { get; set; }
    partial void OnNameChanged();

    public Employee(string name)
    {
        Name = name;
        OnNameChanged();
    }

}