using StudentManagement.App.Models;

namespace StudentManagement.App.Data;

// One concrete implementation of the storage contract, backed by a List<T>.
// It knows NOTHING about business rules — it just stores and returns objects.
public class InMemoryRepository<T> : IRepository<T> where T : IEntity
{
    private readonly List<T> _items = new();
    private int _nextId = 1;

    public void Add(T item)
    {
        // The repository owns Id assignment for a fresh record.
        if (item.Id == 0)
            SetId(item, _nextId);
        _nextId = Math.Max(_nextId, item.Id) + 1;
        _items.Add(item);
    }

    public IEnumerable<T> GetAll() => _items.ToList(); // copy, so callers can't mutate our list

    public T? GetById(int id) => _items.FirstOrDefault(x => x.Id == id);

    public bool Update(T item)
    {
        var index = _items.FindIndex(x => x.Id == item.Id);
        if (index < 0) return false;
        _items[index] = item;
        return true;
    }

    public bool Delete(int id)
    {
        var existing = _items.FirstOrDefault(x => x.Id == id);
        if (existing is null) return false;
        _items.Remove(existing);
        return true;
    }

    // Id is defined as get-only on IEntity, so we set it via the concrete property.
    private static void SetId(T item, int id)
    {
        var prop = typeof(T).GetProperty(nameof(IEntity.Id));
        prop?.SetValue(item, id);
    }
}
