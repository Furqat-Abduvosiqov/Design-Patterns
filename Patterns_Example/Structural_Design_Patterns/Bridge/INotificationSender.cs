namespace Patterns_Example.Structural_Design_Patterns.Bridge;

/// <summary>
/// Implementation interface for the Bridge pattern.
/// This defines the interface for concrete implementations (the "implementor").
/// Different notification channels will implement this interface.
/// </summary>
public interface INotificationSender
{
    Task<NotificationResult> SendAsync(NotificationMessage message);
    Task<NotificationResult> SendBulkAsync(IEnumerable<NotificationMessage> messages);
    bool IsAvailable();
    NotificationCapabilities GetCapabilities();
    Task<bool> ValidateConfigurationAsync();
}

/// <summary>
/// Represents a notification message that can be sent through any channel.
/// </summary>
public class NotificationMessage
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public NotificationPriority Priority { get; set; } = NotificationPriority.Normal;
    public Dictionary<string, string> Metadata { get; set; } = new();
    public List<string> Recipients { get; set; } = new();
    public List<NotificationAttachment> Attachments { get; set; } = new();
    public DateTime ScheduledTime { get; set; } = DateTime.Now;
    public TimeSpan? ExpirationTime { get; set; }

    public override string ToString()
    {
        var recipientList = Recipients.Count > 0 ? string.Join(", ", Recipients) : "No recipients";
        return $"Notification {Id}: '{Title}' to {recipientList} (Priority: {Priority})";
    }
}

/// <summary>
/// Represents the result of a notification sending operation.
/// </summary>
public class NotificationResult
{
    public bool IsSuccess { get; set; }
    public string MessageId { get; set; } = string.Empty;
    public string ExternalId { get; set; } = string.Empty; // ID from external service
    public string ErrorMessage { get; set; } = string.Empty;
    public DateTime SentAt { get; set; } = DateTime.Now;
    public NotificationStatus Status { get; set; }
    public Dictionary<string, object> AdditionalData { get; set; } = new();

    public override string ToString()
    {
        return $"Result for {MessageId}: {(IsSuccess ? "SUCCESS" : "FAILED")} - {Status}" +
               (IsSuccess ? $" (External ID: {ExternalId})" : $" - {ErrorMessage}");
    }
}

/// <summary>
/// Represents an attachment that can be sent with notifications.
/// </summary>
public class NotificationAttachment
{
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public byte[] Data { get; set; } = Array.Empty<byte>();
    public long Size => Data.Length;
    public string Url { get; set; } = string.Empty; // For URL-based attachments

    public override string ToString()
    {
        return $"{FileName} ({ContentType}, {Size} bytes)";
    }
}

/// <summary>
/// Describes the capabilities of a notification sender.
/// </summary>
public class NotificationCapabilities
{
    public bool SupportsRichText { get; set; }
    public bool SupportsAttachments { get; set; }
    public bool SupportsBulkSending { get; set; }
    public bool SupportsScheduling { get; set; }
    public bool SupportsDeliveryTracking { get; set; }
    public bool SupportsReadReceipts { get; set; }
    public int MaxMessageLength { get; set; } = int.MaxValue;
    public int MaxAttachmentSize { get; set; } = int.MaxValue;
    public int MaxRecipientsPerMessage { get; set; } = int.MaxValue;
    public List<string> SupportedContentTypes { get; set; } = new();

    public override string ToString()
    {
        var features = new List<string>();
        if (SupportsRichText) features.Add("Rich Text");
        if (SupportsAttachments) features.Add("Attachments");
        if (SupportsBulkSending) features.Add("Bulk Sending");
        if (SupportsScheduling) features.Add("Scheduling");
        if (SupportsDeliveryTracking) features.Add("Delivery Tracking");
        if (SupportsReadReceipts) features.Add("Read Receipts");

        return $"Capabilities: {string.Join(", ", features)} | " +
               $"Max Message: {MaxMessageLength} chars | " +
               $"Max Recipients: {MaxRecipientsPerMessage}";
    }
}

/// <summary>
/// Priority levels for notifications.
/// </summary>
public enum NotificationPriority
{
    Low,
    Normal,
    High,
    Critical
}

/// <summary>
/// Status of a notification.
/// </summary>
public enum NotificationStatus
{
    Pending,
    Sent,
    Delivered,
    Read,
    Failed,
    Expired
}
