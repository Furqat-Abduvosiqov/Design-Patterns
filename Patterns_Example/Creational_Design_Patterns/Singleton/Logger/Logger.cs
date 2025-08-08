namespace Patterns_Example.Creational_Design_Patterns.Singleton.Logger;

/// <summary>
/// Thread-safe Singleton Logger implementation.
/// Demonstrates another common use case for the Singleton pattern.
/// </summary>
public sealed class Logger
{
    private static readonly Lazy<Logger> _instance = new Lazy<Logger>(() => new Logger());
    private readonly object _logLock = new object();
    private readonly List<LogEntry> _logEntries;
    private LogLevel _minimumLogLevel;

    public static Logger Instance => _instance.Value;
    
    public LogLevel MinimumLogLevel 
    { 
        get => _minimumLogLevel; 
        set => _minimumLogLevel = value; 
    }

    public int LogCount 
    { 
        get 
        { 
            lock (_logLock) 
            { 
                return _logEntries.Count; 
            } 
        } 
    }

    /// <summary>
    /// Private constructor prevents direct instantiation.
    /// </summary>
    private Logger()
    {
        _logEntries = new List<LogEntry>();
        _minimumLogLevel = LogLevel.Information;
    }

    /// <summary>
    /// Logs a message with the specified level.
    /// </summary>
    public void Log(LogLevel level, string message, string? category = null)
    {
        if (level < _minimumLogLevel)
            return;

        var entry = new LogEntry
        {
            Timestamp = DateTime.Now,
            Level = level,
            Message = message,
            Category = category ?? "General",
            ThreadId = Thread.CurrentThread.ManagedThreadId
        };

        lock (_logLock)
        {
            _logEntries.Add(entry);
            
            // In a real implementation, you might write to file, database, or external service
            Console.WriteLine($"[{entry.Timestamp:HH:mm:ss.fff}] [{entry.Level}] [{entry.Category}] {entry.Message}");
        }
    }

    /// <summary>
    /// Logs a debug message.
    /// </summary>
    public void Debug(string message, string? category = null)
    {
        Log(LogLevel.Debug, message, category);
    }

    /// <summary>
    /// Logs an information message.
    /// </summary>
    public void Info(string message, string? category = null)
    {
        Log(LogLevel.Information, message, category);
    }

    /// <summary>
    /// Logs a warning message.
    /// </summary>
    public void Warning(string message, string? category = null)
    {
        Log(LogLevel.Warning, message, category);
    }

    /// <summary>
    /// Logs an error message.
    /// </summary>
    public void Error(string message, string? category = null)
    {
        Log(LogLevel.Error, message, category);
    }

    /// <summary>
    /// Logs a critical error message.
    /// </summary>
    public void Critical(string message, string? category = null)
    {
        Log(LogLevel.Critical, message, category);
    }

    /// <summary>
    /// Gets all log entries.
    /// </summary>
    public List<LogEntry> GetLogEntries()
    {
        lock (_logLock)
        {
            return new List<LogEntry>(_logEntries);
        }
    }

    /// <summary>
    /// Gets log entries filtered by level.
    /// </summary>
    public List<LogEntry> GetLogEntries(LogLevel level)
    {
        lock (_logLock)
        {
            return _logEntries.Where(e => e.Level == level).ToList();
        }
    }

    /// <summary>
    /// Gets log entries filtered by category.
    /// </summary>
    public List<LogEntry> GetLogEntries(string category)
    {
        lock (_logLock)
        {
            return _logEntries.Where(e => e.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }

    /// <summary>
    /// Clears all log entries.
    /// </summary>
    public void ClearLogs()
    {
        lock (_logLock)
        {
            _logEntries.Clear();
        }
    }

    /// <summary>
    /// Gets log statistics.
    /// </summary>
    public LogStatistics GetStatistics()
    {
        lock (_logLock)
        {
            var stats = new LogStatistics
            {
                TotalEntries = _logEntries.Count,
                DebugCount = _logEntries.Count(e => e.Level == LogLevel.Debug),
                InfoCount = _logEntries.Count(e => e.Level == LogLevel.Information),
                WarningCount = _logEntries.Count(e => e.Level == LogLevel.Warning),
                ErrorCount = _logEntries.Count(e => e.Level == LogLevel.Error),
                CriticalCount = _logEntries.Count(e => e.Level == LogLevel.Critical),
                Categories = _logEntries.Select(e => e.Category).Distinct().ToList(),
                FirstEntry = _logEntries.FirstOrDefault()?.Timestamp,
                LastEntry = _logEntries.LastOrDefault()?.Timestamp
            };
            
            return stats;
        }
    }

    public override string ToString()
    {
        return $"Logger: {LogCount} entries, Min Level: {MinimumLogLevel}";
    }
}