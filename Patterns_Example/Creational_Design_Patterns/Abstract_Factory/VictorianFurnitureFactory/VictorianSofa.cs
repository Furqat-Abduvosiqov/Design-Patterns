using Patterns_Example.Creational_Design_Patterns.Abstract_Factory.Abstractions;

namespace Patterns_Example.Creational_Design_Patterns.Abstract_Factory.VictorianFurnitureFactory;

public class VictorianSofa : Sofa
{
    public override string GetName() => GetType().Name;

    public override decimal GetPrice() => 200.0m;
}