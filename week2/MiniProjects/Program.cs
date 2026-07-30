using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
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
        svc.Notify(new PushSender(), "New login detected");

        Console.WriteLine("\n=== Q6 · Library ===");
        var books = new List<Book>
        {
            new("The Hobbit", "Tolkien", "Fantasy", 1937, true),
            new("The Martian", "Weir", "SciFi", 2011, true),
            new("Sapiens", "Harari", "Nonfiction", 2011, true),
            new("The Silmarillion", "Tolkien", "Fantasy", 1977, true),
            new("Project Hail Mary", "Weir", "SciFi", 2021, true),
            new("Homo Deus", "Harari", "Nonfiction", 2015, false),
            new("1984", "Orwell", "Dystopian", 1949, true),
            new("Animal Farm", "Orwell", "Dystopian", 1945, true),
            new("Dune", "Herbert", "SciFi", 1965, false),
            new("Dune Messiah", "Herbert", "SciFi", 1969, true),
            new("The Fellowship of the Ring", "Tolkien", "Fantasy", 1954, true),
            new("Foundation", "Asimov", "SciFi", 1951, true),
            new("I, Robot", "Asimov", "SciFi", 1950, true),
            new("Brave New World", "Huxley", "Dystopian", 1932, false),
            new("Artemis", "Weir", "SciFi", 2017, true),
            new("21 Lessons for the 21st Century", "Harari", "Nonfiction", 2018, true),
        };

        Console.WriteLine("Available books by Tolkien:");
        foreach (var b in LibraryQueries.AvailableByAuthor(books, "Tolkien"))
            Console.WriteLine($"  {b.Title} ({b.Year})");

        Console.WriteLine("Count by genre:");
        foreach (var g in LibraryQueries.CountByGenre(books))
            Console.WriteLine($"  {g.Genre}: {g.Count}");

        Console.WriteLine($"Oldest: {LibraryQueries.Oldest(books).Title}");

        Console.WriteLine("Books after 2010, sorted by title:");
        foreach (var b in LibraryQueries.After2010SortedByTitle(books))
            Console.WriteLine($"  {b.Title} ({b.Year})");

        Console.ReadKey();
    }
}

