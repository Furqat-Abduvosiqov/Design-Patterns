namespace Patterns_Example.Creational_Design_Patterns.Builder;

/// <summary>
/// The Director class defines the order in which to execute the building steps.
/// It works with a builder object through the common Builder interface.
/// Therefore it may not know what product is being built.
/// </summary>
public class ComputerDirector
{
    private IComputerBuilder _builder;

    public ComputerDirector(IComputerBuilder builder)
    {
        _builder = builder;
    }

    public void SetBuilder(IComputerBuilder builder)
    {
        _builder = builder;
    }

    /// <summary>
    /// Builds a basic office computer suitable for general office work
    /// </summary>
    public Computer BuildOfficeComputer()
    {
        return _builder
            .SetCPU("Intel Core i5-12400", 200m)
            .SetRAM(16, 8m) // 16GB at $8 per GB
            .SetStorage(512, "SSD", 80m)
            .SetMotherboard("ASUS Prime B660M", 120m)
            .SetPowerSupply("EVGA 500W", 60m)
            .SetCase("Fractal Design Core 1000", 50m)
            .AddWiFi()
            .AddPeripheral("Wireless Keyboard & Mouse", 45m)
            .Build();
    }

    /// <summary>
    /// Builds a high-end gaming computer with premium components
    /// </summary>
    public Computer BuildGamingComputer()
    {
        return _builder
            .SetCPU("AMD Ryzen 9 7900X", 550m)
            .SetGPU("NVIDIA RTX 4080", 1200m)
            .SetRAM(32, 10m) // 32GB at $10 per GB
            .SetStorage(1000, "NVMe SSD", 150m)
            .SetMotherboard("ASUS ROG Strix X670E", 400m)
            .SetPowerSupply("Corsair RM850x", 150m)
            .SetCase("NZXT H7 Flow", 130m)
            .AddWiFi(75m) // Premium WiFi card
            .AddBluetooth()
            .AddPeripheral("Mechanical Gaming Keyboard", 120m)
            .AddPeripheral("Gaming Mouse", 80m)
            .AddPeripheral("Gaming Headset", 150m)
            .Build();
    }

    /// <summary>
    /// Builds a budget-friendly computer for basic tasks
    /// </summary>
    public Computer BuildBudgetComputer()
    {
        return _builder
            .SetCPU("AMD Ryzen 5 5600G", 150m) // APU with integrated graphics
            .SetRAM(8, 6m) // 8GB at $6 per GB
            .SetStorage(256, "SSD", 40m)
            .SetMotherboard("MSI A520M-A PRO", 60m)
            .SetPowerSupply("EVGA 450W", 45m)
            .SetCase("Cooler Master MasterBox Q300L", 40m)
            .AddPeripheral("Basic Keyboard & Mouse", 25m)
            .Build();
    }

    /// <summary>
    /// Builds a workstation computer for professional content creation
    /// </summary>
    public Computer BuildWorkstationComputer()
    {
        return _builder
            .SetCPU("Intel Core i9-13900K", 600m)
            .SetGPU("NVIDIA RTX 4070 Ti", 800m)
            .SetRAM(64, 12m) // 64GB at $12 per GB
            .SetStorage(2000, "NVMe SSD", 300m)
            .SetMotherboard("ASUS ProArt Z790", 350m)
            .SetPowerSupply("Seasonic Focus GX-850", 180m)
            .SetCase("Fractal Design Define 7", 170m)
            .AddWiFi(100m) // Professional WiFi card
            .AddBluetooth()
            .AddPeripheral("Professional Keyboard", 200m)
            .AddPeripheral("Precision Mouse", 100m)
            .AddPeripheral("4K Monitor", 400m)
            .Build();
    }
}
