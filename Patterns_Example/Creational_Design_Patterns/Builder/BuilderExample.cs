namespace Patterns_Example.Creational_Design_Patterns.Builder;

/// <summary>
/// Example class demonstrating the Builder pattern usage
/// </summary>
public static class BuilderExample
{
    public static void RunExample()
    {
        Console.WriteLine("=== Builder Pattern Example: Computer Configuration ===\n");
        
        // Create a builder
        var builder = new ComputerBuilder();
        var director = new ComputerDirector(builder);

        Console.WriteLine("1. Building an Office Computer using Director:");
        Console.WriteLine(new string('=', 50));
        var officeComputer = director.BuildOfficeComputer();
        Console.WriteLine(officeComputer);
        Console.WriteLine();

        Console.WriteLine("2. Building a Gaming Computer using Director:");
        Console.WriteLine(new string('=', 50));
        var gamingComputer = director.BuildGamingComputer();
        Console.WriteLine(gamingComputer);
        Console.WriteLine();

        Console.WriteLine("3. Building a Budget Computer using Director:");
        Console.WriteLine(new string('=', 50));
        var budgetComputer = director.BuildBudgetComputer();
        Console.WriteLine(budgetComputer);
        Console.WriteLine();

        Console.WriteLine("4. Building a Custom Computer using Builder directly:");
        Console.WriteLine(new string('=', 50));
        // Client can also use the builder directly for custom configurations
        var customComputer = new ComputerBuilder()
            .SetCPU("Intel Core i7-13700K", 400m)
            .SetGPU("AMD RX 7800 XT", 500m)
            .SetRAM(32, 9m)
            .SetStorage(1000, "NVMe SSD", 120m)
            .SetMotherboard("MSI MAG Z790", 250m)
            .SetPowerSupply("Corsair RM750x", 120m)
            .SetCase("Lian Li PC-O11 Dynamic", 150m)
            .AddWiFi()
            .AddBluetooth()
            .AddPeripheral("RGB Mechanical Keyboard", 100m)
            .AddPeripheral("Wireless Gaming Mouse", 70m)
            .Build();
        
        Console.WriteLine(customComputer);
        Console.WriteLine();

        Console.WriteLine("5. Building a Workstation Computer using Director:");
        Console.WriteLine(new string('=', 50));
        var workstationComputer = director.BuildWorkstationComputer();
        Console.WriteLine(workstationComputer);
        Console.WriteLine();

        Console.WriteLine("=== Builder Pattern Benefits Demonstrated ===");
        Console.WriteLine("✓ Complex object construction is simplified");
        Console.WriteLine("✓ Same construction process can create different representations");
        Console.WriteLine("✓ Step-by-step construction with method chaining");
        Console.WriteLine("✓ Director encapsulates common construction algorithms");
        Console.WriteLine("✓ Builder can be reused for multiple objects");
        Console.WriteLine("✓ Easy to add new components without changing existing code");
    }
}
