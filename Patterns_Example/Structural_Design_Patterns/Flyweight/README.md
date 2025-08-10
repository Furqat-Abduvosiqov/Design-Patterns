# Flyweight Pattern Example

This folder contains a comprehensive implementation of the Flyweight design pattern using a real-world text editor system that demonstrates how to achieve massive memory savings through object sharing.

## Files Overview

- **`ICharacterFlyweight.cs`** - Flyweight interface and data structures for character representation
- **`CharacterFlyweight.cs`** - Concrete flyweight implementations for different character types
- **`FlyweightFactory.cs`** - Factory managing flyweight instances and ensuring proper sharing
- **`TextDocument.cs`** - Context class that uses flyweights to represent text documents
- **`FlyweightExample.cs`** - Comprehensive usage demonstrations
- **`FlyweightPatternDemo.cs`** - Main demo program
- **`FlyweightPatternGuide.md`** - Complete pattern explanation

## What Problem Does This Solve?

The Flyweight pattern addresses the memory explosion problem that occurs when creating large numbers of similar objects. Instead of:

```csharp
// ❌ Memory explosion - each character is a full object
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

You can use:

```csharp
// ✅ Flyweight pattern - massive memory savings
// Intrinsic state (shared): glyph data, font metrics
// Extrinsic state (unique): position, color, style
// Result: 95%+ memory reduction (207 MB → 6.6 MB)
```

## Key Benefits Demonstrated

1. **Massive Memory Savings**: 95%+ memory reduction for large documents
2. **Improved Performance**: Fewer objects to allocate and garbage collect
3. **Efficient Sharing**: Same character flyweight used thousands of times
4. **Scalability**: Memory usage grows with unique characters, not total characters
5. **Cache Efficiency**: Better memory locality and CPU cache utilization
6. **Factory Management**: Automatic flyweight creation and reuse

## Real-World Use Cases

This pattern is commonly used for:

- **Text Editors**: Character rendering with shared glyph data
- **Game Development**: Sprites, particles, terrain tiles with shared graphics
- **Graphics Applications**: Icons, symbols, shapes with shared rendering data
- **Web Browsers**: DOM elements with shared styling information
- **Document Processors**: Formatting objects with shared style definitions
- **CAD Applications**: Drawing primitives with shared geometric data

## Components Demonstrated

### 1. Flyweight Interface (`ICharacterFlyweight`)
Defines operations that accept extrinsic state as parameters:
```csharp
public interface ICharacterFlyweight
{
    void Render(CharacterContext context);  // Extrinsic state passed as parameter
    double GetWidth(int fontSize, FontStyle style);
    CharacterIntrinsicState GetIntrinsicState();
    bool CanCombineWith(char nextChar);
}
```

### 2. Intrinsic State (Shared Data)
Data stored in flyweights and shared among instances:
- **Character Value**: The actual character ('A', 'B', etc.)
- **Glyph Data**: Font rendering information (2KB per character)
- **Character Metrics**: Width, height, baseline for different sizes
- **Character Type**: Letter, digit, punctuation, whitespace
- **Font Family**: Arial, Times New Roman, etc.
- **Unicode Category**: Character classification information

### 3. Extrinsic State (Context Data)
Data passed to flyweight methods, not stored in flyweights:
- **Position**: X, Y coordinates in document
- **Colors**: Foreground and background colors
- **Font Size**: 12pt, 14pt, etc.
- **Font Style**: Bold, italic, underline
- **Selection State**: Whether character is selected
- **Line/Column Numbers**: Document position

### 4. Concrete Flyweights

**CharacterFlyweight**: Standard implementation for letters and digits
**WhitespaceFlyweight**: Optimized for space, tab, newline characters
**PunctuationFlyweight**: Enhanced ligature support for punctuation

### 5. Flyweight Factory
Manages flyweight creation and ensures sharing:
```csharp
public class CharacterFlyweightFactory
{
    public ICharacterFlyweight GetFlyweight(char character, string fontFamily)
    {
        // Returns existing flyweight or creates new one
        // Ensures same character+font combination reuses same flyweight
    }
}
```

## Advanced Features

### Memory Usage Statistics
Comprehensive tracking of memory efficiency:
```csharp
public class FlyweightStatistics
{
    public int TotalFlyweights { get; set; }        // Unique flyweights created
    public int TotalRequests { get; set; }          // Total flyweight requests
    public double CacheHitRatio { get; set; }       // Percentage of reused flyweights
    public long MemorySaved { get; set; }           // Bytes saved vs. no flyweight
    public double MemorySavingsRatio { get; set; }  // Percentage memory savings
}
```

### Character Type Specialization
Different flyweight implementations for different character types:
- **Letters/Digits**: Standard flyweight with full metrics
- **Whitespace**: Optimized rendering (often just cursor movement)
- **Punctuation**: Enhanced ligature detection and combination rules

### Font Metrics Caching
Pre-calculated character metrics for common font sizes:
```csharp
public class CharacterIntrinsicState
{
    public Dictionary<int, CharacterMetrics> SizeMetrics { get; }
    // Cached metrics for sizes: 8, 10, 12, 14, 16, 18, 20, 24, 28, 32, 36, 48, 72
}
```

### Ligature Support
Detection of character combinations that can form ligatures:
```csharp
public bool CanCombineWith(char nextChar)
{
    // Detects combinations like "fi", "fl", "ff", "--", etc.
}
```

## How to Run

1. Navigate to the project root directory
2. Update Program.cs to use the Flyweight example:
   ```csharp
   using Patterns_Example.Structural_Design_Patterns.Flyweight;
   FlyweightExample.RunExample();
   ```
3. Run the demo:
   ```bash
   dotnet run --project Patterns_Example
   ```

## Example Output

The demo will show:

1. **Memory Efficiency Problem**: Comparison of memory usage with/without flyweight
2. **Basic Flyweight Usage**: Creating and reusing flyweights
3. **Factory Sharing**: How the factory ensures flyweight reuse
4. **Text Document**: Building documents with flyweight-based characters
5. **Memory Savings**: Dramatic memory reduction with large documents
6. **Advanced Features**: Character metrics, ligatures, type specialization

## Learning Objectives

After studying this example, you should understand:

- When memory usage becomes a critical design concern
- How to identify intrinsic vs. extrinsic state in objects
- The trade-offs between memory efficiency and code complexity
- How factory patterns enable object sharing
- The relationship between Flyweight and other patterns (Factory, Singleton)
- Performance implications of the pattern

## Memory Usage Comparison

### Without Flyweight (100,000 characters):
- **Memory per character**: ~2,072 bytes
- **Total memory**: ~207 MB
- **Problem**: Massive duplication of glyph data

### With Flyweight (100,000 characters):
- **Flyweight memory**: ~2,048 bytes × ~95 unique characters = ~195 KB
- **Context memory**: ~64 bytes × 100,000 = ~6.4 MB  
- **Total memory**: ~6.6 MB
- **Memory savings**: ~96.8% (207 MB → 6.6 MB)

## Design Decisions

### State Separation
Clear distinction between intrinsic and extrinsic state:
- **Intrinsic**: Character value, glyph data, font metrics (shared)
- **Extrinsic**: Position, color, style, selection state (unique per instance)

### Factory Pattern Integration
Singleton factory ensures proper flyweight sharing:
- Thread-safe flyweight creation and retrieval
- Statistics tracking for monitoring efficiency
- Preloading of common characters for performance

### Type Specialization
Different flyweight classes for different character categories:
- Optimized rendering for whitespace characters
- Enhanced ligature support for punctuation
- Standard implementation for letters and digits

## Extension Ideas

Try extending this example by:

1. **Adding Rich Text Features**:
   - Multiple font families and sizes
   - Advanced formatting (subscript, superscript)
   - Text effects (shadow, outline, glow)

2. **Implementing Graphics Flyweights**:
   - Icon flyweights with shared image data
   - Shape flyweights with shared geometry
   - Texture flyweights for game development

3. **Creating Document Features**:
   - Paragraph and line flyweights
   - Style sheet flyweights
   - Template flyweights for document sections

4. **Adding Performance Optimizations**:
   - Lazy loading of glyph data
   - Compressed glyph storage
   - GPU-accelerated rendering

5. **Implementing Advanced Text Features**:
   - Unicode normalization and shaping
   - Right-to-left text support
   - Complex script rendering (Arabic, Thai, etc.)

## Best Practices Demonstrated

1. **Clear State Separation**: Obvious distinction between intrinsic and extrinsic state
2. **Factory Management**: Centralized flyweight creation and sharing
3. **Immutable Flyweights**: Flyweights don't change after creation
4. **Context Efficiency**: Lightweight context objects for extrinsic state
5. **Statistics Tracking**: Monitor flyweight usage and memory savings
6. **Type Specialization**: Different flyweight types for different use cases
7. **Thread Safety**: Safe concurrent access to flyweight factory
8. **Memory Monitoring**: Track actual memory usage and savings
