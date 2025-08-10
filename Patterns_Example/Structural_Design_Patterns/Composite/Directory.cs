namespace Patterns_Example.Structural_Design_Patterns.Composite;

/// <summary>
/// Composite component in the Composite pattern.
/// Represents a directory that can contain other files and directories.
/// </summary>
public class Directory : IFileSystemComponent
{
    private readonly List<IFileSystemComponent> _children;
    
    public string Name { get; private set; }
    public string Path { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public DateTime ModifiedDate { get; private set; }
    public FileSystemPermissions Permissions { get; private set; }

    public long Size => CalculateSize();

    public Directory(string name, string path)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Path = path ?? throw new ArgumentNullException(nameof(path));
        CreatedDate = DateTime.Now;
        ModifiedDate = DateTime.Now;
        Permissions = new FileSystemPermissions { CanExecute = true }; // Directories need execute permission to be traversed
        _children = new List<IFileSystemComponent>();
    }

    public void Display(int depth = 0)
    {
        var indent = new string(' ', depth * 2);
        var sizeStr = FormatFileSize(Size);
        var itemCount = _children.Count;
        var permStr = Permissions.ToString();
        
        Console.WriteLine($"{indent}📁 {Name}/ ({itemCount} items, {sizeStr}) [{permStr}]");
        
        if (depth == 0) // Show additional details for root display
        {
            Console.WriteLine($"{indent}   Path: {Path}");
            Console.WriteLine($"{indent}   Modified: {ModifiedDate:yyyy-MM-dd HH:mm:ss}");
        }
        
        // Display children
        foreach (var child in _children.OrderBy(c => c.Name))
        {
            child.Display(depth + 1);
        }
    }

    public FileSystemInfo GetInfo()
    {
        return new FileSystemInfo
        {
            Name = Name,
            FullPath = Path,
            Type = FileSystemType.Directory,
            Size = Size,
            ItemCount = _children.Count,
            CreatedDate = CreatedDate,
            ModifiedDate = ModifiedDate,
            Permissions = Permissions
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
            FileSystemOperation.Create => Permissions.CanWrite,
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
        
        // Check if this directory matches the pattern
        var nameMatches = options.CaseSensitive 
            ? Name.Contains(pattern) 
            : Name.Contains(pattern, StringComparison.OrdinalIgnoreCase);
            
        if (nameMatches)
        {
            results.Add(this);
        }
        
        // Search children if we have permission and haven't exceeded max depth
        if (HasPermission(FileSystemOperation.Read) && options.MaxDepth > 0)
        {
            var childOptions = new SearchOptions
            {
                IncludeHidden = options.IncludeHidden,
                CaseSensitive = options.CaseSensitive,
                SearchContent = options.SearchContent,
                MaxDepth = options.MaxDepth - 1,
                ModifiedAfter = options.ModifiedAfter,
                ModifiedBefore = options.ModifiedBefore,
                MinSize = options.MinSize,
                MaxSize = options.MaxSize
            };
            
            foreach (var child in _children)
            {
                results.AddRange(child.Search(pattern, childOptions));
            }
        }
        
        return results;
    }

    public List<IFileSystemComponent> FindByType(FileSystemType type)
    {
        var results = new List<IFileSystemComponent>();
        
        if (type == FileSystemType.Directory)
        {
            results.Add(this);
        }
        
        // Search children
        if (HasPermission(FileSystemOperation.Read))
        {
            foreach (var child in _children)
            {
                results.AddRange(child.FindByType(type));
            }
        }
        
        return results;
    }

    public List<IFileSystemComponent> FindBySize(long minSize, long maxSize)
    {
        var results = new List<IFileSystemComponent>();
        
        // Check if this directory matches the size criteria
        if (Size >= minSize && Size <= maxSize)
        {
            results.Add(this);
        }
        
        // Search children
        if (HasPermission(FileSystemOperation.Read))
        {
            foreach (var child in _children)
            {
                results.AddRange(child.FindBySize(minSize, maxSize));
            }
        }
        
        return results;
    }

    // Composite operations
    public void Add(IFileSystemComponent component)
    {
        if (!HasPermission(FileSystemOperation.Create))
            throw new UnauthorizedAccessException("No create permission for this directory.");
            
        if (component == null)
            throw new ArgumentNullException(nameof(component));
            
        if (_children.Any(c => c.Name.Equals(component.Name, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException($"A component with the name '{component.Name}' already exists in this directory.");
            
        _children.Add(component);
        ModifiedDate = DateTime.Now;
    }

    public void Remove(IFileSystemComponent component)
    {
        if (!HasPermission(FileSystemOperation.Delete))
            throw new UnauthorizedAccessException("No delete permission for this directory.");
            
        if (component == null)
            throw new ArgumentNullException(nameof(component));
            
        if (_children.Remove(component))
        {
            ModifiedDate = DateTime.Now;
        }
        else
        {
            throw new InvalidOperationException($"Component '{component.Name}' not found in this directory.");
        }
    }

    public IFileSystemComponent? GetChild(string name)
    {
        if (!HasPermission(FileSystemOperation.Read))
            return null;
            
        return _children.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public IEnumerable<IFileSystemComponent> GetChildren()
    {
        if (!HasPermission(FileSystemOperation.Read))
            return Enumerable.Empty<IFileSystemComponent>();
            
        return _children.AsReadOnly();
    }

    public void Accept(IFileSystemVisitor visitor)
    {
        visitor.VisitDirectory(this);
        
        // Visit children
        foreach (var child in _children)
        {
            child.Accept(visitor);
        }
    }

    // Directory-specific operations
    public void CreateFile(string fileName, string content = "")
    {
        if (!HasPermission(FileSystemOperation.Create))
            throw new UnauthorizedAccessException("No create permission for this directory.");
            
        var filePath = System.IO.Path.Combine(Path, fileName);
        var file = new File(fileName, filePath, content.Length);
        
        if (!string.IsNullOrEmpty(content))
        {
            file.WriteContent(content);
        }
        
        Add(file);
    }

    public Directory CreateSubdirectory(string directoryName)
    {
        if (!HasPermission(FileSystemOperation.Create))
            throw new UnauthorizedAccessException("No create permission for this directory.");
            
        var dirPath = System.IO.Path.Combine(Path, directoryName);
        var directory = new Directory(directoryName, dirPath);
        
        Add(directory);
        return directory;
    }

    public void DeleteChild(string name)
    {
        var child = GetChild(name);
        if (child != null)
        {
            Remove(child);
        }
        else
        {
            throw new FileNotFoundException($"Component '{name}' not found in directory '{Name}'.");
        }
    }

    public bool IsEmpty()
    {
        return _children.Count == 0;
    }

    public int GetTotalFileCount()
    {
        var count = 0;
        
        foreach (var child in _children)
        {
            if (child is File)
            {
                count++;
            }
            else if (child is Directory dir)
            {
                count += dir.GetTotalFileCount();
            }
        }
        
        return count;
    }

    public int GetTotalDirectoryCount()
    {
        var count = 1; // Count this directory
        
        foreach (var child in _children)
        {
            if (child is Directory dir)
            {
                count += dir.GetTotalDirectoryCount();
            }
        }
        
        return count;
    }

    private long CalculateSize()
    {
        return _children.Sum(child => child.Size);
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

    public override string ToString()
    {
        return $"Directory: {Name}/ ({_children.Count} items, {FormatFileSize(Size)})";
    }
}
