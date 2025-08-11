using Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility.Abstractions;
using Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility.Models;
using Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility.Models.Enums;

namespace Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility.SupportHandlers;

/// <summary>
/// Level 2 Support Handler - handles complex technical issues and billing problems.
/// </summary>
public class Level2SupportHandler : BaseSupportHandler
{
    public Level2SupportHandler() : base(new HandlerInfo
    {
        Name = "Level 2 Support",
        Description = "Advanced technical support and billing issues",
        HandledCategories = new List<SupportCategory> 
        { 
            SupportCategory.Technical, 
            SupportCategory.Billing, 
            SupportCategory.Bug,
            SupportCategory.Integration 
        },
        HandledPriorities = new List<Priority> { Priority.Medium, Priority.High },
        HandledTiers = new List<CustomerTier> { CustomerTier.Basic, CustomerTier.Premium, CustomerTier.Enterprise, CustomerTier.VIP },
        AverageProcessingTime = TimeSpan.FromMinutes(15),
        MaxConcurrentRequests = 10
    })
    {
    }

    protected override bool CanHandle(SupportRequest request)
    {
        // Level 2 handles technical and billing issues with medium to high priority
        return SupportsCategory(request.Category) && SupportsPriority(request.Priority);
    }

    protected override SupportResponse ProcessRequest(SupportRequest request)
    {
        Console.WriteLine($"[Level 2] Processing complex request: {request.Subject}");
        
        // Simulate longer processing time for complex issues
        Thread.Sleep(200);
        
        var response = new SupportResponse
        {
            IsHandled = true,
            Message = "Your complex issue has been resolved by our Level 2 technical team.",
            Actions = new List<string>
            {
                "Performed detailed system analysis",
                "Applied advanced troubleshooting procedures",
                "Coordinated with development team if needed"
            }
        };

        // Determine resolution based on category
        response.Resolution = request.Category switch
        {
            SupportCategory.Technical => "Advanced technical issue resolved through detailed analysis and custom solution.",
            SupportCategory.Billing => "Billing discrepancy investigated and resolved. Account adjusted as necessary.",
            SupportCategory.Bug => "Bug confirmed and workaround provided. Issue escalated to development team.",
            SupportCategory.Integration => "Integration issue diagnosed and resolved. Configuration updated.",
            _ => "Complex request processed successfully by Level 2 support."
        };

        // Add specialized actions based on category
        switch (request.Category)
        {
            case SupportCategory.Technical:
                response.Actions.Add("Created detailed technical documentation");
                response.Actions.Add("Updated system configuration");
                break;
            case SupportCategory.Billing:
                response.Actions.Add("Reviewed billing history");
                response.Actions.Add("Applied account credits if applicable");
                break;
            case SupportCategory.Bug:
                response.Actions.Add("Documented bug reproduction steps");
                response.Actions.Add("Created development ticket");
                break;
        }

        // Always follow up on high priority issues
        if (request.Priority >= Priority.High)
        {
            response.RequiresFollowUp = true;
            response.FollowUpDate = DateTime.Now.AddHours(24);
            response.Actions.Add("Scheduled 24-hour follow-up for high priority issue");
        }

        return response;
    }
}