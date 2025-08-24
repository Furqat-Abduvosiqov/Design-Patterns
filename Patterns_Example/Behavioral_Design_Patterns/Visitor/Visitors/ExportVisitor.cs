using Patterns_Example.Behavioral_Design_Patterns.Visitor.Abstractions;
using Patterns_Example.Behavioral_Design_Patterns.Visitor.Elements;

namespace Patterns_Example.Behavioral_Design_Patterns.Visitor.Visitors;

public class ExportVisitor : IDocumentVisitor
{
    public void Visit(TextElement text)
    {
        Console.WriteLine($"Exporting Text to Markdown: {text.Content}");
    }

    public void Visit(ImageElement image)
    {
        Console.WriteLine($"Exporting Image to Markdown: ![{image.AltText}]({image.Url})");
    }

    public void Visit(TableElement table)
    {
        Console.WriteLine("Exporting Table to Markdown:");
        Console.WriteLine("| " + string.Join(" | ", table.Headers) + " |");
        Console.WriteLine("|" + string.Join("|", table.Headers.Select(_ => "---")) + "|");
        foreach (var row in table.Rows)
        {
            Console.WriteLine("| " + string.Join(" | ", row) + " |");
        }
    }
}

