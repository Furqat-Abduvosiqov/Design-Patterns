using Patterns_Example.Behavioral_Design_Patterns.TemplateMethod.Abstractions;
using System.Text.Json;

namespace Patterns_Example.Behavioral_Design_Patterns.TemplateMethod;

public class JsonDataImporter : DataImporter
{
    protected override string ReadFile(string filePath)
    {
        Console.WriteLine($"Reading JSON file: {filePath}");
        return "[{\"name\":\"Alice\",\"age\":30},{\"name\":\"Bob\",\"age\":25}]"; // Simulated content
    }

    protected override object Parse(string rawData)
    {
        Console.WriteLine("Parsing JSON data...");
        return JsonSerializer.Deserialize<List<Dictionary<string, object>>>(rawData);
    }

    protected override bool Validate(object parsedData)
    {
        Console.WriteLine("Validating JSON data...");
        var data = parsedData as List<Dictionary<string, object>>;
        return data != null && data.All(row => row.ContainsKey("name") && row.ContainsKey("age"));
    }

    protected override void Save(object parsedData)
    {
        Console.WriteLine("Saving JSON data to database...");
        // Simulate save
    }
}

