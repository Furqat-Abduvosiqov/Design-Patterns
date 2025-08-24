namespace Patterns_Example.Behavioral_Design_Patterns.TemplateMethod.Abstractions;

public abstract class DataImporter
{
    // Template method
    public void Import(string filePath)
    {
        var rawData = ReadFile(filePath);
        var parsedData = Parse(rawData);
        if (!Validate(parsedData))
        {
            Console.WriteLine($"Validation failed for {filePath}");
            return;
        }
        Save(parsedData);
        Console.WriteLine($"Import completed for {filePath}");
    }

    protected abstract string ReadFile(string filePath);
    protected abstract object Parse(string rawData);
    protected abstract bool Validate(object parsedData);
    protected abstract void Save(object parsedData);
}

