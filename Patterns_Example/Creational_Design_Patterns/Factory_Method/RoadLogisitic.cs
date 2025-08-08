namespace Patterns_Example.Creational_Design_Patterns.Factory_Method;

public class RoadLogisitic : LogisticApplication
{
    public override ITransport CreateTransport() => new Truck();
    protected override string GetLogisticType() => "Road Logistics";
}