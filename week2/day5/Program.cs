// usage — reads like it was always part of the type
Console.WriteLine("hello world".ToTitleCase());     // "Hello World"
Console.WriteLine(new List<int>().IsNullOrEmpty());  // true
Console.WriteLine(147.ToWords());                   // "one hundred forty-seven"

Console.ReadKey();

public static class Extensions
{
    public static string ToTitleCase(this string s)
    {
        if (string.IsNullOrEmpty(s)) return s;
        return string.Join(' ', s.Split(' ')

            .Select(w => w.Length == 0 ? w : char.ToUpper(w[0]) + w[1..].ToLower()));
    }

    public static bool IsNullOrEmpty<T>(this List<T>? list) => list is null || list.Count == 0;

    public static string ToWords(this int n)     // 0–999
    {
        if (n == 0) return "zero";
        string[] ones = { "", "one","two","three","four","five","six","seven","eight","nine",
            "ten","eleven","twelve","thirteen","fourteen","fifteen","sixteen","seventeen",
            "eighteen","nineteen" };
        string[] tens = { "", "", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };
        string r = "";
        if (n >= 100) { r += ones[n / 100] + " hundred"; n %= 100; if (n > 0) r += " "; }
        if (n >= 20) { r += tens[n / 10]; if (n % 10 > 0) r += "-" + ones[n % 10]; }
        else if (n > 0) r += ones[n];
        return r;
    }
}
