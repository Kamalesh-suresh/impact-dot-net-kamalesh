Console.WriteLine("=== Q4 · Payroll ===");
var staff = new List<Employee>
        {
            new FullTimeEmployee { Name = "Alice", Department = "Engineering", MonthlyBase = 5000m },
            new PartTimeEmployee { Name = "Bob",   Department = "Engineering", HourlyRate = 20m, Hours = 100 },
            new ContractEmployee { Name = "Cara",  Department = "Design",      ContractAmount = 3000m },
        };
Console.WriteLine($"Total payroll: {Payroll.Total(staff):C}");
Console.WriteLine($"Total tax:     {Payroll.TotalTax(staff):C}");
foreach (var d in Payroll.ByDepartment(staff))
    Console.WriteLine($"  {d.Department}: {d.Count} staff, {d.Total:C}");

Console.WriteLine("\n=== Q5 · Notification Engine ===");
var svc = new NotificationService();
svc.OnNotificationSent += (s, msg) => Console.WriteLine($"  ✓ logged: \"{msg}\"");
svc.Notify(new EmailSender(), "Order shipped");
svc.Notify(new SmsSender(), "Payment received");

Console.WriteLine("\n=== Q6 · Library ===");
var books = new List<Book>
        {
            new("The Hobbit", "Tolkien", "Fantasy", 1937, true),
            new("The Martian", "Weir", "SciFi", 2011, true),
            new("Sapiens", "Harari", "Nonfiction", 2011, true),
        };
Console.WriteLine($"Oldest: {LibraryQueries.Oldest(books).Title}");
foreach (var g in LibraryQueries.CountByGenre(books))
    Console.WriteLine($"  {g.Genre}: {g.Count}");