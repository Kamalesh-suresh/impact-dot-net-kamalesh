public class OrderProcessorTests
{
    [Fact]
    public void AddOrder_AccumulatesTotalAcrossCalls()
    {
        var processor = new OrderProcessor("Asha");
        processor.AddOrder(250m);
        processor.AddOrder(75.50m);

        Assert.Equal(2, processor.OrderCount);
        Assert.Equal(325.50m, processor.GetTotal());
    }

    [Fact]
    public void AddOrder_NegativeAmount_Throws()
    {
        var processor = new OrderProcessor("Asha");
        Assert.Throws<ArgumentOutOfRangeException>(() => processor.AddOrder(-1m));
    }

    [Fact]
    public void ApplyDiscount_ReducesTotalByPercent()
    {
        var processor = new OrderProcessor("Asha");
        processor.AddOrder(200m);

        Assert.Equal(180m, processor.ApplyDiscount(10));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    public void ApplyDiscount_OutOfRangePercent_Throws(decimal percent)
    {
        var processor = new OrderProcessor("Asha");
        processor.AddOrder(100m);
        Assert.Throws<ArgumentOutOfRangeException>(() => processor.ApplyDiscount(percent));
    }

    [Fact]
    public void TwoInstances_TrackIndependentState()
    {
        var asha = new OrderProcessor("Asha");
        var ravi = new OrderProcessor("Ravi");
        asha.AddOrder(100m);

        Assert.Equal(1, asha.OrderCount);
        Assert.Equal(0, ravi.OrderCount); // proves state lives on the instance, not shared globally
    }
}
