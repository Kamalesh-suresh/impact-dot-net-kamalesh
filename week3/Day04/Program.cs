using System.Reflection;

// --- usage (goes in Program.cs) ---
var sw = System.Diagnostics.Stopwatch.StartNew();
foreach (var i in Enumerable.Range(0, 100))
    WorkSimulator.SimulateWork();
sw.Stop();
Console.WriteLine($"Sequential foreach: {sw.ElapsedMilliseconds} ms"); // ~10,000 ms

sw.Restart();
var threads = Enumerable.Range(0, 100)
    .Select(_ => new Thread(WorkSimulator.SimulateWork)).ToList();
threads.ForEach(t => t.Start());
threads.ForEach(t => t.Join());
sw.Stop();
Console.WriteLine($"100 raw Threads: {sw.ElapsedMilliseconds} ms");

sw.Restart();
var tasks = Enumerable.Range(0, 100).Select(_ => Task.Run(WorkSimulator.SimulateWork));
await Task.WhenAll(tasks);
sw.Stop();
Console.WriteLine($"Task.Run x100: {sw.ElapsedMilliseconds} ms");

sw.Restart();
Parallel.ForEach(Enumerable.Range(0, 100), _ => WorkSimulator.SimulateWork());
sw.Stop();
Console.WriteLine($"Parallel.ForEach: {sw.ElapsedMilliseconds} ms");


// --- usage (goes in Program.cs) ---
Type type = typeof(Invoice);
Console.WriteLine($"Class: {type.Name}");

Console.WriteLine("Properties:");
foreach (var prop in type.GetProperties())
    Console.WriteLine($"  {prop.PropertyType.Name} {prop.Name}");

Console.WriteLine("Methods:");
foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
    Console.WriteLine($"  {method.Name}");

Console.WriteLine("Constructors:");
foreach (var ctor in type.GetConstructors())
{
    var parms = string.Join(", ", ctor.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}"));
    Console.WriteLine($"  Invoice({parms})");
}

object instance = Activator.CreateInstance(type)!;
PropertyInfo amountProp = type.GetProperty("Amount")!;
amountProp.SetValue(instance, 999.50m);
Console.WriteLine(((Invoice)instance).Amount); // 999.50


// --- usage (goes in Program.cs) ---
var user = new User { Name = "Alexandria Whistleblower" };
Validator.Validate(user); // WARNING: Name exceeds max length of 10 (was 24).

public static class WorkSimulator
{
    public static void SimulateWork() => Thread.Sleep(100);
}