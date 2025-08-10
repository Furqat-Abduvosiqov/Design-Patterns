namespace Patterns_Example.Structural_Design_Patterns.Bridge;

/// <summary>
/// High-level notification service that demonstrates the Bridge pattern in action.
/// This service can work with any combination of notification abstractions and implementations.
/// </summary>
public class NotificationService
{
    private readonly Dictionary<string, INotificationSender> _senders;
    private readonly List<NotificationLog> _notificationHistory;

    public NotificationService()
    {
        _senders = new Dictionary<string, INotificationSender>();
        _notificationHistory = new List<NotificationLog>();
    }

    /// <summary>
    /// Registers a notification sender with a specific name.
    /// </summary>
    public void RegisterSender(string name, INotificationSender sender)
    {
        _senders[name] = sender ?? throw new ArgumentNullException(nameof(sender));
        Console.WriteLine($"[NotificationService] Registered sender: {name} ({sender.GetType().Name})");
    }

    /// <summary>
    /// Sends an urgent notification using the specified sender.
    /// </summary>
    public async Task<NotificationResult> SendUrgentNotificationAsync(string senderName, NotificationMessage message)
    {
        if (!_senders.TryGetValue(senderName, out var sender))
        {
            throw new ArgumentException($"Sender '{senderName}' not found");
        }

        var urgentNotification = new UrgentNotification(sender, message);
        var result = await urgentNotification.SendAsync();
        
        LogNotification(senderName, "Urgent", message, result);
        return result;
    }

    /// <summary>
    /// Sends a marketing notification using the specified sender.
    /// </summary>
    public async Task<NotificationResult> SendMarketingNotificationAsync(string senderName, NotificationMessage message, MarketingOptions? options = null)
    {
        if (!_senders.TryGetValue(senderName, out var sender))
        {
            throw new ArgumentException($"Sender '{senderName}' not found");
        }

        var marketingNotification = new MarketingNotification(sender, message, options ?? new MarketingOptions());
        var result = await marketingNotification.SendAsync();
        
        LogNotification(senderName, "Marketing", message, result);
        return result;
    }

    /// <summary>
    /// Schedules a notification for future delivery.
    /// </summary>
    public async Task<NotificationResult> ScheduleNotificationAsync(string senderName, NotificationMessage message, DateTime scheduledTime)
    {
        if (!_senders.TryGetValue(senderName, out var sender))
        {
            throw new ArgumentException($"Sender '{senderName}' not found");
        }

        var scheduledNotification = new ScheduledNotification(sender, message, scheduledTime);
        var result = await scheduledNotification.SendAsync();
        
        LogNotification(senderName, "Scheduled", message, result);
        return result;
    }

    /// <summary>
    /// Sends a bulk notification to multiple recipients using the best available sender.
    /// </summary>
    public async Task<NotificationResult> SendBulkNotificationAsync(List<NotificationMessage> messages, NotificationPriority priority = NotificationPriority.Normal)
    {
        // Find the best sender for bulk operations
        var bulkSender = _senders.Values.FirstOrDefault(s => s.GetCapabilities().SupportsBulkSending);
        
        if (bulkSender == null)
        {
            // Fallback to individual sends
            Console.WriteLine("[NotificationService] No bulk sender available, sending individually");
            var results = new List<NotificationResult>();
            
            foreach (var message in messages)
            {
                var sender = GetBestSenderForMessage(message);
                if (sender != null)
                {
                    var notification = new BasicNotification(sender, message);
                    var result = await notification.SendAsync();
                    results.Add(result);
                    LogNotification(sender.GetType().Name, "Individual", message, result);
                }
            }
            
            var successCount = results.Count(r => r.IsSuccess);
            return new NotificationResult
            {
                IsSuccess = successCount == messages.Count,
                MessageId = $"bulk_{Guid.NewGuid().ToString()[..8]}",
                Status = successCount == messages.Count ? NotificationStatus.Sent : NotificationStatus.Failed,
                AdditionalData = new Dictionary<string, object>
                {
                    ["total_messages"] = messages.Count,
                    ["successful_messages"] = successCount,
                    ["method"] = "individual_sends"
                }
            };
        }
        else
        {
            Console.WriteLine($"[NotificationService] Using bulk sender: {bulkSender.GetType().Name}");
            var result = await bulkSender.SendBulkAsync(messages);
            
            foreach (var message in messages)
            {
                LogNotification(bulkSender.GetType().Name, "Bulk", message, result);
            }
            
            return result;
        }
    }

    /// <summary>
    /// Gets the capabilities of all registered senders.
    /// </summary>
    public Dictionary<string, NotificationCapabilities> GetAllCapabilities()
    {
        return _senders.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value.GetCapabilities()
        );
    }

    /// <summary>
    /// Gets the best sender for a specific message based on its characteristics.
    /// </summary>
    public INotificationSender? GetBestSenderForMessage(NotificationMessage message)
    {
        var availableSenders = _senders.Values.Where(s => s.IsAvailable()).ToList();
        
        if (!availableSenders.Any())
            return null;

        // Priority-based selection
        return message.Priority switch
        {
            NotificationPriority.Critical => availableSenders.FirstOrDefault(s => s is PushNotificationSender) ?? availableSenders.First(),
            NotificationPriority.High => availableSenders.FirstOrDefault(s => s is EmailNotificationSender) ?? availableSenders.First(),
            NotificationPriority.Normal => availableSenders.FirstOrDefault(s => s is EmailNotificationSender) ?? availableSenders.First(),
            NotificationPriority.Low => availableSenders.FirstOrDefault(s => s is SmsNotificationSender) ?? availableSenders.First(),
            _ => availableSenders.First()
        };
    }

    /// <summary>
    /// Gets notification statistics.
    /// </summary>
    public NotificationStatistics GetStatistics()
    {
        var logs = _notificationHistory;
        
        return new NotificationStatistics
        {
            TotalNotifications = logs.Count,
            SuccessfulNotifications = logs.Count(l => l.Result.IsSuccess),
            FailedNotifications = logs.Count(l => !l.Result.IsSuccess),
            NotificationsByType = logs.GroupBy(l => l.NotificationType).ToDictionary(g => g.Key, g => g.Count()),
            NotificationsBySender = logs.GroupBy(l => l.SenderName).ToDictionary(g => g.Key, g => g.Count()),
            AverageResponseTime = logs.Count > 0 ? logs.Average(l => (l.Result.SentAt - l.SentAt).TotalMilliseconds) : 0,
            LastNotificationTime = logs.LastOrDefault()?.SentAt
        };
    }

    /// <summary>
    /// Gets notification history filtered by criteria.
    /// </summary>
    public List<NotificationLog> GetNotificationHistory(DateTime? fromDate = null, string? senderName = null, bool? successOnly = null)
    {
        var query = _notificationHistory.AsEnumerable();
        
        if (fromDate.HasValue)
            query = query.Where(l => l.SentAt >= fromDate.Value);
            
        if (!string.IsNullOrEmpty(senderName))
            query = query.Where(l => l.SenderName.Equals(senderName, StringComparison.OrdinalIgnoreCase));
            
        if (successOnly.HasValue)
            query = query.Where(l => l.Result.IsSuccess == successOnly.Value);
            
        return query.ToList();
    }

    private void LogNotification(string senderName, string notificationType, NotificationMessage message, NotificationResult result)
    {
        var log = new NotificationLog
        {
            SenderName = senderName,
            NotificationType = notificationType,
            Message = message,
            Result = result,
            SentAt = DateTime.Now
        };
        
        _notificationHistory.Add(log);
    }

    private class BasicNotification : Notification
    {
        public BasicNotification(INotificationSender sender, NotificationMessage message) : base(sender, message)
        {
        }
    }
}

/// <summary>
/// Represents a log entry for a sent notification.
/// </summary>
public class NotificationLog
{
    public string SenderName { get; set; } = string.Empty;
    public string NotificationType { get; set; } = string.Empty;
    public NotificationMessage Message { get; set; } = new();
    public NotificationResult Result { get; set; } = new();
    public DateTime SentAt { get; set; }

    public override string ToString()
    {
        return $"{SentAt:HH:mm:ss} - {NotificationType} via {SenderName}: {(Result.IsSuccess ? "SUCCESS" : "FAILED")}";
    }
}

/// <summary>
/// Statistics about notification sending.
/// </summary>
public class NotificationStatistics
{
    public int TotalNotifications { get; set; }
    public int SuccessfulNotifications { get; set; }
    public int FailedNotifications { get; set; }
    public Dictionary<string, int> NotificationsByType { get; set; } = new();
    public Dictionary<string, int> NotificationsBySender { get; set; } = new();
    public double AverageResponseTime { get; set; }
    public DateTime? LastNotificationTime { get; set; }

    public double SuccessRate => TotalNotifications > 0 ? (double)SuccessfulNotifications / TotalNotifications * 100 : 0;

    public override string ToString()
    {
        var typeBreakdown = string.Join(", ", NotificationsByType.Select(kvp => $"{kvp.Key}: {kvp.Value}"));
        var senderBreakdown = string.Join(", ", NotificationsBySender.Select(kvp => $"{kvp.Key}: {kvp.Value}"));
        
        return $"""
            Notification Statistics:
              Total: {TotalNotifications} (Success: {SuccessfulNotifications}, Failed: {FailedNotifications})
              Success Rate: {SuccessRate:F1}%
              Average Response Time: {AverageResponseTime:F1}ms
              By Type: {typeBreakdown}
              By Sender: {senderBreakdown}
              Last Notification: {LastNotificationTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? "None"}
            """;
    }
}
