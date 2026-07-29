var students = new Repository<Student>();
var products = new Repository<Product>();



static IEnumerable<int> GetEvenNumbers(int max)
{
    for (int i = 0; i <= max; i += 2)
    {
        Console.WriteLine($"  yielding {i}");   // proves it runs lazily
        yield return i;                         // pause here, resume on next request
    }
}

foreach (var n in GetEvenNumbers(6))
    Console.WriteLine($"got {n}");


var employees = new List<Employee>
{
    new("Alice", "Engineering", 60000m, new DateTime(2019, 3, 1)),
    new("Bob", "Engineering", 45000m, new DateTime(2021, 7, 15)),
    new("Charlie", "Sales", 55000m, new DateTime(2018, 1, 10)),
    new("Diana", "Sales", 48000m, new DateTime(2022, 5, 20)),
    new("Ethan", "Engineering", 72000m, new DateTime(2016, 9, 12)),
    new("Fiona", "Marketing", 51000m, new DateTime(2020, 11, 3)),
    new("George", "Marketing", 47000m, new DateTime(2023, 2, 18)),
    new("Hannah", "Sales", 63000m, new DateTime(2017, 6, 25)),
    new("Ian", "Engineering", 58000m, new DateTime(2021, 1, 30)),
    new("Julia", "Marketing", 49500m, new DateTime(2019, 8, 14)),
};

// ---- Filter + sort ----
// method
var high = employees.Where(e => e.Salary > 50000)
                    .OrderByDescending(e => e.Salary);
Console.WriteLine("-- high (method) --");
foreach (var e in high)
    Console.WriteLine(e);

// query
var highQ = from e in employees
            where e.Salary > 50000
            orderby e.Salary descending
            select e;
Console.WriteLine("-- highQ (query) --");
foreach (var e in highQ)
    Console.WriteLine(e);

// ---- Group by department: count + average ----
// method
var byDept = employees
    .GroupBy(e => e.Department)
    .Select(g => new { Dept = g.Key, Count = g.Count(), Avg = g.Average(e => e.Salary) });
Console.WriteLine("-- byDept (method) --");
foreach (var g in byDept)
    Console.WriteLine($"{g.Dept}: Count={g.Count}, Avg={g.Avg:C}");

// query
var byDeptQ = from e in employees
              group e by e.Department into g
              select new { Dept = g.Key, Count = g.Count(), Avg = g.Average(e => e.Salary) };
Console.WriteLine("-- byDeptQ (query) --");
foreach (var g in byDeptQ)
    Console.WriteLine($"{g.Dept}: Count={g.Count}, Avg={g.Avg:C}");

// ---- Project to anonymous type ----
var summary = employees.Select(e => new
{
    e.Name,
    Experience = DateTime.Now.Year - e.JoiningDate.Year
});
Console.WriteLine("-- summary --");
foreach (var s in summary)
    Console.WriteLine($"{s.Name}: {s.Experience} yrs");


Console.ReadKey();


class Repository<T> where T : class, new()
{
    private readonly List<T> _items = new();

    public void Add(T item) => _items.Add(item);
    public void Delete(T item) => _items.Remove(item);
    public IReadOnlyList<T> GetAll() => _items;
    public T CreateBlank() => new T();     // ← only legal because of the new() constraint

    public void Update(int index, T item)
    {
        if (index < 0 || index >= _items.Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        _items[index] = item;
    }
}

class Student
{
    public string Name { get; set; } = "";
    public int Age { get; set; }
}

class Product
{
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
}


record Employee(string Name, string Department, decimal Salary, DateTime JoiningDate);


