using Patterns_Example.Behavioral_Design_Patterns.Strategy.Abstractions;

namespace Patterns_Example.Behavioral_Design_Patterns.Strategy.Strategies;

public class CryptoPayment : IPaymentStrategy
{
    public async Task<bool> Pay(string account, decimal amount)
    {
        Console.WriteLine($"[Crypto] Verifying wallet address {account}...");
        await Task.Delay(600); // Simulate wallet verification
        decimal fee = Math.Max(0.0005m * amount, 0.01m);
        Console.WriteLine($"[Crypto] Calculating network fee: ${fee:F4}");
        await Task.Delay(400); // Simulate fee calculation
        Console.WriteLine($"[Crypto] Broadcasting transaction to blockchain...");
        await Task.Delay(1500); // Simulate network confirmation
        Console.WriteLine($"[Crypto] Payment of ${amount:F2} (fee: ${fee:F4}) confirmed!");
        return true;
    }

    public string GetName() => "Cryptocurrency";
}

