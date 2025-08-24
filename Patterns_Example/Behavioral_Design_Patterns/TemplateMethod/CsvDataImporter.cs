using Patterns_Example.Behavioral_Design_Patterns.TemplateMethod.Abstractions;

namespace Patterns_Example.Behavioral_Design_Patterns.TemplateMethod;

public class CsvDataImporter : DataImporter
{
    protected override string ReadFile(string filePath)
    {
        Console.WriteLine($"Reading CSV file: {filePath}");
        return "name,age\nAlice,30\nBob,25"; // Simulated content
    }

    protected override object Parse(string rawData)
    {
        Console.WriteLine("Parsing CSV data...");
        var lines = rawData.Split('\n');
        var headers = lines[0].Split(',');
        var data = new List<Dictionary<string, string>>();
        for (int i = 1; i < lines.Length; i++)
        {
            var values = lines[i].Split(',');
            var row = new Dictionary<string, string>();
            for (int j = 0; j < headers.Length; j++)
                row[headers[j]] = values[j];
            data.Add(row);
        }
        return data;
    }

    protected override bool Validate(object parsedData)
    {
        Console.WriteLine("Validating CSV data...");
        var data = parsedData as List<Dictionary<string, string>>;
        return data != null && data.All(row => row.ContainsKey("name") && row.ContainsKey("age"));
    }

    protected override void Save(object parsedData)
    {
        Console.WriteLine("Saving CSV data to database...");
        // Simulate save
    }
}

