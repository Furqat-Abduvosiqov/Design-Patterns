using Patterns_Example.Creational_Design_Patterns.Abstract_Factory.Abstractions;

namespace Patterns_Example.Creational_Design_Patterns.Abstract_Factory.ModernFurnitureFactory;

public class ModernTable : Table
{
    public override string GetName() => GetType().Name;

    public override decimal GetPrice() => 20.0m;
}