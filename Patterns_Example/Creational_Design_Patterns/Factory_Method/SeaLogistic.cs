namespace Patterns_Example.Creational_Design_Patterns.Factory_Method;

public class SeaLogistic : LogisticApplication
{
    public override ITransport CreateTransport() => new Ship();
    protected override string GetLogisticType() => "Sea Logistics";
}