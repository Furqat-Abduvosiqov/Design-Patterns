using Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility.Models.Enums;

namespace Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility.Models;

/// <summary>
/// Support request that travels through the chain of handlers.
/// </summary>
public class SupportRequest
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public CustomerTier CustomerTier { get; set; } = CustomerTier.Basic;
    public SupportCategory Category { get; set; }
    public Priority Priority { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public List<string> Tags { get; set; } = new();
    public Dictionary<string, object> Metadata { get; set; } = new();
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

    public override string ToString()
    {
        return $"[{Id[..8]}] {CustomerName} - {Subject} ({Priority} priority, {Category} category)";
    }
}