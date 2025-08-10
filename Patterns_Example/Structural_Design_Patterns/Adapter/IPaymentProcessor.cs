namespace Patterns_Example.Structural_Design_Patterns.Adapter;

/// <summary>
/// Target interface that our application expects.
/// This represents the interface our application code is designed to work with.
/// </summary>
public interface IPaymentProcessor
{
    PaymentResult ProcessPayment(PaymentRequest request);
    PaymentResult RefundPayment(string transactionId, decimal amount);
    PaymentStatus GetPaymentStatus(string transactionId);
    bool ValidatePaymentMethod(PaymentMethod paymentMethod);
}

/// <summary>
/// Payment request data structure used by our application.
/// </summary>
public class PaymentRequest
{
    public string TransactionId { get; set; } = Guid.NewGuid().ToString();
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public PaymentMethod PaymentMethod { get; set; } = new();
    public CustomerInfo Customer { get; set; } = new();
    public string Description { get; set; } = string.Empty;
    public DateTime RequestTime { get; set; } = DateTime.Now;

    public override string ToString()
    {
        return $"Payment Request: {TransactionId}, Amount: {Amount:C} {Currency}, Method: {PaymentMethod.Type}";
    }
}

/// <summary>
/// Payment result returned by our application's payment interface.
/// </summary>
public class PaymentResult
{
    public bool IsSuccess { get; set; }
    public string TransactionId { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public PaymentStatus Status { get; set; }
    public DateTime ProcessedAt { get; set; } = DateTime.Now;
    public decimal ProcessedAmount { get; set; }
    public string AuthorizationCode { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"Payment Result: {(IsSuccess ? "SUCCESS" : "FAILED")} - {TransactionId} - {Message}";
    }
}

/// <summary>
/// Payment method information.
/// </summary>
public class PaymentMethod
{
    public PaymentType Type { get; set; }
    public string CardNumber { get; set; } = string.Empty;
    public string ExpiryDate { get; set; } = string.Empty;
    public string CVV { get; set; } = string.Empty;
    public string CardHolderName { get; set; } = string.Empty;
    public string BankAccount { get; set; } = string.Empty;
    public string RoutingNumber { get; set; } = string.Empty;

    public override string ToString()
    {
        return Type switch
        {
            PaymentType.CreditCard => $"Credit Card ending in {CardNumber[^4..]}",
            PaymentType.DebitCard => $"Debit Card ending in {CardNumber[^4..]}",
            PaymentType.BankTransfer => $"Bank Transfer from {BankAccount}",
            PaymentType.DigitalWallet => $"Digital Wallet",
            _ => "Unknown Payment Method"
        };
    }
}

/// <summary>
/// Customer information.
/// </summary>
public class CustomerInfo
{
    public string CustomerId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public Address BillingAddress { get; set; } = new();

    public override string ToString()
    {
        return $"Customer: {Name} ({Email})";
    }
}

/// <summary>
/// Address information.
/// </summary>
public class Address
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"{Street}, {City}, {State} {ZipCode}, {Country}";
    }
}

/// <summary>
/// Payment types supported by our application.
/// </summary>
public enum PaymentType
{
    CreditCard,
    DebitCard,
    BankTransfer,
    DigitalWallet
}

/// <summary>
/// Payment status enumeration.
/// </summary>
public enum PaymentStatus
{
    Pending,
    Processing,
    Completed,
    Failed,
    Cancelled,
    Refunded
}
