namespace Patterns_Example.Creational_Design_Patterns.Singleton.ApplicationCache;

/// <summary>
/// Represents an item in the cache.
/// </summary>
public class CacheItem
{
    public object? Value { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime LastAccessed { get; set; }
    public int AccessCount { get; set; }

    public TimeSpan Age => DateTime.Now - CreatedAt;
    public TimeSpan TimeToExpiration => ExpiresAt - DateTime.Now;
    public bool IsExpired => DateTime.Now >= ExpiresAt;

    public override string ToString()
    {
        return $"CacheItem: Created {CreatedAt:HH:mm:ss}, Expires {ExpiresAt:HH:mm:ss}, Accessed {AccessCount} times";
    }
}