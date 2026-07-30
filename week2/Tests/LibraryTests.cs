using System.Collections.Generic;
using System.Linq;
using Xunit;

public class LibraryTests
{
    private static List<Book> SampleBooks() => new()
    {
        new("The Hobbit", "Tolkien", "Fantasy", 1937, true),
        new("The Silmarillion", "Tolkien", "Fantasy", 1977, false),
        new("The Martian", "Weir", "SciFi", 2011, true),
        new("Project Hail Mary", "Weir", "SciFi", 2021, true),
        new("Sapiens", "Harari", "Nonfiction", 2011, true),
        new("Dune", "Herbert", "SciFi", 1965, false),
        new("1984", "Orwell", "Dystopian", 1949, true),
        new("Animal Farm", "Orwell", "Dystopian", 1945, true),
    };

    // ---- AvailableByAuthor ----

    [Fact]
    public void AvailableByAuthor_ReturnsOnlyAvailableBooksForThatAuthor()
    {
        var result = LibraryQueries.AvailableByAuthor(SampleBooks(), "Tolkien");

        Assert.Single(result);
        Assert.Equal("The Hobbit", result[0].Title); // Silmarillion excluded: IsAvailable == false
    }

    [Fact]
    public void AvailableByAuthor_UnknownAuthor_ReturnsEmpty()
    {
        var result = LibraryQueries.AvailableByAuthor(SampleBooks(), "Nobody");
        Assert.Empty(result);
    }

    [Fact]
    public void AvailableByAuthor_AuthorWithNoAvailableBooks_ReturnsEmpty()
    {
        var result = LibraryQueries.AvailableByAuthor(SampleBooks(), "Herbert"); // Dune is unavailable
        Assert.Empty(result);
    }

    // ---- CountByGenre ----

    [Fact]
    public void CountByGenre_CountsAndSortsAlphabetically()
    {
        var result = LibraryQueries.CountByGenre(SampleBooks());

        Assert.Equal(new[] { "Dystopian", "Fantasy", "Nonfiction", "SciFi" }, result.Select(g => g.Genre));
        Assert.Equal(3, result.First(g => g.Genre == "SciFi").Count); // Martian, Hail Mary, Dune
        Assert.Equal(1, result.First(g => g.Genre == "Nonfiction").Count);
    }

    [Fact]
    public void CountByGenre_EmptyList_ReturnsEmpty()
    {
        Assert.Empty(LibraryQueries.CountByGenre(new List<Book>()));
    }

    // ---- Oldest ----

    [Fact]
    public void Oldest_ReturnsEarliestYear()
    {
        var oldest = LibraryQueries.Oldest(SampleBooks());
        Assert.Equal("The Hobbit", oldest.Title); // 1937
    }

    [Fact]
    public void Oldest_TieOnYear_ReturnsFirstEncountered()
    {
        var books = new List<Book>
        {
            new("Book A", "X", "G", 2000, true),
            new("Book B", "Y", "G", 2000, true),
        };

        var oldest = LibraryQueries.Oldest(books);
        Assert.Equal("Book A", oldest.Title); // stable order on tie
    }

    // ---- After2010SortedByTitle ----

    [Fact]
    public void After2010SortedByTitle_ExcludesYear2010Exactly_AndSortsByTitle()
    {
        var books = new List<Book>
        {
            new("Zebra", "A", "G", 2015, true),
            new("Apple", "B", "G", 2012, true),
            new("Exactly2010", "C", "G", 2010, true),   // boundary: must be excluded (> not >=)
        };

        var result = LibraryQueries.After2010SortedByTitle(books);

        Assert.Equal(new[] { "Apple", "Zebra" }, result.Select(b => b.Title));
    }

    [Fact]
    public void After2010SortedByTitle_NoneMatch_ReturnsEmpty()
    {
        var books = new List<Book> { new("Old One", "A", "G", 1990, true) };
        Assert.Empty(LibraryQueries.After2010SortedByTitle(books));
    }
}
