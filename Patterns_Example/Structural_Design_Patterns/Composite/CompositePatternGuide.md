# Composite Design Pattern

The Composite pattern is a structural design pattern that lets you compose objects into tree structures and work with these structures as if they were individual objects. It allows clients to treat individual objects and compositions of objects uniformly.

## Problem

Imagine you're building a file system management application. You need to work with both individual files and directories that can contain other files and directories. Without the Composite pattern, you'd need different code to handle files vs directories:

```csharp
// ❌ Without Composite pattern - different handling for each type
if (item is File file)
{
    Console.WriteLine($"File: {file.Name} ({file.Size} bytes)");
    // File-specific operations
}
else if (item is Directory directory)
{
    Console.WriteLine($"Directory: {directory.Name}");
    foreach (var child in directory.GetChildren())
    {
        // Recursive handling needed
        if (child is File childFile)
        {
            // Handle file again...
        }
        else if (child is Directory childDir)
        {
            // Handle directory again...
        }
    }
}
```

### Issues with Direct Handling:

1. **Code Duplication**: Similar operations repeated for different types
2. **Complex Client Code**: Clients must know about all component types
3. **Difficult Extension**: Adding new component types requires changing client code
4. **Recursive Complexity**: Tree traversal logic scattered throughout the application
5. **Inconsistent Interface**: Different methods for similar operations

## Solution

The Composite pattern suggests creating a common interface for both simple and complex objects:

1. **Component Interface**: Defines operations common to both simple and complex objects
2. **Leaf**: Represents simple objects that cannot have children
3. **Composite**: Represents complex objects that can have children
4. **Uniform Treatment**: Clients use the same interface for all objects

### Key Components:

1. **Component (IFileSystemComponent)**: Common interface for all objects
2. **Leaf (File)**: Simple objects that cannot contain other objects
3. **Composite (Directory, Archive)**: Complex objects that can contain other objects
4. **Client**: Uses the component interface to work with objects

## Real-World Example: File System Management

Our example demonstrates a file system where:

### Components:
- **Files**: Leaf nodes that contain data but no children
- **Directories**: Composite nodes that can contain files and other directories
- **Archives**: Special composite nodes that can contain compressed files and directories

### Operations:
- **Display**: Show the structure with proper indentation
- **Search**: Find components by name, content, or attributes
- **Size Calculation**: Calculate total size recursively
- **Permissions**: Manage access rights uniformly
- **Visitor Support**: Enable complex operations through the Visitor pattern

## Benefits Demonstrated

1. **Uniform Treatment**: Same interface for files, directories, and archives
2. **Simplified Client Code**: No need to distinguish between component types
3. **Easy Extension**: Add new component types without changing existing code
4. **Recursive Operations**: Tree operations handled automatically
5. **Flexible Hierarchies**: Build complex structures easily
6. **Pattern Integration**: Works well with Visitor and other patterns

## Implementation Structure

```csharp
// Component interface
public interface IFileSystemComponent
{
    string Name { get; }
    long Size { get; }
    void Display(int depth = 0);
    
    // Composite operations
    void Add(IFileSystemComponent component);
    void Remove(IFileSystemComponent component);
    IEnumerable<IFileSystemComponent> GetChildren();
}

// Leaf implementation
public class File : IFileSystemComponent
{
    public void Add(IFileSystemComponent component)
    {
        throw new InvalidOperationException("Cannot add to a file");
    }
    // Other operations...
}

// Composite implementation
public class Directory : IFileSystemComponent
{
    private readonly List<IFileSystemComponent> _children = new();
    
    public void Add(IFileSystemComponent component)
    {
        _children.Add(component);
    }
    
    public long Size => _children.Sum(c => c.Size);
    // Other operations...
}
```

## When to Use Composite Pattern

✅ **Use Composite when:**
- You need to represent part-whole hierarchies of objects
- You want clients to treat individual objects and compositions uniformly
- You're working with tree structures
- You want to simplify client code that works with complex object structures

❌ **Don't use Composite when:**
- Your object structure is not hierarchical
- You don't need uniform treatment of objects
- The overhead of the common interface is too high
- You have only one type of object

## Advanced Features

### 1. Visitor Pattern Integration
```csharp
public interface IFileSystemVisitor
{
    void VisitFile(File file);
    void VisitDirectory(Directory directory);
    void VisitArchive(Archive archive);
}

public void Accept(IFileSystemVisitor visitor)
{
    visitor.VisitDirectory(this);
    foreach (var child in _children)
    {
        child.Accept(visitor);
    }
}
```

### 2. Search Operations
```csharp
public List<IFileSystemComponent> Search(string pattern, SearchOptions options)
{
    var results = new List<IFileSystemComponent>();
    
    // Check this component
    if (MatchesPattern(pattern, options))
        results.Add(this);
    
    // Search children recursively
    foreach (var child in GetChildren())
    {
        results.AddRange(child.Search(pattern, options));
    }
    
    return results;
}
```

### 3. Permission Management
```csharp
public class FileSystemPermissions
{
    public bool CanRead { get; set; }
    public bool CanWrite { get; set; }
    public bool CanExecute { get; set; }
    public bool CanDelete { get; set; }
}
```

## Code Structure

```
Composite/
├── IFileSystemComponent.cs      # Component interface
├── File.cs                      # Leaf implementation
├── Directory.cs                 # Composite implementation
├── Archive.cs                   # Special composite implementation
├── FileSystemVisitors.cs        # Visitor implementations
├── CompositeExample.cs          # Usage demonstrations
└── CompositePatternDemo.cs      # Main demo program
```

## Performance Considerations

1. **Memory Efficiency**: Tree structures can be memory-intensive
2. **Recursive Operations**: Deep hierarchies may cause stack overflow
3. **Caching**: Cache calculated values like size for better performance
4. **Lazy Loading**: Load children only when needed

## Testing Benefits

1. **Uniform Testing**: Same test patterns for all component types
2. **Mock Components**: Easy to create test doubles
3. **Isolated Testing**: Test individual components independently
4. **Integration Testing**: Test complex hierarchies easily

## Common Variations

### 1. Safe Composite
Throws exceptions when leaf operations are called on composites:
```csharp
public void Add(IFileSystemComponent component)
{
    throw new InvalidOperationException("Cannot add to a file");
}
```

### 2. Transparent Composite
Provides default implementations for all operations:
```csharp
public virtual void Add(IFileSystemComponent component)
{
    // Default: do nothing
}
```

### 3. Parent-Aware Composite
Components maintain references to their parents:
```csharp
public IFileSystemComponent? Parent { get; set; }
```

## Learning Objectives

After studying this example, you should understand:

- When tree structures benefit from the Composite pattern
- How to design uniform interfaces for heterogeneous objects
- The trade-offs between safety and transparency in composite design
- How to implement recursive operations efficiently
- The relationship between Composite and other patterns (Visitor, Iterator)
- Performance implications of tree structures

## Extension Ideas

Try extending this example by:

1. **Adding New Component Types**: SymbolicLink, NetworkDrive, VirtualFolder
2. **Implementing File Operations**: Copy, move, rename, delete with undo
3. **Adding Metadata Support**: Extended attributes, tags, comments
4. **Creating File Watchers**: Monitor changes in the file system
5. **Implementing Compression**: Different compression algorithms for archives
6. **Adding Synchronization**: Thread-safe operations for concurrent access
7. **Creating File Filters**: Filter components based on various criteria
