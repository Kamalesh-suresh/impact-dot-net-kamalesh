using StudentManagement.App.Logging;
using StudentManagement.App.Services;
using StudentManagement.App.Views;

namespace StudentManagement.App.Controllers;

// CONTROLLER = ORCHESTRATION ONLY.
// It reads a menu choice, calls the service, and hands the result to the view.
// It NEVER formats output itself and NEVER touches the storage list directly.
public class StudentController
{
    private readonly IStudentService _service;
    private readonly StudentView _view;
    private readonly ITransactionLog _log;

    public StudentController(IStudentService service, StudentView view, ITransactionLog log)
    {
        _service = service;
        _view = view;
        _log = log;
    }

    // Returns the next "mode" the app should run: "students", "teachers", or "exit".
    public string Run()
    {
        while (true)
        {
            _view.ShowMenu();
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    _view.PrintStudents(_service.GetAll());
                    break;

                case "2":
                    try
                    {
                        var student = _view.PromptForStudent();      // view builds it
                        var result = _service.AddStudent(student);   // service decides
                        _view.ShowMessage(result.Message);           // view reports
                    }
                    catch (Exception ex)
                    {
                        // Invalid model input (e.g. bad age) surfaces here.
                        _view.ShowMessage($"Invalid input: {ex.Message}");
                    }
                    break;

                case "3":
                    var updId = _view.PromptForId("update");
                    var current = _service.GetById(updId);
                    if (current is null) { _view.ShowMessage($"No student with Id {updId}."); break; }
                    try
                    {
                        var edited = _view.PromptForStudent();
                        edited.Id = updId;
                        _view.ShowMessage(_service.UpdateStudent(edited).Message);
                    }
                    catch (Exception ex) { _view.ShowMessage($"Invalid input: {ex.Message}"); }
                    break;

                case "4":
                    var delId = _view.PromptForId("delete");
                    _view.ShowMessage(_service.DeleteStudent(delId).Message);
                    break;

                case "5":
                    _view.ShowLog(_log.GetHistory());
                    break;

                case "6":
                    return "teachers";

                case "0":
                    return "exit";

                default:
                    _view.ShowMessage("Unknown choice, try again.");
                    break;
            }
        }
    }
}
