namespace Patterns_Example.Creational_Design_Patterns.Prototype;

/// <summary>
/// Abstract base class for all document types.
/// Implements the Prototype pattern for document cloning.
/// </summary>
public abstract class Document : IPrototype<Document>
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DocumentMetadata Metadata { get; set; } = new();
    public List<string> Attachments { get; set; } = new();
    public abstract string DocumentType { get; }

    protected Document()
    {
        Metadata.CreatedDate = DateTime.Now;
        Metadata.LastModified = DateTime.Now;
    }

    protected Document(string title, string content)
    {
        Title = title;
        Content = content;
        Metadata.CreatedDate = DateTime.Now;
        Metadata.LastModified = DateTime.Now;
    }

    public virtual Document Clone()
    {
        // Shallow copy - creates a new document but shares references to complex objects
        var clone = (Document)MemberwiseClone();
        clone.Id = Guid.NewGuid().ToString(); // Generate new ID for clone
        return clone;
    }

    public virtual Document DeepClone()
    {
        // Deep copy - creates completely independent copy
        var clone = (Document)MemberwiseClone();
        clone.Id = Guid.NewGuid().ToString(); // Generate new ID for clone
        clone.Metadata = Metadata.DeepClone(); // Deep clone metadata
        clone.Attachments = new List<string>(Attachments); // Clone attachments list
        return clone;
    }

    public virtual void UpdateLastModified()
    {
        Metadata.LastModified = DateTime.Now;
    }

    public override string ToString()
    {
        var attachmentsStr = Attachments.Count > 0 ? string.Join(", ", Attachments) : "None";
        
        return $"""
            Document [{DocumentType}]:
            ========================
            ID: {Id}
            Title: {Title}
            Content: {(Content.Length > 50 ? Content[..50] + "..." : Content)}
            Attachments: {attachmentsStr}
            {Metadata}
            """;
    }
}
