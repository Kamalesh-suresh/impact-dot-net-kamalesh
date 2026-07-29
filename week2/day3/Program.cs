static double Add(double a, double b) => a + b;
static double Multiply(double a , double b) => a * b;

MathOperation op = Add;
Console.WriteLine(op(2, 3));

MathOperation multi = Add;
multi = Multiply;
Console.WriteLine(multi(2, 3));


Func<double, double, double> Sum=( a,  b) => a + b;


var clock = new AlarmClock();
clock.OnAlarmRing += (sender, e) => Console.WriteLine($" Person wakes  → {e.AlarmTime:T}");
clock.OnAlarmRing += (sender, e) => Console.WriteLine($" Coffee brews  → {e.AlarmTime:T}");
clock.Ring();

static void ProcessList(List<int> list, Predicate<int> filter,
                        Func<int, int> transform, Action<int> output)
{
    foreach (var n in list)
        if (filter(n))
            output(transform(n));
}

ProcessList(
    new List<int> { 1, 2, 3, 4, 5, 6 },
    n => n % 2 == 0,                 // keep evens
    n => n * n,                      // square them
    n => Console.WriteLine(n));
Console.ReadKey();


delegate double MathOperation(double a, double b);

class AlarmEventArgs : EventArgs          // carry data to subscribers
{
    public DateTime AlarmTime { get; }
    public AlarmEventArgs(DateTime t) => AlarmTime = t;
}

class AlarmClock
{
    public event EventHandler<AlarmEventArgs>? OnAlarmRing;   // the event

    public void Ring() =>
        OnAlarmRing?.Invoke(this, new AlarmEventArgs(DateTime.Now));   // ?. = no subscribers, no crash
}




