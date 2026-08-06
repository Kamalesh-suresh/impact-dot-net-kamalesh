public class ReportData
{
    public string Title { get; set; }
    public decimal Total { get; set; }
}

public interface IXmlReportGenerator
{
    void GenerateFromXml(string xml);
}

public class ThirdPartyXmlGenerator : IXmlReportGenerator
{
    public void GenerateFromXml(string xml) => Console.WriteLine($"Generating report from XML:\n{xml}");
}

public class XmlReportAdapter
{
    private readonly IXmlReportGenerator generator;
    public XmlReportAdapter(IXmlReportGenerator generator) => this.generator = generator;

    public void GenerateFromJson(ReportData data)
    {
        string xml = $"<Report><Title>{data.Title}</Title><Total>{data.Total}</Total></Report>";
        generator.GenerateFromXml(xml); // adapts our shape to what the third party expects
    }
}

// --- Facade ---
public class InventoryService { public void ReserveStock(string sku) => Console.WriteLine($"Reserved stock for {sku}"); }
public class PaymentService { public void Charge(decimal amount) => Console.WriteLine($"Charged {amount:C}"); }
public class ShippingService { public void ScheduleDelivery(string sku) => Console.WriteLine($"Scheduled delivery for {sku}"); }

public class OrderFacade
{
    private readonly InventoryService inventory = new InventoryService();
    private readonly PaymentService payment = new PaymentService();
    private readonly ShippingService shipping = new ShippingService();

    public void PlaceOrder(string sku, decimal amount)
    {
        inventory.ReserveStock(sku);
        payment.Charge(amount);
        shipping.ScheduleDelivery(sku);
        Console.WriteLine("Order placed.");
    }
}

