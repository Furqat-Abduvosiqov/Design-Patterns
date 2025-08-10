namespace Patterns_Example.Structural_Design_Patterns.Composite;

/// <summary>
/// Composite component representing an archive file (like ZIP, TAR, etc.).
/// This demonstrates how the Composite pattern can handle special cases
/// where a "file" can also contain other components.
/// </summary>
public class Archive : IFileSystemComponent
{
    private readonly List<IFileSystemComponent> _contents;
    private long _compressedSize;
    
    public string Name { get; private set; }
    public string Path { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public DateTime ModifiedDate { get; private set; }
    public FileSystemPermissions Permissions { get; private set; }
    public string ArchiveType { get; private set; }
    public double CompressionRatio { get; private set; }

    // Size returns the compressed size of the archive
    public long Size => _compressedSize;
    
    // UncompressedSize returns the total size of all contents
    public long UncompressedSize => _contents.Sum(c => c.Size);

    public Archive(string name, string path, string archiveType = "zip")
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Path = path ?? throw new ArgumentNullException(nameof(path));
        ArchiveType = archiveType.ToLower();
        CreatedDate = DateTime.Now;
        ModifiedDate = DateTime.Now;
        Permissions = new FileSystemPermissions();
        _contents = new List<IFileSystemComponent>();
        _compressedSize = 0;
        CompressionRatio = 0.0;
    }

    public void Display(int depth = 0)
    {
        var indent = new string(' ', depth * 2);
        var compressedSizeStr = FormatFileSize(_compressedSize);
        var uncompressedSizeStr = FormatFileSize(UncompressedSize);
        var compressionStr = CompressionRatio > 0 ? $" ({CompressionRatio:P1} compression)" : "";
        var permStr = Permissions.ToString();
        
        Console.WriteLine($"{indent}🗜️  {Name} [{ArchiveType.ToUpper()}] ({compressedSizeStr} compressed, {uncompressedSizeStr} uncompressed{compressionStr}) [{permStr}]");
        
        if (depth == 0) // Show additional details for root display
        {
            Console.WriteLine($"{indent}   Path: {Path}");
            Console.WriteLine($"{indent}   Type: {ArchiveType.ToUpper()} Archive");
            Console.WriteLine($"{indent}   Contents: {_contents.Count} items");
            Console.WriteLine($"{indent}   Modified: {ModifiedDate:yyyy-MM-dd HH:mm:ss}");
        }
        
        // Display contents if we have read permission
        if (HasPermission(FileSystemOperation.Read))
        {
            Console.WriteLine($"{indent}  📦 Archive Contents:");
            foreach (var item in _contents.OrderBy(c => c.Name))
            {
                item.Display(depth + 2);
            }
        }
        else
        {
            Console.WriteLine($"{indent}  📦 Archive Contents: [Access Denied]");
        }
    }

    public FileSystemInfo GetInfo()
    {
        return new FileSystemInfo
        {
            Name = Name,
            FullPath = Path,
            Type = FileSystemType.Archive,
            Size = _compressedSize,
            ItemCount = _contents.Count,
            CreatedDate = CreatedDate,
            ModifiedDate = ModifiedDate,
            Permissions = Permissions,
            Extension = System.IO.Path.GetExtension(Name),
            MimeType = GetArchiveMimeType(ArchiveType)
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
        
        // Check if this archive matches the pattern
        var nameMatches = options.CaseSensitive 
            ? Name.Contains(pattern) 
            : Name.Contains(pattern, StringComparison.OrdinalIgnoreCase);
            
        if (nameMatches)
        {
            results.Add(this);
        }
        
        // Search contents if we have permission and haven't exceeded max depth
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
            
            foreach (var item in _contents)
            {
                results.AddRange(item.Search(pattern, childOptions));
            }
        }
        
        return results;
    }

    public List<IFileSystemComponent> FindByType(FileSystemType type)
    {
        var results = new List<IFileSystemComponent>();
        
        if (type == FileSystemType.Archive)
        {
            results.Add(this);
        }
        
        // Search contents
        if (HasPermission(FileSystemOperation.Read))
        {
            foreach (var item in _contents)
            {
                results.AddRange(item.FindByType(type));
            }
        }
        
        return results;
    }

    public List<IFileSystemComponent> FindBySize(long minSize, long maxSize)
    {
        var results = new List<IFileSystemComponent>();
        
        // Check if this archive matches the size criteria (using compressed size)
        if (_compressedSize >= minSize && _compressedSize <= maxSize)
        {
            results.Add(this);
        }
        
        // Search contents
        if (HasPermission(FileSystemOperation.Read))
        {
            foreach (var item in _contents)
            {
                results.AddRange(item.FindBySize(minSize, maxSize));
            }
        }
        
        return results;
    }

    // Composite operations
    public void Add(IFileSystemComponent component)
    {
        if (!HasPermission(FileSystemOperation.Write))
            throw new UnauthorizedAccessException("No write permission for this archive.");
            
        if (component == null)
            throw new ArgumentNullException(nameof(component));
            
        if (_contents.Any(c => c.Name.Equals(component.Name, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException($"A component with the name '{component.Name}' already exists in this archive.");
            
        _contents.Add(component);
        UpdateCompressionInfo();
        ModifiedDate = DateTime.Now;
    }

    public void Remove(IFileSystemComponent component)
    {
        if (!HasPermission(FileSystemOperation.Write))
            throw new UnauthorizedAccessException("No write permission for this archive.");
            
        if (component == null)
            throw new ArgumentNullException(nameof(component));
            
        if (_contents.Remove(component))
        {
            UpdateCompressionInfo();
            ModifiedDate = DateTime.Now;
        }
        else
        {
            throw new InvalidOperationException($"Component '{component.Name}' not found in this archive.");
        }
    }

    public IFileSystemComponent? GetChild(string name)
    {
        if (!HasPermission(FileSystemOperation.Read))
            return null;
            
        return _contents.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public IEnumerable<IFileSystemComponent> GetChildren()
    {
        if (!HasPermission(FileSystemOperation.Read))
            return Enumerable.Empty<IFileSystemComponent>();
            
        return _contents.AsReadOnly();
    }

    public void Accept(IFileSystemVisitor visitor)
    {
        visitor.VisitArchive(this);
        
        // Visit contents
        if (HasPermission(FileSystemOperation.Read))
        {
            foreach (var item in _contents)
            {
                item.Accept(visitor);
            }
        }
    }

    // Archive-specific operations
    public void Extract(Directory targetDirectory)
    {
        if (!HasPermission(FileSystemOperation.Read))
            throw new UnauthorizedAccessException("No read permission for this archive.");
            
        if (!targetDirectory.HasPermission(FileSystemOperation.Create))
            throw new UnauthorizedAccessException("No create permission for target directory.");
            
        Console.WriteLine($"Extracting archive '{Name}' to '{targetDirectory.Path}'...");
        
        foreach (var item in _contents)
        {
            // In a real implementation, you would actually extract the files
            // Here we simulate by adding copies to the target directory
            Console.WriteLine($"  Extracting: {item.Name}");
            
            if (item is File file)
            {
                targetDirectory.CreateFile(file.Name, file.ReadContent());
            }
            else if (item is Directory dir)
            {
                var newDir = targetDirectory.CreateSubdirectory(dir.Name);
                // Recursively extract directory contents
                ExtractDirectory(dir, newDir);
            }
        }
        
        Console.WriteLine($"Extraction completed. {_contents.Count} items extracted.");
    }

    public void Compress(double compressionRatio = 0.7)
    {
        if (!HasPermission(FileSystemOperation.Write))
            throw new UnauthorizedAccessException("No write permission for this archive.");
            
        CompressionRatio = Math.Max(0.1, Math.Min(0.9, compressionRatio));
        UpdateCompressionInfo();
        ModifiedDate = DateTime.Now;
        
        Console.WriteLine($"Archive compressed with {CompressionRatio:P1} compression ratio.");
    }

    private void ExtractDirectory(Directory sourceDir, Directory targetDir)
    {
        foreach (var child in sourceDir.GetChildren())
        {
            if (child is File file)
            {
                targetDir.CreateFile(file.Name, file.ReadContent());
            }
            else if (child is Directory dir)
            {
                var newDir = targetDir.CreateSubdirectory(dir.Name);
                ExtractDirectory(dir, newDir);
            }
        }
    }

    private void UpdateCompressionInfo()
    {
        var uncompressedSize = UncompressedSize;
        if (CompressionRatio > 0 && uncompressedSize > 0)
        {
            _compressedSize = (long)(uncompressedSize * (1.0 - CompressionRatio));
        }
        else
        {
            _compressedSize = uncompressedSize; // No compression
        }
    }

    private static string GetArchiveMimeType(string archiveType)
    {
        return archiveType.ToLower() switch
        {
            "zip" => "application/zip",
            "tar" => "application/x-tar",
            "gz" or "gzip" => "application/gzip",
            "rar" => "application/vnd.rar",
            "7z" => "application/x-7z-compressed",
            _ => "application/octet-stream"
        };
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
        return $"Archive: {Name} [{ArchiveType.ToUpper()}] ({_contents.Count} items, {FormatFileSize(_compressedSize)} compressed)";
    }
}
