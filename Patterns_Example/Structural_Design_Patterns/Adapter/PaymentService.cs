namespace Patterns_Example.Structural_Design_Patterns.Adapter;

/// <summary>
/// High-level payment service that uses the IPaymentProcessor interface.
/// This demonstrates how the client code remains unchanged regardless of which
/// payment system is being used behind the scenes.
/// </summary>
public class PaymentService
{
    private readonly IPaymentProcessor _paymentProcessor;
    private readonly List<PaymentTransaction> _transactionHistory;

    public PaymentService(IPaymentProcessor paymentProcessor)
    {
        _paymentProcessor = paymentProcessor ?? throw new ArgumentNullException(nameof(paymentProcessor));
        _transactionHistory = new List<PaymentTransaction>();
    }

    /// <summary>
    /// Processes a payment using the configured payment processor.
    /// </summary>
    public PaymentResult ProcessPayment(PaymentRequest request)
    {
        Console.WriteLine($"\n[Payment Service] Processing payment: {request}");
        
        // Validate payment method
        if (!_paymentProcessor.ValidatePaymentMethod(request.PaymentMethod))
        {
            var validationResult = new PaymentResult
            {
                IsSuccess = false,
                TransactionId = request.TransactionId,
                Message = "Invalid payment method for this processor",
                Status = PaymentStatus.Failed
            };
            
            RecordTransaction(request, validationResult);
            return validationResult;
        }

        // Process the payment
        var result = _paymentProcessor.ProcessPayment(request);
        
        // Record the transaction
        RecordTransaction(request, result);
        
        Console.WriteLine($"[Payment Service] Result: {result}");
        return result;
    }

    /// <summary>
    /// Processes a refund for a previous transaction.
    /// </summary>
    public PaymentResult ProcessRefund(string transactionId, decimal amount, string reason = "Customer request")
    {
        Console.WriteLine($"\n[Payment Service] Processing refund: {amount:C} for transaction {transactionId}");
        Console.WriteLine($"[Payment Service] Reason: {reason}");
        
        // Find the original transaction
        var originalTransaction = _transactionHistory.FirstOrDefault(t => t.Request.TransactionId == transactionId);
        if (originalTransaction == null)
        {
            var notFoundResult = new PaymentResult
            {
                IsSuccess = false,
                TransactionId = transactionId,
                Message = "Original transaction not found",
                Status = PaymentStatus.Failed
            };
            
            Console.WriteLine($"[Payment Service] Refund failed: {notFoundResult.Message}");
            return notFoundResult;
        }

        // Validate refund amount
        if (amount > originalTransaction.Result.ProcessedAmount)
        {
            var invalidAmountResult = new PaymentResult
            {
                IsSuccess = false,
                TransactionId = transactionId,
                Message = $"Refund amount ({amount:C}) cannot exceed original payment amount ({originalTransaction.Result.ProcessedAmount:C})",
                Status = PaymentStatus.Failed
            };
            
            Console.WriteLine($"[Payment Service] Refund failed: {invalidAmountResult.Message}");
            return invalidAmountResult;
        }

        // Process the refund
        var result = _paymentProcessor.RefundPayment(transactionId, amount);
        
        Console.WriteLine($"[Payment Service] Refund result: {result}");
        return result;
    }

    /// <summary>
    /// Gets the status of a payment transaction.
    /// </summary>
    public PaymentStatus GetTransactionStatus(string transactionId)
    {
        Console.WriteLine($"\n[Payment Service] Checking status for transaction: {transactionId}");
        
        var status = _paymentProcessor.GetPaymentStatus(transactionId);
        
        Console.WriteLine($"[Payment Service] Transaction {transactionId} status: {status}");
        return status;
    }

    /// <summary>
    /// Gets the transaction history.
    /// </summary>
    public List<PaymentTransaction> GetTransactionHistory()
    {
        return new List<PaymentTransaction>(_transactionHistory);
    }

    /// <summary>
    /// Gets transactions filtered by status.
    /// </summary>
    public List<PaymentTransaction> GetTransactionsByStatus(PaymentStatus status)
    {
        return _transactionHistory.Where(t => t.Result.Status == status).ToList();
    }

    /// <summary>
    /// Gets transactions for a specific customer.
    /// </summary>
    public List<PaymentTransaction> GetCustomerTransactions(string customerId)
    {
        return _transactionHistory.Where(t => t.Request.Customer.CustomerId == customerId).ToList();
    }

    /// <summary>
    /// Calculates total processed amount for a date range.
    /// </summary>
    public decimal GetTotalProcessedAmount(DateTime fromDate, DateTime toDate)
    {
        return _transactionHistory
            .Where(t => t.Result.IsSuccess && 
                       t.Result.ProcessedAt >= fromDate && 
                       t.Result.ProcessedAt <= toDate)
            .Sum(t => t.Result.ProcessedAmount);
    }

    /// <summary>
    /// Gets payment statistics.
    /// </summary>
    public PaymentStatistics GetPaymentStatistics()
    {
        var transactions = _transactionHistory;
        var successfulTransactions = transactions.Where(t => t.Result.IsSuccess).ToList();
        
        return new PaymentStatistics
        {
            TotalTransactions = transactions.Count,
            SuccessfulTransactions = successfulTransactions.Count,
            FailedTransactions = transactions.Count - successfulTransactions.Count,
            TotalAmount = successfulTransactions.Sum(t => t.Result.ProcessedAmount),
            AverageTransactionAmount = successfulTransactions.Count > 0 
                ? successfulTransactions.Average(t => t.Result.ProcessedAmount) 
                : 0,
            SuccessRate = transactions.Count > 0 
                ? (double)successfulTransactions.Count / transactions.Count * 100 
                : 0,
            PaymentMethodBreakdown = transactions
                .GroupBy(t => t.Request.PaymentMethod.Type)
                .ToDictionary(g => g.Key, g => g.Count())
        };
    }

    private void RecordTransaction(PaymentRequest request, PaymentResult result)
    {
        var transaction = new PaymentTransaction
        {
            Request = request,
            Result = result,
            ProcessedAt = DateTime.Now
        };
        
        _transactionHistory.Add(transaction);
    }
}

/// <summary>
/// Represents a complete payment transaction with request and result.
/// </summary>
public class PaymentTransaction
{
    public PaymentRequest Request { get; set; } = new();
    public PaymentResult Result { get; set; } = new();
    public DateTime ProcessedAt { get; set; }

    public override string ToString()
    {
        return $"Transaction {Request.TransactionId}: {Result.Status} - {Result.ProcessedAmount:C} at {ProcessedAt:yyyy-MM-dd HH:mm:ss}";
    }
}

/// <summary>
/// Payment processing statistics.
/// </summary>
public class PaymentStatistics
{
    public int TotalTransactions { get; set; }
    public int SuccessfulTransactions { get; set; }
    public int FailedTransactions { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal AverageTransactionAmount { get; set; }
    public double SuccessRate { get; set; }
    public Dictionary<PaymentType, int> PaymentMethodBreakdown { get; set; } = new();

    public override string ToString()
    {
        var breakdown = string.Join(", ", PaymentMethodBreakdown.Select(kvp => $"{kvp.Key}: {kvp.Value}"));
        
        return $"""
            Payment Statistics:
              Total Transactions: {TotalTransactions}
              Successful: {SuccessfulTransactions}, Failed: {FailedTransactions}
              Success Rate: {SuccessRate:F1}%
              Total Amount: {TotalAmount:C}
              Average Transaction: {AverageTransactionAmount:C}
              Payment Methods: {breakdown}
            """;
    }
}
