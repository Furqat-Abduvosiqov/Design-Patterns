using Patterns_Example.Behavioral_Design_Patterns.Strategy.Abstractions;

namespace Patterns_Example.Behavioral_Design_Patterns.Strategy.Strategies;

public class PayPalPayment : IPaymentStrategy
{
    public async Task<bool> Pay(string account, decimal amount)
    {
        Console.WriteLine($"[PayPal] Authenticating PayPal account {account}...");
        await Task.Delay(400); // Simulate authentication
        Console.WriteLine($"[PayPal] Transferring ${amount:F2} via PayPal...");
        await Task.Delay(800); // Simulate transfer
        Console.WriteLine($"[PayPal] Payment completed!");
        return true;
    }

    public string GetName() => "PayPal";
}

