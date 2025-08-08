namespace Patterns_Example.Creational_Design_Patterns.Factory_Method;

public interface ITransport
{
    void Deliver();
    string GetTransportType();
    decimal GetCost();
}