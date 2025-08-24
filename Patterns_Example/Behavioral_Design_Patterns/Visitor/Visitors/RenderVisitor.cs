using Patterns_Example.Behavioral_Design_Patterns.Visitor.Abstractions;
using Patterns_Example.Behavioral_Design_Patterns.Visitor.Elements;

namespace Patterns_Example.Behavioral_Design_Patterns.Visitor.Visitors;

public class RenderVisitor : IDocumentVisitor
{
    public void Visit(TextElement text)
    {
        Console.WriteLine($"Rendering Text: {text.Content}");
    }

    public void Visit(ImageElement image)
    {
        Console.WriteLine($"Rendering Image: {image.Url} (alt: {image.AltText})");
    }

    public void Visit(TableElement table)
    {
        Console.WriteLine("Rendering Table:");
        Console.WriteLine(string.Join(" | ", table.Headers));
        foreach (var row in table.Rows)
        {
            Console.WriteLine(string.Join(" | ", row));
        }
    }
}

