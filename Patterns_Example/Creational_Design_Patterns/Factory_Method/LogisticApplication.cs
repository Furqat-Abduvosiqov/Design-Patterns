namespace Patterns_Example.Creational_Design_Patterns.Factory_Method;

public abstract class LogisticApplication
{
    public void PlanDelivery()
    {
        Console.WriteLine($"Planning delivery using {GetLogisticType()}");
        var transport = CreateTransport();
        Console.WriteLine($"Transport created: {transport.GetType().Name}");
    }
    
    public abstract ITransport CreateTransport();
    protected abstract string GetLogisticType();
}
