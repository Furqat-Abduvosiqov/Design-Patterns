using Patterns_Example.Creational_Design_Patterns.Builder;

namespace Patterns_Example.Creational_Design_Patterns.Builder;

/// <summary>
/// Demo program to showcase the Builder pattern implementation
/// Run this to see the Builder pattern in action
/// </summary>
public class BuilderPatternDemo
{
    public static void Main(string[] args)
    {
        Console.WriteLine("🖥️  Builder Pattern Demo: Computer Configuration System");
        Console.WriteLine(new string('=', 60));
        Console.WriteLine();

        try
        {
            BuilderExample.RunExample();
            
            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("✅ Builder Pattern Demo completed successfully!");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error running demo: {ex.Message}");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
