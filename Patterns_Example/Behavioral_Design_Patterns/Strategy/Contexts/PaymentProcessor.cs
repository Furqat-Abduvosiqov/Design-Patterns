using Patterns_Example.Behavioral_Design_Patterns.Strategy.Abstractions;

namespace Patterns_Example.Behavioral_Design_Patterns.Strategy.Contexts;

public class PaymentProcessor
{
    private IPaymentStrategy _strategy;

    public PaymentProcessor(IPaymentStrategy strategy)
    {
        _strategy = strategy;
    }

    public void SetStrategy(IPaymentStrategy strategy)
    {
        _strategy = strategy;
    }

    public async Task<bool> ProcessPayment(string account, decimal amount)
    {
        Console.WriteLine($"Using strategy: {_strategy.GetName()}");
        return await _strategy.Pay(account, amount);
    }
}

