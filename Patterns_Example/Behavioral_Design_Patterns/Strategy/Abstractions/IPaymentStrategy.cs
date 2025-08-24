namespace Patterns_Example.Behavioral_Design_Patterns.Strategy.Abstractions;

public interface IPaymentStrategy
{
    Task<bool> Pay(string account, decimal amount);
    string GetName();
}

