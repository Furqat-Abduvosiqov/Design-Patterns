using Patterns_Example.Behavioral_Design_Patterns.Visitor.Abstractions;
using Patterns_Example.Behavioral_Design_Patterns.Visitor.Elements;

namespace Patterns_Example.Behavioral_Design_Patterns.Visitor.Visitors;

public class SpellCheckVisitor : IDocumentVisitor
{
    public void Visit(TextElement text)
    {
        Console.WriteLine($"Spell-checking Text: {text.Content}");
        if (text.Content.Contains("teh"))
            Console.WriteLine("Found typo: 'teh' should be 'the'.");
        else
            Console.WriteLine("No spelling errors found.");
    }

    public void Visit(ImageElement image)
    {
        Console.WriteLine($"Spell-checking Image Alt Text: {image.AltText}");
        if (string.IsNullOrWhiteSpace(image.AltText))
            Console.WriteLine("Warning: Alt text is missing.");
        else
            Console.WriteLine("Alt text looks good.");
    }

    public void Visit(TableElement table)
    {
        Console.WriteLine("Spell-checking Table Headers...");
        foreach (var header in table.Headers)
        {
            if (header.Contains("teh"))
                Console.WriteLine($"Found typo in header: '{header}'");
        }
        Console.WriteLine("Table spell-check complete.");
    }
}

