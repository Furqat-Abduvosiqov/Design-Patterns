namespace Patterns_Example.Creational_Design_Patterns.Builder;

/// <summary>
/// The Builder interface specifies methods for creating the different parts of the Product objects.
/// This interface defines the contract for all concrete builders.
/// </summary>
public interface IComputerBuilder
{
    IComputerBuilder SetCPU(string cpu, decimal price);
    IComputerBuilder SetGPU(string gpu, decimal price);
    IComputerBuilder SetRAM(int ramGB, decimal pricePerGB);
    IComputerBuilder SetStorage(int storageGB, string storageType, decimal price);
    IComputerBuilder SetMotherboard(string motherboard, decimal price);
    IComputerBuilder SetPowerSupply(string powerSupply, decimal price);
    IComputerBuilder SetCase(string computerCase, decimal price);
    IComputerBuilder AddWiFi(decimal price = 50m);
    IComputerBuilder AddBluetooth(decimal price = 25m);
    IComputerBuilder AddPeripheral(string peripheral, decimal price);
    Computer Build();
    void Reset();
}
