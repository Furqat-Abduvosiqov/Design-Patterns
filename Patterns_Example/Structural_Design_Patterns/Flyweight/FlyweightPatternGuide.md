# Flyweight Design Pattern

The Flyweight pattern is a structural design pattern that minimizes memory usage by sharing efficiently among large numbers of similar objects. It achieves this by separating intrinsic state (shared) from extrinsic state (context-specific) and storing only the intrinsic state in flyweight objects.

## Problem

Imagine you're building a text editor that needs to display thousands or millions of characters. Without the Flyweight pattern, each character would be a separate object containing all its data:

```csharp
// ❌ Without Flyweight pattern - massive memory waste
class Character
{
    char value;           // 2 bytes
    string fontFamily;    // ~50 bytes  
    byte[] glyphData;     // ~2000 bytes (font rendering data)
    int x, y;            // 8 bytes (position)
    Color foreground;     // 4 bytes
    Color background;     // 4 bytes
    FontStyle style;      // 4 bytes
    // Total: ~2072 bytes per character
}

// For 100,000 characters: ~207 MB of memory!
```

### Issues with Direct Object Creation:

1. **Memory Explosion**: Each character object contains duplicate font/glyph data
2. **Performance Degradation**: Excessive memory allocation and garbage collection
3. **Resource Waste**: Same glyph data stored thousands of times
4. **Scalability Issues**: Memory usage grows linearly with character count
5. **Cache Inefficiency**: Poor memory locality due to large object sizes

## Solution

The Flyweight pattern suggests separating object state into two categories:

1. **Intrinsic State**: Data that can be shared among multiple objects (glyph data, font metrics)
2. **Extrinsic State**: Data that varies per object instance (position, color, style)

### Key Components:

1. **Flyweight Interface**: Defines operations that accept extrinsic state as parameters
2. **Concrete Flyweight**: Implements flyweight interface and stores intrinsic state
3. **Flyweight Factory**: Manages flyweight instances and ensures sharing
4. **Context**: Stores extrinsic state and uses flyweights

## Real-World Example: Text Editor System

Our example demonstrates a text editor where:

### Intrinsic State (Shared in Flyweights):
- **Character Value**: The actual character ('A', 'B', etc.)
- **Glyph Data**: Font rendering information
- **Character Metrics**: Width, height, baseline for different sizes
- **Character Type**: Letter, digit, punctuation, whitespace
- **Font Family**: Arial, Times New Roman, etc.

### Extrinsic State (Passed as Context):
- **Position**: X, Y coordinates in document
- **Colors**: Foreground and background colors
- **Font Size**: 12pt, 14pt, etc.
- **Font Style**: Bold, italic, underline
- **Selection State**: Whether character is selected
- **Line/Column Numbers**: Document position

## Benefits Demonstrated

1. **Massive Memory Savings**: 95%+ memory reduction for large documents
2. **Improved Performance**: Fewer objects to allocate and garbage collect
3. **Efficient Sharing**: Same character flyweight used thousands of times
4. **Scalability**: Memory usage grows with unique characters, not total characters
5. **Cache Efficiency**: Better memory locality and CPU cache utilization
6. **Factory Management**: Automatic flyweight creation and reuse

## Implementation Structure

```csharp
// Flyweight interface
public interface ICharacterFlyweight
{
    void Render(CharacterContext context); // Extrinsic state passed as parameter
    double GetWidth(int fontSize, FontStyle style);
}

// Concrete flyweight
public class CharacterFlyweight : ICharacterFlyweight
{
    private readonly CharacterIntrinsicState _intrinsicState; // Shared data
    
    public void Render(CharacterContext context)
    {
        // Use intrinsic state + extrinsic context for rendering
    }
}

// Factory ensures sharing
public class CharacterFlyweightFactory
{
    private readonly Dictionary<string, ICharacterFlyweight> _flyweights;
    
    public ICharacterFlyweight GetFlyweight(char character, string fontFamily)
    {
        var key = $"{character}|{fontFamily}";
        if (!_flyweights.ContainsKey(key))
        {
            _flyweights[key] = new CharacterFlyweight(character, fontFamily);
        }
        return _flyweights[key];
    }
}

// Context stores extrinsic state
public class CharacterContext
{
    public int X { get; set; }
    public int Y { get; set; }
    public Color ForegroundColor { get; set; }
    public FontStyle Style { get; set; }
}
```

## When to Use Flyweight Pattern

✅ **Use Flyweight when:**
- Your application creates a large number of similar objects
- Object creation is expensive due to intrinsic state
- Groups of objects can share intrinsic state
- Extrinsic state can be computed or passed as parameters
- Memory usage is a critical concern

❌ **Don't use Flyweight when:**
- Objects don't share significant intrinsic state
- Extrinsic state cannot be easily externalized
- The application creates few objects
- Memory usage is not a concern
- The complexity overhead outweighs the benefits

## Advanced Features

### 1. Specialized Flyweights
```csharp
public class WhitespaceFlyweight : CharacterFlyweight
{
    // Optimized rendering for whitespace characters
    public override void Render(CharacterContext context)
    {
        // Often just cursor movement, no actual rendering
    }
}
```

### 2. Factory Statistics
```csharp
public class FlyweightStatistics
{
    public int TotalFlyweights { get; set; }
    public int TotalRequests { get; set; }
    public double CacheHitRatio { get; set; }
    public long MemorySaved { get; set; }
}
```

### 3. Character Metrics Caching
```csharp
public class CharacterIntrinsicState
{
    public Dictionary<int, CharacterMetrics> SizeMetrics { get; }
    // Pre-calculated metrics for common font sizes
}
```

## Code Structure

```
Flyweight/
├── ICharacterFlyweight.cs      # Flyweight interface and data structures
├── CharacterFlyweight.cs       # Concrete flyweight implementations
├── FlyweightFactory.cs         # Factory for managing flyweight instances
├── TextDocument.cs             # Context class using flyweights
├── FlyweightExample.cs         # Usage demonstrations
└── FlyweightPatternDemo.cs     # Main demo program
```

## Performance Considerations

1. **Memory Efficiency**: Dramatic reduction in memory usage (95%+ savings)
2. **Creation Overhead**: Factory lookup vs. object creation trade-off
3. **Context Passing**: Slight overhead from passing extrinsic state
4. **Cache Performance**: Better CPU cache utilization due to smaller objects
5. **Garbage Collection**: Fewer objects to collect, better GC performance

## Testing Benefits

1. **Isolated Testing**: Test flyweights independently of context
2. **Factory Testing**: Verify proper sharing and cache behavior
3. **Memory Testing**: Measure actual memory savings
4. **Performance Testing**: Compare with and without flyweight pattern
5. **Context Testing**: Test various extrinsic state combinations

## Common Variations

### 1. Unshared Flyweight
Some objects in the flyweight hierarchy don't need to be shared:
```csharp
public class UnsharedCharacterFlyweight : ICharacterFlyweight
{
    // Contains both intrinsic and extrinsic state
    // Not managed by factory
}
```

### 2. Composite Flyweight
Flyweights that contain other flyweights:
```csharp
public class WordFlyweight : ITextFlyweight
{
    private readonly List<ICharacterFlyweight> _characters;
    // Represents a word as a collection of character flyweights
}
```

### 3. Flyweight with State
Flyweights that maintain some mutable state:
```csharp
public class StatefulFlyweight : ICharacterFlyweight
{
    private int _usageCount; // Tracks how often this flyweight is used
}
```

## Learning Objectives

After studying this example, you should understand:

- When memory usage becomes a critical design concern
- How to identify intrinsic vs. extrinsic state in objects
- The trade-offs between memory efficiency and code complexity
- How factory patterns enable object sharing
- The relationship between Flyweight and other patterns (Factory, Singleton)
- Performance implications of the pattern

## Extension Ideas

Try extending this example by:

1. **Adding Rich Text Features**: Bold, italic, underline, strikethrough formatting
2. **Implementing Font Management**: Multiple font families, font loading, font fallbacks
3. **Creating Graphics Flyweights**: Icons, images, shapes with shared rendering data
4. **Adding Undo/Redo**: Command pattern integration with flyweight-based documents
5. **Implementing Search/Replace**: Efficient text operations on flyweight-based content
6. **Creating Syntax Highlighting**: Color coding based on text content and context
7. **Adding Collaborative Editing**: Multi-user editing with shared flyweight instances

## Memory Usage Comparison

### Without Flyweight (100,000 characters):
- **Memory per character**: ~2,072 bytes
- **Total memory**: ~207 MB
- **Unique characters**: Irrelevant (each is separate object)

### With Flyweight (100,000 characters):
- **Flyweight memory**: ~2,048 bytes × ~95 unique characters = ~195 KB
- **Context memory**: ~64 bytes × 100,000 = ~6.4 MB  
- **Total memory**: ~6.6 MB
- **Memory savings**: ~96.8% (207 MB → 6.6 MB)

## Best Practices Demonstrated

1. **Clear State Separation**: Obvious distinction between intrinsic and extrinsic state
2. **Factory Management**: Centralized flyweight creation and sharing
3. **Immutable Flyweights**: Flyweights don't change after creation
4. **Context Efficiency**: Lightweight context objects for extrinsic state
5. **Statistics Tracking**: Monitor flyweight usage and memory savings
6. **Type Specialization**: Different flyweight types for different character categories
