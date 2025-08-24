using Patterns_Example.Behavioral_Design_Patterns.Visitor.Abstractions;

namespace Patterns_Example.Behavioral_Design_Patterns.Visitor.Elements;

public class TextElement : IDocumentElement
{
    public string Content { get; set; }
    public TextElement(string content) => Content = content;
    public void Accept(IDocumentVisitor visitor) => visitor.Visit(this);
}

