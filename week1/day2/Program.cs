//using SchoolManagement;
////using A = ModuleA.Helper;
////using B = ModuleB.Helper;
//using ModuleA;
//using ModuleB;



////// Call style 1: WITH the using directive — short name works
////Student s1 = new Student();
////s1.Name = "Alex";
////Console.WriteLine("With using: " + s1.Name);

////// Call style 2: WITHOUT relying on the using directive — fully qualified name
////SchoolManagement.Student s2 = new SchoolManagement.Student();
////s2.Name = "Priya";
////Console.WriteLine("Without using: " + s2.Name);



//ModuleA.Helper.Greet();
//ModuleB.Helper.Greet();


//string @class= "chemistry";

//Console.WriteLine(@class);

//Console.ReadKey();



#define TRIAL_VERSION

using System;

#if TRIAL_VERSION
Console.WriteLine("Trial version limited features");
#else
Console.WriteLine("Full version all features");
#endif


var info = new AppInfo("1.0 trial");
info.PrintInfo();
Console.WriteLine(info.Version);
Console.ReadKey();