namespace Patterns_Example.Structural_Design_Patterns.Proxy;

/// <summary>
/// Subject interface that defines the common interface for RealSubject and Proxy.
/// This interface represents file system operations that can be expensive or require access control.
/// </summary>
public interface IFileSystem
{
    /// <summary>
    /// Reads the content of a file.
    /// </summary>
    /// <param name="path">Path to the file</param>
    /// <returns>File content as string</returns>
    string ReadFile(string path);
    
    /// <summary>
    /// Writes content to a file.
    /// </summary>
    /// <param name="path">Path to the file</param>
    /// <param name="content">Content to write</param>
    void WriteFile(string path, string content);
    
    /// <summary>
    /// Deletes a file.
    /// </summary>
    /// <param name="path">Path to the file to delete</param>
    void DeleteFile(string path);
    
    /// <summary>
    /// Creates a directory.
    /// </summary>
    /// <param name="path">Path to the directory to create</param>
    void CreateDirectory(string path);
    
    /// <summary>
    /// Lists files and directories in a given path.
    /// </summary>
    /// <param name="path">Path to list contents of</param>
    /// <returns>List of file and directory names</returns>
    List<string> ListDirectory(string path);
    
    /// <summary>
    /// Gets file information.
    /// </summary>
    /// <param name="path">Path to the file</param>
    /// <returns>File information</returns>
    FileInfo GetFileInfo(string path);
    
    /// <summary>
    /// Checks if a file exists.
    /// </summary>
    /// <param name="path">Path to check</param>
    /// <returns>True if file exists, false otherwise</returns>
    bool FileExists(string path);
    
    /// <summary>
    /// Copies a file from source to destination.
    /// </summary>
    /// <param name="sourcePath">Source file path</param>
    /// <param name="destinationPath">Destination file path</param>
    void CopyFile(string sourcePath, string destinationPath);
    
    /// <summary>
    /// Moves a file from source to destination.
    /// </summary>
    /// <param name="sourcePath">Source file path</param>
    /// <param name="destinationPath">Destination file path</param>
    void MoveFile(string sourcePath, string destinationPath);
    
    /// <summary>
    /// Gets the size of a file in bytes.
    /// </summary>
    /// <param name="path">Path to the file</param>
    /// <returns>File size in bytes</returns>
    long GetFileSize(string path);
}

/// <summary>
/// File information structure.
/// </summary>
public class FileInfo
{
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public long Size { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public DateTime AccessedDate { get; set; }
    public FileAttributes Attributes { get; set; }
    public string Owner { get; set; } = string.Empty;
    public FilePermissions Permissions { get; set; } = new();
    public string MimeType { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"{Name} ({FormatFileSize(Size)}) - Modified: {ModifiedDate:yyyy-MM-dd HH:mm:ss}";
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
/// File attributes enumeration.
/// </summary>
[Flags]
public enum FileAttributes
{
    None = 0,
    ReadOnly = 1,
    Hidden = 2,
    System = 4,
    Directory = 8,
    Archive = 16,
    Compressed = 32,
    Encrypted = 64,
    Temporary = 128
}

/// <summary>
/// File permissions structure.
/// </summary>
public class FilePermissions
{
    public bool CanRead { get; set; } = true;
    public bool CanWrite { get; set; } = true;
    public bool CanExecute { get; set; } = false;
    public bool CanDelete { get; set; } = true;
    public UserRole RequiredRole { get; set; } = UserRole.User;

    public override string ToString()
    {
        var permissions = new List<string>();
        if (CanRead) permissions.Add("Read");
        if (CanWrite) permissions.Add("Write");
        if (CanExecute) permissions.Add("Execute");
        if (CanDelete) permissions.Add("Delete");
        return $"{string.Join(", ", permissions)} (Min Role: {RequiredRole})";
    }
}

/// <summary>
/// User roles for access control.
/// </summary>
public enum UserRole
{
    Guest = 0,
    User = 1,
    PowerUser = 2,
    Administrator = 3,
    System = 4
}

/// <summary>
/// User context for authentication and authorization.
/// </summary>
public class UserContext
{
    public string Username { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.Guest;
    public List<string> Groups { get; set; } = new();
    public DateTime LoginTime { get; set; } = DateTime.Now;
    public string SessionId { get; set; } = Guid.NewGuid().ToString();
    public bool IsAuthenticated { get; set; } = false;

    public bool HasRole(UserRole requiredRole)
    {
        return IsAuthenticated && Role >= requiredRole;
    }

    public override string ToString()
    {
        return $"{Username} ({Role}) - Session: {SessionId[..8]}...";
    }
}

/// <summary>
/// Exception thrown when access is denied.
/// </summary>
public class AccessDeniedException : Exception
{
    public string Path { get; }
    public string Operation { get; }
    public UserContext User { get; }

    public AccessDeniedException(string path, string operation, UserContext user)
        : base($"Access denied: {user.Username} ({user.Role}) cannot {operation} '{path}'")
    {
        Path = path;
        Operation = operation;
        User = user;
    }
}

/// <summary>
/// Exception thrown when a file is not found.
/// </summary>
public class FileNotFoundException : Exception
{
    public string Path { get; }

    public FileNotFoundException(string path)
        : base($"File not found: '{path}'")
    {
        Path = path;
    }
}

/// <summary>
/// File operation types for logging and auditing.
/// </summary>
public enum FileOperation
{
    Read,
    Write,
    Delete,
    Create,
    Copy,
    Move,
    List,
    GetInfo
}

/// <summary>
/// Audit log entry for file operations.
/// </summary>
public class AuditLogEntry
{
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public string Username { get; set; } = string.Empty;
    public UserRole UserRole { get; set; }
    public FileOperation Operation { get; set; }
    public string Path { get; set; } = string.Empty;
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public long? FileSize { get; set; }
    public TimeSpan Duration { get; set; }

    public override string ToString()
    {
        var status = Success ? "SUCCESS" : "FAILED";
        var error = Success ? "" : $" - {ErrorMessage}";
        return $"{Timestamp:HH:mm:ss} [{status}] {Username} ({UserRole}) {Operation} '{Path}'{error}";
    }
}
