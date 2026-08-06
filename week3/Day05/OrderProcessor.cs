public class OrderProcessor
{
    private readonly List<decimal> orderAmounts = new();

    public string CustomerName { get; }

    public OrderProcessor(string customerName) => CustomerName = customerName;

    public void AddOrder(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Order amount cannot be negative.");
        orderAmounts.Add(amount);
    }

    public int OrderCount => orderAmounts.Count;

    public decimal GetTotal() => orderAmounts.Sum();

    public decimal ApplyDiscount(decimal percent)
    {
        if (percent is < 0 or > 100)
            throw new ArgumentOutOfRangeException(nameof(percent), "Discount must be between 0 and 100.");
        return GetTotal() * (1 - percent / 100m);
    }
}
