abstract class Shape
{
    public abstract double CalculateArea();
    public void DisplayArea() => Console.WriteLine($"Area :{CalculateArea():F2}");
}

class Circle : Shape
{
    public double Radius { get; init; }
    public override double CalculateArea() => Math.PI * Radius * Radius;
}

class Rectangle : Shape
{
    public double Width { get; init; }
    public double Height { get; init; }
    public override double CalculateArea() => Width * Height;
}