using System.Reflection;

[AttributeUsage(AttributeTargets.Property)]
public class MaxLengthNoAttribute : Attribute
{
    public int Length { get; }
    public MaxLengthNoAttribute(int length) => Length = length;
}

public class User
{
    [MaxLengthNo(10)]
    public string Name { get; set; } = string.Empty;
}

public static class Validator
{
    public static void Validate(object obj)
    {
        var type = obj.GetType();
        foreach (var prop in type.GetProperties())
        {
            var attr = prop.GetCustomAttribute<MaxLengthNoAttribute>();
            if (attr == null) continue;

            if (prop.GetValue(obj) is string value && value.Length > attr.Length)
            {
                Console.WriteLine($"WARNING: {prop.Name} exceeds max length of {attr.Length} (was {value.Length}).");
            }
        }
    }
}

