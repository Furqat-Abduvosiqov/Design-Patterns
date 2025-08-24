# Strategy Pattern

## Overview
The Strategy Pattern is a behavioral design pattern that enables selecting an algorithm's behavior at runtime. It defines a family of algorithms, encapsulates each one, and makes them interchangeable. The context delegates the work to a strategy object instead of implementing multiple versions of the algorithm.

## Real-World Example: Payment Processing System
This example demonstrates a payment processor that can use different payment methods (Credit Card, PayPal, Cryptocurrency) at runtime. Each payment method is implemented as a strategy, allowing the processor to switch between them dynamically.

### Key Components
- **IPaymentStrategy**: Interface for all payment strategies.
- **PaymentProcessor**: Context that uses a payment strategy to process payments.
- **CreditCardPayment, PayPalPayment, CryptoPayment**: Concrete strategies implementing different payment algorithms.

### Benefits
- **Open/Closed Principle**: Easily add new payment methods without modifying existing code.
- **Single Responsibility Principle**: Each strategy handles its own payment logic.
- **Runtime Flexibility**: Switch strategies dynamically based on user input or business rules.

## Implementation Details

### Strategy Interface
```csharp
public interface IPaymentStrategy {
    Task<bool> Pay(string account, decimal amount);
    string GetName();
}
```

### Context
```csharp
public class PaymentProcessor {
    private IPaymentStrategy _strategy;
    public PaymentProcessor(IPaymentStrategy strategy) { _strategy = strategy; }
    public void SetStrategy(IPaymentStrategy strategy) { _strategy = strategy; }
    public async Task<bool> ProcessPayment(string account, decimal amount) {
        Console.WriteLine($"Using strategy: {_strategy.GetName()}");
        return await _strategy.Pay(account, amount);
    }
}
```

### Example Usage
```csharp
var processor = new PaymentProcessor(new CreditCardPayment());
await processor.ProcessPayment("user123", 150.75m);
processor.SetStrategy(new PayPalPayment());
await processor.ProcessPayment("user123", 150.75m);
processor.SetStrategy(new CryptoPayment());
await processor.ProcessPayment("0xABCDEF1234567890", 0.045m);
```

## Advanced Features
- Asynchronous payment processing
- Simulated validation, authentication, fee calculation, and network confirmation
- Dynamic strategy selection for multiple payments

## When to Use
- When you need to switch between different algorithms or behaviors at runtime
- When you want to avoid large conditional statements for algorithm selection
- When you want to keep related algorithms encapsulated and interchangeable

## Best Practices
- Keep strategies stateless if possible
- Use dependency injection for strategy selection in large systems
- Document each strategy's behavior and limitations

