using Patterns_Example.Behavioral_Design_Patterns.Visitor.Abstractions;

namespace Patterns_Example.Behavioral_Design_Patterns.Visitor.Elements;

public class ImageElement : IDocumentElement
{
    public string Url { get; set; }
    public string AltText { get; set; }
    public ImageElement(string url, string altText)
    {
        Url = url;
        AltText = altText;
    }
    public void Accept(IDocumentVisitor visitor) => visitor.Visit(this);
}

