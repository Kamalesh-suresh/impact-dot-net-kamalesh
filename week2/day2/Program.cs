//var s = new Shape();
var circle = new Circle { Radius = 2 };
Console.WriteLine(circle.CalculateArea());

var rectangle = new Rectangle { Width = 2, Height = 2 };
Console.WriteLine(rectangle.CalculateArea());


var shapes = new List<Shape> { new Circle { Radius = 2 }, new Rectangle { Width = 3, Height = 4 } };
foreach (var s in shapes)
    s.DisplayArea();


Logger a = new OverrideLogger();
a.Log("x");

Logger b = new HidingLogger();
b.Log("x");
((HidingLogger)b).Log("x");


var wallet = new Money(10m, "USD");
var bonus = new Money(5m, "USD");

// use the operators
Console.WriteLine(wallet + bonus);      // 15.00 USD   (operator +)
Console.WriteLine(wallet == bonus);     // False       (operator ==)
Console.WriteLine(wallet != bonus);     // True        (operator !=)
Console.WriteLine(wallet > bonus);      // True        (operator >)
Console.WriteLine(wallet < bonus);      // False       (operator <)

// create a third that equals the first
var same = new Money(10m, "USD");
Console.WriteLine(wallet == same);      // True
Console.WriteLine(wallet.Equals(same)); // True

// the mismatch case — this THROWS
var euros = new Money(10m, "EUR");
try
{
    var bad = wallet + euros;           // different currencies
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Blocked: {ex.Message}");   // Blocked: Currency mismatch
}


Console.ReadKey();

interface IShape
{
    double CalculateArea();
    double CalculatePerimeter();
}

interface IDrawable
{
    void Draw();
}

class Square : IShape, IDrawable
{
    public double Side { get; init; }
    public double CalculateArea() => Side * Side;
    public double CalculatePerimeter() => 4 * Side;
    public void Draw() => Console.WriteLine("◻ drawing a square");
}

class Logger
{
    public virtual void Log(string message)=>Console.WriteLine($"Logger {message} ");
}

class OverrideLogger : Logger
{
    public override void Log(string message)
    {
        Console.WriteLine($"OverrideLogger {message} ");
    }
}

class HidingLogger : Logger
{
    public new void Log(string message)
    {
        Console.WriteLine($"HidingLogger {message}");
    }
}
