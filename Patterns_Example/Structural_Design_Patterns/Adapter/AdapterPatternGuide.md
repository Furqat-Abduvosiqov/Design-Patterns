# Adapter Design Pattern

The Adapter pattern is a structural design pattern that allows objects with incompatible interfaces to collaborate. It acts as a bridge between two incompatible interfaces by wrapping an existing class with a new interface.

## Problem

Imagine you're building an e-commerce application that needs to integrate with multiple payment processors. Each payment provider has its own API with different:

- **Method names**: `ChargeCard()` vs `CreateCharge()` vs `InitiateTransfer()`
- **Parameter formats**: Different request/response structures
- **Data types**: Amounts in dollars vs cents, different date formats
- **Error handling**: Different error codes and messages

### Issues with Direct Integration:

1. **Incompatible Interfaces**: Each payment system has its own unique API
2. **Code Duplication**: Similar logic repeated for each integration
3. **Tight Coupling**: Application code depends on specific payment APIs
4. **Maintenance Nightmare**: Changes in any payment API require application changes

```csharp
// ❌ Direct integration - tightly coupled and hard to maintain
if (paymentProvider == "PayPal")
{
    var paypalRequest = new PayPalRequest { Amount = amount, ... };
    var paypalResponse = paypalSystem.ChargeCard(paypalRequest);
    // Handle PayPal-specific response format
}
else if (paymentProvider == "Stripe")
{
    var stripeParams = new StripeChargeParams { AmountInCents = amount * 100, ... };
    var stripeResult = stripeGateway.CreateCharge(stripeParams);
    // Handle Stripe-specific response format
}
// More if-else blocks for each provider...
```

## Solution

The Adapter pattern suggests creating an adapter class that:
1. **Implements the target interface** your application expects
2. **Wraps the legacy/third-party system** (adaptee)
3. **Translates calls** between the two interfaces
4. **Converts data formats** as needed

### Key Components:

1. **Target Interface (IPaymentProcessor)**: The interface your application expects
2. **Adaptee (Legacy Systems)**: Existing classes with incompatible interfaces
3. **Adapter (PayPalAdapter, StripeAdapter)**: Classes that make adaptees compatible
4. **Client (PaymentService)**: Uses the target interface

## Real-World Example: Payment Processing Integration

Our example demonstrates integrating with three different payment systems:

### 1. Legacy PayPal System
- Uses `ChargeCard()` method
- Requires `PayPalRequest` with specific fields
- Returns `PayPalResponse` with approval status

### 2. Legacy Stripe Gateway
- Uses `CreateCharge()` method
- Requires amounts in cents, not dollars
- Returns `StripeChargeResult` with different field names

### 3. Legacy Bank Transfer API
- Uses `InitiateTransfer()` method
- Completely different semantics (transfers vs charges)
- Asynchronous processing with reference numbers

## Benefits Demonstrated

1. **Uniform Interface**: All payment systems accessible through `IPaymentProcessor`
2. **Easy Integration**: Add new payment providers without changing existing code
3. **Loose Coupling**: Application code independent of specific payment APIs
4. **Maintainability**: Changes isolated to individual adapter classes
5. **Testability**: Easy to mock and test individual adapters
6. **Flexibility**: Switch payment providers at runtime

## Implementation Strategies

### 1. Object Adapter (Composition)
```csharp
public class PayPalAdapter : IPaymentProcessor
{
    private readonly LegacyPayPalSystem _legacyPayPal;
    
    public PayPalAdapter(LegacyPayPalSystem legacyPayPal)
    {
        _legacyPayPal = legacyPayPal;
    }
    
    public PaymentResult ProcessPayment(PaymentRequest request)
    {
        var paypalRequest = ConvertToPayPalRequest(request);
        var paypalResponse = _legacyPayPal.ChargeCard(paypalRequest);
        return ConvertFromPayPalResponse(paypalResponse, request);
    }
}
```

### 2. Composite Adapter (Multiple Adaptees)
```csharp
public class CompositePaymentAdapter : IPaymentProcessor
{
    private readonly Dictionary<PaymentType, IPaymentProcessor> _adapters;
    
    public PaymentResult ProcessPayment(PaymentRequest request)
    {
        if (_adapters.TryGetValue(request.PaymentMethod.Type, out var adapter))
        {
            return adapter.ProcessPayment(request);
        }
        // Handle unsupported payment type
    }
}
```

## When to Use Adapter Pattern

✅ **Use Adapter when:**
- You need to integrate with legacy systems you cannot modify
- Third-party libraries have incompatible interfaces
- You want to reuse existing classes with incompatible interfaces
- You need to support multiple similar but different APIs
- You want to isolate integration complexity

❌ **Don't use Adapter when:**
- You can modify the source code of the classes
- The interfaces are already compatible
- You're designing new systems from scratch
- The adaptation logic is overly complex

## Code Structure

```
Adapter/
├── IPaymentProcessor.cs         # Target interface
├── LegacyPaymentSystems.cs      # Adaptee classes (legacy systems)
├── PaymentAdapters.cs           # PayPal and Stripe adapters
├── BankTransferAdapter.cs       # Bank transfer adapter
├── PaymentService.cs            # Client using target interface
├── AdapterExample.cs            # Usage demonstrations
└── AdapterPatternDemo.cs        # Main demo program
```

## Advanced Features

### 1. Data Transformation
Adapters handle complex data conversions:
- Currency formats (dollars ↔ cents)
- Date formats (DateTime ↔ Unix timestamps)
- Field mapping (different property names)
- Validation rules (different constraints)

### 2. Error Handling Translation
```csharp
private PaymentResult ConvertFromPayPalResponse(PayPalResponse response, PaymentRequest request)
{
    return new PaymentResult
    {
        IsSuccess = response.IsApproved,
        Status = response.IsApproved ? PaymentStatus.Completed : PaymentStatus.Failed,
        Message = response.ResponseMessage,
        // ... other mappings
    };
}
```

### 3. Transaction Mapping
Adapters maintain mappings between internal and external transaction IDs:
```csharp
private readonly Dictionary<string, string> _transactionMapping;
```

## Performance Considerations

1. **Minimal Overhead**: Adapters add minimal performance cost
2. **Caching**: Can cache expensive conversions or lookups
3. **Lazy Loading**: Initialize adaptees only when needed
4. **Connection Pooling**: Reuse connections to external systems

## Testing Benefits

1. **Mock Adapters**: Easy to create test doubles
2. **Isolated Testing**: Test each adapter independently
3. **Integration Testing**: Test with real external systems
4. **Behavior Verification**: Ensure correct API calls are made

## Common Variations

### 1. Two-Way Adapter
Supports both directions of communication:
```csharp
public interface ITwoWayAdapter<TSource, TTarget>
{
    TTarget Adapt(TSource source);
    TSource AdaptBack(TTarget target);
}
```

### 2. Pluggable Adapter
Runtime registration of adapters:
```csharp
public void RegisterAdapter(string providerName, IPaymentProcessor adapter)
{
    _adapters[providerName] = adapter;
}
```

## Learning Objectives

After studying this example, you should understand:

- When the Adapter pattern solves real integration problems
- How to design clean target interfaces
- Techniques for data format conversion
- Error handling and status mapping strategies
- The difference between object and class adapters
- How to combine Adapter with other patterns (Strategy, Composite)

## Extension Ideas

Try extending this example by:

1. **Adding New Payment Providers**: Apple Pay, Google Pay, cryptocurrency
2. **Implementing Retry Logic**: Handle temporary failures gracefully
3. **Adding Logging and Monitoring**: Track adapter performance and errors
4. **Creating Configuration-Driven Adapters**: Load adapter settings from config
5. **Implementing Circuit Breaker**: Prevent cascading failures
6. **Adding Webhook Support**: Handle asynchronous payment notifications
