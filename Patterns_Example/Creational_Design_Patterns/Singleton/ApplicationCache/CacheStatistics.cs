namespace Patterns_Example.Creational_Design_Patterns.Singleton.ApplicationCache;

/// <summary>
/// Cache statistics information.
/// </summary>
public class CacheStatistics
{
    public int TotalItems { get; set; }
    public int ValidItems { get; set; }
    public int ExpiredItems { get; set; }
    public long TotalAccessCount { get; set; }
    public double AverageAccessCount { get; set; }
    public DateTime? OldestItem { get; set; }
    public DateTime? NewestItem { get; set; }
    public string MostAccessedKey { get; set; } = string.Empty;

    public override string ToString()
    {
        var hitRate = TotalAccessCount > 0 ? (double)ValidItems / TotalItems * 100 : 0;
        
        return $"""
                Cache Statistics:
                  Total Items: {TotalItems} (Valid: {ValidItems}, Expired: {ExpiredItems})
                  Total Access Count: {TotalAccessCount}
                  Average Access Count: {AverageAccessCount:F1}
                  Hit Rate: {hitRate:F1}%
                  Most Accessed: {MostAccessedKey}
                  Age Range: {OldestItem:HH:mm:ss} - {NewestItem:HH:mm:ss}
                """;
    }
}