namespace Patterns_Example.Structural_Design_Patterns.Decorator;

/// <summary>
/// Coffee shop class that demonstrates the Decorator pattern in action.
/// Manages orders and provides a menu system for customizing beverages.
/// </summary>
public class CoffeeShop
{
    private readonly List<Order> _orders;
    private readonly Menu _menu;
    private int _nextOrderNumber;

    public string Name { get; }
    public List<Order> Orders => new List<Order>(_orders);

    public CoffeeShop(string name)
    {
        Name = name;
        _orders = new List<Order>();
        _menu = new Menu();
        _nextOrderNumber = 1;
    }

    /// <summary>
    /// Creates a new order and returns the order builder.
    /// </summary>
    public OrderBuilder CreateOrder(string customerName)
    {
        var order = new Order(_nextOrderNumber++, customerName);
        _orders.Add(order);
        return new OrderBuilder(order, this);
    }

    /// <summary>
    /// Gets the menu with available beverages and customizations.
    /// </summary>
    public Menu GetMenu()
    {
        return _menu;
    }

    /// <summary>
    /// Processes an order (simulates preparation).
    /// </summary>
    public async Task<OrderResult> ProcessOrderAsync(Order order)
    {
        Console.WriteLine($"\n☕ Processing Order #{order.OrderNumber} for {order.CustomerName}");
        Console.WriteLine(new string('=', 50));

        var orderResult = new OrderResult
        {
            Order = order,
            StartTime = DateTime.Now,
            BeverageResults = new List<BeveragePreparationResult>()
        };

        foreach (var beverage in order.Beverages)
        {
            var beverageResult = await PrepareBeverageAsync(beverage);
            orderResult.BeverageResults.Add(beverageResult);
        }

        orderResult.CompletionTime = DateTime.Now;
        orderResult.TotalPreparationTime = orderResult.CompletionTime - orderResult.StartTime;

        Console.WriteLine($"\n✅ Order #{order.OrderNumber} completed!");
        Console.WriteLine($"Total preparation time: {orderResult.TotalPreparationTime.TotalMinutes:F1} minutes");
        Console.WriteLine($"Total cost: ${order.TotalPrice:F2}");

        return orderResult;
    }

    /// <summary>
    /// Simulates preparing a single beverage.
    /// </summary>
    private async Task<BeveragePreparationResult> PrepareBeverageAsync(IBeverage beverage)
    {
        Console.WriteLine($"\n🔧 Preparing: {beverage.Name}");
        
        var result = new BeveragePreparationResult
        {
            Beverage = beverage,
            StartTime = DateTime.Now
        };

        // Show preparation steps
        var preparationSteps = beverage.Prepare().Split('\n');
        foreach (var step in preparationSteps)
        {
            if (!string.IsNullOrWhiteSpace(step))
            {
                Console.WriteLine($"   {step}");
                await Task.Delay(200); // Simulate preparation time
            }
        }

        // Simulate actual preparation time
        var preparationTime = beverage.GetPreparationTime();
        var simulatedTime = TimeSpan.FromMilliseconds(preparationTime.TotalMilliseconds / 10); // Speed up for demo
        await Task.Delay(simulatedTime);

        result.CompletionTime = DateTime.Now;
        result.ActualPreparationTime = result.CompletionTime - result.StartTime;

        Console.WriteLine($"   ✓ {beverage.Name} ready! ({result.ActualPreparationTime.TotalSeconds:F1}s)");

        return result;
    }

    /// <summary>
    /// Gets sales statistics for the coffee shop.
    /// </summary>
    public SalesStatistics GetSalesStatistics()
    {
        var stats = new SalesStatistics();
        
        foreach (var order in _orders)
        {
            stats.TotalOrders++;
            stats.TotalRevenue += order.TotalPrice;
            stats.TotalBeverages += order.Beverages.Count;

            foreach (var beverage in order.Beverages)
            {
                // Count base beverage types
                var baseType = GetBaseBeverageType(beverage);
                stats.BeverageTypeCount[baseType] = stats.BeverageTypeCount.GetValueOrDefault(baseType, 0) + 1;

                // Count decorators used
                var decorators = GetDecoratorsUsed(beverage);
                foreach (var decorator in decorators)
                {
                    stats.DecoratorUsageCount[decorator] = stats.DecoratorUsageCount.GetValueOrDefault(decorator, 0) + 1;
                }

                // Size statistics
                stats.SizeCount[beverage.Size] = stats.SizeCount.GetValueOrDefault(beverage.Size, 0) + 1;
            }
        }

        stats.AverageOrderValue = stats.TotalOrders > 0 ? stats.TotalRevenue / stats.TotalOrders : 0;
        stats.AverageBeveragesPerOrder = stats.TotalOrders > 0 ? (double)stats.TotalBeverages / stats.TotalOrders : 0;

        return stats;
    }

    private string GetBaseBeverageType(IBeverage beverage)
    {
        // Unwrap decorators to find the base beverage type
        var current = beverage;
        while (current is BeverageDecorator decorator)
        {
            current = decorator._beverage;
        }
        return current.GetType().Name;
    }

    private List<string> GetDecoratorsUsed(IBeverage beverage)
    {
        var decorators = new List<string>();
        var current = beverage;
        
        while (current is BeverageDecorator decorator)
        {
            decorators.Add(decorator.GetType().Name.Replace("Decorator", ""));
            current = decorator._beverage;
        }
        
        return decorators;
    }
}

/// <summary>
/// Represents a customer order.
/// </summary>
public class Order
{
    public int OrderNumber { get; }
    public string CustomerName { get; }
    public DateTime OrderTime { get; }
    public List<IBeverage> Beverages { get; }
    public decimal TotalPrice => Beverages.Sum(b => b.Price);
    public int TotalCalories => Beverages.Sum(b => b.CaloriesPerServing);

    public Order(int orderNumber, string customerName)
    {
        OrderNumber = orderNumber;
        CustomerName = customerName;
        OrderTime = DateTime.Now;
        Beverages = new List<IBeverage>();
    }

    public void AddBeverage(IBeverage beverage)
    {
        Beverages.Add(beverage);
    }

    public string GetReceipt()
    {
        var receipt = $"""
            ☕ {DateTime.Now:yyyy-MM-dd HH:mm:ss}
            Order #{OrderNumber} - {CustomerName}
            {new string('=', 30)}
            
            """;

        foreach (var beverage in Beverages)
        {
            receipt += beverage.GetReceipt() + "\n";
        }

        receipt += $"""
            {new string('-', 30)}
            Total: ${TotalPrice:F2}
            Total Calories: {TotalCalories}
            
            Thank you for your order!
            """;

        return receipt;
    }
}

/// <summary>
/// Builder class for creating orders with fluent interface.
/// </summary>
public class OrderBuilder
{
    private readonly Order _order;
    private readonly CoffeeShop _coffeeShop;

    public OrderBuilder(Order order, CoffeeShop coffeeShop)
    {
        _order = order;
        _coffeeShop = coffeeShop;
    }

    public BeverageBuilder AddBeverage(IBeverage baseBeverage)
    {
        return new BeverageBuilder(baseBeverage, this);
    }

    public OrderBuilder AddCompleteBeverage(IBeverage beverage)
    {
        _order.AddBeverage(beverage);
        return this;
    }

    public Order Build()
    {
        return _order;
    }

    public async Task<OrderResult> ProcessAsync()
    {
        return await _coffeeShop.ProcessOrderAsync(_order);
    }

    internal void AddBeverageToOrder(IBeverage beverage)
    {
        _order.AddBeverage(beverage);
    }
}

/// <summary>
/// Builder class for customizing beverages with decorators.
/// </summary>
public class BeverageBuilder
{
    private IBeverage _beverage;
    private readonly OrderBuilder _orderBuilder;

    public BeverageBuilder(IBeverage baseBeverage, OrderBuilder orderBuilder)
    {
        _beverage = baseBeverage;
        _orderBuilder = orderBuilder;
    }

    public BeverageBuilder WithSize(BeverageSize size)
    {
        _beverage = _beverage.WithSize(size);
        return this;
    }

    public BeverageBuilder AddMilk()
    {
        _beverage = new MilkDecorator(_beverage);
        return this;
    }

    public BeverageBuilder AddSoyMilk()
    {
        _beverage = new SoyMilkDecorator(_beverage);
        return this;
    }

    public BeverageBuilder AddCoconutMilk()
    {
        _beverage = new CoconutMilkDecorator(_beverage);
        return this;
    }

    public BeverageBuilder AddWhippedCream()
    {
        _beverage = new WhippedCreamDecorator(_beverage);
        return this;
    }

    public BeverageBuilder AddChocolateSyrup()
    {
        _beverage = new ChocolateSyrupDecorator(_beverage);
        return this;
    }

    public BeverageBuilder AddVanillaSyrup()
    {
        _beverage = new VanillaSyrupDecorator(_beverage);
        return this;
    }

    public BeverageBuilder AddCaramelSyrup()
    {
        _beverage = new CaramelSyrupDecorator(_beverage);
        return this;
    }

    public BeverageBuilder AddCinnamon()
    {
        _beverage = new CinnamonDecorator(_beverage);
        return this;
    }

    public BeverageBuilder AddExtraShot()
    {
        _beverage = new ExtraEspressoShotDecorator(_beverage);
        return this;
    }

    public BeverageBuilder AddHoney()
    {
        _beverage = new HoneyDecorator(_beverage);
        return this;
    }

    public BeverageBuilder MakeIced()
    {
        _beverage = new IcedDecorator(_beverage);
        return this;
    }

    public BeverageBuilder MakeDecaf()
    {
        _beverage = new DecafDecorator(_beverage);
        return this;
    }

    public BeverageBuilder MakeExtraHot()
    {
        _beverage = new ExtraHotDecorator(_beverage);
        return this;
    }

    public BeverageBuilder MakeHalfCaf()
    {
        _beverage = new HalfCafDecorator(_beverage);
        return this;
    }

    public BeverageBuilder MakeDoubleStrength()
    {
        _beverage = new DoubleStrengthDecorator(_beverage);
        return this;
    }

    public BeverageBuilder AddFoamArt(string pattern = "heart")
    {
        _beverage = new FoamArtDecorator(_beverage, pattern);
        return this;
    }

    public BeverageBuilder MakeOrganic()
    {
        _beverage = new OrganicDecorator(_beverage);
        return this;
    }

    public OrderBuilder AddToOrder()
    {
        _orderBuilder.AddBeverageToOrder(_beverage);
        return _orderBuilder;
    }

    public IBeverage Build()
    {
        return _beverage;
    }
}

/// <summary>
/// Menu class containing available beverages and customizations.
/// </summary>
public class Menu
{
    public List<IBeverage> BaseBeverages { get; }
    public List<string> AvailableAddOns { get; }
    public List<string> AvailablePreparations { get; }

    public Menu()
    {
        BaseBeverages = new List<IBeverage>
        {
            new Espresso(),
            new HouseBlend(),
            new GreenTea(),
            new HotChocolate()
        };

        AvailableAddOns = new List<string>
        {
            "Milk (+$0.60)", "Soy Milk (+$0.65)", "Coconut Milk (+$0.70)",
            "Whipped Cream (+$0.75)", "Chocolate Syrup (+$0.50)", "Vanilla Syrup (+$0.50)",
            "Caramel Syrup (+$0.55)", "Cinnamon (+$0.25)", "Extra Shot (+$1.00)", "Honey (+$0.40)"
        };

        AvailablePreparations = new List<string>
        {
            "Iced", "Decaf", "Extra Hot", "Half-Caf", "Double Strength (+$0.50)",
            "Foam Art (+$1.00)", "Organic (+$0.75)"
        };
    }

    public void DisplayMenu()
    {
        Console.WriteLine("☕ COFFEE SHOP MENU ☕");
        Console.WriteLine(new string('=', 40));
        
        Console.WriteLine("\nBase Beverages:");
        foreach (var beverage in BaseBeverages)
        {
            Console.WriteLine($"  {beverage.Name} - ${beverage.Price:F2} ({beverage.CaloriesPerServing} cal)");
            Console.WriteLine($"    {beverage.Description}");
        }

        Console.WriteLine("\nAdd-Ons:");
        foreach (var addOn in AvailableAddOns)
        {
            Console.WriteLine($"  {addOn}");
        }

        Console.WriteLine("\nPreparation Options:");
        foreach (var prep in AvailablePreparations)
        {
            Console.WriteLine($"  {prep}");
        }

        Console.WriteLine("\nSizes Available:");
        foreach (BeverageSize size in Enum.GetValues<BeverageSize>())
        {
            Console.WriteLine($"  {size.GetDisplayName()} (×{size.GetSizeMultiplier():F1} price)");
        }
    }
}

/// <summary>
/// Result of processing an order.
/// </summary>
public class OrderResult
{
    public Order Order { get; set; } = null!;
    public DateTime StartTime { get; set; }
    public DateTime CompletionTime { get; set; }
    public TimeSpan TotalPreparationTime { get; set; }
    public List<BeveragePreparationResult> BeverageResults { get; set; } = new();
}

/// <summary>
/// Result of preparing a single beverage.
/// </summary>
public class BeveragePreparationResult
{
    public IBeverage Beverage { get; set; } = null!;
    public DateTime StartTime { get; set; }
    public DateTime CompletionTime { get; set; }
    public TimeSpan ActualPreparationTime { get; set; }
}

/// <summary>
/// Sales statistics for the coffee shop.
/// </summary>
public class SalesStatistics
{
    public int TotalOrders { get; set; }
    public int TotalBeverages { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal AverageOrderValue { get; set; }
    public double AverageBeveragesPerOrder { get; set; }
    public Dictionary<string, int> BeverageTypeCount { get; set; } = new();
    public Dictionary<string, int> DecoratorUsageCount { get; set; } = new();
    public Dictionary<BeverageSize, int> SizeCount { get; set; } = new();

    public override string ToString()
    {
        var beverageBreakdown = string.Join(", ", BeverageTypeCount.Select(kvp => $"{kvp.Key}: {kvp.Value}"));
        var decoratorBreakdown = string.Join(", ", DecoratorUsageCount.OrderByDescending(kvp => kvp.Value).Take(5).Select(kvp => $"{kvp.Key}: {kvp.Value}"));
        var sizeBreakdown = string.Join(", ", SizeCount.Select(kvp => $"{kvp.Key}: {kvp.Value}"));

        return $"""
            📊 Sales Statistics:
              Total Orders: {TotalOrders}
              Total Beverages: {TotalBeverages}
              Total Revenue: ${TotalRevenue:F2}
              Average Order Value: ${AverageOrderValue:F2}
              Average Beverages per Order: {AverageBeveragesPerOrder:F1}
              
              Beverage Types: {beverageBreakdown}
              Popular Add-ons: {decoratorBreakdown}
              Size Distribution: {sizeBreakdown}
            """;
    }
}
