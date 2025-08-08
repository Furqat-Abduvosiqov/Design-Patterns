using Patterns_Example.Creational_Design_Patterns.Abstract_Factory.Abstractions;

namespace Patterns_Example.Creational_Design_Patterns.Abstract_Factory.ModernFurnitureFactory;

public class ModernChair : Chair
{
    public override string GetName() => GetType().Name;

    public override decimal GetPrice() => 12.0m;
}