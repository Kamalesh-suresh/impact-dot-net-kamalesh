using StudentManagement.App.Models;

namespace StudentManagement.App.Views;

// VIEW = PRESENTATION ONLY. It renders and it reads raw input. It contains
// ZERO business "if" statements — no rules, no storage, no decisions about
// whether data is valid. Notice there is not a single business condition here.
public class StudentView
{
    public void ShowMenu()
    {
        Console.WriteLine();
        Console.WriteLine("===== STUDENT MENU =====");
        Console.WriteLine("1) List students");
        Console.WriteLine("2) Add student");
        Console.WriteLine("3) Update student");
        Console.WriteLine("4) Delete student");
        Console.WriteLine("5) View transaction log");
        Console.WriteLine("6) Switch to Teachers");
        Console.WriteLine("0) Exit");
        Console.Write("Choose: ");
    }

    public void PrintStudents(IEnumerable<Student> students)
    {
        Console.WriteLine();
        Console.WriteLine($"{"Id",-3} {"Name",-18} {"Age",-4} {"Roll",-10} {"Email"}");
        Console.WriteLine(new string('-', 60));
        var any = false;
        foreach (var s in students)
        {
            Console.WriteLine(s.ToString());
            any = true;
        }
        if (!any) Console.WriteLine("(no students yet)");
    }

    // Collects raw input and builds a Student. Any invalid value throws from the
    // model; the controller decides what to do with that — the view does not.
    public Student PromptForStudent()
    {
        Console.Write("Name: ");
        var name = Console.ReadLine() ?? "";
        Console.Write("Age (5-100): ");
        var ageText = Console.ReadLine() ?? "";
        Console.Write("Roll number: ");
        var roll = Console.ReadLine() ?? "";
        Console.Write("Email: ");
        var email = Console.ReadLine() ?? "";

        return new Student
        {
            Name = name,
            Age = int.TryParse(ageText, out var age) ? age : -1, // -1 forces model to reject
            RollNumber = roll,
            Email = email
        };
    }

    public int PromptForId(string action)
    {
        Console.Write($"Id to {action}: ");
        return int.TryParse(Console.ReadLine(), out var id) ? id : -1;
    }

    public void ShowMessage(string message) => Console.WriteLine($">> {message}");

    public void ShowLog(IReadOnlyList<string> history)
    {
        Console.WriteLine();
        Console.WriteLine("----- TRANSACTION LOG -----");
        if (history.Count == 0) { Console.WriteLine("(empty)"); return; }
        foreach (var line in history) Console.WriteLine(line);
    }
}
