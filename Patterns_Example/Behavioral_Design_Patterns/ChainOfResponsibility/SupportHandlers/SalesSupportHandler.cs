using Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility.Abstractions;
using Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility.Models;
using Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility.Models.Enums;

namespace Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility.SupportHandlers;

/// <summary>
/// Sales Support Handler - handles sales inquiries and feature requests.
/// </summary>
public class SalesSupportHandler : BaseSupportHandler
{
    public SalesSupportHandler() : base(new HandlerInfo
    {
        Name = "Sales Support",
        Description = "Sales inquiries and feature requests",
        HandledCategories = new List<SupportCategory> 
        { 
            SupportCategory.Sales, 
            SupportCategory.FeatureRequest 
        },
        HandledPriorities = new List<Priority> { Priority.Low, Priority.Medium, Priority.High },
        HandledTiers = new List<CustomerTier> { CustomerTier.Basic, CustomerTier.Premium, CustomerTier.Enterprise, CustomerTier.VIP },
        AverageProcessingTime = TimeSpan.FromMinutes(10),
        MaxConcurrentRequests = 15
    })
    {
    }

    protected override bool CanHandle(SupportRequest request)
    {
        // Sales support handles sales and feature request categories
        return SupportsCategory(request.Category);
    }

    protected override SupportResponse ProcessRequest(SupportRequest request)
    {
        Console.WriteLine($"[Sales Support] Processing sales inquiry: {request.Subject}");
        
        // Simulate sales consultation time
        Thread.Sleep(150);
        
        var response = new SupportResponse
        {
            IsHandled = true,
            Message = "Your sales inquiry has been handled by our specialized sales support team.",
            Actions = new List<string>
            {
                "Reviewed customer requirements",
                "Provided product information and pricing",
                "Connected with appropriate sales representative"
            }
        };

        response.Resolution = request.Category switch
        {
            SupportCategory.Sales => "Sales inquiry processed. Customer connected with sales representative for detailed consultation.",
            SupportCategory.FeatureRequest => "Feature request documented and forwarded to product management team for evaluation.",
            _ => "Sales-related request processed successfully."
        };

        // Add sales-specific actions
        if (request.Category == SupportCategory.Sales)
        {
            response.Actions.Add("Scheduled product demonstration");
            response.Actions.Add("Prepared custom pricing proposal");
            response.RequiresFollowUp = true;
            response.FollowUpDate = DateTime.Now.AddDays(2);
        }
        else if (request.Category == SupportCategory.FeatureRequest)
        {
            response.Actions.Add("Created product backlog item");
            response.Actions.Add("Notified product management team");
            response.RequiresFollowUp = true;
            response.FollowUpDate = DateTime.Now.AddDays(7);
        }

        return response;
    }
}