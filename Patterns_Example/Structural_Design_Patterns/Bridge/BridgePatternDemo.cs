using Patterns_Example.Structural_Design_Patterns.Bridge;

namespace Patterns_Example.Structural_Design_Patterns.Bridge;

/// <summary>
/// Demo program to showcase the Bridge pattern implementation
/// Run this to see the Bridge pattern in action
/// </summary>
public class BridgePatternDemo
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("🌉 Bridge Pattern Demo: Notification System");
        Console.WriteLine(new string('=', 60));
        Console.WriteLine();

        try
        {
            await BridgeExample.RunExample();
            
            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("✅ Bridge Pattern Demo completed successfully!");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error running demo: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
