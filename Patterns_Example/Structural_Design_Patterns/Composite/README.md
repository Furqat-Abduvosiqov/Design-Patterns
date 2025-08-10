# Composite Pattern Example

This folder contains a comprehensive implementation of the Composite design pattern using a real-world file system management system that demonstrates tree structure handling.

## Files Overview

- **`IFileSystemComponent.cs`** - Component interface defining common operations for all file system objects
- **`File.cs`** - Leaf implementation representing individual files
- **`Directory.cs`** - Composite implementation representing directories that can contain other components
- **`Archive.cs`** - Special composite implementation representing archive files with compression
- **`FileSystemVisitors.cs`** - Visitor pattern implementations for various file system operations
- **`CompositeExample.cs`** - Comprehensive usage demonstrations
- **`CompositePatternDemo.cs`** - Main demo program
- **`CompositePatternGuide.md`** - Complete pattern explanation

## What Problem Does This Solve?

The Composite pattern addresses the complexity of working with tree structures where you need to treat individual objects and collections uniformly. Instead of:

```csharp
// ❌ Different handling for each type - complex and error-prone
if (item is File file)
{
    Console.WriteLine($"File: {file.Name} ({file.Size} bytes)");
    ProcessFile(file);
}
else if (item is Directory directory)
{
    Console.WriteLine($"Directory: {directory.Name}");
    foreach (var child in directory.GetChildren())
    {
        // Need to handle each type again recursively...
        if (child is File childFile) { /* ... */ }
        else if (child is Directory childDir) { /* ... */ }
    }
}
```

You can use:

```csharp
// ✅ Composite pattern - uniform treatment
foreach (var component in fileSystemComponents)
{
    component.Display(); // Same method for files, directories, archives
    var size = component.Size; // Same property for all types
    var results = component.Search("pattern", options); // Same interface
}
```

## Key Benefits Demonstrated

1. **Uniform Treatment**: Same interface for files, directories, and archives
2. **Simplified Client Code**: No need to distinguish between component types
3. **Recursive Operations**: Tree operations handled automatically
4. **Easy Extension**: Add new component types without changing existing code
5. **Flexible Hierarchies**: Build complex nested structures easily
6. **Pattern Integration**: Works seamlessly with Visitor and other patterns

## Real-World Use Cases

This pattern is commonly used for:

- **File Systems**: Files, directories, symbolic links, archives
- **UI Component Trees**: Windows, panels, buttons, containers
- **Organization Hierarchies**: Companies, departments, teams, employees
- **Document Structures**: Documents, sections, paragraphs, text
- **Menu Systems**: Menus, submenus, menu items
- **Expression Trees**: Mathematical expressions, operators, operands

## Components Demonstrated

### 1. Component Interface (`IFileSystemComponent`)
Defines common operations for all file system objects:
```csharp
public interface IFileSystemComponent
{
    string Name { get; }
    long Size { get; }
    void Display(int depth = 0);
    List<IFileSystemComponent> Search(string pattern, SearchOptions options);
    void Add(IFileSystemComponent component);    // May throw for leaves
    void Remove(IFileSystemComponent component); // May throw for leaves
    IEnumerable<IFileSystemComponent> GetChildren();
}
```

### 2. Leaf Implementation (`File`)
Represents individual files that cannot contain other components:
- Content management (read/write operations)
- MIME type detection based on file extension
- Permission management
- Throws exceptions for composite operations (Add/Remove)

### 3. Composite Implementations

**Directory**: Standard composite that can contain any file system components
- Hierarchical structure management
- Recursive size calculation
- Child component management
- Permission inheritance

**Archive**: Special composite representing compressed files
- Compression ratio simulation
- Extraction capabilities
- Can contain files and directories
- Demonstrates how "files" can also be composites

### 4. Visitor Pattern Integration
Demonstrates how Composite works with other patterns:

**FileSystemStatisticsVisitor**: Collects statistics about the file system
- File counts by type and extension
- Size analysis and largest files
- Date range analysis

**BackupVisitor**: Simulates backup operations
- Traverses entire tree structure
- Calculates backup size and time
- Demonstrates uniform processing

**SecurityScanVisitor**: Performs security analysis
- Scans for sensitive file names
- Checks permissions
- Identifies potential security issues

## Advanced Features

### Search Operations
Powerful search capabilities across the entire tree:
```csharp
var searchOptions = new SearchOptions
{
    CaseSensitive = false,
    SearchContent = true,
    MaxDepth = 10,
    ModifiedAfter = DateTime.Now.AddDays(-7)
};

var results = rootDirectory.Search("test", searchOptions);
```

### Permission Management
Comprehensive permission system:
```csharp
public class FileSystemPermissions
{
    public bool CanRead { get; set; }
    public bool CanWrite { get; set; }
    public bool CanExecute { get; set; }
    public bool CanDelete { get; set; }
    public string Owner { get; set; }
    public string Group { get; set; }
}
```

### Archive Functionality
Special composite with compression:
```csharp
var archive = new Archive("backup.zip", "/backups/backup.zip");
archive.Add(file1);
archive.Add(directory1);
archive.Compress(0.7); // 70% compression ratio
archive.Extract(targetDirectory);
```

## How to Run

1. Navigate to the project root directory
2. Update Program.cs to use the Composite example:
   ```csharp
   using Patterns_Example.Structural_Design_Patterns.Composite;
   CompositeExample.RunExample();
   ```
3. Run the demo:
   ```bash
   dotnet run --project Patterns_Example
   ```

## Example Output

The demo will show:

1. **Basic Composite Structure**: Building a project directory with files and subdirectories
2. **Uniform Treatment**: Processing different component types with the same interface
3. **Search Operations**: Finding components by name, type, size, and content
4. **Visitor Pattern Integration**: Statistics collection, backup simulation, security scanning
5. **Archive Functionality**: Creating, compressing, and extracting archive files

## Learning Objectives

After studying this example, you should understand:

- When tree structures benefit from the Composite pattern
- How to design uniform interfaces for heterogeneous objects
- The trade-offs between safety and transparency in composite design
- How to implement recursive operations efficiently
- The relationship between Composite and other patterns (Visitor, Iterator)
- Performance implications of tree structures

## Design Decisions

### Safety vs Transparency
This implementation uses the "safety" approach where leaf nodes throw exceptions for composite operations:
```csharp
// In File.cs
public void Add(IFileSystemComponent component)
{
    throw new InvalidOperationException("Cannot add components to a file");
}
```

**Alternative**: "Transparency" approach where all components implement all operations (some as no-ops).

### Parent References
Components don't maintain parent references for simplicity, but this could be added:
```csharp
public IFileSystemComponent? Parent { get; set; }
```

### Caching
Size calculations are performed on-demand. For better performance, you could cache results:
```csharp
private long? _cachedSize;
public long Size => _cachedSize ??= CalculateSize();
```

## Extension Ideas

Try extending this example by:

1. **Adding New Component Types**:
   - `SymbolicLink` for file system links
   - `NetworkDrive` for remote file systems
   - `VirtualFolder` for dynamic content

2. **Implementing File Operations**:
   - Copy, move, rename operations
   - Undo/redo functionality
   - Batch operations

3. **Adding Metadata Support**:
   - Extended attributes and tags
   - File comments and descriptions
   - Custom metadata fields

4. **Creating Advanced Features**:
   - File watching and change notifications
   - Synchronization between file systems
   - Version control integration
   - Thumbnail generation for images

5. **Performance Optimizations**:
   - Lazy loading of directory contents
   - Caching of calculated values
   - Asynchronous operations
   - Memory-efficient tree traversal

## Best Practices Demonstrated

1. **Interface Segregation**: Clean, focused component interface
2. **Exception Handling**: Appropriate exceptions for invalid operations
3. **Resource Management**: Proper handling of file system resources
4. **Visitor Integration**: Seamless integration with other patterns
5. **Search Flexibility**: Configurable search options
6. **Permission Model**: Comprehensive security model
7. **Type Safety**: Strong typing throughout the hierarchy
