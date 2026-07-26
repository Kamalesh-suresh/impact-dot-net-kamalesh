using System;

public class Vehicle
{
	public string Make { get; }
	public string Model { get; }
	public int Year { get; }

	public Vehicle(string make,string model , int year)
	{
		Make = make; Model = model; Year = year;
		Console.WriteLine("Vehicle added");
	}

	public virtual void DisplayInfo()
	{
		Console.WriteLine($"{Year}-{Make}-{Model}");
	}
}



