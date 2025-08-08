using Patterns_Example.Creational_Design_Patterns.Abstract_Factory.Abstractions;

namespace Patterns_Example.Creational_Design_Patterns.Abstract_Factory.ModernFurnitureFactory;

public class ModernFurnitureFactory : FurnitureFactory
{
    public override Furniture CreateChair() => new ModernChair();
    public override Furniture CreateTable() => new ModernTable();
    public override Furniture CreateSofa() => new ModernSofa();
}