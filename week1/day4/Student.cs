using System;

public class Student
{

	private int _age;
	public string Name { get; set; }
	public int Age {
		get => _age;
		set
		{
			if (value<5||value>100)
				throw new ArgumentException(nameof(value),"Age must be 5-100");
			_age = value;
		}
	
	}

	public const string SchoolName = "Psiog Digital";
	
	public readonly DateTime EnrolledOn;


	public Student(string name ,int age )
	{
		Name = name;
		Age = age;
		EnrolledOn = DateTime.Now;
	}

	public Student(string name) : this(name, 18) { }


	//method overlaoding

	public string CalculateGrade(int score) => score >= 60 ? "Pass" : "Fail";
	public string CalculateGrade(int score, int bonus) => CalculateGrade( score + bonus);



}

