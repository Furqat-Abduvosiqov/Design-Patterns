namespace Patterns_Example.Structural_Design_Patterns.Proxy;

/// <summary>
/// Proxy that controls access to the RealFileSystem.
/// This proxy provides access control, caching, logging, and lazy initialization.
/// </summary>
public class FileSystemProxy : IFileSystem
{
    private RealFileSystem? _realFileSystem;
    private readonly UserContext _currentUser;
    private readonly Dictionary<string, CacheEntry> _cache;
    private readonly List<AuditLogEntry> _auditLog;
    private readonly ProxyConfiguration _config;
    private readonly object _lock = new object();

    public FileSystemProxy(UserContext currentUser, ProxyConfiguration? config = null)
    {
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        _cache = new Dictionary<string, CacheEntry>();
        _auditLog = new List<AuditLogEntry>();
        _config = config ?? new ProxyConfiguration();
        
        Console.WriteLine($"[FileSystemProxy] Created proxy for user: {_currentUser}");
    }

    public string ReadFile(string path)
    {
        var startTime = DateTime.Now;
        var logEntry = CreateAuditLogEntry(FileOperation.Read, path);
        
        try
        {
            // Check permissions
            CheckPermission(path, FileOperation.Read);
            
            // Check cache first
            if (_config.EnableCaching && TryGetFromCache(path, out var cachedContent))
            {
                Console.WriteLine($"[FileSystemProxy] Cache hit for: {path}");
                logEntry.Success = true;
                logEntry.Duration = DateTime.Now - startTime;
                _auditLog.Add(logEntry);
                return cachedContent;
            }
            
            // Lazy initialization of real file system
            EnsureRealFileSystemInitialized();
            
            // Delegate to real file system
            var content = _realFileSystem!.ReadFile(path);
            
            // Cache the result
            if (_config.EnableCaching)
            {
                CacheContent(path, content);
            }
            
            logEntry.Success = true;
            logEntry.FileSize = content.Length;
            logEntry.Duration = DateTime.Now - startTime;
            _auditLog.Add(logEntry);
            
            return content;
        }
        catch (Exception ex)
        {
            logEntry.Success = false;
            logEntry.ErrorMessage = ex.Message;
            logEntry.Duration = DateTime.Now - startTime;
            _auditLog.Add(logEntry);
            throw;
        }
    }

    public void WriteFile(string path, string content)
    {
        var startTime = DateTime.Now;
        var logEntry = CreateAuditLogEntry(FileOperation.Write, path);
        
        try
        {
            // Check permissions
            CheckPermission(path, FileOperation.Write);
            
            // Lazy initialization of real file system
            EnsureRealFileSystemInitialized();
            
            // Delegate to real file system
            _realFileSystem!.WriteFile(path, content);
            
            // Invalidate cache
            if (_config.EnableCaching)
            {
                InvalidateCache(path);
            }
            
            logEntry.Success = true;
            logEntry.FileSize = content.Length;
            logEntry.Duration = DateTime.Now - startTime;
            _auditLog.Add(logEntry);
        }
        catch (Exception ex)
        {
            logEntry.Success = false;
            logEntry.ErrorMessage = ex.Message;
            logEntry.Duration = DateTime.Now - startTime;
            _auditLog.Add(logEntry);
            throw;
        }
    }

    public void DeleteFile(string path)
    {
        var startTime = DateTime.Now;
        var logEntry = CreateAuditLogEntry(FileOperation.Delete, path);
        
        try
        {
            // Check permissions
            CheckPermission(path, FileOperation.Delete);
            
            // Lazy initialization of real file system
            EnsureRealFileSystemInitialized();
            
            // Delegate to real file system
            _realFileSystem!.DeleteFile(path);
            
            // Invalidate cache
            if (_config.EnableCaching)
            {
                InvalidateCache(path);
            }
            
            logEntry.Success = true;
            logEntry.Duration = DateTime.Now - startTime;
            _auditLog.Add(logEntry);
        }
        catch (Exception ex)
        {
            logEntry.Success = false;
            logEntry.ErrorMessage = ex.Message;
            logEntry.Duration = DateTime.Now - startTime;
            _auditLog.Add(logEntry);
            throw;
        }
    }

    public void CreateDirectory(string path)
    {
        var startTime = DateTime.Now;
        var logEntry = CreateAuditLogEntry(FileOperation.Create, path);
        
        try
        {
            // Check permissions
            CheckPermission(path, FileOperation.Create);
            
            // Lazy initialization of real file system
            EnsureRealFileSystemInitialized();
            
            // Delegate to real file system
            _realFileSystem!.CreateDirectory(path);
            
            logEntry.Success = true;
            logEntry.Duration = DateTime.Now - startTime;
            _auditLog.Add(logEntry);
        }
        catch (Exception ex)
        {
            logEntry.Success = false;
            logEntry.ErrorMessage = ex.Message;
            logEntry.Duration = DateTime.Now - startTime;
            _auditLog.Add(logEntry);
            throw;
        }
    }

    public List<string> ListDirectory(string path)
    {
        var startTime = DateTime.Now;
        var logEntry = CreateAuditLogEntry(FileOperation.List, path);
        
        try
        {
            // Check permissions
            CheckPermission(path, FileOperation.List);
            
            // Check cache first
            if (_config.EnableCaching && TryGetFromCache($"list:{path}", out var cachedList))
            {
                Console.WriteLine($"[FileSystemProxy] Cache hit for directory listing: {path}");
                logEntry.Success = true;
                logEntry.Duration = DateTime.Now - startTime;
                _auditLog.Add(logEntry);
                return cachedList.Split('\n').ToList();
            }
            
            // Lazy initialization of real file system
            EnsureRealFileSystemInitialized();
            
            // Delegate to real file system
            var items = _realFileSystem!.ListDirectory(path);
            
            // Cache the result
            if (_config.EnableCaching)
            {
                CacheContent($"list:{path}", string.Join('\n', items));
            }
            
            logEntry.Success = true;
            logEntry.Duration = DateTime.Now - startTime;
            _auditLog.Add(logEntry);
            
            return items;
        }
        catch (Exception ex)
        {
            logEntry.Success = false;
            logEntry.ErrorMessage = ex.Message;
            logEntry.Duration = DateTime.Now - startTime;
            _auditLog.Add(logEntry);
            throw;
        }
    }

    public FileInfo GetFileInfo(string path)
    {
        var startTime = DateTime.Now;
        var logEntry = CreateAuditLogEntry(FileOperation.GetInfo, path);
        
        try
        {
            // Check permissions
            CheckPermission(path, FileOperation.GetInfo);
            
            // Lazy initialization of real file system
            EnsureRealFileSystemInitialized();
            
            // Delegate to real file system
            var fileInfo = _realFileSystem!.GetFileInfo(path);
            
            logEntry.Success = true;
            logEntry.FileSize = fileInfo.Size;
            logEntry.Duration = DateTime.Now - startTime;
            _auditLog.Add(logEntry);
            
            return fileInfo;
        }
        catch (Exception ex)
        {
            logEntry.Success = false;
            logEntry.ErrorMessage = ex.Message;
            logEntry.Duration = DateTime.Now - startTime;
            _auditLog.Add(logEntry);
            throw;
        }
    }

    public bool FileExists(string path)
    {
        // File existence check doesn't require full permissions
        EnsureRealFileSystemInitialized();
        return _realFileSystem!.FileExists(path);
    }

    public void CopyFile(string sourcePath, string destinationPath)
    {
        var startTime = DateTime.Now;
        var logEntry = CreateAuditLogEntry(FileOperation.Copy, $"{sourcePath} -> {destinationPath}");
        
        try
        {
            // Check permissions for both source and destination
            CheckPermission(sourcePath, FileOperation.Read);
            CheckPermission(destinationPath, FileOperation.Write);
            
            // Lazy initialization of real file system
            EnsureRealFileSystemInitialized();
            
            // Delegate to real file system
            _realFileSystem!.CopyFile(sourcePath, destinationPath);
            
            // Invalidate cache for destination
            if (_config.EnableCaching)
            {
                InvalidateCache(destinationPath);
            }
            
            logEntry.Success = true;
            logEntry.Duration = DateTime.Now - startTime;
            _auditLog.Add(logEntry);
        }
        catch (Exception ex)
        {
            logEntry.Success = false;
            logEntry.ErrorMessage = ex.Message;
            logEntry.Duration = DateTime.Now - startTime;
            _auditLog.Add(logEntry);
            throw;
        }
    }

    public void MoveFile(string sourcePath, string destinationPath)
    {
        var startTime = DateTime.Now;
        var logEntry = CreateAuditLogEntry(FileOperation.Move, $"{sourcePath} -> {destinationPath}");
        
        try
        {
            // Check permissions for both source and destination
            CheckPermission(sourcePath, FileOperation.Delete);
            CheckPermission(destinationPath, FileOperation.Write);
            
            // Lazy initialization of real file system
            EnsureRealFileSystemInitialized();
            
            // Delegate to real file system
            _realFileSystem!.MoveFile(sourcePath, destinationPath);
            
            // Invalidate cache for both paths
            if (_config.EnableCaching)
            {
                InvalidateCache(sourcePath);
                InvalidateCache(destinationPath);
            }
            
            logEntry.Success = true;
            logEntry.Duration = DateTime.Now - startTime;
            _auditLog.Add(logEntry);
        }
        catch (Exception ex)
        {
            logEntry.Success = false;
            logEntry.ErrorMessage = ex.Message;
            logEntry.Duration = DateTime.Now - startTime;
            _auditLog.Add(logEntry);
            throw;
        }
    }

    public long GetFileSize(string path)
    {
        // Check permissions
        CheckPermission(path, FileOperation.GetInfo);
        
        // Lazy initialization of real file system
        EnsureRealFileSystemInitialized();
        
        return _realFileSystem!.GetFileSize(path);
    }

    /// <summary>
    /// Gets the audit log entries.
    /// </summary>
    public List<AuditLogEntry> GetAuditLog()
    {
        return new List<AuditLogEntry>(_auditLog);
    }

    /// <summary>
    /// Gets cache statistics.
    /// </summary>
    public CacheStatistics GetCacheStatistics()
    {
        lock (_lock)
        {
            var totalEntries = _cache.Count;
            var expiredEntries = _cache.Values.Count(e => e.IsExpired(_config.CacheExpirationMinutes));
            var totalSize = _cache.Values.Sum(e => e.Content.Length);
            
            return new CacheStatistics
            {
                TotalEntries = totalEntries,
                ExpiredEntries = expiredEntries,
                ActiveEntries = totalEntries - expiredEntries,
                TotalSizeBytes = totalSize,
                HitCount = _cache.Values.Sum(e => e.HitCount),
                Configuration = _config
            };
        }
    }

    /// <summary>
    /// Clears the cache.
    /// </summary>
    public void ClearCache()
    {
        lock (_lock)
        {
            _cache.Clear();
            Console.WriteLine("[FileSystemProxy] Cache cleared");
        }
    }

    private void EnsureRealFileSystemInitialized()
    {
        if (_realFileSystem == null)
        {
            lock (_lock)
            {
                _realFileSystem ??= new RealFileSystem();
            }
        }
    }

    private void CheckPermission(string path, FileOperation operation)
    {
        // Basic path-based permission checking
        if (path.StartsWith("/system") && !_currentUser.HasRole(UserRole.Administrator))
        {
            throw new AccessDeniedException(path, operation.ToString(), _currentUser);
        }
        
        if (path.Contains("config") && operation == FileOperation.Write && !_currentUser.HasRole(UserRole.PowerUser))
        {
            throw new AccessDeniedException(path, operation.ToString(), _currentUser);
        }
        
        if (!_currentUser.IsAuthenticated)
        {
            throw new AccessDeniedException(path, operation.ToString(), _currentUser);
        }
    }

    private bool TryGetFromCache(string key, out string content)
    {
        lock (_lock)
        {
            if (_cache.TryGetValue(key, out var entry) && !entry.IsExpired(_config.CacheExpirationMinutes))
            {
                entry.HitCount++;
                entry.LastAccessed = DateTime.Now;
                content = entry.Content;
                return true;
            }
            
            content = string.Empty;
            return false;
        }
    }

    private void CacheContent(string key, string content)
    {
        lock (_lock)
        {
            _cache[key] = new CacheEntry
            {
                Content = content,
                CreatedAt = DateTime.Now,
                LastAccessed = DateTime.Now,
                HitCount = 0
            };
        }
    }

    private void InvalidateCache(string path)
    {
        lock (_lock)
        {
            _cache.Remove(path);
            _cache.Remove($"list:{System.IO.Path.GetDirectoryName(path)}");
        }
    }

    private AuditLogEntry CreateAuditLogEntry(FileOperation operation, string path)
    {
        return new AuditLogEntry
        {
            Username = _currentUser.Username,
            UserRole = _currentUser.Role,
            Operation = operation,
            Path = path
        };
    }
}

/// <summary>
/// Cache entry for storing cached file content.
/// </summary>
public class CacheEntry
{
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime LastAccessed { get; set; }
    public int HitCount { get; set; }

    public bool IsExpired(int expirationMinutes)
    {
        return DateTime.Now - CreatedAt > TimeSpan.FromMinutes(expirationMinutes);
    }
}

/// <summary>
/// Configuration for the proxy behavior.
/// </summary>
public class ProxyConfiguration
{
    public bool EnableCaching { get; set; } = true;
    public bool EnableAuditing { get; set; } = true;
    public bool EnableAccessControl { get; set; } = true;
    public bool EnableLazyInitialization { get; set; } = true;
    public int CacheExpirationMinutes { get; set; } = 30;
    public int MaxCacheEntries { get; set; } = 1000;
}

/// <summary>
/// Cache statistics for monitoring.
/// </summary>
public class CacheStatistics
{
    public int TotalEntries { get; set; }
    public int ActiveEntries { get; set; }
    public int ExpiredEntries { get; set; }
    public long TotalSizeBytes { get; set; }
    public int HitCount { get; set; }
    public ProxyConfiguration Configuration { get; set; } = new();

    public override string ToString()
    {
        var sizeMB = TotalSizeBytes / (1024.0 * 1024.0);
        return $"""
            Cache Statistics:
              Total Entries: {TotalEntries}
              Active Entries: {ActiveEntries}
              Expired Entries: {ExpiredEntries}
              Total Size: {sizeMB:F2} MB
              Total Hits: {HitCount}
              Cache Enabled: {Configuration.EnableCaching}
              Expiration: {Configuration.CacheExpirationMinutes} minutes
            """;
    }
}
