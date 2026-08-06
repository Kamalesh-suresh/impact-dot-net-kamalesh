public class VehicleFactoryTests
{
    [Theory]
    [InlineData("car", typeof(Car))]
    [InlineData("bike", typeof(Bike))]
    [InlineData("truck", typeof(Truck))]
    public void CreateVehicle_KnownType_ReturnsExpectedConcreteType(string type, Type expected)
    {
        var vehicle = VehicleFactory.CreateVehicle(type);
        Assert.IsType(expected, vehicle);
    }

    [Fact]
    public void CreateVehicle_UnknownType_Throws()
    {
        Assert.Throws<ArgumentException>(() => VehicleFactory.CreateVehicle("spaceship"));
    }

    [Fact]
    public void CarFactory_CreateVehicle_ReturnsCar()
    {
        VehicleFactoryBase factory = new CarFactory();
        Assert.IsType<Car>(factory.CreateVehicle());
    }

    [Fact]
    public void BikeFactory_CreateVehicle_ReturnsBike()
    {
        VehicleFactoryBase factory = new BikeFactory();
        Assert.IsType<Bike>(factory.CreateVehicle());
    }

    [Fact]
    public void DeliverVehicle_UsesSubclassCreateVehicle_AndDrivesIt()
    {
        VehicleFactoryBase factory = new CarFactory();

        var originalOut = Console.Out;
        using var writer = new StringWriter();
        Console.SetOut(writer);
        try
        {
            factory.DeliverVehicle();
        }
        finally
        {
            Console.SetOut(originalOut);
        }

        Assert.Contains("Driving a car", writer.ToString());
    }
}
