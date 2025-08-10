namespace Patterns_Example.Structural_Design_Patterns.Adapter;

/// <summary>
/// Legacy payment system that we need to integrate with.
/// This represents an existing system with its own interface that we cannot modify.
/// This is the "Adaptee" in the Adapter pattern.
/// </summary>
public class LegacyPayPalSystem
{
    public PayPalResponse ChargeCard(PayPalRequest request)
    {
        // Simulate legacy PayPal API call
        Console.WriteLine($"[Legacy PayPal] Processing charge: {request.Amount} {request.CurrencyCode}");
        
        // Simulate processing delay
        Thread.Sleep(100);
        
        var isSuccess = request.Amount > 0 && request.Amount <= 10000; // Simulate validation
        
        return new PayPalResponse
        {
            TransactionCode = $"PP_{Guid.NewGuid().ToString()[..8]}",
            IsApproved = isSuccess,
            ResponseMessage = isSuccess ? "Transaction approved" : "Transaction declined - amount too high",
            ProcessingFee = request.Amount * 0.029m, // 2.9% fee
            Timestamp = DateTime.Now
        };
    }

    public PayPalResponse RefundTransaction(string transactionCode, decimal refundAmount)
    {
        Console.WriteLine($"[Legacy PayPal] Processing refund: {refundAmount} for transaction {transactionCode}");
        
        Thread.Sleep(50);
        
        return new PayPalResponse
        {
            TransactionCode = $"RF_{Guid.NewGuid().ToString()[..8]}",
            IsApproved = true,
            ResponseMessage = "Refund processed successfully",
            ProcessingFee = 0,
            Timestamp = DateTime.Now
        };
    }

    public PayPalTransactionStatus CheckTransactionStatus(string transactionCode)
    {
        Console.WriteLine($"[Legacy PayPal] Checking status for transaction: {transactionCode}");
        
        return new PayPalTransactionStatus
        {
            TransactionCode = transactionCode,
            Status = "COMPLETED",
            LastUpdated = DateTime.Now
        };
    }
}

/// <summary>
/// Legacy PayPal request structure.
/// </summary>
public class PayPalRequest
{
    public decimal Amount { get; set; }
    public string CurrencyCode { get; set; } = "USD";
    public string CardNumber { get; set; } = string.Empty;
    public string ExpirationMonth { get; set; } = string.Empty;
    public string ExpirationYear { get; set; } = string.Empty;
    public string SecurityCode { get; set; } = string.Empty;
    public string CardHolderName { get; set; } = string.Empty;
    public string MerchantId { get; set; } = "MERCHANT_123";
}

/// <summary>
/// Legacy PayPal response structure.
/// </summary>
public class PayPalResponse
{
    public string TransactionCode { get; set; } = string.Empty;
    public bool IsApproved { get; set; }
    public string ResponseMessage { get; set; } = string.Empty;
    public decimal ProcessingFee { get; set; }
    public DateTime Timestamp { get; set; }
}

/// <summary>
/// Legacy PayPal transaction status structure.
/// </summary>
public class PayPalTransactionStatus
{
    public string TransactionCode { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime LastUpdated { get; set; }
}

/// <summary>
/// Another legacy payment system with a completely different interface.
/// This represents a different third-party system we need to integrate.
/// </summary>
public class LegacyStripeGateway
{
    public StripeChargeResult CreateCharge(StripeChargeParams chargeParams)
    {
        Console.WriteLine($"[Legacy Stripe] Creating charge: {chargeParams.AmountInCents / 100m:C}");
        
        Thread.Sleep(80);
        
        var success = chargeParams.AmountInCents > 0 && chargeParams.AmountInCents <= 1000000; // $10,000 limit
        
        return new StripeChargeResult
        {
            Id = $"ch_{Guid.NewGuid().ToString().Replace("-", "")[..24]}",
            Succeeded = success,
            FailureMessage = success ? null : "Your card was declined.",
            AmountInCents = chargeParams.AmountInCents,
            Currency = chargeParams.Currency,
            Created = DateTimeOffset.Now.ToUnixTimeSeconds()
        };
    }

    public StripeRefundResult CreateRefund(string chargeId, long amountInCents)
    {
        Console.WriteLine($"[Legacy Stripe] Creating refund: {amountInCents / 100m:C} for charge {chargeId}");
        
        Thread.Sleep(60);
        
        return new StripeRefundResult
        {
            Id = $"re_{Guid.NewGuid().ToString().Replace("-", "")[..24]}",
            ChargeId = chargeId,
            AmountInCents = amountInCents,
            Status = "succeeded",
            Created = DateTimeOffset.Now.ToUnixTimeSeconds()
        };
    }

    public StripeChargeResult RetrieveCharge(string chargeId)
    {
        Console.WriteLine($"[Legacy Stripe] Retrieving charge: {chargeId}");
        
        return new StripeChargeResult
        {
            Id = chargeId,
            Succeeded = true,
            AmountInCents = 5000, // $50.00
            Currency = "usd",
            Created = DateTimeOffset.Now.AddMinutes(-10).ToUnixTimeSeconds()
        };
    }
}

/// <summary>
/// Legacy Stripe charge parameters.
/// </summary>
public class StripeChargeParams
{
    public long AmountInCents { get; set; }
    public string Currency { get; set; } = "usd";
    public string Source { get; set; } = string.Empty; // Token representing payment method
    public string Description { get; set; } = string.Empty;
    public Dictionary<string, string> Metadata { get; set; } = new();
}

/// <summary>
/// Legacy Stripe charge result.
/// </summary>
public class StripeChargeResult
{
    public string Id { get; set; } = string.Empty;
    public bool Succeeded { get; set; }
    public string? FailureMessage { get; set; }
    public long AmountInCents { get; set; }
    public string Currency { get; set; } = string.Empty;
    public long Created { get; set; }
}

/// <summary>
/// Legacy Stripe refund result.
/// </summary>
public class StripeRefundResult
{
    public string Id { get; set; } = string.Empty;
    public string ChargeId { get; set; } = string.Empty;
    public long AmountInCents { get; set; }
    public string Status { get; set; } = string.Empty;
    public long Created { get; set; }
}

/// <summary>
/// Third legacy system with yet another different interface.
/// This represents a bank's direct API.
/// </summary>
public class LegacyBankTransferAPI
{
    public BankTransferResponse InitiateTransfer(BankTransferRequest request)
    {
        Console.WriteLine($"[Legacy Bank API] Initiating transfer: {request.TransferAmount:C}");
        
        Thread.Sleep(200); // Bank transfers are slower
        
        var success = !string.IsNullOrEmpty(request.FromAccount) && 
                     !string.IsNullOrEmpty(request.ToAccount) && 
                     request.TransferAmount > 0;
        
        return new BankTransferResponse
        {
            ReferenceNumber = $"BT{DateTime.Now:yyyyMMddHHmmss}{Random.Shared.Next(1000, 9999)}",
            Status = success ? "INITIATED" : "REJECTED",
            Message = success ? "Transfer initiated successfully" : "Invalid account information",
            EstimatedCompletionTime = DateTime.Now.AddBusinessDays(1)
        };
    }

    public BankTransferStatus GetTransferStatus(string referenceNumber)
    {
        Console.WriteLine($"[Legacy Bank API] Checking transfer status: {referenceNumber}");
        
        return new BankTransferStatus
        {
            ReferenceNumber = referenceNumber,
            CurrentStatus = "COMPLETED",
            CompletedAt = DateTime.Now.AddMinutes(-30),
            StatusHistory = new List<string> { "INITIATED", "PROCESSING", "COMPLETED" }
        };
    }
}

/// <summary>
/// Legacy bank transfer request.
/// </summary>
public class BankTransferRequest
{
    public string FromAccount { get; set; } = string.Empty;
    public string ToAccount { get; set; } = string.Empty;
    public string RoutingNumber { get; set; } = string.Empty;
    public decimal TransferAmount { get; set; }
    public string Purpose { get; set; } = string.Empty;
    public string CustomerReference { get; set; } = string.Empty;
}

/// <summary>
/// Legacy bank transfer response.
/// </summary>
public class BankTransferResponse
{
    public string ReferenceNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime EstimatedCompletionTime { get; set; }
}

/// <summary>
/// Legacy bank transfer status.
/// </summary>
public class BankTransferStatus
{
    public string ReferenceNumber { get; set; } = string.Empty;
    public string CurrentStatus { get; set; } = string.Empty;
    public DateTime CompletedAt { get; set; }
    public List<string> StatusHistory { get; set; } = new();
}

/// <summary>
/// Extension method to add business days to DateTime.
/// </summary>
public static class DateTimeExtensions
{
    public static DateTime AddBusinessDays(this DateTime date, int businessDays)
    {
        var result = date;
        while (businessDays > 0)
        {
            result = result.AddDays(1);
            if (result.DayOfWeek != DayOfWeek.Saturday && result.DayOfWeek != DayOfWeek.Sunday)
            {
                businessDays--;
            }
        }
        return result;
    }
}
