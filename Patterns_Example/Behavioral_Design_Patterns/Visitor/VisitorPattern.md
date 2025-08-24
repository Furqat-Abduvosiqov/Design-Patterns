# Visitor Pattern

## Overview
The Visitor Pattern is a behavioral design pattern that lets you add further operations to objects without modifying them. It separates algorithms from the objects on which they operate, making it easy to add new operations without changing the element classes.

## Real-World Example: Document Editor
This example models a document editor with different element types (Text, Image, Table) and operations (rendering, exporting, spell-checking) that can be performed on them. The Visitor pattern allows adding new operations without modifying the element classes.

### Key Components
- **IDocumentElement**: Interface for document elements, with an `Accept` method.
- **IDocumentVisitor**: Interface for visitors, with methods for each element type.
- **TextElement, ImageElement, TableElement**: Concrete elements implementing `IDocumentElement`.
- **RenderVisitor, ExportVisitor, SpellCheckVisitor**: Concrete visitors implementing operations.

### Benefits
- **Open/Closed Principle**: Add new operations without changing element classes.
- **Single Responsibility Principle**: Separate operations from element data.
- **Extensibility**: Easily add new visitors for new operations.

## Implementation Details

### Visitor Interface
```csharp
public interface IDocumentVisitor {
    void Visit(TextElement text);
    void Visit(ImageElement image);
    void Visit(TableElement table);
}
```

### Element Interface
```csharp
public interface IDocumentElement {
    void Accept(IDocumentVisitor visitor);
}
```

### Example Usage
```csharp
var document = new List<IDocumentElement> {
    new TextElement("This is teh first paragraph."),
    new ImageElement("https://example.com/image.png", "Sample Image"),
    new TableElement(new List<string> { "Name", "Age", "teh Score" }, ...)
};

var renderVisitor = new RenderVisitor();
foreach (var element in document)
    element.Accept(renderVisitor);

var exportVisitor = new ExportVisitor();
foreach (var element in document)
    element.Accept(exportVisitor);

var spellCheckVisitor = new SpellCheckVisitor();
foreach (var element in document)
    element.Accept(spellCheckVisitor);
```

## When to Use
- When you need to perform unrelated operations on objects without changing their classes.
- When you want to keep operations separate from object structure.

## Best Practices
- Use the Visitor pattern when you expect to add new operations more often than new element types.
- Keep visitor methods focused on a single responsibility.
- Document the expected behavior of each visitor.

