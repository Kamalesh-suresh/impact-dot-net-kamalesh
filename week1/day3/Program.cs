//using System.Runtime.CompilerServices;

//int a = 10;
//int b = a;

//b = 11;
//Console.WriteLine($"{a} value is untouched as {b} is only changed");

//int[] arr1 = {1,2 };
//int[] arr2 = arr1;

//arr1[0] = 3;
//Console.WriteLine(arr1[0]);

//var s1 = new CoordS { X = 1, Y = 2 };
//var s2 = s1;
//s2.X = 99;
//Console.WriteLine(s1.X);


//var c1 = new CoordC { X = 1, Y = 2 };
//var c2 = c1;
//c2.X = 99;
//Console.WriteLine(c2.X);




//int dayNumber = 3;
//Console.WriteLine((DaysOfWeek)dayNumber);


var perms = FilePermission.Read | FilePermission.Write;
Console.WriteLine(perms);

bool canWrite = (perms & FilePermission.Write) == FilePermission.Write; 
Console.WriteLine(canWrite);


double applyDiscount (double? discount)
{
    double actual = discount ?? 0.5;
    return actual;
}

Console.WriteLine(applyDiscount(null));

int i = 42;
long l = i;      
float f = l;    
double d = f;   

double precise = 3.99;
int truncated = (int)precise;   

Console.WriteLine($"{precise} -> {truncated} (0.99 lost, not rounded to 4)");

object o = "123";

if (o is string s)
{
    Console.WriteLine(s);
}

string? maybe = o as string;

int viaConvert = Convert.ToInt32("123");            
bool ok = int.TryParse("abc", out int viaTryParse);  


Console.ReadLine();
