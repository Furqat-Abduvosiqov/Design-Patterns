namespace Patterns_Example.Structural_Design_Patterns.Adapter;

/// <summary>
/// Adapter that makes the legacy PayPal system compatible with our IPaymentProcessor interface.
/// This is the "Adapter" in the Adapter pattern.
/// </summary>
public class PayPalAdapter : IPaymentProcessor
{
    private readonly LegacyPayPalSystem _legacyPayPal;
    private readonly Dictionary<string, string> _transactionMapping; // Maps our transaction IDs to PayPal's

    public PayPalAdapter(LegacyPayPalSystem legacyPayPal)
    {
        _legacyPayPal = legacyPayPal ?? throw new ArgumentNullException(nameof(legacyPayPal));
        _transactionMapping = new Dictionary<string, string>();
    }

    public PaymentResult ProcessPayment(PaymentRequest request)
    {
        try
        {
            // Convert our request format to PayPal's expected format
            var paypalRequest = ConvertToPayPalRequest(request);
            
            // Call the legacy PayPal system
            var paypalResponse = _legacyPayPal.ChargeCard(paypalRequest);
            
            // Store the mapping between our transaction ID and PayPal's
            _transactionMapping[request.TransactionId] = paypalResponse.TransactionCode;
            
            // Convert PayPal's response back to our format
            return ConvertFromPayPalResponse(paypalResponse, request);
        }
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
    }

    public PaymentResult RefundPayment(string transactionId, decimal amount)
    {
        try
        {
            // Get the PayPal transaction code from our mapping
            if (!_transactionMapping.TryGetValue(transactionId, out var paypalTransactionCode))
            {
                return new PaymentResult
                {
                    IsSuccess = false,
                    TransactionId = transactionId,
                    Message = "Transaction not found in PayPal system",
                    Status = PaymentStatus.Failed
                };
            }

            // Call PayPal's refund method
            var paypalResponse = _legacyPayPal.RefundTransaction(paypalTransactionCode, amount);
            
            // Convert response
            return new PaymentResult
            {
                IsSuccess = paypalResponse.IsApproved,
                TransactionId = transactionId,
                Message = paypalResponse.ResponseMessage,
                Status = paypalResponse.IsApproved ? PaymentStatus.Refunded : PaymentStatus.Failed,
                ProcessedAmount = amount,
                AuthorizationCode = paypalResponse.TransactionCode
            };
        }
        catch (Exception ex)
        {
            return new PaymentResult
            {
                IsSuccess = false,
                TransactionId = transactionId,
                Message = $"PayPal refund error: {ex.Message}",
                Status = PaymentStatus.Failed
            };
        }
    }

    public PaymentStatus GetPaymentStatus(string transactionId)
    {
        try
        {
            if (!_transactionMapping.TryGetValue(transactionId, out var paypalTransactionCode))
            {
                return PaymentStatus.Failed;
            }

            var status = _legacyPayPal.CheckTransactionStatus(paypalTransactionCode);
            
            // Convert PayPal status to our status enum
            return status.Status.ToUpper() switch
            {
                "COMPLETED" => PaymentStatus.Completed,
                "PENDING" => PaymentStatus.Pending,
                "PROCESSING" => PaymentStatus.Processing,
                "FAILED" => PaymentStatus.Failed,
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
        // PayPal supports credit and debit cards
        return paymentMethod.Type == PaymentType.CreditCard || 
               paymentMethod.Type == PaymentType.DebitCard;
    }

    private PayPalRequest ConvertToPayPalRequest(PaymentRequest request)
    {
        return new PayPalRequest
        {
            Amount = request.Amount,
            CurrencyCode = request.Currency,
            CardNumber = request.PaymentMethod.CardNumber,
            ExpirationMonth = request.PaymentMethod.ExpiryDate.Split('/')[0],
            ExpirationYear = request.PaymentMethod.ExpiryDate.Split('/')[1],
            SecurityCode = request.PaymentMethod.CVV,
            CardHolderName = request.PaymentMethod.CardHolderName
        };
    }

    private PaymentResult ConvertFromPayPalResponse(PayPalResponse response, PaymentRequest originalRequest)
    {
        return new PaymentResult
        {
            IsSuccess = response.IsApproved,
            TransactionId = originalRequest.TransactionId,
            Message = response.ResponseMessage,
            Status = response.IsApproved ? PaymentStatus.Completed : PaymentStatus.Failed,
            ProcessedAmount = originalRequest.Amount,
            AuthorizationCode = response.TransactionCode
        };
    }
}

/// <summary>
/// Adapter that makes the legacy Stripe system compatible with our IPaymentProcessor interface.
/// </summary>
public class StripeAdapter : IPaymentProcessor
{
    private readonly LegacyStripeGateway _legacyStripe;
    private readonly Dictionary<string, string> _transactionMapping;

    public StripeAdapter(LegacyStripeGateway legacyStripe)
    {
        _legacyStripe = legacyStripe ?? throw new ArgumentNullException(nameof(legacyStripe));
        _transactionMapping = new Dictionary<string, string>();
    }

    public PaymentResult ProcessPayment(PaymentRequest request)
    {
        try
        {
            // Convert our request to Stripe's format
            var stripeParams = ConvertToStripeParams(request);
            
            // Call legacy Stripe API
            var stripeResult = _legacyStripe.CreateCharge(stripeParams);
            
            // Store mapping
            _transactionMapping[request.TransactionId] = stripeResult.Id;
            
            // Convert response
            return ConvertFromStripeResult(stripeResult, request);
        }
        catch (Exception ex)
        {
            return new PaymentResult
            {
                IsSuccess = false,
                TransactionId = request.TransactionId,
                Message = $"Stripe processing error: {ex.Message}",
                Status = PaymentStatus.Failed
            };
        }
    }

    public PaymentResult RefundPayment(string transactionId, decimal amount)
    {
        try
        {
            if (!_transactionMapping.TryGetValue(transactionId, out var stripeChargeId))
            {
                return new PaymentResult
                {
                    IsSuccess = false,
                    TransactionId = transactionId,
                    Message = "Transaction not found in Stripe system",
                    Status = PaymentStatus.Failed
                };
            }

            var amountInCents = (long)(amount * 100);
            var refundResult = _legacyStripe.CreateRefund(stripeChargeId, amountInCents);
            
            return new PaymentResult
            {
                IsSuccess = refundResult.Status == "succeeded",
                TransactionId = transactionId,
                Message = refundResult.Status == "succeeded" ? "Refund processed" : "Refund failed",
                Status = refundResult.Status == "succeeded" ? PaymentStatus.Refunded : PaymentStatus.Failed,
                ProcessedAmount = amount,
                AuthorizationCode = refundResult.Id
            };
        }
        catch (Exception ex)
        {
            return new PaymentResult
            {
                IsSuccess = false,
                TransactionId = transactionId,
                Message = $"Stripe refund error: {ex.Message}",
                Status = PaymentStatus.Failed
            };
        }
    }

    public PaymentStatus GetPaymentStatus(string transactionId)
    {
        try
        {
            if (!_transactionMapping.TryGetValue(transactionId, out var stripeChargeId))
            {
                return PaymentStatus.Failed;
            }

            var charge = _legacyStripe.RetrieveCharge(stripeChargeId);
            return charge.Succeeded ? PaymentStatus.Completed : PaymentStatus.Failed;
        }
        catch
        {
            return PaymentStatus.Failed;
        }
    }

    public bool ValidatePaymentMethod(PaymentMethod paymentMethod)
    {
        // Stripe supports credit cards, debit cards, and digital wallets
        return paymentMethod.Type == PaymentType.CreditCard || 
               paymentMethod.Type == PaymentType.DebitCard ||
               paymentMethod.Type == PaymentType.DigitalWallet;
    }

    private StripeChargeParams ConvertToStripeParams(PaymentRequest request)
    {
        return new StripeChargeParams
        {
            AmountInCents = (long)(request.Amount * 100), // Stripe uses cents
            Currency = request.Currency.ToLower(),
            Source = $"card_{request.PaymentMethod.CardNumber[^4..]}", // Simulate token
            Description = request.Description,
            Metadata = new Dictionary<string, string>
            {
                ["customer_id"] = request.Customer.CustomerId,
                ["customer_email"] = request.Customer.Email
            }
        };
    }

    private PaymentResult ConvertFromStripeResult(StripeChargeResult result, PaymentRequest originalRequest)
    {
        return new PaymentResult
        {
            IsSuccess = result.Succeeded,
            TransactionId = originalRequest.TransactionId,
            Message = result.Succeeded ? "Payment processed successfully" : result.FailureMessage ?? "Payment failed",
            Status = result.Succeeded ? PaymentStatus.Completed : PaymentStatus.Failed,
            ProcessedAmount = result.AmountInCents / 100m,
            AuthorizationCode = result.Id
        };
    }
}
