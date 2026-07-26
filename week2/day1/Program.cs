
Console.OutputEncoding = System.Text.Encoding.UTF8;

var account = new BankAccount();
account.Deposit(100);
account.Withdraw(20);
account.GetHistory();


var mazda = new Vehicle("Mazda", "Toyota", 2024);
mazda.DisplayInfo();
Console.ReadKey();

var lancer = new Car("Lancer", "Toyota", 2024,4);
lancer.DisplayInfo();
Console.ReadKey();


var tesla = new ElectricCar("GT600", "Tesla", 2024, 4,5);
tesla.DisplayInfo();
Console.ReadKey();