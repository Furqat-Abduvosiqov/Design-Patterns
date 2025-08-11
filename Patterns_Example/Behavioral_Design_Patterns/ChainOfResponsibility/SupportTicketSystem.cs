using Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility.Abstractions;
using Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility.Models;
using Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility.Models.Enums;
using Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility.SupportHandlers;

namespace Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility;

/// <summary>
/// Support ticket system that manages the chain of responsibility for handling support requests.
/// This class demonstrates how to set up and use the chain of handlers.
/// </summary>
public class SupportTicketSystem
{
    private readonly ISupportHandler _handlerChain;
    private readonly List<SupportRequest> _processedRequests;
    private readonly Dictionary<string, ISupportHandler> _handlers;

    public SupportTicketSystem()
    {
        _processedRequests = new List<SupportRequest>();
        _handlers = new Dictionary<string, ISupportHandler>();
        _handlerChain = BuildHandlerChain();
    }

    /// <summary>
    /// Builds the chain of responsibility for support handlers.
    /// </summary>
    private ISupportHandler BuildHandlerChain()
    {
        // Create all handlers
        var level1Support = new Level1SupportHandler();
        var salesSupport = new SalesSupportHandler();
        var level2Support = new Level2SupportHandler();
        var seniorSupport = new SeniorSupportHandler();
        var emergencyResponse = new EmergencyResponseHandler();

        // Store handlers for reference
        _handlers["Level1"] = level1Support;
        _handlers["Sales"] = salesSupport;
        _handlers["Level2"] = level2Support;
        _handlers["Senior"] = seniorSupport;
        _handlers["Emergency"] = emergencyResponse;

        // Build the chain: Emergency -> Senior -> Level2 -> Level1 -> Sales
        // Emergency has highest priority and should be checked first
        emergencyResponse
            .SetNext(seniorSupport)
            .SetNext(level2Support)
            .SetNext(level1Support)
            .SetNext(salesSupport);

        Console.WriteLine("Support ticket system initialized with handler chain:");
        Console.WriteLine("Emergency Response → Senior Support → Level 2 Support → Level 1 Support → Sales Support");
        Console.WriteLine();

        return emergencyResponse;
    }

    /// <summary>
    /// Processes a support request through the chain of handlers.
    /// </summary>
    public SupportResponse ProcessRequest(SupportRequest request)
    {
        Console.WriteLine($"🎫 Processing support request: {request}");
        Console.WriteLine(new string('-', 80));

        request.AddEvent("RECEIVED", "Support request received by system");

        var response = _handlerChain.Handle(request);
        
        request.AddEvent("COMPLETED", $"Request processing completed - {(response.IsHandled ? "HANDLED" : "UNHANDLED")}");
        
        _processedRequests.Add(request);

        Console.WriteLine($"✅ Request processed: {response}");
        Console.WriteLine($"⏱️  Processing time: {response.ProcessingTime.TotalMilliseconds:F0}ms");
        
        if (response.RequiresFollowUp)
        {
            Console.WriteLine($"📅 Follow-up scheduled: {response.FollowUpDate:yyyy-MM-dd HH:mm}");
        }
        
        Console.WriteLine();
        return response;
    }

    /// <summary>
    /// Creates a sample support request for testing.
    /// </summary>
    public static SupportRequest CreateSampleRequest(
        string customerName,
        CustomerTier tier,
        SupportCategory category,
        Priority priority,
        string subject,
        string description)
    {
        return new SupportRequest
        {
            CustomerName = customerName,
            CustomerEmail = $"{customerName.ToLower().Replace(" ", ".")}@example.com",
            CustomerTier = tier,
            Category = category,
            Priority = priority,
            Subject = subject,
            Description = description,
            Tags = GenerateTags(category, priority),
            Metadata = new Dictionary<string, object>
            {
                ["source"] = "web_portal",
                ["user_agent"] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64)",
                ["session_id"] = Guid.NewGuid().ToString()
            }
        };
    }

    /// <summary>
    /// Gets statistics about processed requests.
    /// </summary>
    public SupportSystemStatistics GetStatistics()
    {
        var stats = new SupportSystemStatistics
        {
            TotalRequests = _processedRequests.Count,
            HandledRequests = _processedRequests.Count(r => r.Events.Any(e => e.EventType == "HANDLED")),
            UnhandledRequests = _processedRequests.Count(r => !r.Events.Any(e => e.EventType == "HANDLED")),
            AverageProcessingTime = _processedRequests.Any() 
                ? TimeSpan.FromMilliseconds(_processedRequests.Average(r => 
                    r.Events.Where(e => e.EventType == "COMPLETED").FirstOrDefault()?.Timestamp.Subtract(r.CreatedAt).TotalMilliseconds ?? 0))
                : TimeSpan.Zero
        };

        // Calculate statistics by category
        foreach (SupportCategory category in Enum.GetValues<SupportCategory>())
        {
            var categoryRequests = _processedRequests.Where(r => r.Category == category).ToList();
            if (categoryRequests.Any())
            {
                stats.RequestsByCategory[category] = categoryRequests.Count;
            }
        }

        // Calculate statistics by priority
        foreach (Priority priority in Enum.GetValues<Priority>())
        {
            var priorityRequests = _processedRequests.Where(r => r.Priority == priority).ToList();
            if (priorityRequests.Any())
            {
                stats.RequestsByPriority[priority] = priorityRequests.Count;
            }
        }

        // Calculate statistics by handler
        foreach (var request in _processedRequests)
        {
            var handledEvent = request.Events.FirstOrDefault(e => e.EventType == "HANDLED");
            if (handledEvent != null)
            {
                var handlerName = handledEvent.HandlerName;
                stats.RequestsByHandler[handlerName] = stats.RequestsByHandler.GetValueOrDefault(handlerName, 0) + 1;
            }
        }

        return stats;
    }

    /// <summary>
    /// Gets detailed information about all handlers in the chain.
    /// </summary>
    public List<HandlerInfo> GetHandlerChainInfo()
    {
        return _handlers.Values.Select(h => h.GetHandlerInfo()).ToList();
    }

    /// <summary>
    /// Gets the processing history for a specific request.
    /// </summary>
    public List<SupportRequestEvent> GetRequestHistory(string requestId)
    {
        var request = _processedRequests.FirstOrDefault(r => r.Id == requestId);
        return request?.Events ?? new List<SupportRequestEvent>();
    }

    /// <summary>
    /// Gets all processed requests.
    /// </summary>
    public List<SupportRequest> GetProcessedRequests()
    {
        return new List<SupportRequest>(_processedRequests);
    }

    private static List<string> GenerateTags(SupportCategory category, Priority priority)
    {
        var tags = new List<string> { category.ToString().ToLower(), priority.ToString().ToLower() };
        
        // Add category-specific tags
        switch (category)
        {
            case SupportCategory.Technical:
                tags.AddRange(new[] { "troubleshooting", "technical" });
                break;
            case SupportCategory.Billing:
                tags.AddRange(new[] { "payment", "invoice" });
                break;
            case SupportCategory.Security:
                tags.AddRange(new[] { "security", "urgent" });
                break;
            case SupportCategory.Bug:
                tags.AddRange(new[] { "defect", "software" });
                break;
            case SupportCategory.FeatureRequest:
                tags.AddRange(new[] { "enhancement", "product" });
                break;
        }

        // Add priority-specific tags
        if (priority >= Priority.High)
        {
            tags.Add("escalated");
        }
        if (priority == Priority.Emergency)
        {
            tags.Add("emergency");
        }

        return tags;
    }
}