namespace Patterns_Example.Behavioral_Design_Patterns.TemplateMethod;

public class TemplateMethodExample
{
    public static void RunExample()
    {
        var csvImporter = new CsvDataImporter();
        csvImporter.Import("data.csv");
        Console.WriteLine();

        var jsonImporter = new JsonDataImporter();
        jsonImporter.Import("data.json");
        Console.WriteLine();

        var xmlImporter = new XmlDataImporter();
        xmlImporter.Import("data.xml");
        Console.WriteLine();
    }
}
