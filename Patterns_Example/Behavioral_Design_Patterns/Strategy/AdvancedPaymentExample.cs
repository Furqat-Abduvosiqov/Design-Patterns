using Patterns_Example.Behavioral_Design_Patterns.Strategy.Abstractions;
using Patterns_Example.Behavioral_Design_Patterns.Strategy.Contexts;
using Patterns_Example.Behavioral_Design_Patterns.Strategy.Strategies;

namespace Patterns_Example.Behavioral_Design_Patterns.Strategy;

public class AdvancedPaymentExample
{
    public static async Task RunExample()
    {
        Console.WriteLine("Advanced Payment Processing Example - Strategy Pattern");
        Console.WriteLine("------------------------------------------------------");

        var processor = new PaymentProcessor(new CreditCardPayment());
        string account = "user123";
        decimal amount = 150.75m;

        // Credit Card Payment
        Console.WriteLine("\n--- Credit Card Payment ---");
        await processor.ProcessPayment(account, amount);

        // Switch to PayPal
        processor.SetStrategy(new PayPalPayment());
        Console.WriteLine("\n--- PayPal Payment ---");
        await processor.ProcessPayment(account, amount);

        // Switch to Crypto
        processor.SetStrategy(new CryptoPayment());
        Console.WriteLine("\n--- Crypto Payment ---");
        await processor.ProcessPayment("0xABCDEF1234567890", 0.045m);

        // Simulate dynamic strategy selection
        Console.WriteLine("\n--- Dynamic Strategy Selection ---");
        var strategies = new IPaymentStrategy []
        {
            new CreditCardPayment(),
            new PayPalPayment(),
            new CryptoPayment()
        };
        
        var accounts = new[] { "userA", "userB", "0xDEADBEEF" };
        var amounts = new[] { 99.99m, 250.00m, 0.12m };
        
        for (int i = 0; i < strategies.Length; i++)
        {
            processor.SetStrategy(strategies[i]);
            await processor.ProcessPayment(accounts[i], amounts[i]);
        }
    }
}

