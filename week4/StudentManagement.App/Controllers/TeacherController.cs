using StudentManagement.App.Services;
using StudentManagement.App.Views;

namespace StudentManagement.App.Controllers;

public class TeacherController
{
    private readonly ITeacherService _service;
    private readonly TeacherView _view;

    public TeacherController(ITeacherService service, TeacherView view)
    {
        _service = service;
        _view = view;
    }

    public string Run()
    {
        while (true)
        {
            _view.ShowMenu();
            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1": _view.PrintTeachers(_service.GetAll()); break;
                case "2":
                    try
                    {
                        var t = _view.PromptForTeacher();
                        _view.ShowMessage(_service.AddTeacher(t).Message);
                    }
                    catch (Exception ex) { _view.ShowMessage($"Invalid input: {ex.Message}"); }
                    break;
                case "3":
                    var id = _view.PromptForId("delete");
                    _view.ShowMessage(_service.DeleteTeacher(id).Message);
                    break;
                case "6": return "students";
                case "0": return "exit";
                default: _view.ShowMessage("Unknown choice, try again."); break;
            }
        }
    }
}
