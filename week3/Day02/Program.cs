var threads = new List<Thread>();
for (int i = 0; i < 5; i++)
{
    var t = new Thread(() => Logger.Instance.Log("from a Thread"));
    threads.Add(t);
    t.Start();
}
threads.ForEach(t => t.Join());

var tasks = Enumerable.Range(0, 5)
    .Select(_ => Task.Run(() => Logger.Instance.Log("from a Task")));
await Task.WhenAll(tasks);
// every printed [hashcode] is identical -> proves it's one shared instance



// --- usage (goes in Program.cs) ---
IVehicle v = VehicleFactory.CreateVehicle("car");
v.Drive();

VehicleFactoryBase factory = new CarFactory();
factory.DeliverVehicle(); // caller never writes `new Car()` anywhere


// --- usage (goes in Program.cs) ---
var ticker = new StockTicker();
ticker.Subscribe(new Investor("Asha"));
ticker.Subscribe(new Investor("Ravi"));
ticker.SetPrice("MSFT", 415.20m);

var tickerEvents = new StockTickerEvents();
tickerEvents.PriceChanged += (symbol, price) => Console.WriteLine($"Asha notified: {symbol} is now {price:C}");
tickerEvents.PriceChanged += (symbol, price) => Console.WriteLine($"Ravi notified: {symbol} is now {price:C}");
tickerEvents.SetPrice("MSFT", 415.20m);

/* Comparison: the custom IObserver interface makes the subscriber contract
   explicit -- you can see exactly what "being an observer" requires just by
   reading the interface, and it naturally supports things like Unsubscribe().
   The event version is much less boilerplate for a simple case, and totally
   unrelated code can subscribe with a one-line lambda with no class needed
   at all. Events are what most real C# codebases reach for by default. */

Console.ReadKey();






public sealed class Logger
{
    private static readonly Lazy<Logger> instance = new Lazy<Logger>(() => new Logger());
    public static Logger Instance => instance.Value;

    private Logger() { } // private constructor: nobody outside can `new Logger()`

    public void Log(string message) => Console.WriteLine($"[{GetHashCode()}] {message}");
}


public interface IVehicle
{
    void Drive();
}

public class Car : IVehicle { public void Drive() => Console.WriteLine("Driving a car"); }
public class Bike : IVehicle { public void Drive() => Console.WriteLine("Riding a bike"); }
public class Truck : IVehicle { public void Drive() => Console.WriteLine("Hauling with a truck"); }

// --- simple factory ---
public static class VehicleFactory
{
    public static IVehicle CreateVehicle(string type) => type switch
    {
        "car" => new Car(),
        "bike" => new Bike(),
        "truck" => new Truck(),
        _ => throw new ArgumentException($"Unknown vehicle type: {type}")
    };
}

// --- Factory Method ---
public abstract class VehicleFactoryBase
{
    public abstract IVehicle CreateVehicle();

    public void DeliverVehicle()
    {
        var vehicle = CreateVehicle(); // subclass decides *what* gets built
        vehicle.Drive();
    }
}

public class CarFactory : VehicleFactoryBase
{
    public override IVehicle CreateVehicle() => new Car();
}

public class BikeFactory : VehicleFactoryBase
{
    public override IVehicle CreateVehicle() => new Bike();
}



// --- version 1: custom interface ---
public interface IStockObserver
{
    void OnPriceChanged(string symbol, decimal newPrice);
}

public class StockTicker
{
    private readonly List<IStockObserver> observers = new List<IStockObserver>();
    public void Subscribe(IStockObserver observer) => observers.Add(observer);

    public void SetPrice(string symbol, decimal price)
    {
        foreach (var observer in observers)
            observer.OnPriceChanged(symbol, price);
    }
}

public class Investor : IStockObserver
{
    public string Name { get; }
    public Investor(string name) => Name = name;
    public void OnPriceChanged(string symbol, decimal newPrice) =>
        Console.WriteLine($"{Name} notified: {symbol} is now {newPrice:C}");
}

// --- version 2: plain C# events ---
public class StockTickerEvents
{
    public event Action<string, decimal> PriceChanged;
    public void SetPrice(string symbol, decimal price) => PriceChanged?.Invoke(symbol, price);
}


