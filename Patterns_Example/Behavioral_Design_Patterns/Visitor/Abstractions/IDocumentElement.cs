namespace Patterns_Example.Behavioral_Design_Patterns.Visitor.Abstractions;

public interface IDocumentElement
{
    void Accept(IDocumentVisitor visitor);
}

