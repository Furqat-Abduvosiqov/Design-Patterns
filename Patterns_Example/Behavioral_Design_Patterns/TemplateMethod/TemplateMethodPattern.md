# Template Method Pattern

## Overview
The Template Method Pattern is a behavioral design pattern that defines the skeleton of an algorithm in a base class, allowing subclasses to override specific steps without changing the overall workflow. This pattern promotes code reuse and enforces a consistent process while enabling customization.

## Real-World Example: Data Import Framework
In this example, a data import framework uses the Template Method pattern to standardize the process of importing data from different formats (CSV, JSON, XML). The steps—reading, parsing, validating, and saving—are fixed in the base class, but each format provides its own implementation for these steps.

### Key Components
- **DataImporter (abstract class):** Defines the template method `Import` and abstract steps.
- **CsvDataImporter, JsonDataImporter, XmlDataImporter:** Concrete classes implementing format-specific logic.

### Benefits
- **Code Reuse:** Common workflow is implemented once in the base class.
- **Consistency:** All importers follow the same process.
- **Extensibility:** New formats can be added by subclassing and implementing the required steps.

## Implementation Details

### Template Method
```csharp
public void Import(string filePath) {
    var rawData = ReadFile(filePath);
    var parsedData = Parse(rawData);
    if (!Validate(parsedData)) {
        Console.WriteLine($"Validation failed for {filePath}");
        return;
    }
    Save(parsedData);
    Console.WriteLine($"Import completed for {filePath}");
}
```

### Example Usage
```csharp
var csvImporter = new CsvDataImporter();
csvImporter.Import("data.csv");

var jsonImporter = new JsonDataImporter();
jsonImporter.Import("data.json");

var xmlImporter = new XmlDataImporter();
xmlImporter.Import("data.xml");
```

## When to Use
- When you have a fixed workflow but need to customize individual steps for different scenarios.
- When you want to enforce a consistent process across multiple implementations.

## Best Practices
- Keep the template method non-virtual to prevent changes to the workflow.
- Use abstract or protected virtual methods for steps that need customization.
- Document the expected behavior of each step for subclass implementers.

