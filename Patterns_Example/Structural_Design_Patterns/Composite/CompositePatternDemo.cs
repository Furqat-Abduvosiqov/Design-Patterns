using Patterns_Example.Structural_Design_Patterns.Composite;

namespace Patterns_Example.Structural_Design_Patterns.Composite;

/// <summary>
/// Demo program to showcase the Composite pattern implementation
/// Run this to see the Composite pattern in action
/// </summary>
public class CompositePatternDemo
{
    public static void Main(string[] args)
    {
        Console.WriteLine("🌳 Composite Pattern Demo: File System Management");
        Console.WriteLine(new string('=', 60));
        Console.WriteLine();

        try
        {
            CompositeExample.RunExample();
            
            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("✅ Composite Pattern Demo completed successfully!");
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
