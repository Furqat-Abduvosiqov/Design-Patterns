namespace Patterns_Example.Creational_Design_Patterns.Abstract_Factory.Abstractions;

public class Chair : Furniture
{
    public override string GetName() => GetType().Name;

    public override decimal GetPrice() => 10.0m;
    
    public void Sit() => Console.WriteLine("Sitting on chair");
}