namespace Patterns_Example.Creational_Design_Patterns.Prototype;

/// <summary>
/// Quick test to verify the fix for the ArgumentOutOfRangeException
/// </summary>
public static class QuickTest
{
    public static void TestStringOperations()
    {
        Console.WriteLine("Testing string operations...");
        
        // Test with short string
        var shortString = "Hello";
        var shortPreview = shortString.Length > 100 ? shortString[..100] + "..." : shortString;
        Console.WriteLine($"Short string preview: {shortPreview}");
        
        // Test with long string
        var longString = "This is a very long string that contains more than 100 characters and should be truncated properly when we try to show a preview of it.";
        var longPreview = longString.Length > 100 ? longString[..100] + "..." : longString;
        Console.WriteLine($"Long string preview: {longPreview}");
        
        // Test business letter template
        var registry = new DocumentRegistry();
        var businessLetter = registry.CreateDocumentDeepCopy("business-letter");
        if (businessLetter is TextDocument letter)
        {
            Console.WriteLine($"Business letter content length: {letter.Content.Length}");
            var preview = letter.Content.Length > 100 ? letter.Content[..100] + "..." : letter.Content;
            Console.WriteLine($"Business letter preview: {preview}");
        }
        
        Console.WriteLine("String operations test completed successfully!");
    }
}
