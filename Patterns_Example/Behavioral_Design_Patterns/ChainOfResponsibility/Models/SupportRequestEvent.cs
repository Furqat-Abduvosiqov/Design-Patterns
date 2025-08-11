namespace Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility.Models;

/// <summary>
/// Event that occurs during request processing.
/// </summary>
public class SupportRequestEvent
{
    public DateTime Timestamp { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string HandlerName { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"{Timestamp:HH:mm:ss} [{EventType}] {Description} ({HandlerName})";
    }
}