using Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility.Models.Enums;
using Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility.SupportHandlers;

namespace Patterns_Example.Behavioral_Design_Patterns.ChainOfResponsibility;

/// <summary>
/// Example class demonstrating the Chain of Responsibility pattern usage
/// </summary>
public static class ChainOfResponsibilityExample
{
    public static void RunExample()
    {
        Console.WriteLine("=== Chain of Responsibility Pattern Example: Customer Support System ===\n");
        
        // Demonstrate the problem without chain of responsibility
        DemonstrateProblemsWithoutChain();
        
        // Demonstrate basic chain of responsibility usage
        DemonstrateBasicChainUsage();
        
        // Demonstrate different request types and routing
        DemonstrateRequestRouting();
        
        // Demonstrate emergency handling
        DemonstrateEmergencyHandling();
        
        // Demonstrate chain flexibility
        DemonstrateChainFlexibility();
        
        // Demonstrate system statistics and monitoring
        DemonstrateSystemMonitoring();
        
        Console.WriteLine("=== Chain of Responsibility Pattern Benefits Demonstrated ===");
        Console.WriteLine("✓ Decoupled request senders from receivers");
        Console.WriteLine("✓ Dynamic chain configuration and modification");
        Console.WriteLine("✓ Flexible request handling based on multiple criteria");
        Console.WriteLine("✓ Easy addition of new handlers without changing existing code");
        Console.WriteLine("✓ Automatic request routing through appropriate handlers");
        Console.WriteLine("✓ Support for complex business logic and escalation paths");
    }

    private static void DemonstrateProblemsWithoutChain()
    {
        Console.WriteLine("1. Problems WITHOUT Chain of Responsibility Pattern:");
        Console.WriteLine(new string('=', 60));
        Console.WriteLine("❌ Tightly coupled request handling:");
        Console.WriteLine();
        
        Console.WriteLine("// Without chain - monolithic request handler");
        Console.WriteLine("public class MonolithicSupportSystem");
        Console.WriteLine("{");
        Console.WriteLine("    public void HandleRequest(SupportRequest request)");
        Console.WriteLine("    {");
        Console.WriteLine("        if (request.Priority == Emergency)");
        Console.WriteLine("            HandleEmergency(request);");
        Console.WriteLine("        else if (request.Category == Technical && request.Priority == High)");
        Console.WriteLine("            HandleSeniorSupport(request);");
        Console.WriteLine("        else if (request.Category == Billing)");
        Console.WriteLine("            HandleLevel2Support(request);");
        Console.WriteLine("        // ... dozens of if-else conditions");
        Console.WriteLine("    }");
        Console.WriteLine("}");
        Console.WriteLine();
        Console.WriteLine("❌ Problems:");
        Console.WriteLine("   • Tight coupling between request types and handlers");
        Console.WriteLine("   • Complex if-else chains that are hard to maintain");
        Console.WriteLine("   • Difficult to add new request types or handlers");
        Console.WriteLine("   • No flexibility in request routing");
        Console.WriteLine("   • Violation of Open/Closed Principle");
        Console.WriteLine("   • Single point of failure");
        Console.WriteLine();
    }

    private static void DemonstrateBasicChainUsage()
    {
        Console.WriteLine("2. Basic Chain of Responsibility Usage:");
        Console.WriteLine(new string('=', 60));
        
        var supportSystem = new SupportTicketSystem();
        
        Console.WriteLine("✅ Chain of Responsibility Benefits:");
        Console.WriteLine("   • Decoupled request handling");
        Console.WriteLine("   • Flexible chain configuration");
        Console.WriteLine("   • Easy to add new handlers");
        Console.WriteLine("   • Automatic request routing");
        Console.WriteLine();
        
        // Show handler chain information
        Console.WriteLine("Handler Chain Information:");
        var handlers = supportSystem.GetHandlerChainInfo();
        foreach (var handler in handlers)
        {
            Console.WriteLine($"  📋 {handler}");
        }
        Console.WriteLine();
        
        // Process a simple request
        Console.WriteLine("Processing a basic support request:");
        var basicRequest = SupportTicketSystem.CreateSampleRequest(
            "John Smith",
            CustomerTier.Basic,
            SupportCategory.General,
            Priority.Low,
            "How to reset my password?",
            "I forgot my password and need help resetting it."
        );
        
        var response = supportSystem.ProcessRequest(basicRequest);
        Console.WriteLine($"Resolution: {response.Resolution}");
        Console.WriteLine();
    }

    private static void DemonstrateRequestRouting()
    {
        Console.WriteLine("3. Request Routing Through Different Handlers:");
        Console.WriteLine(new string('=', 60));
        
        var supportSystem = new SupportTicketSystem();
        
        // Create different types of requests to demonstrate routing
        var requests = new[]
        {
            SupportRequestBuilder.Create()
                .ForCustomer("Alice Johnson", "alice@company.com", CustomerTier.Premium)
                .WithCategory(SupportCategory.Technical)
                .WithPriority(Priority.Medium)
                .WithSubject("Application crashes on startup")
                .WithDescription("The application crashes immediately when I try to start it.")
                .Build(),
                
            SupportRequestBuilder.Create()
                .ForCustomer("Bob Wilson", "bob@enterprise.com", CustomerTier.Enterprise)
                .WithCategory(SupportCategory.Billing)
                .WithPriority(Priority.High)
                .WithSubject("Incorrect billing amount")
                .WithDescription("We were charged twice for the same service this month.")
                .Build(),
                
            SupportRequestBuilder.Create()
                .ForCustomer("Carol Davis", "carol@startup.com", CustomerTier.Basic)
                .WithCategory(SupportCategory.Sales)
                .WithPriority(Priority.Medium)
                .WithSubject("Pricing information for enterprise plan")
                .WithDescription("We're interested in upgrading to enterprise. What are the pricing options?")
                .Build(),
                
            SupportRequestBuilder.Create()
                .ForCustomer("David Brown", "david@vip.com", CustomerTier.VIP)
                .WithCategory(SupportCategory.Security)
                .WithPriority(Priority.Critical)
                .WithSubject("Potential security breach detected")
                .WithDescription("We detected unusual activity in our account that might indicate a security breach.")
                .Build()
        };
        
        Console.WriteLine("Processing different types of requests to show routing:");
        Console.WriteLine();
        
        foreach (var request in requests)
        {
            var response = supportSystem.ProcessRequest(request);
            
            // Show the request journey through the chain
            Console.WriteLine("Request Journey:");
            var history = supportSystem.GetRequestHistory(request.Id);
            foreach (var evt in history)
            {
                Console.WriteLine($"  {evt}");
            }
            Console.WriteLine(response);
        }
    }

    private static void DemonstrateEmergencyHandling()
    {
        Console.WriteLine("4. Emergency Request Handling:");
        Console.WriteLine(new string('=', 60));
        
        var supportSystem = new SupportTicketSystem();
        
        // Create an emergency request
        var emergencyRequest = SupportRequestBuilder.Create()
            .ForCustomer("Emergency Corp", "cto@emergency.com", CustomerTier.Enterprise)
            .WithCategory(SupportCategory.Technical)
            .WithPriority(Priority.Emergency)
            .WithSubject("CRITICAL: Complete system outage")
            .WithDescription("Our entire production system is down. All services are unavailable. This is affecting thousands of users.")
            .WithTags("outage", "production", "critical")
            .WithMetadata("affected_users", 50000)
            .WithMetadata("revenue_impact", "$10000_per_hour")
            .Build();
        
        Console.WriteLine("🚨 Processing EMERGENCY request:");
        var response = supportSystem.ProcessRequest(emergencyRequest);
        
        Console.WriteLine("Emergency Response Details:");
        Console.WriteLine($"  Handler: {response.HandlerName}");
        Console.WriteLine($"  Processing Time: {response.ProcessingTime.TotalMilliseconds:F0}ms");
        Console.WriteLine($"  Actions Taken:");
        foreach (var action in response.Actions)
        {
            Console.WriteLine($"    • {action}");
        }
        Console.WriteLine();
        
        if (response.AdditionalData.ContainsKey("war_room_id"))
        {
            Console.WriteLine($"🏢 War Room ID: {response.AdditionalData["war_room_id"]}");
            Console.WriteLine($"📞 Executive Notification: {response.AdditionalData["executive_notification"]}");
        }
        Console.WriteLine();
    }

    private static void DemonstrateChainFlexibility()
    {
        Console.WriteLine("5. Chain Flexibility and Modification:");
        Console.WriteLine(new string('=', 60));
        
        Console.WriteLine("The Chain of Responsibility pattern allows for:");
        Console.WriteLine("✓ Dynamic chain modification at runtime");
        Console.WriteLine("✓ Adding new handlers without changing existing code");
        Console.WriteLine("✓ Reordering handlers based on business needs");
        Console.WriteLine("✓ Conditional handler activation");
        Console.WriteLine();
        
        // Demonstrate creating a custom chain
        Console.WriteLine("Creating a custom handler chain for VIP customers:");
        
        // Create a VIP-focused chain
        var vipLevel1 = new Level1SupportHandler();
        var vipSenior = new SeniorSupportHandler();
        var vipEmergency = new EmergencyResponseHandler();
        
        // VIP chain: Emergency -> Senior -> Level1 (skipping Level2 for faster response)
        vipEmergency.SetNext(vipSenior).SetNext(vipLevel1);
        
        Console.WriteLine("VIP Chain: Emergency Response → Senior Support → Level 1 Support");
        Console.WriteLine("(Level 2 bypassed for faster VIP response)");
        Console.WriteLine();
        
        // Test VIP chain with a medium priority request
        var vipRequest = SupportRequestBuilder.Create()
            .ForCustomer("VIP Customer", "vip@important.com", CustomerTier.VIP)
            .WithCategory(SupportCategory.Technical)
            .WithPriority(Priority.Medium)
            .WithSubject("VIP technical question")
            .WithDescription("Quick technical question from our VIP customer.")
            .Build();
        
        Console.WriteLine("Processing VIP request through custom chain:");
        vipRequest.AddEvent("RECEIVED", "VIP request received");
        var vipResponse = vipEmergency.Handle(vipRequest);
        
        Console.WriteLine($"VIP Response: {vipResponse}");
        Console.WriteLine("VIP Request Journey:");
        foreach (var evt in vipRequest.Events)
        {
            Console.WriteLine($"  {evt}");
        }
        Console.WriteLine();
    }

    private static void DemonstrateSystemMonitoring()
    {
        Console.WriteLine("6. System Monitoring and Statistics:");
        Console.WriteLine(new string('=', 60));
        
        var supportSystem = new SupportTicketSystem();
        
        // Process multiple requests to generate statistics
        var testRequests = new[]
        {
            ("Tech Issue 1", SupportCategory.Technical, Priority.Low),
            ("Billing Question", SupportCategory.Billing, Priority.Medium),
            ("Sales Inquiry", SupportCategory.Sales, Priority.Low),
            ("Bug Report", SupportCategory.Bug, Priority.High),
            ("Security Alert", SupportCategory.Security, Priority.Critical),
            ("Feature Request", SupportCategory.FeatureRequest, Priority.Medium),
            ("Emergency Outage", SupportCategory.Technical, Priority.Emergency),
            ("Account Issue", SupportCategory.Account, Priority.Low)
        };
        
        Console.WriteLine("Processing multiple requests for statistics...");
        foreach (var (subject, category, priority) in testRequests)
        {
            var request = SupportRequestBuilder.Create()
                .ForCustomer($"Customer {testRequests.ToList().IndexOf((subject, category, priority)) + 1}", 
                           $"customer{testRequests.ToList().IndexOf((subject, category, priority)) + 1}@test.com", 
                           CustomerTier.Premium)
                .WithCategory(category)
                .WithPriority(priority)
                .WithSubject(subject)
                .WithDescription($"Test request for {subject}")
                .Build();
            
            supportSystem.ProcessRequest(request);
        }
        
        Console.WriteLine();
        Console.WriteLine("Support System Statistics:");
        var stats = supportSystem.GetStatistics();
        Console.WriteLine(stats);
        Console.WriteLine();
        
        // Show request distribution
        Console.WriteLine("Request Processing Summary:");
        var processedRequests = supportSystem.GetProcessedRequests();
        var handledCount = processedRequests.Count(r => r.Events.Any(e => e.EventType == "HANDLED"));
        var unhandledCount = processedRequests.Count - handledCount;
        
        Console.WriteLine($"  📊 Total Requests: {processedRequests.Count}");
        Console.WriteLine($"  ✅ Successfully Handled: {handledCount} ({stats.HandlingSuccessRate:F1}%)");
        Console.WriteLine($"  ❌ Unhandled: {unhandledCount}");
        Console.WriteLine($"  ⏱️  Average Processing Time: {stats.AverageProcessingTime.TotalMilliseconds:F0}ms");
        Console.WriteLine();
        
        // Show handler efficiency
        Console.WriteLine("Handler Efficiency:");
        foreach (var handlerStat in stats.RequestsByHandler.OrderByDescending(kvp => kvp.Value))
        {
            var percentage = (double)handlerStat.Value / handledCount * 100;
            Console.WriteLine($"  {handlerStat.Key}: {handlerStat.Value} requests ({percentage:F1}%)");
        }
        Console.WriteLine();
    }
}
