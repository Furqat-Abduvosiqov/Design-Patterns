namespace Patterns_Example.Creational_Design_Patterns.Prototype;

/// <summary>
/// The Prototype Registry provides a convenient way to access frequently-used prototypes.
/// It stores a set of pre-built objects that are ready to be copied.
/// This is an optional component that makes the Prototype pattern more convenient to use.
/// </summary>
public class DocumentRegistry
{
    private readonly Dictionary<string, Document> _prototypes = new();

    public DocumentRegistry()
    {
        InitializeDefaultPrototypes();
    }

    /// <summary>
    /// Registers a prototype with a given key
    /// </summary>
    public void RegisterPrototype(string key, Document prototype)
    {
        _prototypes[key] = prototype;
    }

    /// <summary>
    /// Creates a shallow clone of the prototype with the given key
    /// </summary>
    public Document? CreateDocument(string key)
    {
        return _prototypes.TryGetValue(key, out var prototype) ? prototype.Clone() : null;
    }

    /// <summary>
    /// Creates a deep clone of the prototype with the given key
    /// </summary>
    public Document? CreateDocumentDeepCopy(string key)
    {
        return _prototypes.TryGetValue(key, out var prototype) ? prototype.DeepClone() : null;
    }

    /// <summary>
    /// Gets all available prototype keys
    /// </summary>
    public IEnumerable<string> GetAvailablePrototypes()
    {
        return _prototypes.Keys;
    }

    /// <summary>
    /// Removes a prototype from the registry
    /// </summary>
    public bool RemovePrototype(string key)
    {
        return _prototypes.Remove(key);
    }

    /// <summary>
    /// Checks if a prototype exists in the registry
    /// </summary>
    public bool HasPrototype(string key)
    {
        return _prototypes.ContainsKey(key);
    }

    /// <summary>
    /// Gets the count of registered prototypes
    /// </summary>
    public int Count => _prototypes.Count;

    private void InitializeDefaultPrototypes()
    {
        // Create and register default document templates
        
        // Business Letter Template
        var businessLetter = new TextDocument("Business Letter Template", 
            "Dear [Recipient],\n\n[Your message here]\n\nSincerely,\n[Your name]");
        businessLetter.Metadata.Author = "Template System";
        businessLetter.Metadata.Tags.AddRange(new[] { "template", "business", "letter" });
        businessLetter.ApplyFormatting("Times New Roman", 12);
        RegisterPrototype("business-letter", businessLetter);

        // Meeting Minutes Template
        var meetingMinutes = new TextDocument("Meeting Minutes Template",
            "Meeting: [Meeting Name]\nDate: [Date]\nAttendees: [Names]\n\nAgenda:\n1. [Item 1]\n2. [Item 2]\n\nAction Items:\n- [Action 1]\n- [Action 2]");
        meetingMinutes.Metadata.Author = "Template System";
        meetingMinutes.Metadata.Tags.AddRange(new[] { "template", "meeting", "minutes" });
        meetingMinutes.ApplyFormatting("Arial", 11);
        RegisterPrototype("meeting-minutes", meetingMinutes);

        // Budget Spreadsheet Template
        var budgetSpreadsheet = new SpreadsheetDocument("Budget Template");
        budgetSpreadsheet.SetCellValue("A1", "Category");
        budgetSpreadsheet.SetCellValue("B1", "Budgeted");
        budgetSpreadsheet.SetCellValue("C1", "Actual");
        budgetSpreadsheet.SetCellValue("D1", "Variance");
        budgetSpreadsheet.SetCellValue("A2", "Income");
        budgetSpreadsheet.SetCellValue("A3", "Expenses");
        budgetSpreadsheet.Metadata.Author = "Template System";
        budgetSpreadsheet.Metadata.Tags.AddRange(new[] { "template", "budget", "finance" });
        budgetSpreadsheet.HasFormulas = true;
        RegisterPrototype("budget-spreadsheet", budgetSpreadsheet);

        // Project Presentation Template
        var projectPresentation = new PresentationDocument("Project Presentation Template");
        projectPresentation.SetTheme("Professional");
        projectPresentation.SetTransitionEffect("Fade");
        projectPresentation.AddSlide("Agenda", "• Project Overview\n• Timeline\n• Budget\n• Next Steps");
        projectPresentation.AddSlide("Project Overview", "[Project description and objectives]");
        projectPresentation.AddSlide("Timeline", "[Key milestones and deadlines]");
        projectPresentation.AddSlide("Budget", "[Cost breakdown and resources]");
        projectPresentation.AddSlide("Next Steps", "[Action items and follow-up]");
        projectPresentation.Metadata.Author = "Template System";
        projectPresentation.Metadata.Tags.AddRange(new[] { "template", "presentation", "project" });
        RegisterPrototype("project-presentation", projectPresentation);

        // Report Template
        var reportTemplate = new TextDocument("Report Template",
            "EXECUTIVE SUMMARY\n[Brief overview]\n\nINTRODUCTION\n[Background and purpose]\n\nMETHODOLOGY\n[How the work was done]\n\nFINDINGS\n[Key results]\n\nCONCLUSIONS\n[Summary and recommendations]");
        reportTemplate.Metadata.Author = "Template System";
        reportTemplate.Metadata.Tags.AddRange(new[] { "template", "report", "formal" });
        reportTemplate.ApplyFormatting("Calibri", 11);
        RegisterPrototype("formal-report", reportTemplate);
    }

    public override string ToString()
    {
        var prototypes = string.Join(", ", _prototypes.Keys);
        return $"Document Registry: {Count} prototypes ({prototypes})";
    }
}
