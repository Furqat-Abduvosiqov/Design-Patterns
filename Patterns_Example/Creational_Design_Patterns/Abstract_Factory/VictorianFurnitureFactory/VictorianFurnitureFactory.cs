using Patterns_Example.Creational_Design_Patterns.Abstract_Factory.Abstractions;

namespace Patterns_Example.Creational_Design_Patterns.Abstract_Factory.VictorianFurnitureFactory;

public class VictorianFurnitureFactory : FurnitureFactory
{
    public override Furniture CreateChair() => new VictorianChair();
    public override Furniture CreateTable() => new VictorianTable();
    public override Furniture CreateSofa() => new VictorianSofa();
}