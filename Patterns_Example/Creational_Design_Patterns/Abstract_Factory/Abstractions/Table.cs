namespace Patterns_Example.Creational_Design_Patterns.Abstract_Factory.Abstractions;

public class Table : Furniture
{
    public override string GetName() => GetType().Name;

    public override decimal GetPrice() => 10.0m;
    
    public void Put() => Console.WriteLine("Putting on table");
}