using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using StudentManagement.Api.Controllers;
using StudentManagement.Api.Dtos;
using StudentManagement.Api.Models;
using StudentManagement.Api.Services;
using Xunit;

namespace StudentManagement.Api.Tests;

// Testing Focus: "a small set of controller tests asserting the right status
// code per outcome (200/201/204/400/404) using an in-memory service."
// The service itself is mocked here (fast + isolates the controller's
// orchestration logic from the service's business rules, which are already
// covered by StudentServiceTests).
public class StudentsControllerTests
{
    private static StudentsController BuildController(Mock<IStudentService> service) =>
        new(service.Object, Mock.Of<ILogger<StudentsController>>());

    [Fact]
    public void GetAll_Returns200()
    {
        var service = new Mock<IStudentService>();
        service.Setup(s => s.GetAll()).Returns(new List<Student>());
        var controller = BuildController(service);

        var result = controller.GetAll(null);

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public void GetById_Found_Returns200()
    {
        var service = new Mock<IStudentService>();
        service.Setup(s => s.GetById(1)).Returns(new Student { Id = 1, Name = "A", Age = 20, RollNumber = "R1", Email = "a@x.com" });
        var controller = BuildController(service);

        var result = controller.GetById(1, null);

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public void GetById_Missing_Returns404()
    {
        var service = new Mock<IStudentService>();
        service.Setup(s => s.GetById(99)).Returns((Student?)null);
        var controller = BuildController(service);

        var result = controller.GetById(99, null);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public void Create_Valid_Returns201WithLocation()
    {
        var service = new Mock<IStudentService>();
        service.Setup(s => s.AddStudent(It.IsAny<Student>()))
               .Returns((Student s) => OperationResult<Student>.Ok(s));
        var controller = BuildController(service);

        var result = controller.Create(new StudentCreateDto { Name = "Ravi", Age = 20, RollNumber = "R010", Email = "r@x.com" });

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(StudentsController.GetById), created.ActionName);
    }

    [Fact]
    public void Create_DuplicateRoll_Returns400()
    {
        var service = new Mock<IStudentService>();
        service.Setup(s => s.AddStudent(It.IsAny<Student>()))
               .Returns(OperationResult<Student>.Fail("Roll number 'R001' already exists."));
        var controller = BuildController(service);

        var result = controller.Create(new StudentCreateDto { Name = "B", Age = 21, RollNumber = "R001", Email = "b@x.com" });

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public void Update_Missing_Returns404()
    {
        var service = new Mock<IStudentService>();
        service.Setup(s => s.GetById(99)).Returns((Student?)null);
        var controller = BuildController(service);

        var result = controller.Update(99, new StudentCreateDto { Name = "X", Age = 20, RollNumber = "R99", Email = "x@x.com" });

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Update_Valid_Returns204()
    {
        var service = new Mock<IStudentService>();
        service.Setup(s => s.GetById(1)).Returns(new Student { Id = 1, Name = "A", Age = 20, RollNumber = "R1", Email = "a@x.com" });
        service.Setup(s => s.UpdateStudent(It.IsAny<Student>())).Returns((Student s) => OperationResult<Student>.Ok(s));
        var controller = BuildController(service);

        var result = controller.Update(1, new StudentCreateDto { Name = "A2", Age = 21, RollNumber = "R1", Email = "a2@x.com" });

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void Delete_Found_Returns204()
    {
        var service = new Mock<IStudentService>();
        service.Setup(s => s.DeleteStudent(1)).Returns(true);
        var controller = BuildController(service);

        Assert.IsType<NoContentResult>(controller.Delete(1));
    }

    [Fact]
    public void Delete_Missing_Returns404()
    {
        var service = new Mock<IStudentService>();
        service.Setup(s => s.DeleteStudent(99)).Returns(false);
        var controller = BuildController(service);

        Assert.IsType<NotFoundResult>(controller.Delete(99));
    }

    [Fact]
    public void Search_ReturnsEmptyList_NotNotFound()
    {
        var service = new Mock<IStudentService>();
        service.Setup(s => s.SearchByName("zzz")).Returns(new List<Student>());
        var controller = BuildController(service);

        var result = controller.Search("zzz");

        Assert.IsType<OkObjectResult>(result.Result); // 200 even for zero matches
    }
}
