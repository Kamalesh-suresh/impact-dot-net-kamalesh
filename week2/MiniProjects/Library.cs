using System.Collections.Generic;
using System.Linq;


// ── Q6 · Library Management (LINQ) ────────────────────────────

public record Book(string Title, string Author, string Genre, int Year, bool IsAvailable);

public record GenreCount(string Genre, int Count);

public static class LibraryQueries
{
    // 1. available books by a given author
    public static IReadOnlyList<Book> AvailableByAuthor(IEnumerable<Book> books, string author) =>
        books.Where(b => b.IsAvailable && b.Author == author).ToList();

    // 2. group by genre with count
    public static IReadOnlyList<GenreCount> CountByGenre(IEnumerable<Book> books) =>
        books.GroupBy(b => b.Genre)
             .Select(g => new GenreCount(g.Key, g.Count()))
             .OrderBy(g => g.Genre)
             .ToList();

    // 3. oldest book
    public static Book Oldest(IEnumerable<Book> books) =>
        books.OrderBy(b => b.Year).First();

    // 4. books after 2010, sorted by title
    public static IReadOnlyList<Book> After2010SortedByTitle(IEnumerable<Book> books) =>
        books.Where(b => b.Year > 2010).OrderBy(b => b.Title).ToList();
}
