public interface IRepository<T>
{
    List<T> GetAll();
    T GetById(int id);
    void Add(T entity);
    void Update(T entity);
    void Delete(int id);
}

public class Student { public int Id { get; set; } public string Name { get; set; } }
public class Course { public int Id { get; set; } public string Title { get; set; } }

public class StudentRepository : IRepository<Student>
{
    private readonly List<Student> students = new List<Student>();
    public List<Student> GetAll() => students;
    public Student GetById(int id) => students.FirstOrDefault(s => s.Id == id);
    public void Add(Student entity) => students.Add(entity);
    public void Update(Student entity)
    {
        var index = students.FindIndex(s => s.Id == entity.Id);
        if (index >= 0) students[index] = entity;
    }
    public void Delete(int id) => students.RemoveAll(s => s.Id == id);
}

public class CourseRepository : IRepository<Course>
{
    private readonly List<Course> courses = new List<Course>();
    public List<Course> GetAll() => courses;
    public Course GetById(int id) => courses.FirstOrDefault(c => c.Id == id);
    public void Add(Course entity) => courses.Add(entity);
    public void Update(Course entity)
    {
        var index = courses.FindIndex(c => c.Id == entity.Id);
        if (index >= 0) courses[index] = entity;
    }
    public void Delete(int id) => courses.RemoveAll(c => c.Id == id);
}

public interface IUnitOfWork
{
    IRepository<Student> Students { get; }
    IRepository<Course> Courses { get; }
    void Save();
}

public class UnitOfWork : IUnitOfWork
{
    public IRepository<Student> Students { get; } = new StudentRepository();
    public IRepository<Course> Courses { get; } = new CourseRepository();

    public void Save()
    {
        // a real EF-backed version calls context.SaveChanges() here;
        // this stub shows *where* that single commit point belongs.
        Console.WriteLine("Changes saved.");
    }
}

