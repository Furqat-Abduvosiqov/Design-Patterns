namespace Patterns_Example.Creational_Design_Patterns.Factory_Method;

public class Ship : ITransport
{
    public void Deliver()
    {
        Console.WriteLine("Deliver by Ship - Bulk sea delivery");
    }
    
    public string GetTransportType() => "Sea Transport";
    public decimal GetCost() => 200.0m;
}