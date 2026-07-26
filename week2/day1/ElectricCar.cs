using System;

public class ElectricCar:Car
{
	public int Capacity { get; }

	public ElectricCar(string make,string model,int year,int doorsCount,int capacity):base(make, model, year, doorsCount)
	{
		Capacity = capacity;
	}

	public override void DisplayInfo()
	{
		base.DisplayInfo();
		Console.WriteLine($"The battery capacity is {Capacity} kwh");
	}
}
