namespace Patterns_Example.Structural_Design_Patterns.Composite;

/// <summary>
/// Component interface for the Composite pattern.
/// This defines the interface for both leaf and composite objects in the file system.
/// </summary>
public interface IFileSystemComponent
{
    string Name { get; }
    string Path { get; }
    DateTime CreatedDate { get; }
    DateTime ModifiedDate { get; }
    long Size { get; }
    FileSystemPermissions Permissions { get; }
    
    // Operations that both files and directories can perform
    void Display(int depth = 0);
    FileSystemInfo GetInfo();
    bool HasPermission(FileSystemOperation operation);
    void SetPermissions(FileSystemPermissions permissions);
    
    // Search operations
    List<IFileSystemComponent> Search(string pattern, SearchOptions options);
    List<IFileSystemComponent> FindByType(FileSystemType type);
    List<IFileSystemComponent> FindBySize(long minSize, long maxSize);
    
    // Composite operations (may throw exceptions for leaf nodes)
    void Add(IFileSystemComponent component);
    void Remove(IFileSystemComponent component);
    IFileSystemComponent? GetChild(string name);
    IEnumerable<IFileSystemComponent> GetChildren();
    
    // Visitor pattern support
    void Accept(IFileSystemVisitor visitor);
}

/// <summary>
/// Information about a file system component.
/// </summary>
public class FileSystemInfo
{
    public string Name { get; set; } = string.Empty;
    public string FullPath { get; set; } = string.Empty;
    public FileSystemType Type { get; set; }
    public long Size { get; set; }
    public int ItemCount { get; set; } // For directories: number of children
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public FileSystemPermissions Permissions { get; set; } = new();
    public string Extension { get; set; } = string.Empty;
    public string MimeType { get; set; } = string.Empty;

    public override string ToString()
    {
        var sizeStr = Type == FileSystemType.Directory 
            ? $"{ItemCount} items" 
            : FormatFileSize(Size);
            
        return $"{Name} ({Type}) - {sizeStr} - Modified: {ModifiedDate:yyyy-MM-dd HH:mm}";
    }

    private static string FormatFileSize(long bytes)
    {
        string[] suffixes = { "B", "KB", "MB", "GB", "TB" };
        int counter = 0;
        decimal number = bytes;
        
        while (Math.Round(number / 1024) >= 1)
        {
            number /= 1024;
            counter++;
        }
        
        return $"{number:n1} {suffixes[counter]}";
    }
}

/// <summary>
/// File system permissions.
/// </summary>
public class FileSystemPermissions
{
    public bool CanRead { get; set; } = true;
    public bool CanWrite { get; set; } = true;
    public bool CanExecute { get; set; } = false;
    public bool CanDelete { get; set; } = true;
    public string Owner { get; set; } = "current_user";
    public string Group { get; set; } = "users";

    public override string ToString()
    {
        var perms = "";
        perms += CanRead ? "r" : "-";
        perms += CanWrite ? "w" : "-";
        perms += CanExecute ? "x" : "-";
        perms += CanDelete ? "d" : "-";
        return $"{perms} ({Owner}:{Group})";
    }
}

/// <summary>
/// Search options for file system operations.
/// </summary>
public class SearchOptions
{
    public bool IncludeHidden { get; set; } = false;
    public bool CaseSensitive { get; set; } = false;
    public bool SearchContent { get; set; } = false;
    public int MaxDepth { get; set; } = int.MaxValue;
    public DateTime? ModifiedAfter { get; set; }
    public DateTime? ModifiedBefore { get; set; }
    public long? MinSize { get; set; }
    public long? MaxSize { get; set; }
}

/// <summary>
/// Types of file system components.
/// </summary>
public enum FileSystemType
{
    File,
    Directory,
    SymbolicLink,
    Archive
}

/// <summary>
/// File system operations for permission checking.
/// </summary>
public enum FileSystemOperation
{
    Read,
    Write,
    Execute,
    Delete,
    Create,
    Modify
}

/// <summary>
/// Visitor interface for file system operations.
/// </summary>
public interface IFileSystemVisitor
{
    void VisitFile(File file);
    void VisitDirectory(Directory directory);
    void VisitArchive(Archive archive);
}
