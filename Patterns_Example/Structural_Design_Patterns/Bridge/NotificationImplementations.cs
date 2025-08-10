namespace Patterns_Example.Structural_Design_Patterns.Bridge;

/// <summary>
/// Concrete implementation for email notifications.
/// This is a "Concrete Implementor" in the Bridge pattern.
/// </summary>
public class EmailNotificationSender : INotificationSender
{
    private readonly EmailConfiguration _config;
    private readonly Random _random = new();

    public EmailNotificationSender(EmailConfiguration config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }

    public async Task<NotificationResult> SendAsync(NotificationMessage message)
    {
        Console.WriteLine($"[Email] Sending email notification: {message.Title}");
        
        // Simulate email sending delay
        await Task.Delay(_random.Next(100, 300));
        
        // Simulate occasional failures
        var success = _random.NextDouble() > 0.1; // 90% success rate
        
        if (success)
        {
            var externalId = $"email_{Guid.NewGuid().ToString()[..8]}";
            Console.WriteLine($"[Email] Successfully sent to {string.Join(", ", message.Recipients)}");
            
            return new NotificationResult
            {
                IsSuccess = true,
                MessageId = message.Id,
                ExternalId = externalId,
                Status = NotificationStatus.Sent,
                AdditionalData = new Dictionary<string, object>
                {
                    ["smtp_server"] = _config.SmtpServer,
                    ["from_address"] = _config.FromAddress
                }
            };
        }
        else
        {
            Console.WriteLine($"[Email] Failed to send email: SMTP connection timeout");
            return new NotificationResult
            {
                IsSuccess = false,
                MessageId = message.Id,
                ErrorMessage = "SMTP connection timeout",
                Status = NotificationStatus.Failed
            };
        }
    }

    public async Task<NotificationResult> SendBulkAsync(IEnumerable<NotificationMessage> messages)
    {
        Console.WriteLine($"[Email] Sending bulk email batch of {messages.Count()} messages");
        
        var results = new List<NotificationResult>();
        foreach (var message in messages)
        {
            var result = await SendAsync(message);
            results.Add(result);
        }
        
        var successCount = results.Count(r => r.IsSuccess);
        var totalCount = results.Count;
        
        return new NotificationResult
        {
            IsSuccess = successCount == totalCount,
            MessageId = $"bulk_{Guid.NewGuid().ToString()[..8]}",
            Status = successCount == totalCount ? NotificationStatus.Sent : NotificationStatus.Failed,
            AdditionalData = new Dictionary<string, object>
            {
                ["total_messages"] = totalCount,
                ["successful_messages"] = successCount,
                ["failed_messages"] = totalCount - successCount
            }
        };
    }

    public bool IsAvailable()
    {
        // Simulate checking SMTP server availability
        return !string.IsNullOrEmpty(_config.SmtpServer) && !string.IsNullOrEmpty(_config.FromAddress);
    }

    public NotificationCapabilities GetCapabilities()
    {
        return new NotificationCapabilities
        {
            SupportsRichText = true,
            SupportsAttachments = true,
            SupportsBulkSending = true,
            SupportsScheduling = true,
            SupportsDeliveryTracking = true,
            SupportsReadReceipts = true,
            MaxMessageLength = 1000000, // 1MB
            MaxAttachmentSize = 25 * 1024 * 1024, // 25MB
            MaxRecipientsPerMessage = 100,
            SupportedContentTypes = new List<string> { "text/plain", "text/html" }
        };
    }

    public async Task<bool> ValidateConfigurationAsync()
    {
        Console.WriteLine($"[Email] Validating SMTP configuration for {_config.SmtpServer}");
        await Task.Delay(50); // Simulate validation
        return IsAvailable();
    }
}

/// <summary>
/// Concrete implementation for SMS notifications.
/// </summary>
public class SmsNotificationSender : INotificationSender
{
    private readonly SmsConfiguration _config;
    private readonly Random _random = new();

    public SmsNotificationSender(SmsConfiguration config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }

    public async Task<NotificationResult> SendAsync(NotificationMessage message)
    {
        Console.WriteLine($"[SMS] Sending SMS notification: {message.Title}");
        
        // Simulate SMS sending delay
        await Task.Delay(_random.Next(50, 150));
        
        // Simulate occasional failures
        var success = _random.NextDouble() > 0.05; // 95% success rate
        
        if (success)
        {
            var externalId = $"sms_{Guid.NewGuid().ToString()[..8]}";
            Console.WriteLine($"[SMS] Successfully sent to {string.Join(", ", message.Recipients)}");
            
            return new NotificationResult
            {
                IsSuccess = true,
                MessageId = message.Id,
                ExternalId = externalId,
                Status = NotificationStatus.Sent,
                AdditionalData = new Dictionary<string, object>
                {
                    ["provider"] = _config.Provider,
                    ["from_number"] = _config.FromNumber
                }
            };
        }
        else
        {
            Console.WriteLine($"[SMS] Failed to send SMS: Invalid phone number format");
            return new NotificationResult
            {
                IsSuccess = false,
                MessageId = message.Id,
                ErrorMessage = "Invalid phone number format",
                Status = NotificationStatus.Failed
            };
        }
    }

    public async Task<NotificationResult> SendBulkAsync(IEnumerable<NotificationMessage> messages)
    {
        Console.WriteLine($"[SMS] Sending bulk SMS batch of {messages.Count()} messages");
        
        // SMS providers often have better bulk APIs
        await Task.Delay(_random.Next(200, 500));
        
        var messageCount = messages.Count();
        var successCount = (int)(messageCount * 0.95); // 95% success rate for bulk
        
        return new NotificationResult
        {
            IsSuccess = successCount == messageCount,
            MessageId = $"sms_bulk_{Guid.NewGuid().ToString()[..8]}",
            Status = successCount == messageCount ? NotificationStatus.Sent : NotificationStatus.Failed,
            AdditionalData = new Dictionary<string, object>
            {
                ["total_messages"] = messageCount,
                ["successful_messages"] = successCount,
                ["failed_messages"] = messageCount - successCount,
                ["cost_per_message"] = 0.05m
            }
        };
    }

    public bool IsAvailable()
    {
        return !string.IsNullOrEmpty(_config.ApiKey) && !string.IsNullOrEmpty(_config.FromNumber);
    }

    public NotificationCapabilities GetCapabilities()
    {
        return new NotificationCapabilities
        {
            SupportsRichText = false,
            SupportsAttachments = false,
            SupportsBulkSending = true,
            SupportsScheduling = true,
            SupportsDeliveryTracking = true,
            SupportsReadReceipts = false,
            MaxMessageLength = 160, // Standard SMS length
            MaxAttachmentSize = 0,
            MaxRecipientsPerMessage = 1,
            SupportedContentTypes = new List<string> { "text/plain" }
        };
    }

    public async Task<bool> ValidateConfigurationAsync()
    {
        Console.WriteLine($"[SMS] Validating SMS configuration for provider {_config.Provider}");
        await Task.Delay(30);
        return IsAvailable();
    }
}

/// <summary>
/// Concrete implementation for push notifications.
/// </summary>
public class PushNotificationSender : INotificationSender
{
    private readonly PushConfiguration _config;
    private readonly Random _random = new();

    public PushNotificationSender(PushConfiguration config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }

    public async Task<NotificationResult> SendAsync(NotificationMessage message)
    {
        Console.WriteLine($"[Push] Sending push notification: {message.Title}");
        
        // Simulate push notification delay
        await Task.Delay(_random.Next(30, 100));
        
        // Simulate occasional failures
        var success = _random.NextDouble() > 0.08; // 92% success rate
        
        if (success)
        {
            var externalId = $"push_{Guid.NewGuid().ToString()[..8]}";
            Console.WriteLine($"[Push] Successfully sent to {message.Recipients.Count} devices");
            
            return new NotificationResult
            {
                IsSuccess = true,
                MessageId = message.Id,
                ExternalId = externalId,
                Status = NotificationStatus.Sent,
                AdditionalData = new Dictionary<string, object>
                {
                    ["platform"] = _config.Platform,
                    ["app_id"] = _config.AppId
                }
            };
        }
        else
        {
            Console.WriteLine($"[Push] Failed to send push notification: Invalid device token");
            return new NotificationResult
            {
                IsSuccess = false,
                MessageId = message.Id,
                ErrorMessage = "Invalid device token",
                Status = NotificationStatus.Failed
            };
        }
    }

    public async Task<NotificationResult> SendBulkAsync(IEnumerable<NotificationMessage> messages)
    {
        Console.WriteLine($"[Push] Sending bulk push notifications to {messages.Count()} recipients");
        
        await Task.Delay(_random.Next(100, 300));
        
        var messageCount = messages.Count();
        var successCount = (int)(messageCount * 0.92); // 92% success rate
        
        return new NotificationResult
        {
            IsSuccess = successCount == messageCount,
            MessageId = $"push_bulk_{Guid.NewGuid().ToString()[..8]}",
            Status = successCount == messageCount ? NotificationStatus.Sent : NotificationStatus.Failed,
            AdditionalData = new Dictionary<string, object>
            {
                ["total_messages"] = messageCount,
                ["successful_messages"] = successCount,
                ["failed_messages"] = messageCount - successCount,
                ["platform"] = _config.Platform
            }
        };
    }

    public bool IsAvailable()
    {
        return !string.IsNullOrEmpty(_config.ApiKey) && !string.IsNullOrEmpty(_config.AppId);
    }

    public NotificationCapabilities GetCapabilities()
    {
        return new NotificationCapabilities
        {
            SupportsRichText = true,
            SupportsAttachments = false,
            SupportsBulkSending = true,
            SupportsScheduling = true,
            SupportsDeliveryTracking = true,
            SupportsReadReceipts = false,
            MaxMessageLength = 4000,
            MaxAttachmentSize = 0,
            MaxRecipientsPerMessage = 1000,
            SupportedContentTypes = new List<string> { "text/plain", "application/json" }
        };
    }

    public async Task<bool> ValidateConfigurationAsync()
    {
        Console.WriteLine($"[Push] Validating push configuration for {_config.Platform}");
        await Task.Delay(40);
        return IsAvailable();
    }
}

/// <summary>
/// Configuration for email notifications.
/// </summary>
public class EmailConfiguration
{
    public string SmtpServer { get; set; } = string.Empty;
    public int Port { get; set; } = 587;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FromAddress { get; set; } = string.Empty;
    public string FromName { get; set; } = string.Empty;
    public bool UseSsl { get; set; } = true;
}

/// <summary>
/// Configuration for SMS notifications.
/// </summary>
public class SmsConfiguration
{
    public string Provider { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string FromNumber { get; set; } = string.Empty;
    public string AccountSid { get; set; } = string.Empty;
}

/// <summary>
/// Configuration for push notifications.
/// </summary>
public class PushConfiguration
{
    public string Platform { get; set; } = string.Empty; // iOS, Android, Web
    public string ApiKey { get; set; } = string.Empty;
    public string AppId { get; set; } = string.Empty;
    public string CertificatePath { get; set; } = string.Empty;
}
