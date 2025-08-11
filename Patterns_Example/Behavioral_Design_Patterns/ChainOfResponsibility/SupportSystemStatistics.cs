using Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility.Models.Enums;

namespace Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility;

/// <summary>
/// Statistics about the support system performance.
/// </summary>
public class SupportSystemStatistics
{
    public int TotalRequests { get; set; }
    public int HandledRequests { get; set; }
    public int UnhandledRequests { get; set; }
    public TimeSpan AverageProcessingTime { get; set; }
    public Dictionary<SupportCategory, int> RequestsByCategory { get; set; } = new();
    public Dictionary<Priority, int> RequestsByPriority { get; set; } = new();
    public Dictionary<string, int> RequestsByHandler { get; set; } = new();

    public double HandlingSuccessRate => TotalRequests > 0 ? (double)HandledRequests / TotalRequests * 100 : 0;

    public override string ToString()
    {
        var categoryStats = string.Join(", ", RequestsByCategory.Select(kvp => $"{kvp.Key}: {kvp.Value}"));
        var priorityStats = string.Join(", ", RequestsByPriority.Select(kvp => $"{kvp.Key}: {kvp.Value}"));
        var handlerStats = string.Join(", ", RequestsByHandler.Select(kvp => $"{kvp.Key}: {kvp.Value}"));

        return $"""
                Support System Statistics:
                  Total Requests: {TotalRequests}
                  Handled: {HandledRequests} ({HandlingSuccessRate:F1}%)
                  Unhandled: {UnhandledRequests}
                  Average Processing Time: {AverageProcessingTime.TotalMilliseconds:F0}ms
                  
                  By Category: {categoryStats}
                  By Priority: {priorityStats}
                  By Handler: {handlerStats}
                """;
    }
}