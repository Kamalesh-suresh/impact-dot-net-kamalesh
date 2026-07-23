




static double ToCelsius(double value, Unit from) => from switch
{
    Unit.Celsius => value,
    Unit.Fahrenheit => (value - 32) * 5 / 9,
    Unit.Kelvin => value - 273.15,
    _ => throw new ArgumentException("Unknown unit")
};


static double FromCelsius(double celsius, Unit to)=> to switch
{
    Unit.Celsius    => celsius,
    Unit.Fahrenheit => celsius * 9 / 5 + 32,
    Unit.Kelvin     => celsius + 273.15,
    _               => throw new ArgumentException("Unknown unit")


};


static double Convert(double value, Unit from, Unit to) => FromCelsius(ToCelsius(value, from), to);


double f = Convert(100, Unit.Celsius, Unit.Fahrenheit);
Console.WriteLine($"100°C = {f}°F");          // 212

double c = Convert(32, Unit.Fahrenheit, Unit.Celsius);
Console.WriteLine($"32°F = {c}°C");           // 0

double k = Convert(0, Unit.Celsius, Unit.Kelvin);
Console.WriteLine($"0°C = {k}K");             // 273.15



static(double , double ) ConvertToOthers(double value, Unit from)
{
    double c = ToCelsius( value,  from);
    return from switch
    {
        Unit.Celsius => (FromCelsius(c, Unit.Fahrenheit), FromCelsius(c, Unit.Kelvin)),
        Unit.Fahrenheit => (FromCelsius(c, Unit.Celsius), FromCelsius(c, Unit.Kelvin)),
        Unit.Kelvin => (FromCelsius(c, Unit.Celsius), FromCelsius(c, Unit.Fahrenheit)),
        _ => throw new ArgumentException("Unknown unit")
    };
}

var (both1, both2) = ConvertToOthers(25, Unit.Celsius);
Console.WriteLine($"25°C = {both1}°F and {both2}K");

Console.ReadKey();


