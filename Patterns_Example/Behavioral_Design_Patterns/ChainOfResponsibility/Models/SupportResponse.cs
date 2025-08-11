namespace Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility.Models;

/// <summary>
/// Response from handling a support request.
/// </summary>
public class SupportResponse
{
    public bool IsHandled { get; set; }
    public string HandlerName { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Resolution { get; set; } = string.Empty;
    public TimeSpan ProcessingTime { get; set; }
    public DateTime HandledAt { get; set; } = DateTime.Now;
    public List<string> Actions { get; set; } = new();
    public Dictionary<string, object> AdditionalData { get; set; } = new();
    public bool RequiresFollowUp { get; set; }
    public DateTime? FollowUpDate { get; set; }

    public override string ToString()
    {
        var status = IsHandled ? "HANDLED" : "NOT HANDLED";
        return $"{status} by {HandlerName}: {Message}";
    }
}