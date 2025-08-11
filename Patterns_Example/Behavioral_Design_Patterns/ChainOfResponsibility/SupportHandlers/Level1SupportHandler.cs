using Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility.Abstractions;
using Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility.Models;
using Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility.Models.Enums;

namespace Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility.SupportHandlers;

/// <summary>
/// Level 1 Support Handler - handles basic technical issues and general inquiries.
/// </summary>
public class Level1SupportHandler : BaseSupportHandler
{
    public Level1SupportHandler() : base(new HandlerInfo
    {
        Name = "Level 1 Support",
        Description = "Basic technical support and general inquiries",
        HandledCategories = new List<SupportCategory> { SupportCategory.General, SupportCategory.Account },
        HandledPriorities = new List<Priority> { Priority.Low, Priority.Medium },
        HandledTiers = new List<CustomerTier> { CustomerTier.Basic, CustomerTier.Premium, CustomerTier.Enterprise, CustomerTier.VIP },
        AverageProcessingTime = TimeSpan.FromMinutes(5),
        MaxConcurrentRequests = 20
    })
    {
    }

    protected override bool CanHandle(SupportRequest request)
    {
        // Level 1 handles basic requests with low to medium priority
        return (SupportsCategory(request.Category) && SupportsPriority(request.Priority)) ||
               (request.Category == SupportCategory.Technical && request.Priority <= Priority.Medium);
    }

    protected override SupportResponse ProcessRequest(SupportRequest request)
    {
        Console.WriteLine($"[Level 1] Processing request: {request.Subject}");
        
        // Simulate processing time
        Thread.Sleep(100);
        
        var response = new SupportResponse
        {
            IsHandled = true,
            Message = "Your request has been resolved by our Level 1 support team.",
            Actions = new List<string>
            {
                "Reviewed account information",
                "Provided standard troubleshooting steps",
                "Updated customer knowledge base"
            }
        };

        // Determine resolution based on category
        response.Resolution = request.Category switch
        {
            SupportCategory.General => "Provided general information and guidance. Issue resolved.",
            SupportCategory.Account => "Account issue resolved. Customer credentials updated if needed.",
            SupportCategory.Technical => "Basic technical issue resolved with standard troubleshooting.",
            _ => "Request processed successfully by Level 1 support."
        };

        // Check if follow-up is needed
        if (request.CustomerTier >= CustomerTier.Premium)
        {
            response.RequiresFollowUp = true;
            response.FollowUpDate = DateTime.Now.AddDays(1);
            response.Actions.Add("Scheduled follow-up for premium customer");
        }

        return response;
    }
}