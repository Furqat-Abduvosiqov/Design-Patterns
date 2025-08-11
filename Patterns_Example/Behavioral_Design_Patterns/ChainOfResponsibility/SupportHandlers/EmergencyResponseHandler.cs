using Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility.Abstractions;
using Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility.Models;
using Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility.Models.Enums;

namespace Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility.SupportHandlers;

/// <summary>
/// Emergency Response Handler - handles emergency situations and system outages.
/// </summary>
public class EmergencyResponseHandler : BaseSupportHandler
{
    public EmergencyResponseHandler() : base(new HandlerInfo
    {
        Name = "Emergency Response Team",
        Description = "24/7 emergency response for critical system issues",
        HandledCategories = new List<SupportCategory> 
        { 
            SupportCategory.Technical, 
            SupportCategory.Security, 
            SupportCategory.Performance,
            SupportCategory.Bug
        },
        HandledPriorities = new List<Priority> { Priority.Emergency },
        HandledTiers = new List<CustomerTier> { CustomerTier.Basic, CustomerTier.Premium, CustomerTier.Enterprise, CustomerTier.VIP },
        AverageProcessingTime = TimeSpan.FromMinutes(10),
        MaxConcurrentRequests = 3
    })
    {
    }

    protected override bool CanHandle(SupportRequest request)
    {
        // Emergency team only handles emergency priority requests
        return request.Priority == Priority.Emergency;
    }

    protected override SupportResponse ProcessRequest(SupportRequest request)
    {
        Console.WriteLine($"[EMERGENCY] 🚨 EMERGENCY RESPONSE ACTIVATED: {request.Subject}");
        
        // Immediate response for emergencies
        Thread.Sleep(50);
        
        var response = new SupportResponse
        {
            IsHandled = true,
            Message = "🚨 EMERGENCY: Your critical system issue is being handled by our emergency response team with highest priority.",
            Actions = new List<string>
            {
                "🚨 Emergency response team activated",
                "📞 Immediate phone contact initiated",
                "⚡ All available resources allocated",
                "🔧 Emergency engineering team engaged",
                "📊 Real-time monitoring activated"
            }
        };

        response.Resolution = "EMERGENCY RESPONSE: Immediate action taken to address critical system issue. " +
                              "Emergency team deployed with full authority to resolve the situation. " +
                              "Customer will receive real-time updates every 15 minutes until resolution.";

        // Emergency-specific actions
        response.Actions.Add("🎯 War room established");
        response.Actions.Add("📱 SMS alerts activated for customer");
        response.Actions.Add("🔄 Automatic escalation to C-level executives");
        response.Actions.Add("📋 Post-incident review scheduled");

        // Immediate follow-up for emergencies
        response.RequiresFollowUp = true;
        response.FollowUpDate = DateTime.Now.AddMinutes(15);
        response.AdditionalData["emergency_ticket"] = true;
        response.AdditionalData["war_room_id"] = Guid.NewGuid().ToString();
        response.AdditionalData["executive_notification"] = true;

        return response;
    }
}