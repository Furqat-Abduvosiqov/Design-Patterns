namespace Patterns_Example.Creational_Design_Patterns.Singleton.Logger;

/// <summary>
/// Represents a log entry.
/// </summary>
public class LogEntry
{
    public DateTime Timestamp { get; set; }
    public LogLevel Level { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int ThreadId { get; set; }

    public override string ToString()
    {
        return $"[{Timestamp:HH:mm:ss.fff}] [{Level}] [{Category}] {Message}";
    }
}