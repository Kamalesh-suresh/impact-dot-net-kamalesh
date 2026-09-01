using StudentManagement.Api.Data;
using StudentManagement.Api.Models;
using StudentManagement.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// ---- DI registration (Task 5.3) ----
// Repository = Singleton: the in-memory List<T> must be the SAME instance
// for the whole app's lifetime, or data added on one request would be
// invisible to the next (same reasoning as Week 4's Microsoft DI container).
builder.Services.AddSingleton<IRepository<Student>, InMemoryRepository<Student>>();
builder.Services.AddSingleton<IRepository<Teacher>, InMemoryRepository<Teacher>>();

// Service = Scoped: the conventional ASP.NET Core default for anything
// resolved per-request. The service itself holds no state — it always reads
// through to the Singleton repository — so Scoped costs nothing here but
// matches the lifetime you'd use the moment a service gained per-request
// state (e.g. "current user") later.
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ITeacherService, TeacherService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ---- Middleware pipeline (Task 5.1) ----
// Real default order from `dotnet new webapi -controllers`:
//   1. Swagger + Swagger UI   (Development only)
//   2. HTTPS redirection
//   3. Authorization           (no-op here — Week 5 is unsecured on purpose)
//   4. Controller endpoints
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// ---- Seed data so the API isn't empty on first run ----
using (var scope = app.Services.CreateScope())
{
    var studentRepo = scope.ServiceProvider.GetRequiredService<IRepository<Student>>();
    studentRepo.Add(new Student { Name = "Anita Rao", Age = 20, RollNumber = "R001", Email = "anita@college.edu", InternalAuditNote = "seeded" });
    studentRepo.Add(new Student { Name = "Bhaskar Iyer", Age = 22, RollNumber = "R002", Email = "bhaskar@college.edu", InternalAuditNote = "seeded" });
    studentRepo.Add(new Student { Name = "Chitra Nair", Age = 19, RollNumber = "R003", Email = "chitra@college.edu", InternalAuditNote = "seeded" });

    var teacherRepo = scope.ServiceProvider.GetRequiredService<IRepository<Teacher>>();
    teacherRepo.Add(new Teacher { Name = "Dr. Priya Sharma", Email = "priya@college.edu", Designation = "Professor" });
}

app.Run();

// Exposed for WebApplicationFactory-style integration tests (Task 5.5's
// controller tests reference this so the test project can spin the app up
// in-memory without a real port).
public partial class Program { }
