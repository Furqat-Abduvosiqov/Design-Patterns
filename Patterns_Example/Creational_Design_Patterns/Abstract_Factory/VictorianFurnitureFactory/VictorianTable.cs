using Patterns_Example.Creational_Design_Patterns.Abstract_Factory.Abstractions;

namespace Patterns_Example.Creational_Design_Patterns.Abstract_Factory.VictorianFurnitureFactory;

public class VictorianTable : Table
{
    public override string GetName() => GetType().Name;

    public override decimal GetPrice() => 30.0m;
}