using Patterns_Example.Behavioral_Design_Patterns.Strategy.Abstractions;

namespace Patterns_Example.Behavioral_Design_Patterns.Strategy.Strategies;

public class CreditCardPayment : IPaymentStrategy
{
    public async Task<bool> Pay(string account, decimal amount)
    {
        Console.WriteLine($"[CreditCard] Validating card for account {account}...");
        await Task.Delay(500); // Simulate validation
        Console.WriteLine($"[CreditCard] Processing payment of ${amount:F2}...");
        await Task.Delay(1000); // Simulate processing
        Console.WriteLine($"[CreditCard] Payment successful!");
        return true;
    }

    public string GetName() => "Credit Card";
}

