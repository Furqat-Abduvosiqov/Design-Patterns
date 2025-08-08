namespace Patterns_Example.Creational_Design_Patterns.Prototype;

/// <summary>
/// Represents security settings for a document.
/// Demonstrates cloning of nested objects.
/// </summary>
public class DocumentSecurity : IPrototype<DocumentSecurity>
{
    public bool IsEncrypted { get; set; }
    public string EncryptionLevel { get; set; } = "None";
    public List<string> AllowedUsers { get; set; } = new();
    public List<string> Permissions { get; set; } = new();
    public bool RequirePassword { get; set; }
    public DateTime? ExpirationDate { get; set; }

    public DocumentSecurity Clone()
    {
        // Shallow copy
        return (DocumentSecurity)MemberwiseClone();
    }

    public DocumentSecurity DeepClone()
    {
        // Deep copy
        return new DocumentSecurity
        {
            IsEncrypted = IsEncrypted,
            EncryptionLevel = EncryptionLevel,
            AllowedUsers = new List<string>(AllowedUsers),
            Permissions = new List<string>(Permissions),
            RequirePassword = RequirePassword,
            ExpirationDate = ExpirationDate
        };
    }

    public override string ToString()
    {
        var usersStr = AllowedUsers.Count > 0 ? string.Join(", ", AllowedUsers) : "All";
        var permsStr = Permissions.Count > 0 ? string.Join(", ", Permissions) : "Read";
        var expStr = ExpirationDate?.ToString("yyyy-MM-dd") ?? "Never";
        
        return $"Encrypted: {IsEncrypted}, Level: {EncryptionLevel}, Users: {usersStr}, Permissions: {permsStr}, Expires: {expStr}";
    }
}
