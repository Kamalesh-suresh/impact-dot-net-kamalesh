var cart = new ShoppingCart();
cart.SetPaymentStrategy(new UpiPayment());
cart.Checkout(1500m); // via UPI

cart.SetPaymentStrategy(new CreditCardPayment()); // swapped at runtime
cart.Checkout(1500m); // via Credit Card -- ShoppingCart's own code never changed


IUnitOfWork uow = new UnitOfWork();
uow.Students.Add(new Student { Id = 1, Name = "Asha" });
uow.Courses.Add(new Course { Id = 1, Title = "C# Fundamentals" });
uow.Save();


var adapter = new XmlReportAdapter(new ThirdPartyXmlGenerator());
adapter.GenerateFromJson(new ReportData { Title = "Q1 Sales", Total = 48200m });

new OrderFacade().PlaceOrder("SKU-123", 2499m); // one call, three subsystems coordinated

Console.ReadKey();


public interface IPaymentStrategy
{
    void Pay(decimal amount);
}

public class CreditCardPayment : IPaymentStrategy
{
    public void Pay(decimal amount) => Console.WriteLine($"Paid {amount:C} via Credit Card");
}

public class UpiPayment : IPaymentStrategy
{
    public void Pay(decimal amount) => Console.WriteLine($"Paid {amount:C} via UPI");
}

public class NetBankingPayment : IPaymentStrategy
{
    public void Pay(decimal amount) => Console.WriteLine($"Paid {amount:C} via Net Banking");
}

public class ShoppingCart
{
    private IPaymentStrategy strategy;
    public void SetPaymentStrategy(IPaymentStrategy strategy) => this.strategy = strategy;
    public void Checkout(decimal amount) => strategy.Pay(amount);
}


