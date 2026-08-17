namespace StudentManagement.App.Models;

// A tiny contract so the generic repository knows every entity has an Id.
// (Model layer = data only. This is just a shape, not business logic.)
public interface IEntity
{
    int Id { get; }
}
