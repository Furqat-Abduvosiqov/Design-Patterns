# Bridge Design Pattern

The Bridge pattern is a structural design pattern that separates an abstraction from its implementation, allowing both to vary independently. It's particularly useful when you want to share an implementation among multiple objects or when you need to switch implementations at runtime.

## Problem

Imagine you're building a notification system that needs to support multiple types of notifications (urgent, marketing, scheduled) and multiple delivery channels (email, SMS, push notifications). Without the Bridge pattern, you might end up with a class explosion:

```csharp
// ❌ Class explosion without Bridge pattern
class UrgentEmailNotification { }
class UrgentSmsNotification { }
class UrgentPushNotification { }
class MarketingEmailNotification { }
class MarketingSmsNotification { }
class MarketingPushNotification { }
class ScheduledEmailNotification { }
class ScheduledSmsNotification { }
class ScheduledPushNotification { }
// 9 classes for just 3 types × 3 channels!
```

### Issues with Direct Implementation:

1. **Class Explosion**: N abstractions × M implementations = N×M classes
2. **Tight Coupling**: Abstractions tied to specific implementations
3. **Difficult Extension**: Adding new types or channels requires many changes
4. **Code Duplication**: Similar logic repeated across multiple classes
5. **Runtime Inflexibility**: Cannot switch implementations dynamically

## Solution

The Bridge pattern suggests separating the abstraction (what you want to do) from the implementation (how you do it):

1. **Abstraction**: Defines the high-level interface (notification types)
2. **Implementation**: Defines the low-level interface (delivery channels)
3. **Bridge**: Connects abstraction to implementation through composition
4. **Independence**: Both can evolve separately

### Key Components:

1. **Abstraction (Notification)**: High-level interface for clients
2. **Refined Abstractions**: Specific notification types (Urgent, Marketing, Scheduled)
3. **Implementation Interface (INotificationSender)**: Low-level interface
4. **Concrete Implementations**: Specific delivery channels (Email, SMS, Push)

## Real-World Example: Notification System

Our example demonstrates a notification system where:

### Abstractions (What to send):
- **UrgentNotification**: High-priority alerts with special formatting
- **MarketingNotification**: Promotional messages with branding
- **ScheduledNotification**: Time-delayed message delivery

### Implementations (How to send):
- **EmailNotificationSender**: Rich text, attachments, bulk sending
- **SmsNotificationSender**: Plain text, character limits, high reliability
- **PushNotificationSender**: Real-time delivery, device targeting

## Benefits Demonstrated

1. **Separation of Concerns**: Notification logic separate from delivery mechanism
2. **Independent Extension**: Add new notification types or channels independently
3. **Runtime Flexibility**: Switch delivery channels dynamically
4. **Code Reuse**: Same implementation works with multiple abstractions
5. **Platform Independence**: Abstract away platform-specific details
6. **Maintainability**: Changes isolated to specific components

## Implementation Structure

```csharp
// Abstraction
public abstract class Notification
{
    protected readonly INotificationSender _sender;
    
    public virtual async Task<NotificationResult> SendAsync()
    {
        // Template method using the bridge
        return await _sender.SendAsync(_message);
    }
}

// Refined Abstraction
public class UrgentNotification : Notification
{
    protected override void PrepareMessage()
    {
        _message.Title = $"🚨 URGENT: {_message.Title}";
        // Urgent-specific formatting
    }
}

// Implementation Interface
public interface INotificationSender
{
    Task<NotificationResult> SendAsync(NotificationMessage message);
}

// Concrete Implementation
public class EmailNotificationSender : INotificationSender
{
    public async Task<NotificationResult> SendAsync(NotificationMessage message)
    {
        // Email-specific sending logic
    }
}
```

## When to Use Bridge Pattern

✅ **Use Bridge when:**
- You want to avoid permanent binding between abstraction and implementation
- Both abstractions and implementations should be extensible through subclassing
- Changes in implementation shouldn't affect clients
- You want to share implementation among multiple objects
- You need to switch implementations at runtime

❌ **Don't use Bridge when:**
- You have only one implementation
- The abstraction and implementation are unlikely to change
- The relationship between abstraction and implementation is simple
- Performance is critical and the extra indirection is costly

## Advanced Features

### 1. Capability-Based Validation
Each implementation declares its capabilities:
```csharp
public class NotificationCapabilities
{
    public bool SupportsRichText { get; set; }
    public bool SupportsAttachments { get; set; }
    public int MaxMessageLength { get; set; }
    // ... other capabilities
}
```

### 2. Template Method Integration
Abstractions use template methods for consistent behavior:
```csharp
public virtual async Task<NotificationResult> SendAsync()
{
    ValidateMessage();      // Template step
    PrepareMessage();       // Customizable step
    var result = await _sender.SendAsync(_message);  // Bridge call
    PostProcessResult(result);  // Template step
    return result;
}
```

### 3. Composite Implementation
Multiple implementations can be combined:
```csharp
public class CompositeNotificationSender : INotificationSender
{
    private readonly List<INotificationSender> _senders;
    
    public async Task<NotificationResult> SendAsync(NotificationMessage message)
    {
        // Send via multiple channels
    }
}
```

## Code Structure

```
Bridge/
├── INotificationSender.cs           # Implementation interface
├── NotificationImplementations.cs   # Concrete implementations
├── NotificationAbstractions.cs      # Abstraction hierarchy
├── NotificationService.cs           # High-level service
├── BridgeExample.cs                 # Usage demonstrations
└── BridgePatternDemo.cs            # Main demo program
```

## Performance Considerations

1. **Minimal Overhead**: Bridge adds minimal performance cost
2. **Lazy Loading**: Implementations can be loaded on demand
3. **Caching**: Results can be cached at abstraction level
4. **Async Support**: Full async/await support for modern applications

## Testing Benefits

1. **Mock Implementations**: Easy to create test doubles
2. **Isolated Testing**: Test abstractions and implementations separately
3. **Integration Testing**: Test different combinations easily
4. **Behavior Verification**: Verify correct implementation calls

## Common Variations

### 1. Abstract Factory Bridge
Combine with Abstract Factory for implementation families:
```csharp
public interface INotificationSenderFactory
{
    INotificationSender CreateEmailSender();
    INotificationSender CreateSmsSender();
    INotificationSender CreatePushSender();
}
```

### 2. Strategy Bridge
Use with Strategy pattern for algorithm selection:
```csharp
public class AdaptiveNotification : Notification
{
    public AdaptiveNotification(INotificationStrategy strategy, INotificationSender sender)
    {
        // Choose implementation based on strategy
    }
}
```

## Learning Objectives

After studying this example, you should understand:

- The difference between Bridge and Adapter patterns
- How to separate abstraction from implementation
- When class explosion indicates need for Bridge pattern
- How to design flexible, extensible hierarchies
- The relationship between Bridge and other patterns
- Performance and testing implications of the pattern

## Extension Ideas

Try extending this example by:

1. **Adding New Notification Types**: Reminder, Survey, Newsletter notifications
2. **Adding New Channels**: Slack, Discord, WhatsApp, Telegram
3. **Implementing Retry Logic**: Automatic retry with exponential backoff
4. **Adding Analytics**: Track delivery rates and user engagement
5. **Creating Template System**: Predefined message templates
6. **Implementing A/B Testing**: Test different message variations
7. **Adding Localization**: Multi-language notification support
