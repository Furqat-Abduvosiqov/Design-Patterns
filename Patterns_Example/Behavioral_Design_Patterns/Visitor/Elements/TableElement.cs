using Patterns_Example.Behavioral_Design_Patterns.Visitor.Abstractions;

namespace Patterns_Example.Behavioral_Design_Patterns.Visitor.Elements;

public class TableElement : IDocumentElement
{
    public List<string> Headers { get; set; }
    public List<List<string>> Rows { get; set; }
    public TableElement(List<string> headers, List<List<string>> rows)
    {
        Headers = headers;
        Rows = rows;
    }
    public void Accept(IDocumentVisitor visitor) => visitor.Visit(this);
}

