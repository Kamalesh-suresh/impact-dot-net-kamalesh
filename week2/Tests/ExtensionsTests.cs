using System.Collections.Generic;
using Xunit;

public class ExtensionsTests
{
    // ---- ToTitleCase ----

    [Theory]
    [InlineData("hello world", "Hello World")]
    [InlineData("MULTIPLE WORDS HERE", "Multiple Words Here")]
    [InlineData("already Title Case", "Already Title Case")]
    [InlineData("single", "Single")]
    public void ToTitleCase_ConvertsWordsToTitleCase(string input, string expected)
    {
        Assert.Equal(expected, input.ToTitleCase());
    }

    [Fact]
    public void ToTitleCase_EmptyString_ReturnsEmptyString()
    {
        Assert.Equal("", "".ToTitleCase());
    }

    [Fact]
    public void ToTitleCase_Null_ReturnsNull()
    {
        string? input = null;
        Assert.Null(Extensions.ToTitleCase(input!));
    }

    [Fact]
    public void ToTitleCase_MultipleConsecutiveSpaces_PreservesEmptyToken()
    {
        // "hello  world" splits into ["hello", "", "world"]; the empty token
        // hits the w.Length == 0 branch and is left untouched.
        Assert.Equal("Hello  World", "hello  world".ToTitleCase());
    }

    // ---- IsNullOrEmpty ----

    [Fact]
    public void IsNullOrEmpty_NullList_ReturnsTrue()
    {
        List<int>? list = null;
        Assert.True(list.IsNullOrEmpty());
    }

    [Fact]
    public void IsNullOrEmpty_EmptyList_ReturnsTrue()
    {
        Assert.True(new List<int>().IsNullOrEmpty());
    }

    [Fact]
    public void IsNullOrEmpty_NonEmptyList_ReturnsFalse()
    {
        Assert.False(new List<int> { 1 }.IsNullOrEmpty());
    }

    // ---- ToWords ----

    [Theory]
    [InlineData(0, "zero")]
    [InlineData(7, "seven")]
    [InlineData(13, "thirteen")]
    [InlineData(19, "nineteen")]
    [InlineData(20, "twenty")]        // exact tens, no dash branch
    [InlineData(21, "twenty-one")]    // tens + ones dash branch
    [InlineData(90, "ninety")]
    [InlineData(99, "ninety-nine")]
    [InlineData(100, "one hundred")]  // exact hundred, no remainder branch
    [InlineData(101, "one hundred one")]
    [InlineData(120, "one hundred twenty")]
    [InlineData(147, "one hundred forty-seven")]
    [InlineData(999, "nine hundred ninety-nine")]
    public void ToWords_ConvertsNumberToWords(int input, string expected)
    {
        Assert.Equal(expected, input.ToWords());
    }
}
