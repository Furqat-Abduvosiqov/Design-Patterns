# Bridge Pattern Example

This folder contains a comprehensive implementation of the Bridge design pattern using a real-world notification system that separates notification types from delivery channels.

## Files Overview

- **`INotificationSender.cs`** - Implementation interface defining delivery channel contracts
- **`NotificationImplementations.cs`** - Concrete implementations for Email, SMS, and Push notifications
- **`NotificationAbstractions.cs`** - Abstraction hierarchy with different notification types
- **`NotificationService.cs`** - High-level service demonstrating the pattern in action
- **`BridgeExample.cs`** - Comprehensive usage demonstrations
- **`BridgePatternDemo.cs`** - Main demo program
- **`BridgePatternGuide.md`** - Complete pattern explanation

## What Problem Does This Solve?

The Bridge pattern addresses the class explosion problem that occurs when you have multiple dimensions of variation. Instead of:

```csharp
// ❌ Class explosion - N types × M channels = N×M classes
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

You can use:

```csharp
// ✅ Bridge pattern - N types + M channels = N+M classes
// Notification types (abstractions)
var urgentNotification = new UrgentNotification(emailSender, message);
var marketingNotification = new MarketingNotification(smsSender, message);
var scheduledNotification = new ScheduledNotification(pushSender, message);

// Any notification type can use any delivery channel!
```

## Key Benefits Demonstrated

1. **Separation of Concerns**: Notification logic separate from delivery mechanism
2. **Independent Extension**: Add new types or channels without affecting existing code
3. **Runtime Flexibility**: Switch delivery channels dynamically based on conditions
4. **Code Reuse**: Same delivery implementation works with multiple notification types
5. **Maintainability**: Changes isolated to specific abstraction or implementation
6. **Testing**: Easy to mock and test components independently

## Real-World Use Cases

This pattern is commonly used for:

- **UI Frameworks**: Separate UI controls from platform-specific rendering
- **Database Access**: Abstract data operations from specific database implementations
- **Graphics Systems**: Separate drawing operations from rendering engines
- **Communication Systems**: Abstract message types from delivery protocols
- **Device Drivers**: Separate device operations from hardware implementations
- **Cross-Platform Development**: Abstract platform-specific functionality

## Components Demonstrated

### 1. Abstraction Hierarchy
**Base Abstraction (`Notification`)**:
- Defines the interface for clients
- Maintains reference to implementation
- Provides template method for sending

**Refined Abstractions**:
- **`UrgentNotification`**: High-priority alerts with special formatting
- **`MarketingNotification`**: Promotional messages with branding and tracking
- **`ScheduledNotification`**: Time-delayed message delivery

### 2. Implementation Hierarchy
**Implementation Interface (`INotificationSender`)**:
- Defines the interface for concrete implementations
- Declares capabilities and validation methods

**Concrete Implementations**:
- **`EmailNotificationSender`**: Rich text, attachments, bulk sending
- **`SmsNotificationSender`**: Plain text, character limits, high reliability
- **`PushNotificationSender`**: Real-time delivery, device targeting

### 3. Bridge Connection
The bridge is established through composition:
```csharp
public abstract class Notification
{
    protected readonly INotificationSender _sender;  // Bridge to implementation
    
    protected Notification(INotificationSender sender, NotificationMessage message)
    {
        _sender = sender;  // Dependency injection of implementation
    }
}
```

## Advanced Features

### Capability-Based Validation
Each implementation declares its capabilities:
```csharp
public class NotificationCapabilities
{
    public bool SupportsRichText { get; set; }
    public bool SupportsAttachments { get; set; }
    public int MaxMessageLength { get; set; }
    public int MaxRecipientsPerMessage { get; set; }
}
```

### Template Method Integration
Abstractions use template methods for consistent behavior:
```csharp
public virtual async Task<NotificationResult> SendAsync()
{
    // 1. Validate configuration
    // 2. Check availability
    // 3. Validate message
    // 4. Prepare message (customizable)
    // 5. Send via implementation (bridge call)
    // 6. Post-process result (customizable)
}
```

### Runtime Implementation Selection
```csharp
public INotificationSender GetBestSenderForMessage(NotificationMessage message)
{
    return message.Priority switch
    {
        NotificationPriority.Critical => pushSender,
        NotificationPriority.High => emailSender,
        NotificationPriority.Normal => emailSender,
        NotificationPriority.Low => smsSender,
        _ => emailSender
    };
}
```

## How to Run

1. Navigate to the project root directory
2. Update Program.cs to use the Bridge example:
   ```csharp
   using Patterns_Example.Structural_Design_Patterns.Bridge;
   await BridgeExample.RunExample();
   ```
3. Run the demo:
   ```bash
   dotnet run --project Patterns_Example
   ```

## Example Output

The demo will show:

1. **Basic Bridge Pattern**: Same abstraction with different implementations
2. **Different Abstractions**: Multiple notification types using same delivery channel
3. **Different Implementations**: Same notification type using different channels
4. **Notification Service**: High-level service managing multiple combinations
5. **Capabilities and Validation**: How implementations declare and enforce constraints

## Learning Objectives

After studying this example, you should understand:

- The difference between Bridge and Adapter patterns
- How to identify when class explosion indicates need for Bridge pattern
- How to separate abstraction from implementation effectively
- When to use composition vs inheritance for flexibility
- How to design capability-based validation systems
- The relationship between Bridge and other patterns (Template Method, Strategy)

## Pattern Comparison

### Bridge vs Adapter
- **Bridge**: Designed upfront to separate abstraction from implementation
- **Adapter**: Added later to make incompatible interfaces work together

### Bridge vs Strategy
- **Bridge**: Separates what you do from how you do it
- **Strategy**: Separates different ways of doing the same thing

### Bridge vs Abstract Factory
- **Bridge**: Focuses on abstraction-implementation separation
- **Abstract Factory**: Focuses on creating families of related objects

## Extension Ideas

Try extending this example by:

1. **Adding New Notification Types**: 
   - `ReminderNotification` with snooze functionality
   - `SurveyNotification` with response tracking
   - `NewsletterNotification` with subscription management

2. **Adding New Delivery Channels**:
   - `SlackNotificationSender` for team communications
   - `DiscordNotificationSender` for gaming communities
   - `WhatsAppNotificationSender` for personal messaging

3. **Implementing Advanced Features**:
   - Retry logic with exponential backoff
   - Message templating system
   - A/B testing for message variations
   - Analytics and delivery tracking
   - Multi-language support

4. **Creating Composite Implementations**:
   - `MultiChannelSender` that sends via multiple channels
   - `FallbackSender` that tries alternative channels on failure
   - `LoadBalancingSender` that distributes load across providers

## Best Practices Demonstrated

1. **Interface Segregation**: Clean, focused interfaces for implementations
2. **Dependency Injection**: Implementations injected into abstractions
3. **Template Method**: Consistent behavior with customization points
4. **Capability Declaration**: Implementations declare their constraints
5. **Async/Await**: Modern asynchronous programming patterns
6. **Validation**: Input validation before processing
7. **Error Handling**: Comprehensive error handling and reporting

## Testing Strategies

The Bridge pattern enables excellent testing:

- **Unit Testing**: Test abstractions and implementations independently
- **Integration Testing**: Test different abstraction-implementation combinations
- **Mock Testing**: Easy to create test doubles for implementations
- **Capability Testing**: Verify implementations correctly declare capabilities
- **Validation Testing**: Test message validation logic
- **Performance Testing**: Measure overhead of abstraction layer
