using StudentManagement.App.Models;

namespace StudentManagement.App.Data;

// The storage contract. The Service depends on THIS interface, never on a
// concrete storage class — so we can swap in-memory for a database later
// without touching the Service. (This is the interface reused from Week 3.)
public interface IRepository<T> where T : IEntity
{
    void Add(T item);
    IEnumerable<T> GetAll();
    T? GetById(int id);
    bool Update(T item);   // returns false if the id doesn't exist
    bool Delete(int id);   // returns false if the id doesn't exist
}
