namespace Patterns_Example.Creational_Design_Patterns.Builder;

/// <summary>
/// The Product class represents the complex object being built.
/// In this case, it's a Computer with various components.
/// </summary>
public class Computer
{
    public string? CPU { get; set; }
    public string? GPU { get; set; }
    public int RAM { get; set; } // in GB
    public int Storage { get; set; } // in GB
    public string? StorageType { get; set; } // SSD, HDD
    public string? Motherboard { get; set; }
    public string? PowerSupply { get; set; }
    public string? Case { get; set; }
    public bool HasWiFi { get; set; }
    public bool HasBluetooth { get; set; }
    public List<string> Peripherals { get; set; } = new();
    public decimal TotalPrice { get; set; }

    public override string ToString()
    {
        var peripheralsStr = Peripherals.Count > 0 ? string.Join(", ", Peripherals) : "None";
        
        return $"""
            Computer Configuration:
            =====================
            CPU: {CPU ?? "Not specified"}
            GPU: {GPU ?? "Not specified"}
            RAM: {RAM} GB
            Storage: {Storage} GB {StorageType}
            Motherboard: {Motherboard ?? "Not specified"}
            Power Supply: {PowerSupply ?? "Not specified"}
            Case: {Case ?? "Not specified"}
            WiFi: {(HasWiFi ? "Yes" : "No")}
            Bluetooth: {(HasBluetooth ? "Yes" : "No")}
            Peripherals: {peripheralsStr}
            Total Price: ${TotalPrice:F2}
            """;
    }
}
