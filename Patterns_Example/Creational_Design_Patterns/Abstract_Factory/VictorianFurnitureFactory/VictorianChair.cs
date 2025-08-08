using Patterns_Example.Creational_Design_Patterns.Abstract_Factory.Abstractions;

namespace Patterns_Example.Creational_Design_Patterns.Abstract_Factory.VictorianFurnitureFactory;

public class VictorianChair : Chair
{
    public override string GetName() => GetType().Name;

    public override decimal GetPrice() => 25.0m;
}