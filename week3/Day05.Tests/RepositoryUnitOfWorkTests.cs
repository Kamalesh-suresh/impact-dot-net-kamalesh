public class RepositoryUnitOfWorkTests
{
    [Fact]
    public void StudentRepository_Add_GetAll_GetById_Update_Delete_RoundTrips()
    {
        IRepository<Student> repo = new StudentRepository();

        repo.Add(new Student { Id = 1, Name = "Asha" });
        repo.Add(new Student { Id = 2, Name = "Ravi" });
        Assert.Equal(2, repo.GetAll().Count);

        var found = repo.GetById(1);
        Assert.NotNull(found);
        Assert.Equal("Asha", found!.Name);

        repo.Update(new Student { Id = 1, Name = "Asha Verma" });
        Assert.Equal("Asha Verma", repo.GetById(1)!.Name);

        repo.Delete(1);
        Assert.Null(repo.GetById(1));
        Assert.Single(repo.GetAll());
    }

    [Fact]
    public void CourseRepository_Add_GetAll_GetById_Update_Delete_RoundTrips()
    {
        IRepository<Course> repo = new CourseRepository();

        repo.Add(new Course { Id = 1, Title = "C# Fundamentals" });
        Assert.Single(repo.GetAll());

        repo.Update(new Course { Id = 1, Title = "C# Fundamentals (Updated)" });
        Assert.Equal("C# Fundamentals (Updated)", repo.GetById(1)!.Title);

        repo.Delete(1);
        Assert.Empty(repo.GetAll());
    }

    [Fact]
    public void UnitOfWork_CoordinatesRepositoriesAndSaves()
    {
        IUnitOfWork uow = new UnitOfWork();
        uow.Students.Add(new Student { Id = 1, Name = "Asha" });
        uow.Courses.Add(new Course { Id = 1, Title = "C# Fundamentals" });

        Assert.Single(uow.Students.GetAll());
        Assert.Single(uow.Courses.GetAll());

        var exception = Record.Exception(() => uow.Save());
        Assert.Null(exception); // single commit point, doesn't throw
    }
}
