# Proxy Design Pattern

The Proxy pattern is a structural design pattern that provides a placeholder or surrogate for another object to control access to it. The proxy acts as an intermediary that can add functionality such as lazy initialization, access control, caching, or logging without changing the original object's interface.

## Problem

Imagine you're building a file system that needs to handle expensive operations like reading large files, network access, or database queries. Without the Proxy pattern, you'd face several issues:

```csharp
// ❌ Without Proxy pattern - direct access problems
class ExpensiveFileSystem
{
    public ExpensiveFileSystem()
    {
        // Expensive initialization - always happens
        LoadFileSystemIndex(); // Takes 2 seconds
        ConnectToNetwork();    // Takes 1 second
        InitializeSecurity();  // Takes 500ms
    }
    
    public string ReadFile(string path)
    {
        // No access control
        // No caching - always reads from disk
        // No audit logging
        return File.ReadAllText(path);
    }
}

// Client always pays the full initialization cost
var fs = new ExpensiveFileSystem(); // 3.5 seconds every time!
```

### Issues with Direct Access:

1. **Expensive Initialization**: Heavy objects created even when not needed
2. **No Access Control**: Anyone can access any resource
3. **No Caching**: Repeated expensive operations
4. **No Audit Trail**: No logging of operations
5. **Resource Waste**: Full initialization for simple operations
6. **Security Risks**: Uncontrolled access to sensitive resources

## Solution

The Proxy pattern suggests creating a proxy class that implements the same interface as the original object but adds control mechanisms:

1. **Subject Interface**: Common interface for both proxy and real object
2. **Real Subject**: The actual object that does the work
3. **Proxy**: Controls access to the real subject and adds functionality
4. **Client**: Uses the proxy as if it were the real object

### Key Components:

1. **Subject (IFileSystem)**: Interface defining file system operations
2. **RealSubject (RealFileSystem)**: Expensive file system implementation
3. **Proxy (FileSystemProxy)**: Controls access and adds functionality
4. **Client**: Uses file system through proxy interface

## Real-World Example: Virtual File System

Our example demonstrates a virtual file system where:

### Expensive Real Subject (RealFileSystem):
- **Heavy Initialization**: Simulates expensive file system setup
- **Slow Operations**: File I/O operations with realistic delays
- **Resource Intensive**: Large memory footprint and processing time
- **No Built-in Security**: Direct access to all operations

### Intelligent Proxy (FileSystemProxy):
- **Lazy Initialization**: Creates real object only when needed
- **Access Control**: Role-based permissions and authentication
- **Caching**: Stores frequently accessed files in memory
- **Audit Logging**: Tracks all operations with timestamps and user info
- **Performance Optimization**: Reduces redundant expensive operations

## Benefits Demonstrated

1. **Controlled Access**: Security and permission checking before operations
2. **Lazy Initialization**: Expensive objects created only when needed
3. **Performance Caching**: Frequently accessed data cached for speed
4. **Audit Logging**: Complete trail of all operations and access attempts
5. **Transparent Interface**: Clients don't know they're using a proxy
6. **Resource Optimization**: Efficient use of expensive resources

## Implementation Structure

```csharp
// Subject interface
public interface IFileSystem
{
    string ReadFile(string path);
    void WriteFile(string path, string content);
    // ... other operations
}

// Real subject (expensive)
public class RealFileSystem : IFileSystem
{
    public RealFileSystem()
    {
        // Expensive initialization
        Thread.Sleep(1000); // Simulate heavy setup
    }
    
    public string ReadFile(string path)
    {
        // Expensive file operation
        Thread.Sleep(50); // Simulate I/O delay
        return actualFileContent;
    }
}

// Proxy (controls access)
public class FileSystemProxy : IFileSystem
{
    private RealFileSystem? _realFileSystem;
    private readonly UserContext _user;
    private readonly Dictionary<string, string> _cache;
    
    public string ReadFile(string path)
    {
        // 1. Check permissions
        CheckAccess(path, "read");
        
        // 2. Check cache
        if (_cache.TryGetValue(path, out var cached))
            return cached;
            
        // 3. Lazy initialization
        _realFileSystem ??= new RealFileSystem();
        
        // 4. Delegate to real object
        var content = _realFileSystem.ReadFile(path);
        
        // 5. Cache result
        _cache[path] = content;
        
        // 6. Log operation
        LogOperation("read", path, true);
        
        return content;
    }
}
```

## When to Use Proxy Pattern

✅ **Use Proxy when:**
- You need to control access to expensive or sensitive resources
- You want to add functionality without modifying the original object
- You need lazy initialization of heavy objects
- You want to implement caching, logging, or access control
- You need to add security or audit capabilities

❌ **Don't use Proxy when:**
- The object is lightweight and doesn't need control
- Direct access is sufficient and no additional functionality is needed
- The proxy would add unnecessary complexity
- Performance overhead of proxy outweighs benefits

## Types of Proxies Demonstrated

### 1. Virtual Proxy (Lazy Initialization)
```csharp
public class VirtualProxy : IFileSystem
{
    private RealFileSystem? _realFileSystem;
    
    private void EnsureInitialized()
    {
        _realFileSystem ??= new RealFileSystem(); // Create only when needed
    }
}
```

### 2. Protection Proxy (Access Control)
```csharp
public class ProtectionProxy : IFileSystem
{
    public string ReadFile(string path)
    {
        if (!_user.HasPermission(path, "read"))
            throw new AccessDeniedException();
        return _realFileSystem.ReadFile(path);
    }
}
```

### 3. Caching Proxy (Performance)
```csharp
public class CachingProxy : IFileSystem
{
    private readonly Dictionary<string, CacheEntry> _cache;
    
    public string ReadFile(string path)
    {
        if (_cache.TryGetValue(path, out var cached) && !cached.IsExpired)
            return cached.Content;
        
        var content = _realFileSystem.ReadFile(path);
        _cache[path] = new CacheEntry(content);
        return content;
    }
}
```

### 4. Logging Proxy (Audit Trail)
```csharp
public class LoggingProxy : IFileSystem
{
    public string ReadFile(string path)
    {
        var startTime = DateTime.Now;
        try
        {
            var result = _realFileSystem.ReadFile(path);
            LogSuccess("ReadFile", path, DateTime.Now - startTime);
            return result;
        }
        catch (Exception ex)
        {
            LogFailure("ReadFile", path, ex);
            throw;
        }
    }
}
```

## Code Structure

```
Proxy/
├── IFileSystem.cs              # Subject interface and data structures
├── RealFileSystem.cs           # Expensive real subject implementation
├── FileSystemProxy.cs          # Proxy with access control, caching, logging
├── ProxyExample.cs             # Usage demonstrations
└── ProxyPatternDemo.cs         # Main demo program
```

## Performance Considerations

1. **Lazy Initialization**: Saves memory and startup time
2. **Caching Benefits**: Dramatic speed improvements for repeated operations
3. **Access Control Overhead**: Minimal cost for security checks
4. **Logging Impact**: Small overhead for audit trail
5. **Memory Usage**: Cache uses memory but saves expensive operations

## Testing Benefits

1. **Mock Proxies**: Easy to create test doubles
2. **Behavior Verification**: Test proxy functionality independently
3. **Performance Testing**: Measure caching and lazy loading benefits
4. **Security Testing**: Verify access control mechanisms
5. **Integration Testing**: Test real subject through proxy

## Common Variations

### 1. Remote Proxy
Represents objects in different address spaces:
```csharp
public class RemoteFileSystemProxy : IFileSystem
{
    private readonly HttpClient _httpClient;
    
    public string ReadFile(string path)
    {
        // Make HTTP request to remote file system
        return await _httpClient.GetStringAsync($"/files/{path}");
    }
}
```

### 2. Smart Reference Proxy
Adds reference counting or cleanup:
```csharp
public class SmartReferenceProxy : IFileSystem
{
    private int _referenceCount;
    
    public string ReadFile(string path)
    {
        _referenceCount++;
        try
        {
            return _realFileSystem.ReadFile(path);
        }
        finally
        {
            _referenceCount--;
            if (_referenceCount == 0)
                _realFileSystem.Dispose(); // Cleanup when no references
        }
    }
}
```

### 3. Copy-on-Write Proxy
Delays expensive copying until modification:
```csharp
public class CopyOnWriteProxy : IFileSystem
{
    private bool _copied = false;
    
    public void WriteFile(string path, string content)
    {
        if (!_copied)
        {
            _realFileSystem = _realFileSystem.Clone(); // Copy only when writing
            _copied = true;
        }
        _realFileSystem.WriteFile(path, content);
    }
}
```

## Learning Objectives

After studying this example, you should understand:

- When expensive resource access needs control
- How to implement transparent proxies that add functionality
- The different types of proxies and their use cases
- How to combine multiple proxy behaviors (caching + access control + logging)
- The trade-offs between functionality and performance
- How proxies enable security and monitoring without changing clients

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
