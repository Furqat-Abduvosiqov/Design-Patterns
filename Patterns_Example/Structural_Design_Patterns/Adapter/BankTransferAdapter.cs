namespace Patterns_Example.Structural_Design_Patterns.Adapter;

/// <summary>
/// Adapter that makes the legacy Bank Transfer API compatible with our IPaymentProcessor interface.
/// This demonstrates adapting a system with very different semantics (bank transfers vs card payments).
/// </summary>
public class BankTransferAdapter : IPaymentProcessor
{
    private readonly LegacyBankTransferAPI _legacyBankAPI;
    private readonly Dictionary<string, string> _transactionMapping;

    public BankTransferAdapter(LegacyBankTransferAPI legacyBankAPI)
    {
        _legacyBankAPI = legacyBankAPI ?? throw new ArgumentNullException(nameof(legacyBankAPI));
        _transactionMapping = new Dictionary<string, string>();
    }

    public PaymentResult ProcessPayment(PaymentRequest request)
    {
        try
        {
            // Validate that this is a bank transfer request
            if (request.PaymentMethod.Type != PaymentType.BankTransfer)
            {
                return new PaymentResult
                {
                    IsSuccess = false,
                    TransactionId = request.TransactionId,
                    Message = "Bank Transfer adapter only supports bank transfer payments",
                    Status = PaymentStatus.Failed
                };
            }

            // Convert our request to bank transfer format
            var bankRequest = ConvertToBankTransferRequest(request);
            
            // Call legacy bank API
            var bankResponse = _legacyBankAPI.InitiateTransfer(bankRequest);
            
            // Store mapping
            _transactionMapping[request.TransactionId] = bankResponse.ReferenceNumber;
            
            // Convert response
            return ConvertFromBankTransferResponse(bankResponse, request);
        }
        catch (Exception ex)
        {
            return new PaymentResult
            {
                IsSuccess = false,
                TransactionId = request.TransactionId,
                Message = $"Bank transfer processing error: {ex.Message}",
                Status = PaymentStatus.Failed
            };
        }
    }

    public PaymentResult RefundPayment(string transactionId, decimal amount)
    {
        try
        {
            if (!_transactionMapping.TryGetValue(transactionId, out var bankReferenceNumber))
            {
                return new PaymentResult
                {
                    IsSuccess = false,
                    TransactionId = transactionId,
                    Message = "Transaction not found in bank system",
                    Status = PaymentStatus.Failed
                };
            }

            // For bank transfers, "refund" means initiating a reverse transfer
            // This is a simplified implementation - in reality, this would be more complex
            Console.WriteLine($"[Bank Transfer Adapter] Initiating reverse transfer for {bankReferenceNumber}");
            
            // Simulate reverse transfer
            var reverseRequest = new BankTransferRequest
            {
                FromAccount = "MERCHANT_ACCOUNT", // Our merchant account
                ToAccount = "CUSTOMER_ACCOUNT", // Customer's account (would be stored from original transaction)
                TransferAmount = amount,
                Purpose = $"Refund for transaction {transactionId}",
                CustomerReference = $"REFUND_{transactionId}"
            };

            var reverseResponse = _legacyBankAPI.InitiateTransfer(reverseRequest);
            
            return new PaymentResult
            {
                IsSuccess = reverseResponse.Status == "INITIATED",
                TransactionId = transactionId,
                Message = reverseResponse.Message,
                Status = reverseResponse.Status == "INITIATED" ? PaymentStatus.Processing : PaymentStatus.Failed,
                ProcessedAmount = amount,
                AuthorizationCode = reverseResponse.ReferenceNumber
            };
        }
        catch (Exception ex)
        {
            return new PaymentResult
            {
                IsSuccess = false,
                TransactionId = transactionId,
                Message = $"Bank transfer refund error: {ex.Message}",
                Status = PaymentStatus.Failed
            };
        }
    }

    public PaymentStatus GetPaymentStatus(string transactionId)
    {
        try
        {
            if (!_transactionMapping.TryGetValue(transactionId, out var bankReferenceNumber))
            {
                return PaymentStatus.Failed;
            }

            var status = _legacyBankAPI.GetTransferStatus(bankReferenceNumber);
            
            // Convert bank status to our status enum
            return status.CurrentStatus.ToUpper() switch
            {
                "INITIATED" => PaymentStatus.Pending,
                "PROCESSING" => PaymentStatus.Processing,
                "COMPLETED" => PaymentStatus.Completed,
                "REJECTED" => PaymentStatus.Failed,
                "CANCELLED" => PaymentStatus.Cancelled,
                _ => PaymentStatus.Failed
            };
        }
        catch
        {
            return PaymentStatus.Failed;
        }
    }

    public bool ValidatePaymentMethod(PaymentMethod paymentMethod)
    {
        // This adapter only supports bank transfers
        return paymentMethod.Type == PaymentType.BankTransfer &&
               !string.IsNullOrEmpty(paymentMethod.BankAccount) &&
               !string.IsNullOrEmpty(paymentMethod.RoutingNumber);
    }

    private BankTransferRequest ConvertToBankTransferRequest(PaymentRequest request)
    {
        return new BankTransferRequest
        {
            FromAccount = request.PaymentMethod.BankAccount,
            ToAccount = "MERCHANT_ACCOUNT_123", // Our merchant account
            RoutingNumber = request.PaymentMethod.RoutingNumber,
            TransferAmount = request.Amount,
            Purpose = request.Description,
            CustomerReference = request.TransactionId
        };
    }

    private PaymentResult ConvertFromBankTransferResponse(BankTransferResponse response, PaymentRequest originalRequest)
    {
        var isSuccess = response.Status == "INITIATED";
        
        return new PaymentResult
        {
            IsSuccess = isSuccess,
            TransactionId = originalRequest.TransactionId,
            Message = response.Message,
            Status = isSuccess ? PaymentStatus.Processing : PaymentStatus.Failed, // Bank transfers start as processing
            ProcessedAmount = originalRequest.Amount,
            AuthorizationCode = response.ReferenceNumber
        };
    }
}

/// <summary>
/// Composite adapter that can route payments to different legacy systems based on payment method.
/// This demonstrates the Adapter pattern combined with the Strategy pattern.
/// </summary>
public class CompositePaymentAdapter : IPaymentProcessor
{
    private readonly Dictionary<PaymentType, IPaymentProcessor> _adapters;

    public CompositePaymentAdapter()
    {
        _adapters = new Dictionary<PaymentType, IPaymentProcessor>();
    }

    /// <summary>
    /// Registers an adapter for a specific payment type.
    /// </summary>
    public void RegisterAdapter(PaymentType paymentType, IPaymentProcessor adapter)
    {
        _adapters[paymentType] = adapter;
    }

    public PaymentResult ProcessPayment(PaymentRequest request)
    {
        if (_adapters.TryGetValue(request.PaymentMethod.Type, out var adapter))
        {
            Console.WriteLine($"[Composite Adapter] Routing {request.PaymentMethod.Type} payment to appropriate adapter");
            return adapter.ProcessPayment(request);
        }

        return new PaymentResult
        {
            IsSuccess = false,
            TransactionId = request.TransactionId,
            Message = $"No adapter available for payment type: {request.PaymentMethod.Type}",
            Status = PaymentStatus.Failed
        };
    }

    public PaymentResult RefundPayment(string transactionId, decimal amount)
    {
        // In a real implementation, you'd need to track which adapter was used for the original transaction
        // For this demo, we'll try each adapter until we find one that can handle the refund
        foreach (var adapter in _adapters.Values)
        {
            var result = adapter.RefundPayment(transactionId, amount);
            if (result.IsSuccess || result.Message != "Transaction not found in PayPal system" && 
                result.Message != "Transaction not found in Stripe system" && 
                result.Message != "Transaction not found in bank system")
            {
                return result;
            }
        }

        return new PaymentResult
        {
            IsSuccess = false,
            TransactionId = transactionId,
            Message = "Transaction not found in any payment system",
            Status = PaymentStatus.Failed
        };
    }

    public PaymentStatus GetPaymentStatus(string transactionId)
    {
        // Similar to refund, try each adapter
        foreach (var adapter in _adapters.Values)
        {
            var status = adapter.GetPaymentStatus(transactionId);
            if (status != PaymentStatus.Failed)
            {
                return status;
            }
        }

        return PaymentStatus.Failed;
    }

    public bool ValidatePaymentMethod(PaymentMethod paymentMethod)
    {
        if (_adapters.TryGetValue(paymentMethod.Type, out var adapter))
        {
            return adapter.ValidatePaymentMethod(paymentMethod);
        }

        return false;
    }

    /// <summary>
    /// Gets information about registered adapters.
    /// </summary>
    public Dictionary<PaymentType, string> GetRegisteredAdapters()
    {
        return _adapters.ToDictionary(
            kvp => kvp.Key, 
            kvp => kvp.Value.GetType().Name
        );
    }
}
