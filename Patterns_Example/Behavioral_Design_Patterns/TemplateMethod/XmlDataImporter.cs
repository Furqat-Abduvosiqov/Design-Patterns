using Patterns_Example.Behavioral_Design_Patterns.TemplateMethod.Abstractions;
using System.Xml.Linq;

namespace Patterns_Example.Behavioral_Design_Patterns.TemplateMethod;

public class XmlDataImporter : DataImporter
{
    protected override string ReadFile(string filePath)
    {
        Console.WriteLine($"Reading XML file: {filePath}");
        return "<People><Person><Name>Alice</Name><Age>30</Age></Person><Person><Name>Bob</Name><Age>25</Age></Person></People>"; // Simulated content
    }

    protected override object Parse(string rawData)
    {
        Console.WriteLine("Parsing XML data...");
        var xdoc = XDocument.Parse(rawData);
        var data = xdoc.Descendants("Person")
            .Select(p => new Dictionary<string, string>
            {
                ["name"] = p.Element("Name")?.Value ?? "",
                ["age"] = p.Element("Age")?.Value ?? ""
            }).ToList();
        return data;
    }

    protected override bool Validate(object parsedData)
    {
        Console.WriteLine("Validating XML data...");
        var data = parsedData as List<Dictionary<string, string>>;
        return data != null && data.All(row => !string.IsNullOrEmpty(row["name"]) && !string.IsNullOrEmpty(row["age"]));
    }

    protected override void Save(object parsedData)
    {
        Console.WriteLine("Saving XML data to database...");
        // Simulate save
    }
}

