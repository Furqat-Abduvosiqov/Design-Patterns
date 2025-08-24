using Patterns_Example.Behavioral_Design_Patterns.Visitor.Elements;

namespace Patterns_Example.Behavioral_Design_Patterns.Visitor.Abstractions;

public interface IDocumentVisitor
{
    void Visit(TextElement text);
    void Visit(ImageElement image);
    void Visit(TableElement table);
}

