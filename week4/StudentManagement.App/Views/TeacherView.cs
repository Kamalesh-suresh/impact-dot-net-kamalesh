using StudentManagement.App.Models;

namespace StudentManagement.App.Views;

public class TeacherView
{
    public void ShowMenu()
    {
        Console.WriteLine();
        Console.WriteLine("===== TEACHER MENU =====");
        Console.WriteLine("1) List teachers");
        Console.WriteLine("2) Add teacher");
        Console.WriteLine("3) Delete teacher");
        Console.WriteLine("6) Switch to Students");
        Console.WriteLine("0) Exit");
        Console.Write("Choose: ");
    }

    public void PrintTeachers(IEnumerable<Teacher> teachers)
    {
        Console.WriteLine();
        Console.WriteLine($"{"Id",-3} {"Name",-18} {"Designation",-16} {"Email"}");
        Console.WriteLine(new string('-', 60));
        var any = false;
        foreach (var t in teachers) { Console.WriteLine(t.ToString()); any = true; }
        if (!any) Console.WriteLine("(no teachers yet)");
    }

    public Teacher PromptForTeacher()
    {
        Console.Write("Name: ");
        var name = Console.ReadLine() ?? "";
        Console.Write("Designation: ");
        var desig = Console.ReadLine() ?? "";
        Console.Write("Email: ");
        var email = Console.ReadLine() ?? "";
        return new Teacher { Name = name, Designation = desig, Email = email };
    }

    public int PromptForId(string action)
    {
        Console.Write($"Id to {action}: ");
        return int.TryParse(Console.ReadLine(), out var id) ? id : -1;
    }

    public void ShowMessage(string message) => Console.WriteLine($">> {message}");
}
