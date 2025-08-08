namespace Patterns_Example.Creational_Design_Patterns.Prototype;

/// <summary>
/// Concrete implementation of a spreadsheet document.
/// Demonstrates prototype pattern with complex data structures.
/// </summary>
public class SpreadsheetDocument : Document
{
    public override string DocumentType => "Spreadsheet";
    public int Rows { get; set; } = 100;
    public int Columns { get; set; } = 26;
    public Dictionary<string, string> Cells { get; set; } = new();
    public List<string> Worksheets { get; set; } = new() { "Sheet1" };
    public string ActiveWorksheet { get; set; } = "Sheet1";
    public bool HasFormulas { get; set; }
    public bool HasCharts { get; set; }

    public SpreadsheetDocument() : base() { }

    public SpreadsheetDocument(string title) : base(title, "Spreadsheet Data") { }

    public override Document DeepClone()
    {
        var clone = new SpreadsheetDocument
        {
            Id = Guid.NewGuid().ToString(),
            Title = Title,
            Content = Content,
            Metadata = Metadata.DeepClone(),
            Attachments = new List<string>(Attachments),
            Rows = Rows,
            Columns = Columns,
            Cells = new Dictionary<string, string>(Cells),
            Worksheets = new List<string>(Worksheets),
            ActiveWorksheet = ActiveWorksheet,
            HasFormulas = HasFormulas,
            HasCharts = HasCharts
        };
        
        return clone;
    }

    public void SetCellValue(string cellAddress, string value)
    {
        Cells[cellAddress] = value;
        UpdateLastModified();
    }

    public string GetCellValue(string cellAddress)
    {
        return Cells.TryGetValue(cellAddress, out var value) ? value : string.Empty;
    }

    public void AddWorksheet(string name)
    {
        if (!Worksheets.Contains(name))
        {
            Worksheets.Add(name);
            UpdateLastModified();
        }
    }

    public void SetActiveWorksheet(string name)
    {
        if (Worksheets.Contains(name))
        {
            ActiveWorksheet = name;
            UpdateLastModified();
        }
    }

    public override string ToString()
    {
        var baseStr = base.ToString();
        var sheetsStr = string.Join(", ", Worksheets);
        var cellCount = Cells.Count;
        
        var spreadsheetInfo = $"""
            Spreadsheet Info:
              Dimensions: {Rows} x {Columns}
              Worksheets: {sheetsStr}
              Active Sheet: {ActiveWorksheet}
              Cells with Data: {cellCount}
              Has Formulas: {HasFormulas}
              Has Charts: {HasCharts}
            """;
        
        return baseStr + spreadsheetInfo;
    }
}
