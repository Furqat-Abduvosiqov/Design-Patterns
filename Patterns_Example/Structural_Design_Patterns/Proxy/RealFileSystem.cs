namespace Patterns_Example.Structural_Design_Patterns.Proxy;

/// <summary>
/// RealSubject that implements the actual file system operations.
/// This class performs the real work but is expensive to create and use.
/// The proxy will control access to this class.
/// </summary>
public class RealFileSystem : IFileSystem
{
    private readonly Dictionary<string, VirtualFile> _files;
    private readonly Dictionary<string, VirtualDirectory> _directories;
    private readonly Random _random;

    public RealFileSystem()
    {
        Console.WriteLine("[RealFileSystem] Initializing real file system (expensive operation)...");
        
        _files = new Dictionary<string, VirtualFile>();
        _directories = new Dictionary<string, VirtualDirectory>();
        _random = new Random();
        
        // Simulate expensive initialization
        Thread.Sleep(100);
        
        // Create some initial file system structure
        InitializeFileSystem();
        
        Console.WriteLine("[RealFileSystem] Real file system initialized successfully");
    }

    public string ReadFile(string path)
    {
        Console.WriteLine($"[RealFileSystem] Reading file: {path}");
        
        // Simulate file reading delay
        Thread.Sleep(50);
        
        if (!_files.TryGetValue(path, out var file))
        {
            throw new FileNotFoundException(path);
        }
        
        file.AccessedDate = DateTime.Now;
        Console.WriteLine($"[RealFileSystem] File read successfully: {file.Content.Length} characters");
        return file.Content;
    }

    public void WriteFile(string path, string content)
    {
        Console.WriteLine($"[RealFileSystem] Writing file: {path} ({content.Length} characters)");
        
        // Simulate file writing delay
        Thread.Sleep(30);
        
        if (_files.TryGetValue(path, out var existingFile))
        {
            existingFile.Content = content;
            existingFile.ModifiedDate = DateTime.Now;
            existingFile.AccessedDate = DateTime.Now;
            existingFile.Size = content.Length;
        }
        else
        {
            var newFile = new VirtualFile
            {
                Path = path,
                Name = System.IO.Path.GetFileName(path),
                Content = content,
                Size = content.Length,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                AccessedDate = DateTime.Now,
                Extension = System.IO.Path.GetExtension(path),
                MimeType = GetMimeType(path)
            };
            _files[path] = newFile;
        }
        
        Console.WriteLine("[RealFileSystem] File written successfully");
    }

    public void DeleteFile(string path)
    {
        Console.WriteLine($"[RealFileSystem] Deleting file: {path}");
        
        // Simulate file deletion delay
        Thread.Sleep(20);
        
        if (!_files.Remove(path))
        {
            throw new FileNotFoundException(path);
        }
        
        Console.WriteLine("[RealFileSystem] File deleted successfully");
    }

    public void CreateDirectory(string path)
    {
        Console.WriteLine($"[RealFileSystem] Creating directory: {path}");
        
        // Simulate directory creation delay
        Thread.Sleep(25);
        
        if (!_directories.ContainsKey(path))
        {
            var directory = new VirtualDirectory
            {
                Path = path,
                Name = System.IO.Path.GetFileName(path),
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
            _directories[path] = directory;
        }
        
        Console.WriteLine("[RealFileSystem] Directory created successfully");
    }

    public List<string> ListDirectory(string path)
    {
        Console.WriteLine($"[RealFileSystem] Listing directory: {path}");
        
        // Simulate directory listing delay
        Thread.Sleep(40);
        
        var items = new List<string>();
        
        // Add subdirectories
        foreach (var dir in _directories.Keys)
        {
            if (System.IO.Path.GetDirectoryName(dir) == path)
            {
                items.Add(System.IO.Path.GetFileName(dir) + "/");
            }
        }
        
        // Add files
        foreach (var file in _files.Keys)
        {
            if (System.IO.Path.GetDirectoryName(file) == path)
            {
                items.Add(System.IO.Path.GetFileName(file));
            }
        }
        
        Console.WriteLine($"[RealFileSystem] Directory listed: {items.Count} items found");
        return items;
    }

    public FileInfo GetFileInfo(string path)
    {
        Console.WriteLine($"[RealFileSystem] Getting file info: {path}");
        
        // Simulate file info retrieval delay
        Thread.Sleep(15);
        
        if (!_files.TryGetValue(path, out var file))
        {
            throw new FileNotFoundException(path);
        }
        
        var fileInfo = new FileInfo
        {
            Name = file.Name,
            Path = file.Path,
            Size = file.Size,
            CreatedDate = file.CreatedDate,
            ModifiedDate = file.ModifiedDate,
            AccessedDate = file.AccessedDate,
            Extension = file.Extension,
            MimeType = file.MimeType,
            Attributes = file.Attributes,
            Owner = file.Owner,
            Permissions = file.Permissions
        };
        
        Console.WriteLine($"[RealFileSystem] File info retrieved: {fileInfo}");
        return fileInfo;
    }

    public bool FileExists(string path)
    {
        Console.WriteLine($"[RealFileSystem] Checking if file exists: {path}");
        
        // Simulate file existence check delay
        Thread.Sleep(10);
        
        var exists = _files.ContainsKey(path);
        Console.WriteLine($"[RealFileSystem] File exists check: {exists}");
        return exists;
    }

    public void CopyFile(string sourcePath, string destinationPath)
    {
        Console.WriteLine($"[RealFileSystem] Copying file: {sourcePath} -> {destinationPath}");
        
        // Simulate file copy delay
        Thread.Sleep(60);
        
        if (!_files.TryGetValue(sourcePath, out var sourceFile))
        {
            throw new FileNotFoundException(sourcePath);
        }
        
        var copiedFile = new VirtualFile
        {
            Path = destinationPath,
            Name = System.IO.Path.GetFileName(destinationPath),
            Content = sourceFile.Content,
            Size = sourceFile.Size,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now,
            AccessedDate = DateTime.Now,
            Extension = sourceFile.Extension,
            MimeType = sourceFile.MimeType,
            Attributes = sourceFile.Attributes,
            Owner = sourceFile.Owner,
            Permissions = sourceFile.Permissions
        };
        
        _files[destinationPath] = copiedFile;
        Console.WriteLine("[RealFileSystem] File copied successfully");
    }

    public void MoveFile(string sourcePath, string destinationPath)
    {
        Console.WriteLine($"[RealFileSystem] Moving file: {sourcePath} -> {destinationPath}");
        
        // Simulate file move delay
        Thread.Sleep(45);
        
        if (!_files.TryGetValue(sourcePath, out var file))
        {
            throw new FileNotFoundException(sourcePath);
        }
        
        // Update file path and name
        file.Path = destinationPath;
        file.Name = System.IO.Path.GetFileName(destinationPath);
        file.ModifiedDate = DateTime.Now;
        
        // Move in dictionary
        _files.Remove(sourcePath);
        _files[destinationPath] = file;
        
        Console.WriteLine("[RealFileSystem] File moved successfully");
    }

    public long GetFileSize(string path)
    {
        Console.WriteLine($"[RealFileSystem] Getting file size: {path}");
        
        // Simulate file size retrieval delay
        Thread.Sleep(5);
        
        if (!_files.TryGetValue(path, out var file))
        {
            throw new FileNotFoundException(path);
        }
        
        Console.WriteLine($"[RealFileSystem] File size: {file.Size} bytes");
        return file.Size;
    }

    private void InitializeFileSystem()
    {
        // Create some sample directories
        _directories["/"] = new VirtualDirectory { Path = "/", Name = "", CreatedDate = DateTime.Now, ModifiedDate = DateTime.Now };
        _directories["/documents"] = new VirtualDirectory { Path = "/documents", Name = "documents", CreatedDate = DateTime.Now, ModifiedDate = DateTime.Now };
        _directories["/system"] = new VirtualDirectory { Path = "/system", Name = "system", CreatedDate = DateTime.Now, ModifiedDate = DateTime.Now };
        _directories["/temp"] = new VirtualDirectory { Path = "/temp", Name = "temp", CreatedDate = DateTime.Now, ModifiedDate = DateTime.Now };
        
        // Create some sample files
        CreateSampleFile("/documents/readme.txt", "Welcome to the virtual file system!\nThis is a demonstration of the Proxy pattern.", UserRole.User);
        CreateSampleFile("/documents/report.pdf", "Binary content of a PDF report...", UserRole.User);
        CreateSampleFile("/system/config.sys", "System configuration file content", UserRole.Administrator);
        CreateSampleFile("/system/boot.ini", "Boot configuration", UserRole.System);
        CreateSampleFile("/temp/cache.tmp", "Temporary cache data", UserRole.User);
    }

    private void CreateSampleFile(string path, string content, UserRole requiredRole)
    {
        var file = new VirtualFile
        {
            Path = path,
            Name = System.IO.Path.GetFileName(path),
            Content = content,
            Size = content.Length,
            CreatedDate = DateTime.Now.AddDays(-_random.Next(1, 30)),
            ModifiedDate = DateTime.Now.AddDays(-_random.Next(0, 7)),
            AccessedDate = DateTime.Now.AddDays(-_random.Next(0, 3)),
            Extension = System.IO.Path.GetExtension(path),
            MimeType = GetMimeType(path),
            Owner = "system",
            Permissions = new FilePermissions
            {
                CanRead = true,
                CanWrite = requiredRole <= UserRole.PowerUser,
                CanExecute = path.Contains("system"),
                CanDelete = requiredRole <= UserRole.PowerUser,
                RequiredRole = requiredRole
            }
        };
        
        _files[path] = file;
    }

    private static string GetMimeType(string path)
    {
        var extension = System.IO.Path.GetExtension(path).ToLowerInvariant();
        return extension switch
        {
            ".txt" => "text/plain",
            ".pdf" => "application/pdf",
            ".sys" => "application/octet-stream",
            ".ini" => "text/plain",
            ".tmp" => "application/octet-stream",
            ".html" => "text/html",
            ".css" => "text/css",
            ".js" => "application/javascript",
            ".json" => "application/json",
            ".xml" => "application/xml",
            _ => "application/octet-stream"
        };
    }
}

/// <summary>
/// Internal representation of a virtual file.
/// </summary>
internal class VirtualFile
{
    public string Path { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public long Size { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public DateTime AccessedDate { get; set; }
    public string Extension { get; set; } = string.Empty;
    public string MimeType { get; set; } = string.Empty;
    public FileAttributes Attributes { get; set; } = FileAttributes.None;
    public string Owner { get; set; } = "system";
    public FilePermissions Permissions { get; set; } = new();
}

/// <summary>
/// Internal representation of a virtual directory.
/// </summary>
internal class VirtualDirectory
{
    public string Path { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public string Owner { get; set; } = "system";
    public FilePermissions Permissions { get; set; } = new();
}
