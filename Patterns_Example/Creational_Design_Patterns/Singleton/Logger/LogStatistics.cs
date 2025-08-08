namespace Patterns_Example.Creational_Design_Patterns.Singleton.Logger;

/// <summary>
/// Log statistics information.
/// </summary>
public class LogStatistics
{
    public int TotalEntries { get; set; }
    public int DebugCount { get; set; }
    public int InfoCount { get; set; }
    public int WarningCount { get; set; }
    public int ErrorCount { get; set; }
    public int CriticalCount { get; set; }
    public List<string> Categories { get; set; } = new();
    public DateTime? FirstEntry { get; set; }
    public DateTime? LastEntry { get; set; }

    public override string ToString()
    {
        var duration = FirstEntry.HasValue && LastEntry.HasValue 
            ? (LastEntry.Value - FirstEntry.Value).TotalSeconds 
            : 0;
            
        return $"""
                Log Statistics:
                  Total Entries: {TotalEntries}
                  Debug: {DebugCount}, Info: {InfoCount}, Warning: {WarningCount}
                  Error: {ErrorCount}, Critical: {CriticalCount}
                  Categories: {string.Join(", ", Categories)}
                  Duration: {duration:F1} seconds
                """;
    }
}