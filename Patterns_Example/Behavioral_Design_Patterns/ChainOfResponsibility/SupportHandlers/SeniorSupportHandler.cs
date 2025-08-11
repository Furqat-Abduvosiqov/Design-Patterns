using Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility.Abstractions;
using Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility.Models;
using Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility.Models.Enums;

namespace Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility.SupportHandlers;

/// <summary>
/// Senior Support Handler - handles critical issues and enterprise customer requests.
/// </summary>
public class SeniorSupportHandler : BaseSupportHandler
{
    public SeniorSupportHandler() : base(new HandlerInfo
    {
        Name = "Senior Support",
        Description = "Critical issues and enterprise customer support",
        HandledCategories = new List<SupportCategory> 
        { 
            SupportCategory.Technical, 
            SupportCategory.Security, 
            SupportCategory.Performance,
            SupportCategory.Integration,
            SupportCategory.Bug
        },
        HandledPriorities = new List<Priority> { Priority.High, Priority.Critical },
        HandledTiers = new List<CustomerTier> { CustomerTier.Enterprise, CustomerTier.VIP },
        AverageProcessingTime = TimeSpan.FromMinutes(30),
        MaxConcurrentRequests = 5
    })
    {
    }

    protected override bool CanHandle(SupportRequest request)
    {
        // Senior support handles critical issues or enterprise/VIP customers
        return (SupportsPriority(request.Priority) && SupportsCategory(request.Category)) ||
               (SupportsTier(request.CustomerTier) && request.Priority >= Priority.High);
    }

    protected override SupportResponse ProcessRequest(SupportRequest request)
    {
        Console.WriteLine($"[Senior Support] Handling critical request: {request.Subject}");
        
        // Simulate extensive processing time for critical issues
        Thread.Sleep(300);
        
        var response = new SupportResponse
        {
            IsHandled = true,
            Message = "Your critical issue has been personally handled by our senior support specialist.",
            Actions = new List<string>
            {
                "Conducted comprehensive system review",
                "Coordinated with multiple technical teams",
                "Implemented custom enterprise solution",
                "Provided direct communication channel"
            }
        };

        // Determine resolution based on category and priority
        response.Resolution = request.Category switch
        {
            SupportCategory.Security => "Security issue thoroughly investigated and resolved. System hardened with additional safeguards.",
            SupportCategory.Performance => "Performance bottleneck identified and optimized. System monitoring enhanced.",
            SupportCategory.Technical => "Critical technical issue resolved with custom enterprise-grade solution.",
            SupportCategory.Integration => "Complex integration issue resolved with dedicated technical consultation.",
            SupportCategory.Bug => "Critical bug prioritized for immediate fix. Hotfix deployed or workaround implemented.",
            _ => "Critical request resolved with senior-level expertise and enterprise support."
        };

        // Add enterprise-specific actions
        if (request.CustomerTier >= CustomerTier.Enterprise)
        {
            response.Actions.Add("Assigned dedicated account manager");
            response.Actions.Add("Created enterprise escalation path");
            response.Actions.Add("Scheduled architecture review session");
        }

        // Critical issues always require immediate follow-up
        if (request.Priority >= Priority.Critical)
        {
            response.RequiresFollowUp = true;
            response.FollowUpDate = DateTime.Now.AddHours(4);
            response.Actions.Add("Scheduled 4-hour critical follow-up");
            response.AdditionalData["escalation_path"] = "direct_to_engineering";
        }

        return response;
    }
}