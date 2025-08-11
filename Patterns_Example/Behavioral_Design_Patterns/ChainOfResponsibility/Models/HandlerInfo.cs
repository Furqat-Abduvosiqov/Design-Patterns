using Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility.Models.Enums;

namespace Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility.Models;

/// <summary>
/// Information about a handler in the chain.
/// </summary>
public class HandlerInfo
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<SupportCategory> HandledCategories { get; set; } = new();
    public List<Priority> HandledPriorities { get; set; } = new();
    public List<CustomerTier> HandledTiers { get; set; } = new();
    public TimeSpan AverageProcessingTime { get; set; }
    public int MaxConcurrentRequests { get; set; } = 10;
    public bool IsAvailable { get; set; } = true;

    public override string ToString()
    {
        var categories = string.Join(", ", HandledCategories);
        var priorities = string.Join(", ", HandledPriorities);
        return $"{Name}: {Description} | Categories: {categories} | Priorities: {priorities}";
    }
}