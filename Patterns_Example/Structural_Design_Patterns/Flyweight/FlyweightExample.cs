namespace Patterns_Example.Structural_Design_Patterns.Flyweight;

/// <summary>
/// Example class demonstrating the Flyweight pattern usage
/// </summary>
public static class FlyweightExample
{
    public static void RunExample()
    {
        Console.WriteLine("=== Flyweight Pattern Example: Text Editor System ===\n");
        
        // Demonstrate the memory efficiency problem
        DemonstrateMemoryEfficiencyProblem();
        
        // Demonstrate basic flyweight usage
        DemonstrateBasicFlyweightUsage();
        
        // Demonstrate flyweight factory and sharing
        DemonstrateFlyweightFactory();
        
        // Demonstrate text document with flyweights
        DemonstrateTextDocument();
        
        // Demonstrate memory savings with large documents
        DemonstrateMemorySavings();
        
        // Demonstrate advanced features
        DemonstrateAdvancedFeatures();
        
        Console.WriteLine("=== Flyweight Pattern Benefits Demonstrated ===");
        Console.WriteLine("✓ Significant memory savings through object sharing");
        Console.WriteLine("✓ Separation of intrinsic and extrinsic state");
        Console.WriteLine("✓ Efficient handling of large numbers of similar objects");
        Console.WriteLine("✓ Factory-managed flyweight creation and reuse");
        Console.WriteLine("✓ Transparent usage - clients don't need to know about sharing");
        Console.WriteLine("✓ Performance optimization for memory-intensive applications");
    }

    private static void DemonstrateMemoryEfficiencyProblem()
    {
        Console.WriteLine("1. Memory Efficiency Problem:");
        Console.WriteLine(new string('=', 50));
        Console.WriteLine("❌ Without Flyweight Pattern:");
        Console.WriteLine();
        
        Console.WriteLine("// Without flyweight - each character is a separate object");
        Console.WriteLine("class Character {");
        Console.WriteLine("    char value;           // 2 bytes");
        Console.WriteLine("    string fontFamily;    // ~50 bytes");
        Console.WriteLine("    byte[] glyphData;     // ~2000 bytes");
        Console.WriteLine("    int x, y;            // 8 bytes");
        Console.WriteLine("    Color foreground;     // 4 bytes");
        Console.WriteLine("    Color background;     // 4 bytes");
        Console.WriteLine("    FontStyle style;      // 4 bytes");
        Console.WriteLine("    // Total: ~2072 bytes per character");
        Console.WriteLine("}");
        Console.WriteLine();
        
        Console.WriteLine("For a 100,000 character document:");
        Console.WriteLine($"  Memory usage: {100000 * 2072 / (1024.0 * 1024.0):F1} MB");
        Console.WriteLine("  Problem: Massive memory waste for repeated characters!");
        Console.WriteLine();
        
        Console.WriteLine("✅ With Flyweight Pattern:");
        Console.WriteLine("  Intrinsic state (shared): glyph data, font metrics");
        Console.WriteLine("  Extrinsic state (unique): position, color, style");
        Console.WriteLine("  Result: Dramatic memory reduction!");
        Console.WriteLine();
    }

    private static void DemonstrateBasicFlyweightUsage()
    {
        Console.WriteLine("2. Basic Flyweight Usage:");
        Console.WriteLine(new string('=', 50));
        
        var factory = CharacterFlyweightFactory.Instance;
        
        // Get flyweights for some characters
        var charA = factory.GetFlyweight('A');
        var charB = factory.GetFlyweight('B');
        var charA2 = factory.GetFlyweight('A'); // Should reuse existing flyweight
        
        Console.WriteLine($"Character A flyweight: {charA}");
        Console.WriteLine($"Character B flyweight: {charB}");
        Console.WriteLine($"Character A (second request): {charA2}");
        Console.WriteLine($"A and A2 are same instance: {ReferenceEquals(charA, charA2)}");
        Console.WriteLine();
        
        // Demonstrate rendering with different contexts
        Console.WriteLine("Rendering same flyweight with different contexts:");
        
        var context1 = new CharacterContext(0, 0)
        {
            ForegroundColor = ConsoleColor.Red,
            FontSize = 12
        };
        
        var context2 = new CharacterContext(1, 0)
        {
            ForegroundColor = ConsoleColor.Blue,
            FontSize = 16
        };
        
        Console.Write("Red A (size 12): ");
        charA.Render(context1);
        Console.WriteLine();
        
        Console.Write("Blue A (size 16): ");
        charA.Render(context2);
        Console.WriteLine();
        Console.WriteLine();
    }

    private static void DemonstrateFlyweightFactory()
    {
        Console.WriteLine("3. Flyweight Factory and Sharing:");
        Console.WriteLine(new string('=', 50));
        
        var factory = CharacterFlyweightFactory.Instance;
        factory.ClearCache(); // Start fresh for demo
        
        Console.WriteLine("Creating flyweights for the word 'HELLO':");
        var word = "HELLO";
        var flyweights = new List<ICharacterFlyweight>();
        
        foreach (char c in word)
        {
            var flyweight = factory.GetFlyweight(c);
            flyweights.Add(flyweight);
            Console.WriteLine($"  '{c}': {(factory.CacheHits > 0 ? "REUSED" : "CREATED")}");
        }
        
        Console.WriteLine();
        Console.WriteLine("Factory statistics after creating 'HELLO':");
        var stats = factory.GetStatistics();
        Console.WriteLine($"  Total flyweights created: {stats.TotalFlyweights}");
        Console.WriteLine($"  Total requests: {stats.TotalRequests}");
        Console.WriteLine($"  Cache hits: {stats.CacheHits}");
        Console.WriteLine($"  Cache hit ratio: {stats.CacheHitRatio:P1}");
        Console.WriteLine();
        
        Console.WriteLine("Creating flyweights for 'HELLO WORLD' (reusing existing):");
        var phrase = "HELLO WORLD";
        foreach (char c in phrase)
        {
            factory.GetFlyweight(c);
        }
        
        var newStats = factory.GetStatistics();
        Console.WriteLine($"  Total flyweights: {newStats.TotalFlyweights} (only {newStats.TotalFlyweights - stats.TotalFlyweights} new)");
        Console.WriteLine($"  Cache hit ratio: {newStats.CacheHitRatio:P1}");
        Console.WriteLine();
    }

    private static void DemonstrateTextDocument()
    {
        Console.WriteLine("4. Text Document with Flyweights:");
        Console.WriteLine(new string('=', 50));
        
        var document = new TextDocument("Flyweight Demo Document");
        
        // Add some text with different formatting
        var normalFormat = new TextFormatting
        {
            ForegroundColor = ConsoleColor.White,
            BackgroundColor = ConsoleColor.Black,
            FontSize = 12
        };
        
        var boldFormat = new TextFormatting
        {
            ForegroundColor = ConsoleColor.Yellow,
            BackgroundColor = ConsoleColor.Black,
            FontSize = 12,
            FontStyle = FontStyle.Bold
        };
        
        document.AddText("The Flyweight Pattern ", normalFormat);
        document.AddText("efficiently manages memory", boldFormat);
        document.AddText(" by sharing intrinsic state between objects.\n", normalFormat);
        document.AddText("This is especially useful for text editors and graphics applications.", normalFormat);
        
        Console.WriteLine("Document content:");
        Console.WriteLine($"'{document.GetText()}'");
        Console.WriteLine();
        
        // Show document statistics
        var docStats = document.GetStatistics();
        Console.WriteLine("Document statistics:");
        Console.WriteLine($"  Total characters: {docStats.TotalCharacters}");
        Console.WriteLine($"  Unique characters: {docStats.UniqueCharacters}");
        Console.WriteLine($"  Memory efficiency: {(1.0 - (double)docStats.UniqueCharacters / docStats.TotalCharacters):P1} savings");
        Console.WriteLine();
        
        // Demonstrate text operations
        Console.WriteLine("Demonstrating text operations:");
        document.SelectRange(4, 12); // Select "Flyweight"
        Console.WriteLine("  Selected characters 4-12 ('Flyweight')");
        
        document.HighlightRange(20, 30); // Highlight "efficiently"
        Console.WriteLine("  Highlighted characters 20-30 ('efficiently')");
        Console.WriteLine();
    }

    private static void DemonstrateMemorySavings()
    {
        Console.WriteLine("5. Memory Savings with Large Documents:");
        Console.WriteLine(new string('=', 50));
        
        // Create a large document to demonstrate memory savings
        var largeDocument = new TextDocument("Large Document Demo");
        
        // Add repeated text to simulate a real document
        var sampleTexts = new[]
        {
            "The quick brown fox jumps over the lazy dog. ",
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. ",
            "This is a sample text with repeated characters and words. ",
            "Flyweight pattern reduces memory usage significantly. ",
            "Programming patterns help solve common design problems. "
        };
        
        Console.WriteLine("Creating large document with repeated text...");
        var random = new Random(42); // Fixed seed for consistent results
        
        for (int i = 0; i < 1000; i++) // Add 1000 lines
        {
            var text = sampleTexts[random.Next(sampleTexts.Length)];
            largeDocument.AddText(text);
            
            if (i % 100 == 0)
            {
                Console.Write(".");
            }
        }
        Console.WriteLine(" Done!");
        Console.WriteLine();
        
        // Show memory savings
        var stats = largeDocument.GetStatistics();
        Console.WriteLine("Large document statistics:");
        Console.WriteLine($"  Total characters: {stats.TotalCharacters:N0}");
        Console.WriteLine($"  Unique characters: {stats.UniqueCharacters:N0}");
        Console.WriteLine($"  Flyweights created: {stats.FlyweightStatistics.TotalFlyweights:N0}");
        Console.WriteLine($"  Memory with flyweight: {stats.EstimatedMemoryUsage / (1024.0 * 1024.0):F2} MB");
        Console.WriteLine($"  Memory without flyweight: {stats.EstimatedMemoryWithoutFlyweight / (1024.0 * 1024.0):F2} MB");
        Console.WriteLine($"  Memory saved: {(stats.EstimatedMemoryWithoutFlyweight - stats.EstimatedMemoryUsage) / (1024.0 * 1024.0):F2} MB");
        Console.WriteLine($"  Memory efficiency: {(1.0 - (double)stats.EstimatedMemoryUsage / stats.EstimatedMemoryWithoutFlyweight):P1}");
        Console.WriteLine();
    }

    private static void DemonstrateAdvancedFeatures()
    {
        Console.WriteLine("6. Advanced Flyweight Features:");
        Console.WriteLine(new string('=', 50));
        
        var factory = CharacterFlyweightFactory.Instance;
        
        // Demonstrate different character types
        Console.WriteLine("Different flyweight types:");
        var letter = factory.GetFlyweight('A');
        var space = factory.GetFlyweight(' ');
        var punctuation = factory.GetFlyweight('!');
        
        Console.WriteLine($"  Letter 'A': {letter.GetType().Name}");
        Console.WriteLine($"  Space ' ': {space.GetType().Name}");
        Console.WriteLine($"  Punctuation '!': {punctuation.GetType().Name}");
        Console.WriteLine();
        
        // Demonstrate character metrics
        Console.WriteLine("Character metrics for different sizes:");
        var charA = factory.GetFlyweight('A');
        for (int size = 10; size <= 20; size += 5)
        {
            var width = charA.GetWidth(size, FontStyle.Normal);
            var height = charA.GetHeight(size, FontStyle.Normal);
            Console.WriteLine($"  Size {size}: {width:F1}w × {height:F1}h");
        }
        Console.WriteLine();
        
        // Demonstrate ligature detection
        Console.WriteLine("Ligature detection:");
        var charF = factory.GetFlyweight('f');
        Console.WriteLine($"  'f' can combine with 'i': {charF.CanCombineWith('i')}");
        Console.WriteLine($"  'f' can combine with 'l': {charF.CanCombineWith('l')}");
        Console.WriteLine($"  'f' can combine with 'x': {charF.CanCombineWith('x')}");
        Console.WriteLine();
        
        // Show flyweight distribution by type
        Console.WriteLine("Flyweight distribution by character type:");
        var flyweightsByType = factory.GetFlyweightsByType();
        foreach (var kvp in flyweightsByType)
        {
            Console.WriteLine($"  {kvp.Key}: {kvp.Value.Count} flyweights");
        }
        Console.WriteLine();
        
        // Final factory statistics
        Console.WriteLine("Final factory statistics:");
        var finalStats = factory.GetStatistics();
        Console.WriteLine($"  Total flyweights: {finalStats.TotalFlyweights}");
        Console.WriteLine($"  Total requests: {finalStats.TotalRequests}");
        Console.WriteLine($"  Memory efficiency: {finalStats.MemorySavingsRatio:P1}");
        Console.WriteLine();
    }
}
