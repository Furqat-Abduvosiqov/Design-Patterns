namespace Patterns_Example.Structural_Design_Patterns.Adapter;

/// <summary>
/// Example class demonstrating the Adapter pattern usage
/// </summary>
public static class AdapterExample
{
    public static void RunExample()
    {
        Console.WriteLine("=== Adapter Pattern Example: Payment Processing System ===\n");
        
        // Demonstrate individual adapters
        DemonstratePayPalAdapter();
        DemonstrateStripeAdapter();
        DemonstrateBankTransferAdapter();
        
        // Demonstrate composite adapter
        DemonstrateCompositeAdapter();
        
        // Demonstrate payment service with different adapters
        DemonstratePaymentServiceWithDifferentAdapters();
        
        Console.WriteLine("=== Adapter Pattern Benefits Demonstrated ===");
        Console.WriteLine("✓ Integration with legacy systems without modifying their code");
        Console.WriteLine("✓ Uniform interface for different payment providers");
        Console.WriteLine("✓ Easy switching between payment processors");
        Console.WriteLine("✓ Isolation of integration complexity in adapter classes");
        Console.WriteLine("✓ Support for multiple incompatible interfaces");
        Console.WriteLine("✓ Maintainable and testable payment processing");
    }

    private static void DemonstratePayPalAdapter()
    {
        Console.WriteLine("1. PayPal Adapter Demonstration:");
        Console.WriteLine(new string('=', 50));
        
        // Create legacy PayPal system and adapter
        var legacyPayPal = new LegacyPayPalSystem();
        var paypalAdapter = new PayPalAdapter(legacyPayPal);
        
        // Create a payment request
        var request = CreateSamplePaymentRequest(PaymentType.CreditCard, 99.99m);
        
        // Process payment through adapter
        var result = paypalAdapter.ProcessPayment(request);
        Console.WriteLine($"Payment Result: {result}");
        
        // Check status
        var status = paypalAdapter.GetPaymentStatus(request.TransactionId);
        Console.WriteLine($"Payment Status: {status}");
        
        // Process refund
        if (result.IsSuccess)
        {
            var refundResult = paypalAdapter.RefundPayment(request.TransactionId, 50.00m);
            Console.WriteLine($"Refund Result: {refundResult}");
        }
        
        Console.WriteLine();
    }

    private static void DemonstrateStripeAdapter()
    {
        Console.WriteLine("2. Stripe Adapter Demonstration:");
        Console.WriteLine(new string('=', 50));
        
        // Create legacy Stripe system and adapter
        var legacyStripe = new LegacyStripeGateway();
        var stripeAdapter = new StripeAdapter(legacyStripe);
        
        // Create a payment request
        var request = CreateSamplePaymentRequest(PaymentType.CreditCard, 149.99m);
        
        // Process payment through adapter
        var result = stripeAdapter.ProcessPayment(request);
        Console.WriteLine($"Payment Result: {result}");
        
        // Check status
        var status = stripeAdapter.GetPaymentStatus(request.TransactionId);
        Console.WriteLine($"Payment Status: {status}");
        
        // Process refund
        if (result.IsSuccess)
        {
            var refundResult = stripeAdapter.RefundPayment(request.TransactionId, 75.00m);
            Console.WriteLine($"Refund Result: {refundResult}");
        }
        
        Console.WriteLine();
    }

    private static void DemonstrateBankTransferAdapter()
    {
        Console.WriteLine("3. Bank Transfer Adapter Demonstration:");
        Console.WriteLine(new string('=', 50));
        
        // Create legacy bank API and adapter
        var legacyBankAPI = new LegacyBankTransferAPI();
        var bankAdapter = new BankTransferAdapter(legacyBankAPI);
        
        // Create a bank transfer payment request
        var request = CreateSamplePaymentRequest(PaymentType.BankTransfer, 500.00m);
        request.PaymentMethod.BankAccount = "123456789";
        request.PaymentMethod.RoutingNumber = "021000021";
        
        // Process payment through adapter
        var result = bankAdapter.ProcessPayment(request);
        Console.WriteLine($"Payment Result: {result}");
        
        // Check status
        var status = bankAdapter.GetPaymentStatus(request.TransactionId);
        Console.WriteLine($"Payment Status: {status}");
        
        Console.WriteLine();
    }

    private static void DemonstrateCompositeAdapter()
    {
        Console.WriteLine("4. Composite Adapter Demonstration:");
        Console.WriteLine(new string('=', 50));
        
        // Create composite adapter and register individual adapters
        var compositeAdapter = new CompositePaymentAdapter();
        
        // Register adapters for different payment types
        compositeAdapter.RegisterAdapter(PaymentType.CreditCard, new PayPalAdapter(new LegacyPayPalSystem()));
        compositeAdapter.RegisterAdapter(PaymentType.DebitCard, new StripeAdapter(new LegacyStripeGateway()));
        compositeAdapter.RegisterAdapter(PaymentType.BankTransfer, new BankTransferAdapter(new LegacyBankTransferAPI()));
        
        Console.WriteLine("Registered adapters:");
        foreach (var adapter in compositeAdapter.GetRegisteredAdapters())
        {
            Console.WriteLine($"  {adapter.Key}: {adapter.Value}");
        }
        Console.WriteLine();
        
        // Process different types of payments
        var requests = new[]
        {
            CreateSamplePaymentRequest(PaymentType.CreditCard, 199.99m),
            CreateSamplePaymentRequest(PaymentType.DebitCard, 89.99m),
            CreateSamplePaymentRequest(PaymentType.BankTransfer, 1000.00m)
        };
        
        // Set up bank transfer details
        requests[2].PaymentMethod.BankAccount = "987654321";
        requests[2].PaymentMethod.RoutingNumber = "021000021";
        
        foreach (var request in requests)
        {
            Console.WriteLine($"Processing {request.PaymentMethod.Type} payment...");
            var result = compositeAdapter.ProcessPayment(request);
            Console.WriteLine($"Result: {result}");
            Console.WriteLine();
        }
    }

    private static void DemonstratePaymentServiceWithDifferentAdapters()
    {
        Console.WriteLine("5. Payment Service with Different Adapters:");
        Console.WriteLine(new string('=', 50));
        
        // Demonstrate how the same PaymentService can work with different adapters
        var adapters = new Dictionary<string, IPaymentProcessor>
        {
            ["PayPal"] = new PayPalAdapter(new LegacyPayPalSystem()),
            ["Stripe"] = new StripeAdapter(new LegacyStripeGateway()),
            ["Bank"] = new BankTransferAdapter(new LegacyBankTransferAPI())
        };
        
        foreach (var adapterPair in adapters)
        {
            Console.WriteLine($"--- Using {adapterPair.Key} Adapter ---");
            
            var paymentService = new PaymentService(adapterPair.Value);
            
            // Create appropriate payment request for each adapter
            PaymentRequest request;
            if (adapterPair.Key == "Bank")
            {
                request = CreateSamplePaymentRequest(PaymentType.BankTransfer, 250.00m);
                request.PaymentMethod.BankAccount = "555666777";
                request.PaymentMethod.RoutingNumber = "021000021";
            }
            else
            {
                request = CreateSamplePaymentRequest(PaymentType.CreditCard, 125.00m);
            }
            
            // Process payment
            var result = paymentService.ProcessPayment(request);
            
            // Check status
            var status = paymentService.GetTransactionStatus(request.TransactionId);
            
            // Process refund if successful
            if (result.IsSuccess)
            {
                var refundResult = paymentService.ProcessRefund(request.TransactionId, 50.00m, "Customer requested refund");
            }
            
            // Show statistics
            var stats = paymentService.GetPaymentStatistics();
            Console.WriteLine(stats);
            Console.WriteLine();
        }
    }

    private static PaymentRequest CreateSamplePaymentRequest(PaymentType paymentType, decimal amount)
    {
        return new PaymentRequest
        {
            Amount = amount,
            Currency = "USD",
            PaymentMethod = new PaymentMethod
            {
                Type = paymentType,
                CardNumber = "4111111111111111",
                ExpiryDate = "12/25",
                CVV = "123",
                CardHolderName = "John Doe"
            },
            Customer = new CustomerInfo
            {
                CustomerId = "CUST_" + Random.Shared.Next(1000, 9999),
                Name = "John Doe",
                Email = "john.doe@example.com",
                Phone = "+1-555-123-4567",
                BillingAddress = new Address
                {
                    Street = "123 Main St",
                    City = "Anytown",
                    State = "CA",
                    ZipCode = "12345",
                    Country = "USA"
                }
            },
            Description = $"Sample {paymentType} payment"
        };
    }
}
