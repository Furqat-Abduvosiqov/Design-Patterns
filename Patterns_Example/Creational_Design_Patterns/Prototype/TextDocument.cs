namespace Patterns_Example.Creational_Design_Patterns.Prototype;

/// <summary>
/// Concrete implementation of a text document.
/// Demonstrates prototype pattern with specific document type.
/// </summary>
public class TextDocument : Document
{
    public override string DocumentType => "Text Document";
    public string FontFamily { get; set; } = "Arial";
    public int FontSize { get; set; } = 12;
    public bool IsBold { get; set; }
    public bool IsItalic { get; set; }
    public string TextColor { get; set; } = "Black";
    public int WordCount => Content.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;

    public TextDocument() : base() { }

    public TextDocument(string title, string content) : base(title, content) { }

    public override Document DeepClone()
    {
        var clone = new TextDocument
        {
            Id = Guid.NewGuid().ToString(),
            Title = Title,
            Content = Content,
            Metadata = Metadata.DeepClone(),
            Attachments = new List<string>(Attachments),
            FontFamily = FontFamily,
            FontSize = FontSize,
            IsBold = IsBold,
            IsItalic = IsItalic,
            TextColor = TextColor
        };
        
        return clone;
    }

    public void ApplyFormatting(string fontFamily, int fontSize, bool bold = false, bool italic = false, string color = "Black")
    {
        FontFamily = fontFamily;
        FontSize = fontSize;
        IsBold = bold;
        IsItalic = italic;
        TextColor = color;
        UpdateLastModified();
    }

    public override string ToString()
    {
        var baseStr = base.ToString();
        var formatting = $"""
            Formatting:
              Font: {FontFamily}, {FontSize}pt
              Style: {(IsBold ? "Bold" : "Normal")}{(IsItalic ? ", Italic" : "")}
              Color: {TextColor}
              Word Count: {WordCount}
            """;
        
        return baseStr + formatting;
    }
}
