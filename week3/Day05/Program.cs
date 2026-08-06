// ===================================================================
// Task 3.13 — interface vs abstract, static vs instance
// ===================================================================

Console.WriteLine("--- Interface scenario: unrelated types, multiple contracts ---");
var report = new Report("Q2 Sales");
var invoice = new Invoice(1042);
report.RecordAccess("Kamalesh");
Console.WriteLine(report.ToExportFormat());
Console.WriteLine(invoice.ToExportFormat());

Console.WriteLine();
Console.WriteLine("--- Abstract class scenario: shared state + template method ---");
Shape[] shapes = { new Circle(3), new Rectangle(4, 5) };
foreach (var shape in shapes)
    Console.WriteLine(shape.Describe());

Console.WriteLine();
Console.WriteLine("--- MathHelper (static) ---");
Console.WriteLine($"5! = {MathHelper.Factorial(5)}");
Console.WriteLine($"IsPrime(17) = {MathHelper.IsPrime(17)}");
Console.WriteLine($"GCD(48, 18) = {MathHelper.GCD(48, 18)}");

Console.WriteLine();
Console.WriteLine("--- OrderProcessor (instance) ---");
var asha = new OrderProcessor("Asha");
asha.AddOrder(250m);
asha.AddOrder(75.50m);
Console.WriteLine($"{asha.CustomerName}: {asha.OrderCount} orders, total {asha.GetTotal():C}, after 10% discount {asha.ApplyDiscount(10):C}");

var ravi = new OrderProcessor("Ravi"); // independent state from asha's processor
ravi.AddOrder(1000m);
Console.WriteLine($"{ravi.CustomerName}: {ravi.OrderCount} orders, total {ravi.GetTotal():C}");


// ===================================================================
// Task 3.14 — Employee sorting: IComparable<T> vs IComparer<T>
// ===================================================================

Console.WriteLine();
Console.WriteLine("--- Sorted by salary (default, via IComparable<Employee>) ---");
var employees = new List<Employee>
{
    new("Meera", 95000),
    new("Ravi", 62000),
    new("Asha", 78000),
    new("Karthik", 54000),
    new("Divya", 110000),
    new("Naveen", 71000),
    new("Priya", 88000),
    new("Suresh", 47000),
    new("Anjali", 130000),
    new("Vikram", 66000),
};

var bySalary = new List<Employee>(employees);
bySalary.Sort(); // uses Employee.CompareTo -> salary ascending
foreach (var e in bySalary) Console.WriteLine(e);

Console.WriteLine();
Console.WriteLine("--- Sorted by name (via EmployeeNameComparer) ---");
var byName = new List<Employee>(employees);
byName.Sort(new EmployeeNameComparer());
foreach (var e in byName) Console.WriteLine(e);
