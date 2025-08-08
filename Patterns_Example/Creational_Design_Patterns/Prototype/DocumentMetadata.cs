namespace Patterns_Example.Creational_Design_Patterns.Prototype;

/// <summary>
/// Represents metadata for a document. This class demonstrates deep cloning
/// of complex nested objects.
/// </summary>
public class DocumentMetadata : IPrototype<DocumentMetadata>
{
    public string Author { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime LastModified { get; set; }
    public string Version { get; set; } = "1.0";
    public List<string> Tags { get; set; } = new();
    public Dictionary<string, string> CustomProperties { get; set; } = new();
    public DocumentSecurity Security { get; set; } = new();

    public DocumentMetadata()
    {
        CreatedDate = DateTime.Now;
        LastModified = DateTime.Now;
    }

    public DocumentMetadata Clone()
    {
        // Shallow copy - collections and objects are shared
        return (DocumentMetadata)MemberwiseClone();
    }

    public DocumentMetadata DeepClone()
    {
        // Deep copy - all nested objects and collections are cloned
        var clone = new DocumentMetadata
        {
            Author = Author,
            CreatedDate = CreatedDate,
            LastModified = LastModified,
            Version = Version,
            Tags = new List<string>(Tags), // Create new list with same elements
            CustomProperties = new Dictionary<string, string>(CustomProperties), // Create new dictionary
            Security = Security.DeepClone() // Deep clone the security object
        };
        
        return clone;
    }

    public override string ToString()
    {
        var tagsStr = Tags.Count > 0 ? string.Join(", ", Tags) : "None";
        var propsStr = CustomProperties.Count > 0 
            ? string.Join(", ", CustomProperties.Select(kv => $"{kv.Key}={kv.Value}"))
            : "None";
            
        return $"""
            Metadata:
              Author: {Author}
              Created: {CreatedDate:yyyy-MM-dd HH:mm}
              Modified: {LastModified:yyyy-MM-dd HH:mm}
              Version: {Version}
              Tags: {tagsStr}
              Properties: {propsStr}
              Security: {Security}
            """;
    }
}
