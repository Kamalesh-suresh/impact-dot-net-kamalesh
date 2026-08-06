// ===================================================================
// Task 3.13 (part 1) — Interface vs Abstract class
// ===================================================================
//
// INHERITANCE
//   - A class can implement any number of interfaces, but can only
//     extend ONE base class (abstract or not). Interfaces are the
//     tool when a type needs several unrelated capabilities at once.
//
// STATE
//   - Interfaces cannot hold instance fields (auto-properties in an
//     interface are really just an abstract get/set pair — there is
//     no backing storage). An abstract class CAN declare fields and
//     properties with real backing storage that every derived type
//     inherits for free.
//
// CONSTRUCTORS
//   - Interfaces have no constructors; there's nothing to initialize
//     because there's no state. Abstract classes DO have constructors,
//     and every derived class's constructor implicitly (or explicitly
//     via `base(...)`) runs the abstract class's constructor first —
//     useful for enforcing invariants (e.g. "every Shape has a name").
//
// VERSIONING
//   - Adding a new member to an interface breaks every existing
//     implementer unless a default implementation is supplied
//     (C# 8+ default interface members soften this, but it's still
//     a bigger contract change).
//   - Adding a new concrete/virtual method to an abstract class does
//     NOT break existing derived classes — they simply inherit the
//     new behaviour. Only adding a new *abstract* member breaks them,
//     which puts the abstract-class author more in control of when
//     a breaking change actually happens.
//
// ---------------------------------------------------------------
// SCENARIO FAVOURING INTERFACE: unrelated types, no shared state,
// each type may need more than one contract at once.
// ---------------------------------------------------------------

public interface IExportable
{
    string ToExportFormat();
}

public interface IAuditable
{
    void RecordAccess(string who);
}

// Report and Invoice share no state or base implementation, and
// Report needs BOTH capabilities — impossible with single inheritance
// from a common abstract class, trivial with interfaces.
public class Report : IExportable, IAuditable
{
    public string Title { get; }
    public Report(string title) => Title = title;

    public string ToExportFormat() => $"[REPORT] {Title}";
    public void RecordAccess(string who) => Console.WriteLine($"Report '{Title}' accessed by {who}");
}

public class Invoice : IExportable
{
    public int Number { get; }
    public Invoice(int number) => Number = number;

    public string ToExportFormat() => $"[INVOICE] #{Number}";
}

// ---------------------------------------------------------------
// SCENARIO FAVOURING ABSTRACT CLASS: a family of closely related
// types that share state (Name) and a constructor invariant, plus
// a template method (Describe) built on top of each subclass's
// own Area() implementation.
// ---------------------------------------------------------------

public abstract class Shape
{
    public string Name { get; }

    protected Shape(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Shape must have a name.", nameof(name));
        Name = name; // shared state every subclass gets for free
    }

    public abstract double Area();

    // template method: shared behaviour built on the abstract piece
    public string Describe() => $"{Name}: area = {Area():F2}";
}

public class Circle : Shape
{
    private readonly double radius;
    public Circle(double radius) : base("Circle") => this.radius = radius;
    public override double Area() => Math.PI * radius * radius;
}

public class Rectangle : Shape
{
    private readonly double width, height;
    public Rectangle(double width, double height) : base("Rectangle")
    {
        this.width = width;
        this.height = height;
    }
    public override double Area() => width * height;
}
