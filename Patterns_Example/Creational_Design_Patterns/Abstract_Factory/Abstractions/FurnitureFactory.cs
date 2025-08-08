namespace Patterns_Example.Creational_Design_Patterns.Abstract_Factory.Abstractions;

public abstract class FurnitureFactory
{
    public virtual Furniture CreateChair() => new Chair();
    public virtual Furniture CreateTable() => new Table();
    public virtual Furniture CreateSofa() => new Sofa();
}