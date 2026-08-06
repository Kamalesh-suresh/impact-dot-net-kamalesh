public class PaymentStrategyTests
{
    private static string CaptureConsole(Action action)
    {
        var originalOut = Console.Out;
        using var writer = new StringWriter();
        Console.SetOut(writer);
        try
        {
            action();
        }
        finally
        {
            Console.SetOut(originalOut);
        }
        return writer.ToString();
    }

    [Fact]
    public void CreditCardPayment_Pay_WritesCreditCardMessage()
    {
        var output = CaptureConsole(() => new CreditCardPayment().Pay(100m));
        Assert.Contains("Credit Card", output);
    }

    [Fact]
    public void UpiPayment_Pay_WritesUpiMessage()
    {
        var output = CaptureConsole(() => new UpiPayment().Pay(100m));
        Assert.Contains("UPI", output);
    }

    [Fact]
    public void NetBankingPayment_Pay_WritesNetBankingMessage()
    {
        var output = CaptureConsole(() => new NetBankingPayment().Pay(100m));
        Assert.Contains("Net Banking", output);
    }

    [Fact]
    public void ShoppingCart_SwappingStrategyAtRuntime_ChangesCheckoutBehaviour()
    {
        var cart = new ShoppingCart();

        cart.SetPaymentStrategy(new UpiPayment());
        var firstOutput = CaptureConsole(() => cart.Checkout(500m));
        Assert.Contains("UPI", firstOutput);

        cart.SetPaymentStrategy(new CreditCardPayment()); // same cart, no code change, new behaviour
        var secondOutput = CaptureConsole(() => cart.Checkout(500m));
        Assert.Contains("Credit Card", secondOutput);
    }
}
