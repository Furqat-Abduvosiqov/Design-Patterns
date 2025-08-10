namespace Patterns_Example.Structural_Design_Patterns.Bridge;

/// <summary>
/// Abstract base class for notifications (Abstraction in Bridge pattern).
/// This defines the interface for clients and maintains a reference to the implementor.
/// </summary>
public abstract class Notification
{
    protected readonly INotificationSender _sender;
    protected readonly NotificationMessage _message;

    protected Notification(INotificationSender sender, NotificationMessage message)
    {
        _sender = sender ?? throw new ArgumentNullException(nameof(sender));
        _message = message ?? throw new ArgumentNullException(nameof(message));
    }

    /// <summary>
    /// Template method that defines the basic notification sending algorithm.
    /// </summary>
    public virtual async Task<NotificationResult> SendAsync()
    {
        Console.WriteLine($"[Notification] Preparing to send: {_message}");
        
        // Validate configuration
        if (!await _sender.ValidateConfigurationAsync())
        {
            return new NotificationResult
            {
                IsSuccess = false,
                MessageId = _message.Id,
                ErrorMessage = "Invalid sender configuration",
                Status = NotificationStatus.Failed
            };
        }

        // Check if sender is available
        if (!_sender.IsAvailable())
        {
            return new NotificationResult
            {
                IsSuccess = false,
                MessageId = _message.Id,
                ErrorMessage = "Notification sender is not available",
                Status = NotificationStatus.Failed
            };
        }

        // Validate message against sender capabilities
        var validationResult = ValidateMessage();
        if (!validationResult.IsValid)
        {
            return new NotificationResult
            {
                IsSuccess = false,
                MessageId = _message.Id,
                ErrorMessage = validationResult.ErrorMessage,
                Status = NotificationStatus.Failed
            };
        }

        // Prepare message (implemented by subclasses)
        PrepareMessage();

        // Send the message
        var result = await _sender.SendAsync(_message);
        
        // Post-process result (implemented by subclasses)
        PostProcessResult(result);
        
        return result;
    }

    /// <summary>
    /// Validates the message against the sender's capabilities.
    /// </summary>
    protected virtual ValidationResult ValidateMessage()
    {
        var capabilities = _sender.GetCapabilities();
        
        // Check message length
        if (_message.Content.Length > capabilities.MaxMessageLength)
        {
            return new ValidationResult
            {
                IsValid = false,
                ErrorMessage = $"Message content exceeds maximum length of {capabilities.MaxMessageLength} characters"
            };
        }

        // Check recipient count
        if (_message.Recipients.Count > capabilities.MaxRecipientsPerMessage)
        {
            return new ValidationResult
            {
                IsValid = false,
                ErrorMessage = $"Too many recipients. Maximum allowed: {capabilities.MaxRecipientsPerMessage}"
            };
        }

        // Check attachments
        if (_message.Attachments.Count > 0 && !capabilities.SupportsAttachments)
        {
            return new ValidationResult
            {
                IsValid = false,
                ErrorMessage = "This notification channel does not support attachments"
            };
        }

        return new ValidationResult { IsValid = true };
    }

    /// <summary>
    /// Prepares the message for sending. Can be overridden by subclasses.
    /// </summary>
    protected virtual void PrepareMessage()
    {
        // Default implementation - can be overridden
        Console.WriteLine($"[Notification] Message prepared for sending via {_sender.GetType().Name}");
    }

    /// <summary>
    /// Post-processes the result after sending. Can be overridden by subclasses.
    /// </summary>
    protected virtual void PostProcessResult(NotificationResult result)
    {
        // Default implementation - can be overridden
        Console.WriteLine($"[Notification] Post-processing result: {result.Status}");
    }

    /// <summary>
    /// Gets the capabilities of the underlying sender.
    /// </summary>
    public NotificationCapabilities GetCapabilities()
    {
        return _sender.GetCapabilities();
    }
}

/// <summary>
/// Refined abstraction for urgent notifications.
/// Adds specific behavior for high-priority notifications.
/// </summary>
public class UrgentNotification : Notification
{
    public UrgentNotification(INotificationSender sender, NotificationMessage message) 
        : base(sender, message)
    {
        // Ensure the message is marked as high priority
        _message.Priority = NotificationPriority.Critical;
    }

    protected override void PrepareMessage()
    {
        base.PrepareMessage();
        
        // Add urgent prefixes and formatting
        _message.Title = $"🚨 URGENT: {_message.Title}";
        _message.Content = $"⚠️ HIGH PRIORITY MESSAGE ⚠️\n\n{_message.Content}\n\n--- This is an urgent notification ---";
        
        // Add metadata for urgent handling
        _message.Metadata["urgent"] = "true";
        _message.Metadata["retry_count"] = "3";
        _message.Metadata["timeout"] = "30";
        
        Console.WriteLine("[UrgentNotification] Message formatted for urgent delivery");
    }

    protected override ValidationResult ValidateMessage()
    {
        var baseValidation = base.ValidateMessage();
        if (!baseValidation.IsValid)
            return baseValidation;

        // Additional validation for urgent notifications
        if (_message.Recipients.Count == 0)
        {
            return new ValidationResult
            {
                IsValid = false,
                ErrorMessage = "Urgent notifications must have at least one recipient"
            };
        }

        return new ValidationResult { IsValid = true };
    }

    protected override void PostProcessResult(NotificationResult result)
    {
        base.PostProcessResult(result);
        
        if (result.IsSuccess)
        {
            Console.WriteLine("[UrgentNotification] Urgent notification sent successfully - logging for audit");
            // In a real system, you might log to a special audit system
        }
        else
        {
            Console.WriteLine("[UrgentNotification] Urgent notification failed - triggering escalation");
            // In a real system, you might trigger an escalation process
        }
    }
}

/// <summary>
/// Refined abstraction for marketing notifications.
/// Adds specific behavior for promotional messages.
/// </summary>
public class MarketingNotification : Notification
{
    private readonly MarketingOptions _options;

    public MarketingNotification(INotificationSender sender, NotificationMessage message, MarketingOptions options) 
        : base(sender, message)
    {
        _options = options ?? new MarketingOptions();
        _message.Priority = NotificationPriority.Low; // Marketing messages are typically low priority
    }

    protected override void PrepareMessage()
    {
        base.PrepareMessage();
        
        // Add marketing-specific formatting
        if (_options.IncludeBranding)
        {
            _message.Title = $"[{_options.BrandName}] {_message.Title}";
            _message.Content += $"\n\n---\nSent by {_options.BrandName}\nUnsubscribe: {_options.UnsubscribeUrl}";
        }

        // Add tracking parameters
        _message.Metadata["campaign_id"] = _options.CampaignId;
        _message.Metadata["tracking_enabled"] = "true";
        _message.Metadata["category"] = "marketing";
        
        Console.WriteLine($"[MarketingNotification] Message prepared for campaign: {_options.CampaignId}");
    }

    protected override ValidationResult ValidateMessage()
    {
        var baseValidation = base.ValidateMessage();
        if (!baseValidation.IsValid)
            return baseValidation;

        // Marketing-specific validation
        if (_options.RequireUnsubscribeLink && string.IsNullOrEmpty(_options.UnsubscribeUrl))
        {
            return new ValidationResult
            {
                IsValid = false,
                ErrorMessage = "Marketing notifications must include an unsubscribe link"
            };
        }

        return new ValidationResult { IsValid = true };
    }

    protected override void PostProcessResult(NotificationResult result)
    {
        base.PostProcessResult(result);
        
        if (result.IsSuccess)
        {
            Console.WriteLine($"[MarketingNotification] Campaign {_options.CampaignId} message sent - updating analytics");
            // In a real system, you might update campaign analytics
        }
    }
}

/// <summary>
/// Refined abstraction for scheduled notifications.
/// Adds behavior for time-delayed message delivery.
/// </summary>
public class ScheduledNotification : Notification
{
    private readonly DateTime _scheduledTime;

    public ScheduledNotification(INotificationSender sender, NotificationMessage message, DateTime scheduledTime) 
        : base(sender, message)
    {
        _scheduledTime = scheduledTime;
        _message.ScheduledTime = scheduledTime;
    }

    public override async Task<NotificationResult> SendAsync()
    {
        // Check if it's time to send
        if (DateTime.Now < _scheduledTime)
        {
            var delay = _scheduledTime - DateTime.Now;
            Console.WriteLine($"[ScheduledNotification] Message scheduled for {_scheduledTime:yyyy-MM-dd HH:mm:ss} (in {delay.TotalMinutes:F1} minutes)");
            
            return new NotificationResult
            {
                IsSuccess = true,
                MessageId = _message.Id,
                Status = NotificationStatus.Pending,
                AdditionalData = new Dictionary<string, object>
                {
                    ["scheduled_time"] = _scheduledTime,
                    ["delay_minutes"] = delay.TotalMinutes
                }
            };
        }

        // Time to send - proceed with normal sending
        return await base.SendAsync();
    }

    protected override void PrepareMessage()
    {
        base.PrepareMessage();
        
        // Add scheduling metadata
        _message.Metadata["scheduled_time"] = _scheduledTime.ToString("yyyy-MM-dd HH:mm:ss");
        _message.Metadata["notification_type"] = "scheduled";
        
        Console.WriteLine($"[ScheduledNotification] Message prepared for scheduled delivery at {_scheduledTime:HH:mm:ss}");
    }
}

/// <summary>
/// Options for marketing notifications.
/// </summary>
public class MarketingOptions
{
    public string CampaignId { get; set; } = Guid.NewGuid().ToString();
    public string BrandName { get; set; } = "Our Company";
    public bool IncludeBranding { get; set; } = true;
    public bool RequireUnsubscribeLink { get; set; } = true;
    public string UnsubscribeUrl { get; set; } = "https://example.com/unsubscribe";
}

/// <summary>
/// Result of message validation.
/// </summary>
public class ValidationResult
{
    public bool IsValid { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
}
