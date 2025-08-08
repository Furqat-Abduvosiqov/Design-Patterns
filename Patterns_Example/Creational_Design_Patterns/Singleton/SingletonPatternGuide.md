# Singleton Design Pattern

The Singleton pattern is a creational design pattern that ensures a class has only one instance and provides a global point of access to that instance. It's one of the most well-known design patterns but also one of the most controversial due to potential misuse.

## Problem

In many applications, you need exactly one instance of certain classes:

- **Configuration Manager**: Application settings should be consistent across the entire application
- **Logger**: All parts of the application should write to the same log
- **Database Connection Pool**: Expensive resources should be shared and managed centrally
- **Cache**: Application-wide cache should be accessible from anywhere

### Issues with Multiple Instances:

1. **Inconsistent State**: Multiple configuration instances could have different settings
2. **Resource Waste**: Creating multiple expensive objects (like connection pools)
3. **Coordination Problems**: Multiple loggers writing to the same file
4. **Memory Overhead**: Unnecessary duplication of shared resources

```csharp
// ❌ Problems with multiple instances
var config1 = new ConfigurationManager();
var config2 = new ConfigurationManager();
config1.SetSetting("Theme", "Dark");
config2.SetSetting("Theme", "Light");
// Which theme is correct? Inconsistent state!
```

## Solution

The Singleton pattern ensures that:
1. **Only one instance exists** throughout the application lifecycle
2. **Global access point** is provided to that instance
3. **Lazy initialization** creates the instance only when needed
4. **Thread safety** prevents race conditions in multi-threaded environments

### Key Components:

1. **Private Constructor**: Prevents direct instantiation
2. **Static Instance Property**: Provides global access
3. **Thread-Safe Implementation**: Handles concurrent access
4. **Lazy Initialization**: Creates instance only when needed

## Real-World Example: Application Infrastructure

Our example demonstrates four common Singleton use cases:

### 1. Configuration Manager
- Manages application settings consistently
- Thread-safe access to configuration data
- Supports both string and strongly-typed settings

### 2. Logger
- Centralized logging for the entire application
- Thread-safe log entry management
- Multiple log levels and categories

### 3. Database Connection Pool
- Manages expensive database connections
- Prevents connection exhaustion
- Provides connection reuse and lifecycle management

### 4. Application Cache
- Application-wide caching mechanism
- Automatic expiration and cleanup
- Thread-safe cache operations

## Implementation Strategies

### 1. Thread-Safe Lazy Initialization (Recommended)
```csharp
public sealed class Singleton
{
    private static readonly Lazy<Singleton> _instance = 
        new Lazy<Singleton>(() => new Singleton());
    
    public static Singleton Instance => _instance.Value;
    
    private Singleton() { }
}
```

### 2. Double-Checked Locking
```csharp
public sealed class Singleton
{
    private static readonly object _lock = new object();
    private static Singleton? _instance;
    
    public static Singleton Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new Singleton();
                }
            }
            return _instance;
        }
    }
    
    private Singleton() { }
}
```

## When to Use Singleton Pattern

✅ **Use Singleton when:**
- You need exactly one instance of a class
- The instance should be accessible globally
- You want to control access to shared resources
- Lazy initialization is beneficial
- The class manages application-wide state

❌ **Don't use Singleton when:**
- You might need multiple instances in the future
- The class doesn't manage shared state
- Testing becomes difficult due to global state
- You can use dependency injection instead
- The singleton becomes a "god object"

## Benefits Demonstrated

1. **Consistency**: All parts of the application use the same configuration
2. **Resource Management**: Connection pools prevent resource exhaustion
3. **Performance**: Lazy initialization and resource reuse
4. **Thread Safety**: Proper synchronization prevents race conditions
5. **Global Access**: Easy access from anywhere in the application

## Common Pitfalls and Solutions

### 1. Testing Difficulties
**Problem**: Singletons create global state that's hard to reset between tests
**Solution**: Use dependency injection and interfaces when possible

### 2. Hidden Dependencies
**Problem**: Classes using singletons have hidden dependencies
**Solution**: Make dependencies explicit through constructor injection

### 3. Violation of Single Responsibility
**Problem**: Singletons often become "god objects"
**Solution**: Keep singletons focused on a single responsibility

### 4. Thread Safety Issues
**Problem**: Improper implementation can cause race conditions
**Solution**: Use proven thread-safe patterns like `Lazy<T>`

## Code Structure

```
Singleton/
├── IConfigurationManager.cs        # Configuration interface
├── ConfigurationManager.cs         # Configuration singleton
├── Logger.cs                       # Logging singleton
├── DatabaseConnectionPool.cs       # Connection pool singleton
├── ApplicationCache.cs             # Cache singleton
├── SingletonExample.cs             # Usage examples
└── SingletonPatternDemo.cs         # Main demo program
```

## Thread Safety Features

All singleton implementations in this example are thread-safe:

- **ConfigurationManager**: Uses double-checked locking
- **Logger**: Uses `Lazy<T>` initialization
- **DatabaseConnectionPool**: Uses double-checked locking with proper synchronization
- **ApplicationCache**: Uses `Lazy<T>` with lock-based operations

## Performance Considerations

1. **Lazy Initialization**: Instances created only when needed
2. **Memory Efficiency**: Single instance reduces memory footprint
3. **Resource Sharing**: Connection pools and caches improve performance
4. **Lock Contention**: Minimized through proper synchronization design

## Usage Examples

```csharp
// Configuration access from anywhere
var config = ConfigurationManager.Instance;
var dbConnection = config.GetSetting("DatabaseConnectionString");

// Logging from any component
var logger = Logger.Instance;
logger.Info("Operation completed successfully");

// Database operations
var pool = DatabaseConnectionPool.Instance;
using var connection = pool.GetConnection();
connection.ExecuteQuery("SELECT * FROM Users");

// Caching data
var cache = ApplicationCache.Instance;
cache.Set("user:123", userData, TimeSpan.FromMinutes(30));
var cachedUser = cache.Get<UserData>("user:123");
```

## Learning Objectives

After studying this example, you should understand:

- When the Singleton pattern is appropriate
- How to implement thread-safe singletons
- The trade-offs between different implementation approaches
- Common pitfalls and how to avoid them
- How singletons fit into modern application architecture
- The relationship between Singleton and dependency injection

## Extension Ideas

Try extending this example by:

1. **Adding Configuration Providers**: File, database, environment variables
2. **Implementing Log Targets**: File, database, remote logging services
3. **Creating Cache Policies**: LRU, LFU, time-based eviction
4. **Adding Metrics Collection**: Performance monitoring singleton
5. **Implementing Feature Flags**: Runtime feature toggle management
6. **Creating Service Registry**: Singleton for service discovery
