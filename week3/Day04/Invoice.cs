public class Invoice
{
    public int InvoiceNumber { get; set; }
    public decimal Amount { get; set; }
    public string CustomerName { get; set; } = string.Empty;

    public Invoice() { }
    public Invoice(int invoiceNumber, decimal amount)
    {
        InvoiceNumber = invoiceNumber;
        Amount = amount;
    }

    public void Print() => Console.WriteLine($"Invoice #{InvoiceNumber}: {Amount:C}");
}

