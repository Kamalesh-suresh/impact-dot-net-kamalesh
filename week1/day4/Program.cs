
var x = 5;

//x = "hello";

dynamic y = "hello";

Console.WriteLine(y.GetType());

y = 25;
Console.WriteLine(y.GetType());

y=false;
Console.WriteLine(y.GetType());



(int Min , int Max) GetMinMax(int[] Numbers){
    return (Numbers.Min(), Numbers.Max());
}

var (min, max) = GetMinMax(new[] { 1, 2, 3, 4, 5 });
Console.WriteLine($"Min value is {min} and Max value is {max}");


(string Name, int Age , string Department )LookUpEmployeeById(int id)
{
    return ("kamalesh", 25, "Mac6");

}

var (name, age, department) = LookUpEmployeeById(1);
Console.WriteLine($"{name}, {age}, {department}");


  

string Describe(object value) => value switch
{
    int i => $"int: {i}",
    string s => $"string: {s}",
    double d => $"double: {d}",
    null => "nothing",
    _ => "unknown type"
};

Console.WriteLine(Describe(1));

string Grade(int score) => score switch
{
    >= 90 => "A",
    >= 80 => "B",
    >= 70 => "C",
    _ => "F"
};

Console.WriteLine(Grade(66));


decimal Discount(Order order) => order switch
{
    { Status: "VIP", Amount: > 100 } => order.Amount * 0.2m,
    { Status: "VIP" } => order.Amount * 0.1m,
    _ => 0
};

Console.WriteLine(Discount(new Order("VIP", 200)));

var Kamalesh = new Student("Kamalesh");
Console.WriteLine( $"{Kamalesh.Name} age is {Kamalesh.Age} ");

Console.ReadKey();


