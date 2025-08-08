namespace Patterns_Example.Creational_Design_Patterns.Prototype;

/// <summary>
/// Example class demonstrating the Prototype pattern usage
/// </summary>
public static class PrototypeExample
{
    public static void RunExample()
    {
        Console.WriteLine("=== Prototype Pattern Example: Document Management System ===\n");
        
        // Demonstrate basic cloning
        DemonstrateBasicCloning();
        
        // Demonstrate shallow vs deep cloning
        DemonstrateShallowVsDeepCloning();
        
        // Demonstrate prototype registry
        DemonstratePrototypeRegistry();
        
        // Demonstrate performance benefits
        DemonstratePerformanceBenefits();
        
        Console.WriteLine("=== Prototype Pattern Benefits Demonstrated ===");
        Console.WriteLine("✓ Object creation by cloning existing instances");
        Console.WriteLine("✓ Avoids expensive initialization and setup");
        Console.WriteLine("✓ Dynamic object creation at runtime");
        Console.WriteLine("✓ Prototype registry for managing templates");
        Console.WriteLine("✓ Both shallow and deep cloning strategies");
        Console.WriteLine("✓ Reduces subclassing for object creation");
    }

    private static void DemonstrateBasicCloning()
    {
        Console.WriteLine("1. Basic Document Cloning:");
        Console.WriteLine(new string('=', 50));
        
        // Create original document
        var originalDoc = new TextDocument("Original Report", "This is the original content of the report.");
        originalDoc.Metadata.Author = "John Doe";
        originalDoc.Metadata.Tags.AddRange(new[] { "report", "quarterly", "sales" });
        originalDoc.ApplyFormatting("Arial", 12, bold: true);
        originalDoc.Attachments.Add("sales_data.xlsx");
        
        Console.WriteLine("Original Document:");
        Console.WriteLine(originalDoc);
        
        // Clone the document
        var clonedDoc = (TextDocument)originalDoc.DeepClone();
        clonedDoc.Title = "Cloned Report";
        clonedDoc.Content = "This is the modified content of the cloned report.";
        clonedDoc.Metadata.Author = "Jane Smith";
        
        Console.WriteLine("\nCloned and Modified Document:");
        Console.WriteLine(clonedDoc);
        Console.WriteLine();
    }

    private static void DemonstrateShallowVsDeepCloning()
    {
        Console.WriteLine("2. Shallow vs Deep Cloning Demonstration:");
        Console.WriteLine(new string('=', 50));
        
        var original = new SpreadsheetDocument("Budget 2024");
        original.SetCellValue("A1", "Revenue");
        original.SetCellValue("B1", "100000");
        original.Metadata.Author = "Finance Team";
        original.Metadata.Tags.Add("budget");
        original.Metadata.CustomProperties["Department"] = "Finance";
        
        Console.WriteLine("Original Spreadsheet:");
        Console.WriteLine($"Author: {original.Metadata.Author}");
        Console.WriteLine($"Tags: {string.Join(", ", original.Metadata.Tags)}");
        Console.WriteLine($"Cell A1: {original.GetCellValue("A1")}");
        Console.WriteLine($"Custom Props: {string.Join(", ", original.Metadata.CustomProperties.Select(kv => $"{kv.Key}={kv.Value}"))}");
        
        // Shallow clone
        var shallowClone = (SpreadsheetDocument)original.Clone();
        shallowClone.Title = "Shallow Clone";
        
        // Deep clone
        var deepClone = (SpreadsheetDocument)original.DeepClone();
        deepClone.Title = "Deep Clone";
        
        // Modify original's metadata (affects shallow clone but not deep clone)
        original.Metadata.Tags.Add("modified");
        original.Metadata.CustomProperties["Status"] = "Updated";
        
        Console.WriteLine("\nAfter modifying original metadata:");
        Console.WriteLine($"Original Tags: {string.Join(", ", original.Metadata.Tags)}");
        Console.WriteLine($"Shallow Clone Tags: {string.Join(", ", shallowClone.Metadata.Tags)}"); // Same reference!
        Console.WriteLine($"Deep Clone Tags: {string.Join(", ", deepClone.Metadata.Tags)}"); // Independent copy!
        
        Console.WriteLine($"Original Custom Props: {string.Join(", ", original.Metadata.CustomProperties.Select(kv => $"{kv.Key}={kv.Value}"))}");
        Console.WriteLine($"Shallow Clone Custom Props: {string.Join(", ", shallowClone.Metadata.CustomProperties.Select(kv => $"{kv.Key}={kv.Value}"))}");
        Console.WriteLine($"Deep Clone Custom Props: {string.Join(", ", deepClone.Metadata.CustomProperties.Select(kv => $"{kv.Key}={kv.Value}"))}");
        Console.WriteLine();
    }

    private static void DemonstratePrototypeRegistry()
    {
        Console.WriteLine("3. Prototype Registry Usage:");
        Console.WriteLine(new string('=', 50));
        
        var registry = new DocumentRegistry();
        
        Console.WriteLine("Available document templates:");
        foreach (var prototype in registry.GetAvailablePrototypes())
        {
            Console.WriteLine($"  - {prototype}");
        }
        Console.WriteLine();
        
        // Create documents from templates
        var businessLetter = registry.CreateDocumentDeepCopy("business-letter");
        if (businessLetter is TextDocument letter)
        {
            letter.Title = "Customer Inquiry Response";
            letter.Content = letter.Content.Replace("[Recipient]", "Mr. Johnson")
                                         .Replace("[Your message here]", "Thank you for your inquiry about our services.")
                                         .Replace("[Your name]", "Customer Service Team");
            letter.Metadata.Author = "Support Team";
            
            Console.WriteLine("Business Letter created from template:");
            Console.WriteLine($"Title: {letter.Title}");
            Console.WriteLine($"Content Preview: {(letter.Content.Length > 100 ? letter.Content[..100] + "..." : letter.Content)}");
            Console.WriteLine();
        }
        
        var presentation = registry.CreateDocumentDeepCopy("project-presentation");
        if (presentation is PresentationDocument pres)
        {
            pres.Title = "Q4 Marketing Campaign";
            pres.Slides[1].Title = "Campaign Overview";
            pres.Slides[1].Content = "Launch new product line targeting millennials";
            
            Console.WriteLine("Presentation created from template:");
            Console.WriteLine($"Title: {pres.Title}");
            Console.WriteLine($"Slides: {pres.Slides.Count}");
            Console.WriteLine($"First slide: {pres.Slides[1].Title}");
            Console.WriteLine();
        }
    }

    private static void DemonstratePerformanceBenefits()
    {
        Console.WriteLine("4. Performance Benefits Demonstration:");
        Console.WriteLine(new string('=', 50));
        
        // Create a complex document with lots of setup
        var complexDoc = new PresentationDocument("Complex Presentation");
        for (int i = 0; i < 20; i++)
        {
            complexDoc.AddSlide($"Slide {i + 1}", $"Content for slide {i + 1}");
        }
        complexDoc.Metadata.Tags.AddRange(new[] { "complex", "large", "presentation" });
        complexDoc.Metadata.CustomProperties["CreationTime"] = DateTime.Now.ToString();
        complexDoc.SetTheme("Corporate");
        complexDoc.HasAnimations = true;
        complexDoc.HasSpeakerNotes = true;
        
        // Measure time for creating from scratch vs cloning
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        
        // Create 100 similar documents from scratch
        var documentsFromScratch = new List<PresentationDocument>();
        for (int i = 0; i < 100; i++)
        {
            var newDoc = new PresentationDocument($"Presentation {i}");
            for (int j = 0; j < 20; j++)
            {
                newDoc.AddSlide($"Slide {j + 1}", $"Content for slide {j + 1}");
            }
            newDoc.Metadata.Tags.AddRange(new[] { "complex", "large", "presentation" });
            newDoc.SetTheme("Corporate");
            newDoc.HasAnimations = true;
            newDoc.HasSpeakerNotes = true;
            documentsFromScratch.Add(newDoc);
        }
        
        var timeFromScratch = stopwatch.ElapsedMilliseconds;
        stopwatch.Restart();
        
        // Create 100 similar documents by cloning
        var documentsFromCloning = new List<PresentationDocument>();
        for (int i = 0; i < 100; i++)
        {
            var clonedDoc = (PresentationDocument)complexDoc.DeepClone();
            clonedDoc.Title = $"Cloned Presentation {i}";
            documentsFromCloning.Add(clonedDoc);
        }
        
        var timeFromCloning = stopwatch.ElapsedMilliseconds;
        stopwatch.Stop();
        
        Console.WriteLine($"Creating 100 complex documents from scratch: {timeFromScratch} ms");
        Console.WriteLine($"Creating 100 complex documents by cloning: {timeFromCloning} ms");
        Console.WriteLine($"Performance improvement: {(double)timeFromScratch / timeFromCloning:F1}x faster");
        Console.WriteLine($"Time saved: {timeFromScratch - timeFromCloning} ms");
        Console.WriteLine();
    }
}
