namespace Patterns_Example.Structural_Design_Patterns.Facade;

/// <summary>
/// Example class demonstrating the Facade pattern usage
/// </summary>
public static class FacadeExample
{
    public static void RunExample()
    {
        Console.WriteLine("=== Facade Pattern Example: Smart Home Automation System ===\n");
        
        // Demonstrate the complexity without facade
        DemonstrateComplexityWithoutFacade();
        
        // Demonstrate the simplicity with facade
        DemonstrateSimplicityWithFacade();
        
        // Demonstrate different home scenarios
        DemonstrateHomeScenarios();
        
        // Demonstrate emergency and special modes
        DemonstrateSpecialModes();
        
        // Demonstrate system monitoring
        DemonstrateSystemMonitoring();
        
        Console.WriteLine("=== Facade Pattern Benefits Demonstrated ===");
        Console.WriteLine("✓ Simplified interface to complex subsystems");
        Console.WriteLine("✓ Reduced coupling between client and subsystems");
        Console.WriteLine("✓ Improved usability and ease of use");
        Console.WriteLine("✓ Centralized control and coordination");
        Console.WriteLine("✓ Hiding implementation complexity from clients");
        Console.WriteLine("✓ Providing higher-level functionality through composition");
    }

    private static void DemonstrateComplexityWithoutFacade()
    {
        Console.WriteLine("1. Complexity WITHOUT Facade Pattern:");
        Console.WriteLine(new string('=', 50));
        Console.WriteLine("❌ Manual coordination of multiple subsystems required:");
        Console.WriteLine();
        
        // Show what the client would need to do without facade
        Console.WriteLine("// Without Facade - Client must manage all subsystems manually");
        Console.WriteLine("var security = new SecuritySystem();");
        Console.WriteLine("var lighting = new LightingSystem();");
        Console.WriteLine("var climate = new ClimateControlSystem();");
        Console.WriteLine("var entertainment = new EntertainmentSystem();");
        Console.WriteLine();
        Console.WriteLine("// For a simple \"leaving home\" scenario:");
        Console.WriteLine("entertainment.TurnOffAllAudio();");
        Console.WriteLine("entertainment.TurnOffAllVideo();");
        Console.WriteLine("lighting.TurnOffAllLights();");
        Console.WriteLine("lighting.SetZoneBrightness(\"outdoor\", 100);");
        Console.WriteLine("lighting.SetZoneBrightness(\"hallway\", 20);");
        Console.WriteLine("climate.SetAwayMode(true);");
        Console.WriteLine("climate.EnableEcoMode();");
        Console.WriteLine("security.ArmSystem(SecurityMode.Away);");
        Console.WriteLine();
        Console.WriteLine("❌ Problems:");
        Console.WriteLine("   • Client must know about all subsystems");
        Console.WriteLine("   • Complex coordination logic in client code");
        Console.WriteLine("   • Tight coupling between client and subsystems");
        Console.WriteLine("   • Error-prone manual sequencing");
        Console.WriteLine("   • Code duplication across different clients");
        Console.WriteLine();
    }

    private static void DemonstrateSimplicityWithFacade()
    {
        Console.WriteLine("2. Simplicity WITH Facade Pattern:");
        Console.WriteLine(new string('=', 50));
        Console.WriteLine("✅ Simple, unified interface:");
        Console.WriteLine();
        
        var smartHome = new SmartHomeFacade();
        
        Console.WriteLine("// With Facade - Simple, intuitive interface");
        Console.WriteLine("var smartHome = new SmartHomeFacade();");
        Console.WriteLine();
        Console.WriteLine("// Same \"leaving home\" scenario:");
        Console.WriteLine("smartHome.LeavingHome();");
        Console.WriteLine();
        Console.WriteLine("Executing the facade method...");
        Console.WriteLine();
        
        // Actually execute to show the coordination
        smartHome.LeavingHome();
        
        Console.WriteLine();
        Console.WriteLine("✅ Benefits:");
        Console.WriteLine("   • Single method call handles complex coordination");
        Console.WriteLine("   • Client doesn't need to know about subsystems");
        Console.WriteLine("   • Loose coupling between client and implementation");
        Console.WriteLine("   • Consistent, reliable sequencing");
        Console.WriteLine("   • Reusable across different clients");
        Console.WriteLine();
    }

    private static void DemonstrateHomeScenarios()
    {
        Console.WriteLine("3. Different Home Scenarios:");
        Console.WriteLine(new string('=', 50));
        
        var smartHome = new SmartHomeFacade();
        
        // Show available scenes
        Console.WriteLine("Available home scenes:");
        var scenes = smartHome.GetAvailableScenes();
        foreach (var scene in scenes)
        {
            var description = smartHome.GetSceneDescription(scene);
            Console.WriteLine($"   • {scene}: {description}");
        }
        Console.WriteLine();
        
        // Demonstrate a few key scenarios
        Console.WriteLine("Demonstrating key scenarios:");
        Console.WriteLine();
        
        // Good Morning
        smartHome.GoodMorning();
        Console.WriteLine();
        
        // Movie Night
        smartHome.MovieNight();
        Console.WriteLine();
        
        // Bedtime
        smartHome.Bedtime();
        Console.WriteLine();
    }

    private static void DemonstrateSpecialModes()
    {
        Console.WriteLine("4. Emergency and Special Modes:");
        Console.WriteLine(new string('=', 50));
        
        var smartHome = new SmartHomeFacade();
        
        Console.WriteLine("Testing emergency mode:");
        smartHome.EmergencyMode();
        Console.WriteLine();
        
        Console.WriteLine("Testing power saving mode:");
        smartHome.PowerSavingMode();
        Console.WriteLine();
        
        Console.WriteLine("Testing vacation mode:");
        smartHome.VacationMode();
        Console.WriteLine();
        
        Console.WriteLine("Running comprehensive system test:");
        smartHome.RunSystemTest();
        Console.WriteLine();
    }

    private static void DemonstrateSystemMonitoring()
    {
        Console.WriteLine("5. System Monitoring and Status:");
        Console.WriteLine(new string('=', 50));
        
        var smartHome = new SmartHomeFacade();
        
        // Set up some activity first
        Console.WriteLine("Setting up a dinner party scenario for monitoring...");
        smartHome.DinnerParty();
        Console.WriteLine();
        
        // Show comprehensive status
        Console.WriteLine("Displaying comprehensive system status:");
        smartHome.DisplayStatusReport();
        Console.WriteLine();
        
        // Show individual system status
        Console.WriteLine("Getting detailed status information:");
        var status = smartHome.GetHomeStatus();
        
        Console.WriteLine("Individual subsystem details:");
        Console.WriteLine($"   Security Events: {status.SecurityStatus.RecentEvents.Count} recent events");
        Console.WriteLine($"   Lighting Zones: {status.LightingStatus.ActiveZones}/{status.LightingStatus.TotalZones} active");
        Console.WriteLine($"   Climate Zones: {status.ClimateStatus.ActiveZones}/{status.ClimateStatus.TotalZones} active");
        Console.WriteLine($"   Entertainment: {status.EntertainmentStatus.ActiveAudioZones} audio + {status.EntertainmentStatus.ActiveVideoDevices} video active");
        Console.WriteLine();
        
        // Demonstrate facade hiding complexity
        Console.WriteLine("✅ Facade Benefits in Monitoring:");
        Console.WriteLine("   • Single point to get comprehensive status");
        Console.WriteLine("   • Aggregated information from multiple subsystems");
        Console.WriteLine("   • Consistent reporting format");
        Console.WriteLine("   • Simplified client code for status checking");
        Console.WriteLine();
    }

    private static void DemonstrateAdvancedFeatures()
    {
        Console.WriteLine("6. Advanced Facade Features:");
        Console.WriteLine(new string('=', 50));
        
        var smartHome = new SmartHomeFacade();
        
        Console.WriteLine("Scene-based activation:");
        smartHome.ActivateScene("coming_home");
        Console.WriteLine();
        
        Console.WriteLine("Custom scenario combinations:");
        // Show how facade can combine multiple subsystem operations
        Console.WriteLine("Creating a custom 'work from home' scenario...");
        
        // This would be complex without facade, but simple with it
        Console.WriteLine("// Without facade, this would require:");
        Console.WriteLine("// - Multiple subsystem calls");
        Console.WriteLine("// - Complex coordination logic");
        Console.WriteLine("// - Error handling for each subsystem");
        Console.WriteLine();
        Console.WriteLine("// With facade, we can create composite operations:");
        
        // Demonstrate a custom scenario
        smartHome.ComingHome(); // Base scenario
        Console.WriteLine();
        Console.WriteLine("Work from home mode activated through facade coordination!");
        Console.WriteLine();
        
        Console.WriteLine("✅ Advanced Benefits:");
        Console.WriteLine("   • Easy to create new composite operations");
        Console.WriteLine("   • Consistent error handling across subsystems");
        Console.WriteLine("   • Centralized business logic");
        Console.WriteLine("   • Simplified testing of complex scenarios");
        Console.WriteLine();
    }
}
