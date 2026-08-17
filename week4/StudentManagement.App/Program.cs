using Microsoft.Extensions.DependencyInjection;
using StudentManagement.App.Controllers;
using StudentManagement.App.Data;
using StudentManagement.App.Logging;
using StudentManagement.App.Models;
using StudentManagement.App.Services;
using StudentManagement.App.Views;

namespace StudentManagement.App;

public static class Program
{
    public static void Main(string[] args)
    {
        // Two wiring styles are implemented for this week:
        //   Task 4.7 -> BuildManually()      (new up everything by hand)
        //   Task 4.8 -> BuildWithContainer() (Microsoft DI container)
        // They produce an app that behaves IDENTICALLY. We run the container one.
        // Pass "manual" as an argument to run the hand-wired version instead.
        var useManual = args.Length > 0 && args[0] == "manual";

        Console.WriteLine(useManual
            ? "[Wiring: MANUAL dependency injection - Task 4.7]"
            : "[Wiring: Microsoft DI CONTAINER - Task 4.8]");

        var app = useManual ? BuildManually() : BuildWithContainer();
        app.Run();
    }

    // ---------------- Task 4.7: manual DI ----------------
    // Notice we construct from the inside out: repo -> service -> controller.
    // To swap storage you change ONE line (the repo) and nothing else.
    private static AppRunner BuildManually()
    {
        ITransactionLog log = new TransactionLog();

        IRepository<Student> studentRepo = new InMemoryRepository<Student>();
        IRepository<Teacher> teacherRepo = new InMemoryRepository<Teacher>();

        IStudentService studentService = new StudentService(studentRepo, log);
        ITeacherService teacherService = new TeacherService(teacherRepo, log);

        var studentController = new StudentController(studentService, new StudentView(), log);
        var teacherController = new TeacherController(teacherService, new TeacherView());

        Seed(studentService);
        return new AppRunner(studentController, teacherController);
    }

    // ---------------- Task 4.8: Microsoft DI container ----------------
    private static AppRunner BuildWithContainer()
    {
        var services = new ServiceCollection();

        // Register abstractions -> implementations.
        // Singleton is chosen deliberately: our "database" is an in-memory list,
        // so every part of the app must share the SAME instance for the whole run.
        // (Scoped/Transient would hand out fresh, empty stores and lose our data.)
        services.AddSingleton<ITransactionLog, TransactionLog>();
        services.AddSingleton<IRepository<Student>, InMemoryRepository<Student>>();
        services.AddSingleton<IRepository<Teacher>, InMemoryRepository<Teacher>>();
        services.AddSingleton<IStudentService, StudentService>();
        services.AddSingleton<ITeacherService, TeacherService>();

        // Views and controllers have no state to share, so Transient is fine.
        services.AddTransient<StudentView>();
        services.AddTransient<TeacherView>();
        services.AddTransient<StudentController>();
        services.AddTransient<TeacherController>();
        services.AddTransient<AppRunner>();

        var provider = services.BuildServiceProvider();

        Seed(provider.GetRequiredService<IStudentService>());
        return provider.GetRequiredService<AppRunner>();
    }

    // Task 4.2: seed 3 students at startup.
    private static void Seed(IStudentService studentService)
    {
        studentService.AddStudent(new Student { Name = "Anita Rao",   Age = 20, RollNumber = "R001", Email = "anita@college.edu" });
        studentService.AddStudent(new Student { Name = "Bhaskar Iyer", Age = 22, RollNumber = "R002", Email = "bhaskar@college.edu" });
        studentService.AddStudent(new Student { Name = "Chitra Nair",  Age = 19, RollNumber = "R003", Email = "chitra@college.edu" });
    }
}

// Top-level loop that switches between the Student and Teacher sub-apps
// (Task 4.10). Registered in the container like everything else.
public class AppRunner
{
    private readonly StudentController _students;
    private readonly TeacherController _teachers;

    public AppRunner(StudentController students, TeacherController teachers)
    {
        _students = students;
        _teachers = teachers;
    }

    public void Run()
    {
        var mode = "students";
        while (mode != "exit")
        {
            mode = mode == "students" ? _students.Run() : _teachers.Run();
        }
        Console.WriteLine("Goodbye!");
    }
}
