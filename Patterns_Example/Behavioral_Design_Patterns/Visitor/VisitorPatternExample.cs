using Patterns_Example.Behavioral_Design_Patterns.Visitor.Elements;
using Patterns_Example.Behavioral_Design_Patterns.Visitor.Visitors;
using Patterns_Example.Behavioral_Design_Patterns.Visitor.Abstractions;

namespace Patterns_Example.Behavioral_Design_Patterns.Visitor;

public class VisitorPatternExample
{
    public static void RunExample()
    {
        var document = new List<IDocumentElement>
        {
            new TextElement("This is teh first paragraph."),
            new ImageElement("https://example.com/image.png", "Sample Image"),
            new TableElement(
                ["Name", "Age", "teh Score"],
                [
                    new() { "Alice", "30", "95" },
                    new() { "Bob", "25", "88" }
                ])
        };

        Console.WriteLine("--- Rendering Document ---");
        var renderVisitor = new RenderVisitor();
        foreach (var element in document)
            element.Accept(renderVisitor);
        Console.WriteLine();

        Console.WriteLine("--- Exporting Document ---");
        var exportVisitor = new ExportVisitor();
        foreach (var element in document)
            element.Accept(exportVisitor);
        Console.WriteLine();

        Console.WriteLine("--- Spell-Checking Document ---");
        var spellCheckVisitor = new SpellCheckVisitor();
        foreach (var element in document)
            element.Accept(spellCheckVisitor);
        Console.WriteLine();
    }
}

