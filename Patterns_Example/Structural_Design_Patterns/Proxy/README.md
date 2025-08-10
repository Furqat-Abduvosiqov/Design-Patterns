# Proxy Pattern Example

This folder contains a comprehensive implementation of the Proxy design pattern using a real-world virtual file system that demonstrates how to control access to expensive resources while adding security, caching, and monitoring capabilities.

## Files Overview

- **`IFileSystem.cs`** - Subject interface and data structures for file system operations
- **`RealFileSystem.cs`** - Expensive real subject that performs actual file operations
- **`FileSystemProxy.cs`** - Proxy with access control, caching, logging, and lazy initialization
- **`ProxyExample.cs`** - Comprehensive usage demonstrations
- **`ProxyPatternDemo.cs`** - Main demo program
- **`ProxyPatternGuide.md`** - Complete pattern explanation

## What Problem Does This Solve?

The Proxy pattern addresses the issues of uncontrolled access to expensive or sensitive resources. Instead of:

```csharp
// ❌ Direct access problems - expensive and uncontrolled
class ExpensiveFileSystem
{
    public ExpensiveFileSystem()
    {
        // Always expensive initialization (3.5 seconds)
        LoadFileSystemIndex(); // 2 seconds
        ConnectToNetwork();    // 1 second  
        InitializeSecurity();  // 0.5 seconds
    }
    
    public string ReadFile(string path)
    {
        // No access control - anyone can read anything
        // No caching - always reads from disk (50ms delay)
        // No audit logging - no trace of operations
        return File.ReadAllText(path); // Expensive every time!
    }
}

// Client always pays full cost
var fs = new ExpensiveFileSystem(); // 3.5 seconds every time!
var content = fs.ReadFile("/secret/file.txt"); // No security check!
```

You can use:

```csharp
// ✅ Proxy pattern - controlled, efficient, secure access
var user = new UserContext { Username = "john", Role = UserRole.User };
var proxy = new FileSystemProxy(user); // Instant creation!

// Lazy initialization, access control, caching, logging
var content = proxy.ReadFile("/documents/file.txt"); // Secure & efficient!
```

## Key Benefits Demonstrated

1. **Controlled Access**: Role-based security and authentication before operations
2. **Lazy Initialization**: Expensive objects created only when actually needed
3. **Performance Caching**: Frequently accessed data cached for dramatic speed improvements
4. **Audit Logging**: Complete trail of all operations with timestamps and user info
5. **Transparent Interface**: Clients don't know they're using a proxy
6. **Resource Optimization**: Efficient use of expensive resources

## Real-World Use Cases

This pattern is commonly used for:

- **Database Connection Pools**: Controlling access to expensive database connections
- **Remote Service Proxies**: Adding retry logic, caching, and circuit breakers
- **Security Proxies**: Authentication and authorization for sensitive operations
- **Caching Proxies**: Web caches, CDNs, and application-level caching
- **Virtual Memory**: Operating system virtual memory management
- **ORM Frameworks**: Lazy loading of database entities

## Components Demonstrated

### 1. Subject Interface (`IFileSystem`)
Common interface for both proxy and real implementation:
```csharp
public interface IFileSystem
{
    string ReadFile(string path);
    void WriteFile(string path, string content);
    void DeleteFile(string path);
    List<string> ListDirectory(string path);
    FileInfo GetFileInfo(string path);
    // ... other file operations
}
```

### 2. Real Subject (`RealFileSystem`)
Expensive implementation that does the actual work:
- **Heavy Initialization**: Simulates expensive file system setup (100ms delay)
- **Slow Operations**: File I/O operations with realistic delays (5-60ms per operation)
- **Resource Intensive**: Large memory footprint and processing time
- **No Built-in Security**: Direct access to all operations

### 3. Proxy (`FileSystemProxy`)
Intelligent proxy that adds multiple layers of functionality:

**Access Control**:
- User authentication and role-based permissions
- Path-based security (system files require admin access)
- Operation-specific permissions (read vs. write vs. delete)

**Caching**:
- In-memory cache for frequently accessed files
- Configurable cache expiration (default: 30 minutes)
- Cache statistics and monitoring
- Automatic cache invalidation on file modifications

**Lazy Initialization**:
- Real file system created only when first operation is performed
- Saves memory and startup time when proxy is created but not used
- Thread-safe initialization for concurrent access

**Audit Logging**:
- Complete log of all operations with timestamps
- User context tracking (username, role, session)
- Success/failure status and error messages
- Operation duration tracking for performance monitoring

## Advanced Features

### User Context and Roles
Comprehensive user management with role-based access:
```csharp
public class UserContext
{
    public string Username { get; set; }
    public UserRole Role { get; set; }        // Guest, User, PowerUser, Administrator, System
    public bool IsAuthenticated { get; set; }
    public List<string> Groups { get; set; }
    public string SessionId { get; set; }
}
```

### Cache Management
Intelligent caching with statistics and monitoring:
```csharp
public class CacheStatistics
{
    public int TotalEntries { get; set; }     // Total cached items
    public int ActiveEntries { get; set; }   // Non-expired items
    public int ExpiredEntries { get; set; }  // Expired items
    public long TotalSizeBytes { get; set; } // Memory usage
    public int HitCount { get; set; }        // Cache hits
}
```

### Audit Trail
Comprehensive logging for security and compliance:
```csharp
public class AuditLogEntry
{
    public DateTime Timestamp { get; set; }
    public string Username { get; set; }
    public UserRole UserRole { get; set; }
    public FileOperation Operation { get; set; }  // Read, Write, Delete, etc.
    public string Path { get; set; }
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public TimeSpan Duration { get; set; }
}
```

### Proxy Configuration
Flexible configuration for different proxy behaviors:
```csharp
public class ProxyConfiguration
{
    public bool EnableCaching { get; set; } = true;
    public bool EnableAuditing { get; set; } = true;
    public bool EnableAccessControl { get; set; } = true;
    public bool EnableLazyInitialization { get; set; } = true;
    public int CacheExpirationMinutes { get; set; } = 30;
    public int MaxCacheEntries { get; set; } = 1000;
}
```

## How to Run

1. Navigate to the project root directory
2. Update Program.cs to use the Proxy example:
   ```csharp
   using Patterns_Example.Structural_Design_Patterns.Proxy;
   ProxyExample.RunExample();
   ```
3. Run the demo:
   ```bash
   dotnet run --project Patterns_Example
   ```

## Example Output

The demo will show:

1. **Problems Without Proxy**: Direct access issues and expensive initialization
2. **Basic Proxy Usage**: Lazy initialization and transparent interface
3. **Access Control**: Role-based security with different user types
4. **Caching Benefits**: Dramatic performance improvements from caching
5. **Lazy Initialization**: Delayed expensive object creation
6. **Audit Logging**: Complete operation trail with statistics

## Learning Objectives

After studying this example, you should understand:

- When expensive resource access needs control
- How to implement transparent proxies that add functionality
- The different types of proxies and their use cases
- How to combine multiple proxy behaviors (caching + access control + logging)
- The trade-offs between functionality and performance
- How proxies enable security and monitoring without changing clients

## Types of Proxies Demonstrated

### 1. Virtual Proxy (Lazy Initialization)
Delays expensive object creation until actually needed:
```csharp
// Proxy created instantly
var proxy = new FileSystemProxy(user); // 0ms

// Real object created on first use
var content = proxy.ReadFile("/file.txt"); // 100ms initialization + operation
```

### 2. Protection Proxy (Access Control)
Controls access based on user permissions:
```csharp
// Regular user accessing system files
proxy.ReadFile("/system/config.sys"); // Throws AccessDeniedException

// Admin user accessing same file
adminProxy.ReadFile("/system/config.sys"); // Success
```

### 3. Caching Proxy (Performance)
Stores results for faster subsequent access:
```csharp
// First read: 50ms (from disk)
var content1 = proxy.ReadFile("/file.txt");

// Second read: 1ms (from cache)
var content2 = proxy.ReadFile("/file.txt"); // 50x faster!
```

### 4. Logging Proxy (Audit Trail)
Records all operations for monitoring and compliance:
```csharp
// All operations automatically logged
proxy.ReadFile("/file.txt");
proxy.WriteFile("/file.txt", "new content");

// View audit log
var auditLog = proxy.GetAuditLog();
// Shows: timestamp, user, operation, path, success/failure, duration
```

## Performance Benefits

### Lazy Initialization Savings:
- **Proxy Creation**: 0ms (instant)
- **Real Object Creation**: 100ms (only when needed)
- **Memory Savings**: No allocation until first use

### Caching Performance:
- **First Read**: 50ms (disk access)
- **Cached Read**: 1ms (memory access)
- **Speed Improvement**: 50x faster for cached content

### Access Control Overhead:
- **Permission Check**: <1ms (minimal impact)
- **Security Benefit**: Prevents unauthorized access
- **Audit Value**: Complete operation trail

## Design Decisions

### Interface Consistency
The proxy implements the exact same interface as the real subject:
- Clients don't know they're using a proxy
- Easy to swap proxy and real implementations
- Transparent functionality addition

### Layered Functionality
Multiple proxy behaviors combined in single class:
- Access control → Caching → Lazy initialization → Real operation
- Each layer adds value without breaking the chain
- Configurable enabling/disabling of features

### Thread Safety
Proxy designed for concurrent access:
- Thread-safe lazy initialization
- Synchronized cache operations
- Atomic audit log updates

## Extension Ideas

Try extending this example by:

1. **Adding Network File System**: Remote file access through HTTP/FTP
2. **Implementing Compression Proxy**: Automatic file compression/decompression
3. **Creating Encryption Proxy**: Transparent file encryption/decryption
4. **Adding Quota Management**: File size and count limits per user
5. **Implementing Versioning**: Automatic file versioning and history
6. **Creating Synchronization Proxy**: Multi-user file locking and coordination
7. **Adding Backup Proxy**: Automatic backup of modified files

## Best Practices Demonstrated

1. **Interface Consistency**: Proxy implements same interface as real subject
2. **Transparent Operation**: Clients don't know they're using a proxy
3. **Lazy Initialization**: Expensive objects created only when needed
4. **Proper Error Handling**: Exceptions handled and logged appropriately
5. **Resource Management**: Efficient use of memory and processing power
6. **Security Integration**: Access control seamlessly integrated
7. **Performance Monitoring**: Cache statistics and operation timing
8. **Audit Compliance**: Complete logging for security and compliance
