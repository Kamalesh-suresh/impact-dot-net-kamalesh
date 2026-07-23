

Console.WriteLine("Hello, World!");

var Kamal = new Employee("Kamal");


var a1 = new Address("1 Elm St", "Springfield", "12345");
var a2 = new Address("1 Elm St", "Springfield", "12345");
Console.WriteLine(a1 == a2);

var  arr1 = new[] { 1, 2, 3, 4 };
var  arr2 = new[] { 1, 2, 3, 4 };
Console.WriteLine(arr1 == arr2);


var a3 = a1 with { City = "Shelbyville" };
Console.WriteLine(a1 == a3);


var playlist = new Playlist();
playlist[0] = "Song A";
Console.WriteLine(playlist[0]);          // int indexer
Console.WriteLine(playlist["Song A"]);


Console.ReadKey();