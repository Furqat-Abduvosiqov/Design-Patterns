# Chain of Responsibility Design Pattern

The Chain of Responsibility pattern is a behavioral design pattern that passes requests along a chain of handlers. Upon receiving a request, each handler decides either to process the request or to pass it to the next handler in the chain. This pattern allows you to decouple request senders from receivers and gives multiple objects a chance to handle the request.

## Problem

Imagine you're building a customer support system that needs to route different types of requests to appropriate handlers. Without the Chain of Responsibility pattern, you'd face several issues:

```csharp
// ❌ Without Chain of Responsibility - monolithic request handling
public class MonolithicSupportSystem
{
    public void HandleRequest(SupportRequest request)
    {
        if (request.Priority == Priority.Emergency)
        {
            HandleEmergency(request);
        }
        else if (request.Category == SupportCategory.Technical && request.Priority == Priority.High)
        {
            HandleSeniorSupport(request);
        }
        else if (request.Category == SupportCategory.Billing && request.Priority >= Priority.Medium)
        {
            HandleLevel2Support(request);
        }
        else if (request.Category == SupportCategory.General)
        {
            HandleLevel1Support(request);
        }
        // ... dozens more if-else conditions
        else
        {
            HandleUnknownRequest(request);
        }
    }
}
```

### Issues with Monolithic Handling:

1. **Tight Coupling**: Request types are tightly coupled to specific handlers
2. **Complex Conditionals**: Long if-else chains that are hard to maintain
3. **Inflexibility**: Difficult to add new request types or change routing logic
4. **Violation of Open/Closed Principle**: Must modify existing code to add new handlers
5. **Single Point of Failure**: All routing logic concentrated in one place
6. **No Dynamic Configuration**: Cannot change request routing at runtime

## Solution

The Chain of Responsibility pattern suggests creating a chain of handler objects, each capable of processing certain types of requests:

1. **Handler Interface**: Defines the contract for handling requests
2. **Concrete Handlers**: Implement specific request handling logic
3. **Chain Setup**: Handlers are linked together in a chain
4. **Request Processing**: Requests flow through the chain until handled

### Key Components:

1. **Handler Interface (ISupportHandler)**: Common interface for all handlers
2. **Abstract Handler (BaseSupportHandler)**: Base implementation with chain logic
3. **Concrete Handlers**: Specific handlers for different request types
4. **Client (SupportTicketSystem)**: Sets up and uses the chain

## Real-World Example: Customer Support System

Our example demonstrates a customer support ticket system where:

### Request Types:
- **General Inquiries**: Basic questions and account issues
- **Technical Issues**: Software problems and troubleshooting
- **Billing Problems**: Payment and invoice issues
- **Sales Inquiries**: Product information and pricing
- **Security Issues**: Security breaches and vulnerabilities
- **Emergency Situations**: Critical system outages

### Handler Chain:
1. **Emergency Response Handler**: Handles emergency situations (Priority.Emergency)
2. **Senior Support Handler**: Handles critical issues and enterprise customers
3. **Level 2 Support Handler**: Handles complex technical and billing issues
4. **Level 1 Support Handler**: Handles basic technical and general inquiries
5. **Sales Support Handler**: Handles sales inquiries and feature requests

## Benefits Demonstrated

1. **Decoupled Request Handling**: Senders don't need to know which handler will process the request
2. **Dynamic Chain Configuration**: Chain can be modified at runtime
3. **Flexible Request Routing**: Multiple criteria can determine which handler processes a request
4. **Easy Extension**: New handlers can be added without changing existing code
5. **Automatic Escalation**: Requests automatically flow to appropriate handlers
6. **Complex Business Logic**: Supports sophisticated routing based on multiple factors

## Implementation Structure

```csharp
// Handler interface
public interface ISupportHandler
{
    ISupportHandler SetNext(ISupportHandler nextHandler);
    SupportResponse Handle(SupportRequest request);
}

// Abstract base handler
public abstract class BaseSupportHandler : ISupportHandler
{
    private ISupportHandler? _nextHandler;
    
    public virtual ISupportHandler SetNext(ISupportHandler nextHandler)
    {
        _nextHandler = nextHandler;
        return nextHandler;
    }
    
    public virtual SupportResponse Handle(SupportRequest request)
    {
        if (CanHandle(request))
        {
            return ProcessRequest(request);
        }
        
        return _nextHandler?.Handle(request) ?? CreateUnhandledResponse();
    }
    
    protected abstract bool CanHandle(SupportRequest request);
    protected abstract SupportResponse ProcessRequest(SupportRequest request);
}

// Concrete handler
public class Level1SupportHandler : BaseSupportHandler
{
    protected override bool CanHandle(SupportRequest request)
    {
        return request.Category == SupportCategory.General && 
               request.Priority <= Priority.Medium;
    }
    
    protected override SupportResponse ProcessRequest(SupportRequest request)
    {
        // Handle the request
        return new SupportResponse { IsHandled = true, /* ... */ };
    }
}

// Chain setup
var emergency = new EmergencyResponseHandler();
var senior = new SeniorSupportHandler();
var level2 = new Level2SupportHandler();
var level1 = new Level1SupportHandler();

emergency.SetNext(senior).SetNext(level2).SetNext(level1);

// Usage
var response = emergency.Handle(request);
```

## When to Use Chain of Responsibility Pattern

✅ **Use Chain of Responsibility when:**
- You want to decouple request senders from receivers
- Multiple objects should have a chance to handle a request
- The set of handlers and their order should be specified dynamically
- You want to issue a request to one of several objects without specifying the receiver explicitly
- You need to support undo operations or request logging

❌ **Don't use Chain of Responsibility when:**
- Only one object should handle each request
- The chain is very short (1-2 handlers) and unlikely to change
- Performance is critical and the chain might be long
- The request handling logic is simple and doesn't benefit from the pattern

## Advanced Features

### 1. Multi-Criteria Request Routing
```csharp
protected override bool CanHandle(SupportRequest request)
{
    return SupportsCategory(request.Category) && 
           SupportsPriority(request.Priority) &&
           SupportsTier(request.CustomerTier);
}
```

### 2. Request Event Tracking
```csharp
public class SupportRequest
{
    public List<SupportRequestEvent> Events { get; set; } = new();
    
    public void AddEvent(string eventType, string description, string handlerName = "")
    {
        Events.Add(new SupportRequestEvent
        {
            Timestamp = DateTime.Now,
            EventType = eventType,
            Description = description,
            HandlerName = handlerName
        });
    }
}
```

### 3. Handler Statistics and Monitoring
```csharp
public class SupportSystemStatistics
{
    public int TotalRequests { get; set; }
    public int HandledRequests { get; set; }
    public TimeSpan AverageProcessingTime { get; set; }
    public Dictionary<string, int> RequestsByHandler { get; set; }
    public double HandlingSuccessRate => (double)HandledRequests / TotalRequests * 100;
}
```

### 4. Fluent Request Builder
```csharp
var request = SupportRequestBuilder.Create()
    .ForCustomer("John Doe", "john@example.com", CustomerTier.Premium)
    .WithCategory(SupportCategory.Technical)
    .WithPriority(Priority.High)
    .WithSubject("Application crashes")
    .WithDescription("App crashes on startup")
    .Build();
```

## Code Structure

```
ChainOfResponsibility/
├── ISupportHandler.cs                    # Handler interface and data structures
├── SupportHandlers.cs                    # Concrete handler implementations
├── SupportTicketSystem.cs                # Chain setup and management
├── ChainOfResponsibilityExample.cs       # Usage demonstrations
└── ChainOfResponsibilityPatternDemo.cs   # Main demo program
```

## Performance Considerations

1. **Chain Length**: Longer chains may impact performance
2. **Handler Complexity**: Complex CanHandle logic can slow down routing
3. **Request Frequency**: High-frequency requests benefit from efficient early handlers
4. **Memory Usage**: Each handler maintains a reference to the next handler
5. **Caching**: Handler decisions can be cached for repeated similar requests

## Testing Benefits

1. **Handler Isolation**: Each handler can be tested independently
2. **Chain Testing**: Test different chain configurations
3. **Request Routing**: Verify requests reach the correct handlers
4. **Edge Cases**: Test unhandled requests and chain termination
5. **Performance Testing**: Measure chain traversal performance

## Common Variations

### 1. Pure Chain of Responsibility
Each handler either processes the request completely or passes it on:
```csharp
public SupportResponse Handle(SupportRequest request)
{
    if (CanHandle(request))
        return ProcessRequest(request); // Handle completely
    
    return _nextHandler?.Handle(request) ?? CreateUnhandledResponse();
}
```

### 2. Chain of Responsibility with Partial Processing
Handlers can partially process requests and still pass them on:
```csharp
public SupportResponse Handle(SupportRequest request)
{
    if (CanPartiallyHandle(request))
        PartiallyProcess(request); // Add to request, don't stop chain
    
    return _nextHandler?.Handle(request) ?? CreateFinalResponse(request);
}
```

### 3. Conditional Chain Branching
Different chains based on request properties:
```csharp
public ISupportHandler GetChainForRequest(SupportRequest request)
{
    return request.CustomerTier == CustomerTier.VIP 
        ? _vipChain 
        : _standardChain;
}
```

## Learning Objectives

After studying this example, you should understand:

- When request routing becomes complex enough to benefit from the pattern
- How to design flexible handler chains that can be modified at runtime
- The difference between pure and partial processing chains
- How to implement complex routing logic based on multiple criteria
- The trade-offs between flexibility and performance
- How to monitor and optimize chain performance

## Extension Ideas

Try extending this example by:

1. **Adding Workflow Support**: Multi-step request processing with state tracking
2. **Implementing Priority Queues**: Different chains for different priority levels
3. **Creating Handler Pools**: Load balancing across multiple instances of the same handler
4. **Adding Circuit Breakers**: Automatic handler disabling when overloaded
5. **Implementing Request Caching**: Cache responses for identical requests
6. **Creating Dynamic Chains**: AI-powered chain optimization based on historical data
7. **Adding Async Support**: Asynchronous request processing through the chain

## Best Practices Demonstrated

1. **Clear Handler Responsibilities**: Each handler has a well-defined purpose
2. **Flexible Chain Configuration**: Easy to modify chain structure
3. **Comprehensive Logging**: Track request journey through the chain
4. **Error Handling**: Graceful handling of unprocessed requests
5. **Performance Monitoring**: Track processing times and success rates
6. **Testable Design**: Handlers can be tested in isolation
7. **Business Logic Separation**: Handler logic separated from chain management
8. **Extensible Architecture**: Easy to add new handlers and request types
