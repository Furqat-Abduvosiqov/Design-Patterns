# Prototype Design Pattern

The Prototype pattern is a creational design pattern that lets you copy existing objects without making your code dependent on their classes. It allows you to create new objects by cloning existing instances rather than creating them from scratch.

## Problem

Imagine you need to create multiple similar documents in a document management system. Each document has complex initialization:

- Setting up metadata (author, creation date, tags)
- Configuring formatting and styles
- Adding default content and structure
- Setting security permissions
- Initializing collections and nested objects

### Issues with Traditional Object Creation:

1. **Expensive Initialization**: Creating objects from scratch requires complex setup
2. **Code Duplication**: Similar objects require repeating initialization code
3. **Tight Coupling**: Client code must know concrete classes and their constructors
4. **Performance Issues**: Complex object creation can be slow

```csharp
// ❌ Traditional approach - expensive and repetitive
var doc1 = new TextDocument();
doc1.SetupMetadata();
doc1.ApplyDefaultFormatting();
doc1.AddDefaultContent();
doc1.ConfigureSecurity();

var doc2 = new TextDocument();
doc2.SetupMetadata();  // Repeating the same setup
doc2.ApplyDefaultFormatting();
doc2.AddDefaultContent();
doc2.ConfigureSecurity();
```

## Solution

The Prototype pattern suggests creating objects by cloning existing instances (prototypes) instead of creating them from scratch.

### Key Components:

1. **Prototype Interface (IPrototype)**: Declares cloning methods
2. **Concrete Prototypes (Document classes)**: Implement cloning logic
3. **Prototype Registry (DocumentRegistry)**: Manages and provides access to prototypes
4. **Client**: Uses prototypes to create new objects

## Real-World Example: Document Management System

Our example demonstrates a document management system with different document types:

- **Text Documents**: Word processing documents with formatting
- **Spreadsheet Documents**: Excel-like documents with cells and formulas
- **Presentation Documents**: PowerPoint-like documents with slides
- **Document Registry**: Template system for common document types

### Benefits Demonstrated:

1. **Performance**: Cloning is faster than creating from scratch
2. **Flexibility**: Create variations of existing objects easily
3. **Template System**: Registry provides pre-configured document templates
4. **Shallow vs Deep Cloning**: Different cloning strategies for different needs
5. **Reduced Coupling**: Client doesn't need to know concrete classes

## Shallow vs Deep Cloning

### Shallow Cloning
- Copies the object but shares references to nested objects
- Fast and memory-efficient
- Changes to nested objects affect all shallow clones

### Deep Cloning
- Creates completely independent copies including nested objects
- Slower but provides complete isolation
- Changes to nested objects don't affect other clones

```csharp
// Shallow clone - shares metadata reference
var shallowClone = document.Clone();

// Deep clone - independent metadata copy
var deepClone = document.DeepClone();
```

## When to Use Prototype Pattern

✅ **Use Prototype when:**
- Object creation is expensive or complex
- You need to create many similar objects
- You want to avoid subclassing for object creation
- Objects have many possible configurations
- You need to create objects at runtime based on dynamic criteria

❌ **Don't use Prototype when:**
- Object creation is simple and fast
- Objects don't have complex initialization
- You rarely need to create similar objects
- Deep cloning is complex due to circular references

## Code Structure

```
Prototype/
├── IPrototype.cs                    # Prototype interface
├── Document.cs                      # Abstract document base class
├── TextDocument.cs                  # Concrete text document
├── SpreadsheetDocument.cs           # Concrete spreadsheet document
├── PresentationDocument.cs          # Concrete presentation document
├── DocumentMetadata.cs              # Complex nested object
├── DocumentSecurity.cs              # Security settings object
├── DocumentRegistry.cs              # Prototype registry/manager
├── PrototypeExample.cs              # Usage examples
└── PrototypePatternDemo.cs          # Main demo program
```

## Key Features Demonstrated

### 1. Template System
Pre-configured document templates for common use cases:
- Business letter template
- Meeting minutes template
- Budget spreadsheet template
- Project presentation template
- Formal report template

### 2. Performance Benefits
Cloning complex documents is significantly faster than creating from scratch:
- Avoids expensive initialization
- Reduces object creation overhead
- Enables efficient template-based document creation

### 3. Flexible Cloning Strategies
- **Shallow cloning** for performance when sharing is acceptable
- **Deep cloning** for complete independence
- **Registry-based cloning** for template management

### 4. Real-World Complexity
Documents include realistic features:
- Rich metadata with nested objects
- Security settings and permissions
- Complex data structures (slides, cells, formatting)
- Collections and relationships

## Usage Examples

```csharp
// Create from template
var registry = new DocumentRegistry();
var letter = registry.CreateDocumentDeepCopy("business-letter");

// Clone existing document
var originalDoc = new TextDocument("Report", "Content...");
var clonedDoc = originalDoc.DeepClone();

// Modify clone independently
clonedDoc.Title = "Modified Report";
clonedDoc.Metadata.Author = "Different Author";
```

## Learning Objectives

After studying this example, you should understand:

- When cloning is more efficient than construction
- The difference between shallow and deep cloning
- How to implement both cloning strategies
- The role of prototype registries in managing templates
- Performance benefits of the Prototype pattern
- How to handle complex object graphs during cloning

## Extension Ideas

Try extending this example by:

1. Adding a `PDFDocument` type with page-based content
2. Implementing a `DocumentVersioning` system using prototypes
3. Creating a `DocumentBuilder` that uses prototypes as starting points
4. Adding serialization support for persistent prototype storage
5. Implementing a `DocumentComparison` tool for cloned documents
