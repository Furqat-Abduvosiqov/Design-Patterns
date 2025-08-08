namespace Patterns_Example.Creational_Design_Patterns.Factory_Method;

public class Truck : ITransport
{
    public void Deliver()
    {
        Console.WriteLine("Deliver by Truck - Fast land delivery");
    }
    
    public string GetTransportType() => "Road Transport";
    public decimal GetCost() => 50.0m;
}