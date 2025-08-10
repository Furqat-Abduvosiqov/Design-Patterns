namespace Patterns_Example.Structural_Design_Patterns.Decorator;

/// <summary>
/// Example class demonstrating the Decorator pattern usage
/// </summary>
public static class DecoratorExample
{
    public static async Task RunExample()
    {
        Console.WriteLine("=== Decorator Pattern Example: Coffee Shop Ordering System ===\n");
        
        // Demonstrate basic decorator usage
        DemonstrateBasicDecorators();
        
        // Demonstrate multiple decorators
        DemonstrateMultipleDecorators();
        
        // Demonstrate different decorator types
        DemonstrateDifferentDecoratorTypes();
        
        // Demonstrate coffee shop ordering system
        await DemonstrateCoffeeShopSystem();
        
        // Demonstrate nutritional information and dietary restrictions
        DemonstrateNutritionalInfo();
        
        Console.WriteLine("=== Decorator Pattern Benefits Demonstrated ===");
        Console.WriteLine("✓ Dynamic addition of responsibilities to objects");
        Console.WriteLine("✓ Flexible alternative to subclassing for extending functionality");
        Console.WriteLine("✓ Composition over inheritance principle");
        Console.WriteLine("✓ Open/Closed principle - open for extension, closed for modification");
        Console.WriteLine("✓ Runtime configuration of object behavior");
        Console.WriteLine("✓ Transparent interface - decorators look like the original object");
    }

    private static void DemonstrateBasicDecorators()
    {
        Console.WriteLine("1. Basic Decorator Usage:");
        Console.WriteLine(new string('=', 50));
        
        // Start with a simple espresso
        IBeverage beverage = new Espresso();
        Console.WriteLine($"Base beverage: {beverage.Name}");
        Console.WriteLine($"Price: ${beverage.Price:F2}");
        Console.WriteLine($"Calories: {beverage.CaloriesPerServing}");
        Console.WriteLine();
        
        // Add milk
        beverage = new MilkDecorator(beverage);
        Console.WriteLine($"After adding milk: {beverage.Name}");
        Console.WriteLine($"Price: ${beverage.Price:F2}");
        Console.WriteLine($"Calories: {beverage.CaloriesPerServing}");
        Console.WriteLine();
        
        // Add whipped cream
        beverage = new WhippedCreamDecorator(beverage);
        Console.WriteLine($"After adding whipped cream: {beverage.Name}");
        Console.WriteLine($"Price: ${beverage.Price:F2}");
        Console.WriteLine($"Calories: {beverage.CaloriesPerServing}");
        Console.WriteLine();
        
        // Show detailed description
        Console.WriteLine("Detailed description:");
        Console.WriteLine(beverage.GetDetailedDescription());
        Console.WriteLine();
    }

    private static void DemonstrateMultipleDecorators()
    {
        Console.WriteLine("2. Multiple Decorators Chaining:");
        Console.WriteLine(new string('=', 50));
        
        // Create a complex beverage with multiple decorators
        IBeverage complexBeverage = new HouseBlend();
        complexBeverage = new MilkDecorator(complexBeverage);
        complexBeverage = new VanillaSyrupDecorator(complexBeverage);
        complexBeverage = new CaramelSyrupDecorator(complexBeverage);
        complexBeverage = new WhippedCreamDecorator(complexBeverage);
        complexBeverage = new CinnamonDecorator(complexBeverage);
        complexBeverage = complexBeverage.WithSize(BeverageSize.Large);
        
        Console.WriteLine($"Complex beverage: {complexBeverage.Name}");
        Console.WriteLine($"Final price: ${complexBeverage.Price:F2}");
        Console.WriteLine($"Total calories: {complexBeverage.CaloriesPerServing}");
        Console.WriteLine();
        
        Console.WriteLine("Ingredients:");
        foreach (var ingredient in complexBeverage.Ingredients)
        {
            Console.WriteLine($"  - {ingredient}");
        }
        Console.WriteLine();
        
        Console.WriteLine("Preparation steps:");
        Console.WriteLine(complexBeverage.Prepare());
        Console.WriteLine();
        
        Console.WriteLine("Receipt:");
        Console.WriteLine(complexBeverage.GetReceipt());
        Console.WriteLine();
    }

    private static void DemonstrateDifferentDecoratorTypes()
    {
        Console.WriteLine("3. Different Types of Decorators:");
        Console.WriteLine(new string('=', 50));
        
        // Demonstrate add-on decorators
        Console.WriteLine("Add-on Decorators (ingredients):");
        IBeverage espresso1 = new Espresso();
        espresso1 = new ExtraEspressoShotDecorator(espresso1);
        espresso1 = new HoneyDecorator(espresso1);
        Console.WriteLine($"  {espresso1.Name} - ${espresso1.Price:F2}");
        Console.WriteLine();

        // Demonstrate preparation decorators
        Console.WriteLine("Preparation Decorators (how it's made):");
        IBeverage espresso2 = new Espresso();
        espresso2 = new IcedDecorator(espresso2);
        espresso2 = new OrganicDecorator(espresso2);
        espresso2 = new DoubleStrengthDecorator(espresso2);
        Console.WriteLine($"  {espresso2.Name} - ${espresso2.Price:F2}");
        Console.WriteLine();

        // Demonstrate mixed decorators
        Console.WriteLine("Mixed Decorators:");
        IBeverage greenTea = new GreenTea();
        greenTea = new HoneyDecorator(greenTea); // Add-on
        greenTea = new IcedDecorator(greenTea); // Preparation
        greenTea = new OrganicDecorator(greenTea); // Preparation
        greenTea = greenTea.WithSize(BeverageSize.ExtraLarge);
        Console.WriteLine($"  {greenTea.Name} - ${greenTea.Price:F2}");
        Console.WriteLine($"  Caffeine: {greenTea.GetNutritionalInfo().Caffeine:F1}mg");
        Console.WriteLine();
    }

    private static async Task DemonstrateCoffeeShopSystem()
    {
        Console.WriteLine("4. Coffee Shop Ordering System:");
        Console.WriteLine(new string('=', 50));
        
        var coffeeShop = new CoffeeShop("Decorator Café");
        
        // Show menu
        Console.WriteLine("Displaying menu:");
        coffeeShop.GetMenu().DisplayMenu();
        Console.WriteLine();
        
        // Create and process some orders
        Console.WriteLine("Creating customer orders...");
        
        // Order 1: Simple order
        var order1 = coffeeShop.CreateOrder("Alice")
            .AddBeverage(new Espresso())
                .WithSize(BeverageSize.Small)
                .AddMilk()
                .AddToOrder()
            .Build();
        
        // Order 2: Complex order
        var order2 = coffeeShop.CreateOrder("Bob")
            .AddBeverage(new HouseBlend())
                .WithSize(BeverageSize.Large)
                .AddSoyMilk()
                .AddVanillaSyrup()
                .AddExtraShot()
                .MakeIced()
                .AddToOrder()
            .AddBeverage(new HotChocolate())
                .WithSize(BeverageSize.Medium)
                .AddWhippedCream()
                .AddCaramelSyrup()
                .AddToOrder()
            .Build();
        
        // Order 3: Specialty order
        var order3 = coffeeShop.CreateOrder("Carol")
            .AddBeverage(new GreenTea())
                .WithSize(BeverageSize.Large)
                .AddHoney()
                .MakeOrganic()
                .AddToOrder()
            .AddBeverage(new Espresso())
                .WithSize(BeverageSize.Medium)
                .AddCoconutMilk()
                .AddFoamArt("rosetta")
                .MakeDecaf()
                .AddToOrder()
            .Build();
        
        // Process orders
        await coffeeShop.ProcessOrderAsync(order1);
        await coffeeShop.ProcessOrderAsync(order2);
        await coffeeShop.ProcessOrderAsync(order3);
        
        // Show receipts
        Console.WriteLine("\n📄 Order Receipts:");
        Console.WriteLine(order1.GetReceipt());
        Console.WriteLine(order2.GetReceipt());
        Console.WriteLine(order3.GetReceipt());
        
        // Show sales statistics
        var stats = coffeeShop.GetSalesStatistics();
        Console.WriteLine(stats);
        Console.WriteLine();
    }

    private static void DemonstrateNutritionalInfo()
    {
        Console.WriteLine("5. Nutritional Information and Dietary Restrictions:");
        Console.WriteLine(new string('=', 50));
        
        // Create beverages with different dietary considerations
        var beverages = new List<(string name, IBeverage beverage)>
        {
            ("Regular Latte", CreateRegularLatte()),
            ("Vegan Latte", CreateVeganLatte()),
            ("Low-Calorie Option", CreateLowCalorieOption()),
            ("High-Caffeine Drink", CreateHighCaffeineDrink())
        };
        
        foreach (var (name, beverage) in beverages)
        {
            Console.WriteLine($"{name}:");
            Console.WriteLine($"  Price: ${beverage.Price:F2}");
            Console.WriteLine($"  Calories: {beverage.CaloriesPerServing}");
            Console.WriteLine($"  Vegan: {(beverage.IsVegan() ? "Yes" : "No")}");
            Console.WriteLine($"  Dairy-Free: {(beverage.IsDairyFree() ? "Yes" : "No")}");
            Console.WriteLine($"  Gluten-Free: {(beverage.IsGlutenFree() ? "Yes" : "No")}");
            
            var nutrition = beverage.GetNutritionalInfo();
            Console.WriteLine($"  Caffeine: {nutrition.Caffeine:F1}mg");
            Console.WriteLine($"  Sugar: {nutrition.Sugar:F1}g");
            Console.WriteLine($"  Protein: {nutrition.Protein:F1}g");
            
            if (beverage.Allergens.Count > 0)
            {
                Console.WriteLine($"  Allergens: {string.Join(", ", beverage.Allergens)}");
            }
            Console.WriteLine();
        }
    }

    private static IBeverage CreateRegularLatte()
    {
        return new MilkDecorator(
            new VanillaSyrupDecorator(
                new Espresso().WithSize(BeverageSize.Medium)));
    }

    private static IBeverage CreateVeganLatte()
    {
        return new SoyMilkDecorator(
            new VanillaSyrupDecorator(
                new Espresso().WithSize(BeverageSize.Medium)));
    }

    private static IBeverage CreateLowCalorieOption()
    {
        return new CinnamonDecorator(
            new IcedDecorator(
                new GreenTea().WithSize(BeverageSize.Large)));
    }

    private static IBeverage CreateHighCaffeineDrink()
    {
        return new ExtraEspressoShotDecorator(
            new ExtraEspressoShotDecorator(
                new DoubleStrengthDecorator(
                    new HouseBlend().WithSize(BeverageSize.ExtraLarge))));
    }
}
