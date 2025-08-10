namespace Patterns_Example.Structural_Design_Patterns.Composite;

/// <summary>
/// Leaf component in the Composite pattern.
/// Represents a file in the file system that cannot contain other components.
/// </summary>
public class File : IFileSystemComponent
{
    private readonly List<string> _content;
    
    public string Name { get; private set; }
    public string Path { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public DateTime ModifiedDate { get; private set; }
    public long Size { get; private set; }
    public FileSystemPermissions Permissions { get; private set; }
    public string Extension { get; private set; }
    public string MimeType { get; private set; }

    public File(string name, string path, long size = 0)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Path = path ?? throw new ArgumentNullException(nameof(path));
        Size = size;
        CreatedDate = DateTime.Now;
        ModifiedDate = DateTime.Now;
        Permissions = new FileSystemPermissions();
        Extension = System.IO.Path.GetExtension(name);
        MimeType = GetMimeType(Extension);
        _content = new List<string>();
        
        // Set execute permission for executable files
        if (IsExecutableFile(Extension))
        {
            Permissions.CanExecute = true;
        }
    }

    public void Display(int depth = 0)
    {
        var indent = new string(' ', depth * 2);
        var sizeStr = FormatFileSize(Size);
        var permStr = Permissions.ToString();
        
        Console.WriteLine($"{indent}📄 {Name} ({sizeStr}) [{permStr}]");
        
        if (depth == 0) // Show additional details for root display
        {
            Console.WriteLine($"{indent}   Path: {Path}");
            Console.WriteLine($"{indent}   Type: {MimeType}");
            Console.WriteLine($"{indent}   Modified: {ModifiedDate:yyyy-MM-dd HH:mm:ss}");
        }
    }

    public FileSystemInfo GetInfo()
    {
        return new FileSystemInfo
        {
            Name = Name,
            FullPath = Path,
            Type = FileSystemType.File,
            Size = Size,
            ItemCount = 0,
            CreatedDate = CreatedDate,
            ModifiedDate = ModifiedDate,
            Permissions = Permissions,
            Extension = Extension,
            MimeType = MimeType
        };
    }

    public bool HasPermission(FileSystemOperation operation)
    {
        return operation switch
        {
            FileSystemOperation.Read => Permissions.CanRead,
            FileSystemOperation.Write => Permissions.CanWrite,
            FileSystemOperation.Execute => Permissions.CanExecute,
            FileSystemOperation.Delete => Permissions.CanDelete,
            FileSystemOperation.Modify => Permissions.CanWrite,
            _ => false
        };
    }

    public void SetPermissions(FileSystemPermissions permissions)
    {
        Permissions = permissions ?? throw new ArgumentNullException(nameof(permissions));
        ModifiedDate = DateTime.Now;
    }

    public List<IFileSystemComponent> Search(string pattern, SearchOptions options)
    {
        var results = new List<IFileSystemComponent>();
        
        // Check if this file matches the pattern
        var nameMatches = options.CaseSensitive 
            ? Name.Contains(pattern) 
            : Name.Contains(pattern, StringComparison.OrdinalIgnoreCase);
            
        var contentMatches = false;
        if (options.SearchContent && HasPermission(FileSystemOperation.Read))
        {
            contentMatches = _content.Any(line => 
                options.CaseSensitive 
                    ? line.Contains(pattern) 
                    : line.Contains(pattern, StringComparison.OrdinalIgnoreCase));
        }
        
        // Check date filters
        var dateMatches = true;
        if (options.ModifiedAfter.HasValue && ModifiedDate < options.ModifiedAfter.Value)
            dateMatches = false;
        if (options.ModifiedBefore.HasValue && ModifiedDate > options.ModifiedBefore.Value)
            dateMatches = false;
            
        // Check size filters
        var sizeMatches = true;
        if (options.MinSize.HasValue && Size < options.MinSize.Value)
            sizeMatches = false;
        if (options.MaxSize.HasValue && Size > options.MaxSize.Value)
            sizeMatches = false;
        
        if ((nameMatches || contentMatches) && dateMatches && sizeMatches)
        {
            results.Add(this);
        }
        
        return results;
    }

    public List<IFileSystemComponent> FindByType(FileSystemType type)
    {
        return type == FileSystemType.File ? new List<IFileSystemComponent> { this } : new List<IFileSystemComponent>();
    }

    public List<IFileSystemComponent> FindBySize(long minSize, long maxSize)
    {
        return Size >= minSize && Size <= maxSize 
            ? new List<IFileSystemComponent> { this } 
            : new List<IFileSystemComponent>();
    }

    // Composite operations - Files cannot contain other components
    public void Add(IFileSystemComponent component)
    {
        throw new InvalidOperationException("Cannot add components to a file. Files are leaf nodes.");
    }

    public void Remove(IFileSystemComponent component)
    {
        throw new InvalidOperationException("Cannot remove components from a file. Files are leaf nodes.");
    }

    public IFileSystemComponent? GetChild(string name)
    {
        return null; // Files have no children
    }

    public IEnumerable<IFileSystemComponent> GetChildren()
    {
        return Enumerable.Empty<IFileSystemComponent>(); // Files have no children
    }

    public void Accept(IFileSystemVisitor visitor)
    {
        visitor.VisitFile(this);
    }

    // File-specific operations
    public void WriteContent(string content)
    {
        if (!HasPermission(FileSystemOperation.Write))
            throw new UnauthorizedAccessException("No write permission for this file.");
            
        _content.Clear();
        _content.AddRange(content.Split('\n'));
        Size = content.Length;
        ModifiedDate = DateTime.Now;
    }

    public void AppendContent(string content)
    {
        if (!HasPermission(FileSystemOperation.Write))
            throw new UnauthorizedAccessException("No write permission for this file.");
            
        _content.AddRange(content.Split('\n'));
        Size += content.Length;
        ModifiedDate = DateTime.Now;
    }

    public string ReadContent()
    {
        if (!HasPermission(FileSystemOperation.Read))
            throw new UnauthorizedAccessException("No read permission for this file.");
            
        return string.Join('\n', _content);
    }

    public List<string> ReadLines()
    {
        if (!HasPermission(FileSystemOperation.Read))
            throw new UnauthorizedAccessException("No read permission for this file.");
            
        return new List<string>(_content);
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

    private static string GetMimeType(string extension)
    {
        return extension.ToLower() switch
        {
            ".txt" => "text/plain",
            ".html" => "text/html",
            ".css" => "text/css",
            ".js" => "application/javascript",
            ".json" => "application/json",
            ".xml" => "application/xml",
            ".pdf" => "application/pdf",
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".mp4" => "video/mp4",
            ".mp3" => "audio/mpeg",
            ".zip" => "application/zip",
            ".exe" => "application/x-executable",
            ".dll" => "application/x-msdownload",
            _ => "application/octet-stream"
        };
    }

    private static bool IsExecutableFile(string extension)
    {
        var executableExtensions = new[] { ".exe", ".bat", ".cmd", ".sh", ".ps1", ".com" };
        return executableExtensions.Contains(extension.ToLower());
    }

    public override string ToString()
    {
        return $"File: {Name} ({FormatFileSize(Size)})";
    }
}
