using System;

public class Car:Vehicle
{
	public int NumberOfDoors { get; }



	public Car( string make,string model,int year,int doorsCount):base(make,model,year)
	{
    NumberOfDoors = doorsCount;
	}

	public override void DisplayInfo()
	{
    base.DisplayInfo();
	Console.WriteLine($"The car has {NumberOfDoors} doors");
	}
}
