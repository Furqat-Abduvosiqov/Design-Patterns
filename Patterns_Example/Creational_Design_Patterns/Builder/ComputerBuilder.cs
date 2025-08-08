namespace Patterns_Example.Creational_Design_Patterns.Builder;

/// <summary>
/// The Concrete Builder classes follow the Builder interface and provide
/// specific implementations of the building steps. Your program may have several
/// variations of Builders, implemented differently.
/// </summary>
public class ComputerBuilder : IComputerBuilder
{
    private Computer _computer;

    public ComputerBuilder()
    {
        Reset();
    }

    public void Reset()
    {
        _computer = new Computer();
    }

    public IComputerBuilder SetCPU(string cpu, decimal price)
    {
        _computer.CPU = cpu;
        _computer.TotalPrice += price;
        return this;
    }

    public IComputerBuilder SetGPU(string gpu, decimal price)
    {
        _computer.GPU = gpu;
        _computer.TotalPrice += price;
        return this;
    }

    public IComputerBuilder SetRAM(int ramGB, decimal pricePerGB)
    {
        _computer.RAM = ramGB;
        _computer.TotalPrice += ramGB * pricePerGB;
        return this;
    }

    public IComputerBuilder SetStorage(int storageGB, string storageType, decimal price)
    {
        _computer.Storage = storageGB;
        _computer.StorageType = storageType;
        _computer.TotalPrice += price;
        return this;
    }

    public IComputerBuilder SetMotherboard(string motherboard, decimal price)
    {
        _computer.Motherboard = motherboard;
        _computer.TotalPrice += price;
        return this;
    }

    public IComputerBuilder SetPowerSupply(string powerSupply, decimal price)
    {
        _computer.PowerSupply = powerSupply;
        _computer.TotalPrice += price;
        return this;
    }

    public IComputerBuilder SetCase(string computerCase, decimal price)
    {
        _computer.Case = computerCase;
        _computer.TotalPrice += price;
        return this;
    }

    public IComputerBuilder AddWiFi(decimal price = 50m)
    {
        _computer.HasWiFi = true;
        _computer.TotalPrice += price;
        return this;
    }

    public IComputerBuilder AddBluetooth(decimal price = 25m)
    {
        _computer.HasBluetooth = true;
        _computer.TotalPrice += price;
        return this;
    }

    public IComputerBuilder AddPeripheral(string peripheral, decimal price)
    {
        _computer.Peripherals.Add(peripheral);
        _computer.TotalPrice += price;
        return this;
    }

    public Computer Build()
    {
        var result = _computer;
        Reset(); // Reset for next build
        return result;
    }
}
