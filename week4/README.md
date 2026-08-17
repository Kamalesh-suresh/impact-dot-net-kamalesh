# Week 4 — Student Management Console App (Layered MVC)

A console application built with strict **Model / View / Controller / Service / Repository**
separation. No web, no auth, no database — an in-memory `List<T>` is the store.

## Layer responsibilities (4 lines)
- **Model** holds data and its own validation invariants only (e.g. age 5–100).
- **Service** holds every business rule (e.g. no duplicate roll numbers) and is the primary tested unit.
- **View** only renders output and reads raw input — zero business logic.
- **Controller** only orchestrates: read choice → call service → hand result to the view; it never formats output or touches the store.

## Project layout
```
StudentManagement.App/
  Models/       Student, Teacher, IEntity      (data + invariants)
  Data/         IRepository<T>, InMemoryRepository<T>   (storage behind an interface)
  Services/     IStudentService, StudentService, OperationResult, Teacher*  (business rules)
  Views/        StudentView, TeacherView       (presentation only)
  Controllers/  StudentController, TeacherController  (orchestration only)
  Logging/      ITransactionLog, TransactionLog (audit trail via encapsulation)
  Program.cs    Manual DI (Task 4.7) + Microsoft DI container (Task 4.8) + seed
StudentManagement.Tests/   xUnit + Moq tests targeting the Service (+ Model)
```

## Run
```bash
dotnet run --project StudentManagement.App              # Microsoft DI container (Task 4.8)
dotnet run --project StudentManagement.App -- manual    # manual wiring (Task 4.7)
```

## Test + coverage (≥80% on the service)
```bash
dotnet test --collect:"XPlat Code Coverage"
```
The first run restores NuGet packages (xunit, Moq, coverlet). Requires internet once.
