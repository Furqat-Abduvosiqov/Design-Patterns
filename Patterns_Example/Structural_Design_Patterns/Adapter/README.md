# Adapter Pattern Example

This folder contains a comprehensive implementation of the Adapter design pattern using a real-world payment processing integration system.

## Files Overview

- **`IPaymentProcessor.cs`** - Target interface that our application expects
- **`LegacyPaymentSystems.cs`** - Legacy/third-party systems with incompatible interfaces
- **`PaymentAdapters.cs`** - PayPal and Stripe adapters implementing the target interface
- **`BankTransferAdapter.cs`** - Bank transfer adapter with composite adapter example
- **`PaymentService.cs`** - High-level service using the target interface
- **`AdapterExample.cs`** - Comprehensive usage demonstrations
- **`AdapterPatternDemo.cs`** - Main demo program
- **`AdapterPatternGuide.md`** - Complete pattern explanation

## What Problem Does This Solve?

The Adapter pattern addresses the challenge of integrating with multiple systems that have incompatible interfaces. Instead of:

```csharp
// ❌ Direct integration - tightly coupled and hard to maintain
if (provider == "PayPal")
{
    var paypalRequest = new PayPalRequest { Amount = amount, ... };
    var paypalResponse = paypalSystem.ChargeCard(paypalRequest);
    // Handle PayPal-specific response
}
else if (provider == "Stripe")
{
    var stripeParams = new StripeChargeParams { AmountInCents = amount * 100, ... };
    var stripeResult = stripeGateway.CreateCharge(stripeParams);
    // Handle Stripe-specific response
}
// More if-else blocks for each provider...
```

You can use:

```csharp
// ✅ Adapter pattern - uniform interface for all providers
IPaymentProcessor processor = GetPaymentProcessor(provider);
var result = processor.ProcessPayment(request);
// Same interface regardless of underlying provider!
```

## Key Benefits Demonstrated

1. **Uniform Interface**: All payment systems accessible through `IPaymentProcessor`
2. **Easy Integration**: Add new providers without changing existing code
3. **Loose Coupling**: Application independent of specific payment APIs
4. **Data Translation**: Automatic conversion between different formats
5. **Error Handling**: Consistent error handling across all providers
6. **Maintainability**: Changes isolated to individual adapter classes

## Real-World Integration Scenarios

This pattern is commonly used for:

- **Payment Processing**: Integrating multiple payment gateways
- **Database Systems**: Supporting different database providers
- **Cloud Services**: Abstracting different cloud provider APIs
- **Legacy System Integration**: Modernizing old system interfaces
- **Third-Party APIs**: Standardizing external service calls
- **File Format Conversion**: Supporting multiple file formats

## Components Demonstrated

### 1. Target Interface (`IPaymentProcessor`)
Defines the interface our application expects:
```csharp
public interface IPaymentProcessor
{
    PaymentResult ProcessPayment(PaymentRequest request);
    PaymentResult RefundPayment(string transactionId, decimal amount);
    PaymentStatus GetPaymentStatus(string transactionId);
    bool ValidatePaymentMethod(PaymentMethod paymentMethod);
}
```

### 2. Legacy Systems (Adaptees)
Three different payment systems with incompatible interfaces:

- **PayPal System**: Uses `ChargeCard()`, requires `PayPalRequest`
- **Stripe Gateway**: Uses `CreateCharge()`, amounts in cents
- **Bank Transfer API**: Uses `InitiateTransfer()`, different semantics

### 3. Adapters
Classes that bridge the gap between target interface and legacy systems:

- **PayPalAdapter**: Converts between our format and PayPal's API
- **StripeAdapter**: Handles Stripe's cent-based amounts and different field names
- **BankTransferAdapter**: Adapts bank transfer semantics to payment interface

### 4. Composite Adapter
Routes requests to appropriate adapters based on payment type:
```csharp
var compositeAdapter = new CompositePaymentAdapter();
compositeAdapter.RegisterAdapter(PaymentType.CreditCard, paypalAdapter);
compositeAdapter.RegisterAdapter(PaymentType.DebitCard, stripeAdapter);
compositeAdapter.RegisterAdapter(PaymentType.BankTransfer, bankAdapter);
```

## Data Transformation Examples

The adapters handle complex data conversions:

### Currency Conversion
```csharp
// PayPal uses dollars
Amount = request.Amount

// Stripe uses cents
AmountInCents = (long)(request.Amount * 100)
```

### Field Mapping
```csharp
// Our format → PayPal format
CardNumber = request.PaymentMethod.CardNumber
ExpirationMonth = request.PaymentMethod.ExpiryDate.Split('/')[0]
ExpirationYear = request.PaymentMethod.ExpiryDate.Split('/')[1]
```

### Status Translation
```csharp
// PayPal status → Our status
return status.Status.ToUpper() switch
{
    "COMPLETED" => PaymentStatus.Completed,
    "PENDING" => PaymentStatus.Pending,
    "FAILED" => PaymentStatus.Failed,
    _ => PaymentStatus.Failed
};
```

## How to Run

1. Navigate to the project root directory
2. Update Program.cs to use the Adapter example:
   ```csharp
   using Patterns_Example.Structural_Design_Patterns.Adapter;
   AdapterExample.RunExample();
   ```
3. Run the demo:
   ```bash
   dotnet run --project Patterns_Example
   ```

## Example Output

The demo will show:

1. **Individual Adapter Demonstrations**: PayPal, Stripe, and Bank Transfer adapters
2. **Composite Adapter Usage**: Routing different payment types to appropriate adapters
3. **Payment Service Integration**: How the same service works with different adapters
4. **Data Transformation**: Conversion between different formats and APIs
5. **Error Handling**: Consistent error handling across all providers

## Learning Objectives

After studying this example, you should understand:

- When to use the Adapter pattern vs other integration approaches
- How to design clean target interfaces for your application
- Techniques for converting between incompatible data formats
- Error handling and status mapping strategies
- The difference between object adapters and class adapters
- How to combine Adapter with other patterns (Strategy, Composite)

## Advanced Features

### Transaction Mapping
Adapters maintain mappings between internal and external transaction IDs:
```csharp
private readonly Dictionary<string, string> _transactionMapping;
_transactionMapping[request.TransactionId] = paypalResponse.TransactionCode;
```

### Validation Translation
Each adapter validates payment methods according to what the underlying system supports:
```csharp
public bool ValidatePaymentMethod(PaymentMethod paymentMethod)
{
    return paymentMethod.Type == PaymentType.CreditCard || 
           paymentMethod.Type == PaymentType.DebitCard;
}
```

### Error Handling
Consistent error handling across all adapters:
```csharp
catch (Exception ex)
{
    return new PaymentResult
    {
        IsSuccess = false,
        TransactionId = request.TransactionId,
        Message = $"PayPal processing error: {ex.Message}",
        Status = PaymentStatus.Failed
    };
}
```

## Extension Ideas

Try extending this example by:

1. **Adding New Payment Providers**: Apple Pay, Google Pay, cryptocurrency wallets
2. **Implementing Retry Logic**: Handle temporary failures with exponential backoff
3. **Adding Circuit Breaker**: Prevent cascading failures when providers are down
4. **Creating Configuration-Driven Adapters**: Load provider settings from configuration
5. **Implementing Webhook Handlers**: Process asynchronous payment notifications
6. **Adding Monitoring and Metrics**: Track adapter performance and success rates
7. **Creating Multi-Currency Support**: Handle currency conversion in adapters
8. **Implementing Fraud Detection**: Add fraud checking layer to adapters

## Best Practices Demonstrated

1. **Interface Segregation**: Clean, focused target interface
2. **Single Responsibility**: Each adapter handles one integration
3. **Error Handling**: Consistent error handling across all adapters
4. **Data Validation**: Input validation before calling external systems
5. **Resource Management**: Proper handling of external system connections
6. **Logging**: Comprehensive logging for debugging and monitoring

## Testing Strategies

The adapter pattern enables excellent testing:

- **Unit Testing**: Test each adapter independently
- **Integration Testing**: Test with real external systems
- **Mock Testing**: Easy to create test doubles for adapters
- **Contract Testing**: Verify adapters maintain interface contracts
