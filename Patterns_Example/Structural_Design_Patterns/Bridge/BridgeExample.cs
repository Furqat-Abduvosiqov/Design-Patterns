namespace Patterns_Example.Structural_Design_Patterns.Bridge;

/// <summary>
/// Example class demonstrating the Bridge pattern usage
/// </summary>
public static class BridgeExample
{
    public static async Task RunExample()
    {
        Console.WriteLine("=== Bridge Pattern Example: Notification System ===\n");
        
        // Demonstrate basic bridge pattern
        await DemonstrateBasicBridgePattern();
        
        // Demonstrate different abstractions with same implementations
        await DemonstrateDifferentAbstractions();
        
        // Demonstrate same abstraction with different implementations
        await DemonstrateDifferentImplementations();
        
        // Demonstrate notification service
        await DemonstrateNotificationService();
        
        // Demonstrate capabilities and validation
        await DemonstrateCapabilitiesAndValidation();
        
        Console.WriteLine("=== Bridge Pattern Benefits Demonstrated ===");
        Console.WriteLine("✓ Separation of abstraction from implementation");
        Console.WriteLine("✓ Independent extension of abstractions and implementations");
        Console.WriteLine("✓ Runtime switching between implementations");
        Console.WriteLine("✓ Hiding implementation details from clients");
        Console.WriteLine("✓ Sharing implementations among multiple abstractions");
        Console.WriteLine("✓ Platform-independent abstractions");
    }

    private static async Task DemonstrateBasicBridgePattern()
    {
        Console.WriteLine("1. Basic Bridge Pattern Demonstration:");
        Console.WriteLine(new string('=', 50));
        
        // Create different implementations
        var emailSender = new EmailNotificationSender(new EmailConfiguration
        {
            SmtpServer = "smtp.example.com",
            FromAddress = "noreply@example.com",
            FromName = "Example App"
        });
        
        var smsSender = new SmsNotificationSender(new SmsConfiguration
        {
            Provider = "Twilio",
            ApiKey = "test_api_key",
            FromNumber = "+1234567890"
        });
        
        // Create a message
        var message = CreateSampleMessage("System Alert", "Your account has been accessed from a new device.");
        
        // Use same abstraction with different implementations
        var emailNotification = new UrgentNotification(emailSender, message);
        var smsNotification = new UrgentNotification(smsSender, message);
        
        Console.WriteLine("Sending urgent notification via Email:");
        var emailResult = await emailNotification.SendAsync();
        Console.WriteLine($"Result: {emailResult}\n");
        
        Console.WriteLine("Sending urgent notification via SMS:");
        var smsResult = await smsNotification.SendAsync();
        Console.WriteLine($"Result: {smsResult}\n");
    }

    private static async Task DemonstrateDifferentAbstractions()
    {
        Console.WriteLine("2. Different Abstractions with Same Implementation:");
        Console.WriteLine(new string('=', 50));
        
        // Use the same email sender with different abstractions
        var emailSender = new EmailNotificationSender(new EmailConfiguration
        {
            SmtpServer = "smtp.example.com",
            FromAddress = "marketing@example.com",
            FromName = "Marketing Team"
        });
        
        var message1 = CreateSampleMessage("Security Alert", "Suspicious login detected on your account.");
        var message2 = CreateSampleMessage("Special Offer", "Get 50% off on all products this weekend!");
        var message3 = CreateSampleMessage("Weekly Newsletter", "Here's what happened this week...");
        
        // Different abstractions using the same implementation
        var urgentNotification = new UrgentNotification(emailSender, message1);
        var marketingNotification = new MarketingNotification(emailSender, message2, new MarketingOptions
        {
            CampaignId = "WEEKEND_SALE_2024",
            BrandName = "TechStore"
        });
        var scheduledNotification = new ScheduledNotification(emailSender, message3, DateTime.Now.AddMinutes(1));
        
        Console.WriteLine("Urgent notification:");
        await urgentNotification.SendAsync();
        Console.WriteLine();
        
        Console.WriteLine("Marketing notification:");
        await marketingNotification.SendAsync();
        Console.WriteLine();
        
        Console.WriteLine("Scheduled notification:");
        await scheduledNotification.SendAsync();
        Console.WriteLine();
    }

    private static async Task DemonstrateDifferentImplementations()
    {
        Console.WriteLine("3. Same Abstraction with Different Implementations:");
        Console.WriteLine(new string('=', 50));
        
        // Create different implementations
        var implementations = new Dictionary<string, INotificationSender>
        {
            ["Email"] = new EmailNotificationSender(new EmailConfiguration
            {
                SmtpServer = "smtp.example.com",
                FromAddress = "alerts@example.com"
            }),
            ["SMS"] = new SmsNotificationSender(new SmsConfiguration
            {
                Provider = "Twilio",
                ApiKey = "test_key",
                FromNumber = "+1234567890"
            }),
            ["Push"] = new PushNotificationSender(new PushConfiguration
            {
                Platform = "iOS",
                ApiKey = "push_api_key",
                AppId = "com.example.app"
            })
        };
        
        var message = CreateSampleMessage("System Maintenance", "Scheduled maintenance will begin in 30 minutes.");
        
        // Use the same abstraction (UrgentNotification) with different implementations
        foreach (var impl in implementations)
        {
            Console.WriteLine($"Sending via {impl.Key}:");
            var notification = new UrgentNotification(impl.Value, message);
            var result = await notification.SendAsync();
            Console.WriteLine($"Result: {result.Status}\n");
        }
    }

    private static async Task DemonstrateNotificationService()
    {
        Console.WriteLine("4. Notification Service Demonstration:");
        Console.WriteLine(new string('=', 50));
        
        // Create notification service and register senders
        var notificationService = new NotificationService();
        
        notificationService.RegisterSender("email", new EmailNotificationSender(new EmailConfiguration
        {
            SmtpServer = "smtp.example.com",
            FromAddress = "service@example.com"
        }));
        
        notificationService.RegisterSender("sms", new SmsNotificationSender(new SmsConfiguration
        {
            Provider = "Twilio",
            ApiKey = "test_key",
            FromNumber = "+1234567890"
        }));
        
        notificationService.RegisterSender("push", new PushNotificationSender(new PushConfiguration
        {
            Platform = "Android",
            ApiKey = "push_key",
            AppId = "com.example.app"
        }));
        
        // Send different types of notifications
        var urgentMessage = CreateSampleMessage("Critical Alert", "Database connection lost!");
        var marketingMessage = CreateSampleMessage("New Features", "Check out our latest updates!");
        var scheduledMessage = CreateSampleMessage("Daily Report", "Your daily summary is ready.");
        
        Console.WriteLine("Sending urgent notification via push:");
        await notificationService.SendUrgentNotificationAsync("push", urgentMessage);
        Console.WriteLine();
        
        Console.WriteLine("Sending marketing notification via email:");
        await notificationService.SendMarketingNotificationAsync("email", marketingMessage);
        Console.WriteLine();
        
        Console.WriteLine("Scheduling notification via SMS:");
        await notificationService.ScheduleNotificationAsync("sms", scheduledMessage, DateTime.Now.AddSeconds(2));
        Console.WriteLine();
        
        // Send bulk notifications
        var bulkMessages = new List<NotificationMessage>
        {
            CreateSampleMessage("Bulk Message 1", "First bulk message"),
            CreateSampleMessage("Bulk Message 2", "Second bulk message"),
            CreateSampleMessage("Bulk Message 3", "Third bulk message")
        };
        
        Console.WriteLine("Sending bulk notifications:");
        await notificationService.SendBulkNotificationAsync(bulkMessages);
        Console.WriteLine();
        
        // Show statistics
        var stats = notificationService.GetStatistics();
        Console.WriteLine(stats);
        Console.WriteLine();
    }

    private static async Task DemonstrateCapabilitiesAndValidation()
    {
        Console.WriteLine("5. Capabilities and Validation Demonstration:");
        Console.WriteLine(new string('=', 50));
        
        var senders = new Dictionary<string, INotificationSender>
        {
            ["Email"] = new EmailNotificationSender(new EmailConfiguration { SmtpServer = "smtp.test.com", FromAddress = "test@test.com" }),
            ["SMS"] = new SmsNotificationSender(new SmsConfiguration { Provider = "Test", ApiKey = "key", FromNumber = "+1234567890" }),
            ["Push"] = new PushNotificationSender(new PushConfiguration { Platform = "iOS", ApiKey = "key", AppId = "app" })
        };
        
        // Show capabilities of each sender
        foreach (var sender in senders)
        {
            Console.WriteLine($"{sender.Key} Capabilities:");
            var capabilities = sender.Value.GetCapabilities();
            Console.WriteLine($"  {capabilities}");
            Console.WriteLine();
        }
        
        // Test validation with different message types
        var longMessage = CreateSampleMessage("Long Message", new string('A', 200)); // Long content
        var messageWithAttachment = CreateSampleMessage("With Attachment", "This has an attachment");
        messageWithAttachment.Attachments.Add(new NotificationAttachment
        {
            FileName = "document.pdf",
            ContentType = "application/pdf",
            Data = new byte[1024]
        });
        
        Console.WriteLine("Testing message validation:");
        
        // Test long message with SMS (should fail due to length limit)
        Console.WriteLine("Sending long message via SMS:");
        var smsNotification = new UrgentNotification(senders["SMS"], longMessage);
        var smsResult = await smsNotification.SendAsync();
        Console.WriteLine($"Result: {smsResult.Status} - {smsResult.ErrorMessage}\n");
        
        // Test attachment with SMS (should fail - SMS doesn't support attachments)
        Console.WriteLine("Sending message with attachment via SMS:");
        var smsWithAttachment = new UrgentNotification(senders["SMS"], messageWithAttachment);
        var attachmentResult = await smsWithAttachment.SendAsync();
        Console.WriteLine($"Result: {attachmentResult.Status} - {attachmentResult.ErrorMessage}\n");
        
        // Test attachment with Email (should succeed)
        Console.WriteLine("Sending message with attachment via Email:");
        var emailWithAttachment = new UrgentNotification(senders["Email"], messageWithAttachment);
        var emailAttachmentResult = await emailWithAttachment.SendAsync();
        Console.WriteLine($"Result: {emailAttachmentResult.Status}\n");
    }

    private static NotificationMessage CreateSampleMessage(string title, string content)
    {
        return new NotificationMessage
        {
            Title = title,
            Content = content,
            Recipients = new List<string> { "user@example.com", "+1234567890", "device_token_123" },
            Priority = NotificationPriority.Normal
        };
    }
}
