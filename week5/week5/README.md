# Week 5 — ASP.NET Core Web API Fundamentals with Design Patterns

An **unsecured, in-memory** Student/Teacher Web API — the Week 4 console logic promoted
into real HTTP endpoints, applying the Repository pattern and a Strategy/Factory pattern
along the way.

## Project layout
```
StudentManagement.Api/
  Models/       Student, Teacher, IEntity           (entities — Student carries an
                                                       internal-only field, Task 5.7)
  Dtos/         StudentCreateDto/ReadDto, Teacher*   (the wire shapes, Task 5.7)
  Data/         IRepository<T>, InMemoryRepository<T> (reused unchanged from Week 4)
  Services/     IStudentService, StudentService,       (business rules + search, Task 5.9)
                OperationResult<T>, Teacher*
  Formatters/   IStudentViewFormatter,                 (Strategy/Factory, Task 5.8)
                Detailed/MinimalStudentViewFormatter,
                StudentViewFormatterFactory
  Controllers/  StudentsController, TeachersController (thin — orchestration only)
  Program.cs    DI registration, middleware pipeline, Swagger, seed data
StudentManagement.Api.Tests/  xUnit + Moq — service, formatter/factory, controller, DTO-leak tests
```

## Run it
```bash
dotnet run --project StudentManagement.Api
```
Then open **https://localhost:7xxx/swagger** (the exact port is printed on startup, and
`launchUrl` in `launchSettings.json` opens Swagger automatically) to exercise every endpoint.

## Endpoints
| Verb | Route | Success | Failure |
|---|---|---|---|
| GET | `/api/students?view=detailed\|minimal` | 200 | — |
| GET | `/api/students/{id}` | 200 | 404 |
| GET | `/api/students/search?name=...` | 200 (incl. empty list) | — |
| POST | `/api/students` | 201 + `Location` header | 400 (validation or duplicate roll) |
| PUT | `/api/students/{id}` | 204 | 404 / 400 |
| DELETE | `/api/students/{id}` | 204 | 404 |
| (same shape) | `/api/teachers` | — | — |

## Test + coverage (≥80% on service + formatter/factory + controller)
```bash
dotnet test --collect:"XPlat Code Coverage"
```
First run restores xUnit, Moq, and coverlet from NuGet — needs internet once.
