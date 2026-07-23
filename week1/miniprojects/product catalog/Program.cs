using System.Linq;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var products = new List<Product>
{
    new Product { Name = "Laptop",  Price = 1200m, Category = Category.Electronics },
    new Product { Name = "Phone",   Price = 800m,  Category = Category.Electronics },
    new Product { Name = "Bread",   Price = 3m,    Category = Category.Food },
    new Product { Name = "Apples",  Price = 4m,    Category = Category.Food },
    new Product { Name = "T-Shirt", Price = 15m,   Category = Category.Clothing }
};

var grouped = products.GroupBy(p => p.Category);

foreach (var group in grouped)
{
    Console.WriteLine(group.Key);
    foreach (var p in group)
        Console.WriteLine($"  {p.Name} - {p.Price:C}");
}

Console.ReadKey();
