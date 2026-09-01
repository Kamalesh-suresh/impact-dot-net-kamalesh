using StudentManagement.Api.Models;

namespace StudentManagement.Api.Data;

// Reused from Week 4 unchanged. The Service depends on THIS, never on a
// concrete storage class — same reasoning as before, now proven across
// TWO different front ends (a console app, and this API).
public interface IRepository<T> where T : IEntity
{
    void Add(T item);
    IEnumerable<T> GetAll();
    T? GetById(int id);
    bool Update(T item);
    bool Delete(int id);
}
