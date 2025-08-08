namespace Patterns_Example.Creational_Design_Patterns.Singleton.ApplicationCache;

/// <summary>
/// Thread-safe Singleton Application Cache.
/// Demonstrates the Singleton pattern for managing shared application state.
/// </summary>
public sealed class ApplicationCache
{
    private static readonly Lazy<ApplicationCache> _instance = new (() => new ApplicationCache());
    private readonly Dictionary<string, CacheItem> _cache;
    private readonly object _cacheLock = new ();
    private readonly Timer _cleanupTimer;
    private readonly int _defaultExpirationMinutes;

    public static ApplicationCache Instance => _instance.Value;
    
    public int Count 
    { 
        get 
        { 
            lock (_cacheLock) 
            { 
                return _cache.Count; 
            } 
        } 
    }

    /// <summary>
    /// Private constructor prevents direct instantiation.
    /// </summary>
    private ApplicationCache()
    {
        _cache = new Dictionary<string, CacheItem>();
        _defaultExpirationMinutes = ConfigurationManager.ConfigurationManager.Instance.GetSetting<int>("CacheExpirationMinutes", 60);
        
        // Setup cleanup timer to run every 5 minutes
        _cleanupTimer = new Timer(CleanupExpiredItems, null, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5));
        
        Logger.Logger.Instance.Info($"Application cache initialized with {_defaultExpirationMinutes} minute default expiration", "Cache");
    }

    /// <summary>
    /// Adds or updates an item in the cache.
    /// </summary>
    public void Set<T>(string key, T value, TimeSpan? expiration = null)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentException("Key cannot be null or empty", nameof(key));

        var expirationTime = expiration ?? TimeSpan.FromMinutes(_defaultExpirationMinutes);
        var cacheItem = new CacheItem
        {
            Value = value,
            CreatedAt = DateTime.Now,
            ExpiresAt = DateTime.Now.Add(expirationTime),
            AccessCount = 0,
            LastAccessed = DateTime.Now
        };

        lock (_cacheLock)
        {
            _cache[key] = cacheItem;
            Logger.Logger.Instance.Debug($"Cached item '{key}' with expiration {cacheItem.ExpiresAt:HH:mm:ss}", "Cache");
        }
    }

    /// <summary>
    /// Gets an item from the cache.
    /// </summary>
    public T? Get<T>(string key)
    {
        if (string.IsNullOrEmpty(key))
            return default(T);

        lock (_cacheLock)
        {
            if (_cache.TryGetValue(key, out var cacheItem))
            {
                if (cacheItem.ExpiresAt > DateTime.Now)
                {
                    cacheItem.AccessCount++;
                    cacheItem.LastAccessed = DateTime.Now;
                    Logger.Logger.Instance.Debug($"Cache hit for '{key}' (accessed {cacheItem.AccessCount} times)", "Cache");
                    return (T?)cacheItem.Value;
                }
                else
                {
                    // Item expired, remove it
                    _cache.Remove(key);
                    Logger.Logger.Instance.Debug($"Cache item '{key}' expired and removed", "Cache");
                }
            }
            else
            {
                Logger.Logger.Instance.Debug($"Cache miss for '{key}'", "Cache");
            }
        }

        return default(T);
    }

    /// <summary>
    /// Checks if an item exists in the cache and is not expired.
    /// </summary>
    public bool Contains(string key)
    {
        if (string.IsNullOrEmpty(key))
            return false;

        lock (_cacheLock)
        {
            if (_cache.TryGetValue(key, out var cacheItem))
            {
                if (cacheItem.ExpiresAt > DateTime.Now)
                {
                    return true;
                }
                else
                {
                    // Item expired, remove it
                    _cache.Remove(key);
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Removes an item from the cache.
    /// </summary>
    public bool Remove(string key)
    {
        if (string.IsNullOrEmpty(key))
            return false;

        lock (_cacheLock)
        {
            var removed = _cache.Remove(key);
            if (removed)
            {
                Logger.Logger.Instance.Debug($"Removed cache item '{key}'", "Cache");
            }
            return removed;
        }
    }

    /// <summary>
    /// Clears all items from the cache.
    /// </summary>
    public void Clear()
    {
        lock (_cacheLock)
        {
            var count = _cache.Count;
            _cache.Clear();
            Logger.Logger.Instance.Info($"Cleared {count} items from cache", "Cache");
        }
    }

    /// <summary>
    /// Gets cache statistics.
    /// </summary>
    public CacheStatistics GetStatistics()
    {
        lock (_cacheLock)
        {
            var now = DateTime.Now;
            var items = _cache.Values.ToList();
            
            return new CacheStatistics
            {
                TotalItems = items.Count,
                ExpiredItems = items.Count(i => i.ExpiresAt <= now),
                ValidItems = items.Count(i => i.ExpiresAt > now),
                TotalAccessCount = items.Sum(i => i.AccessCount),
                AverageAccessCount = items.Count > 0 ? items.Average(i => i.AccessCount) : 0,
                OldestItem = items.MinBy(i => i.CreatedAt)?.CreatedAt,
                NewestItem = items.MaxBy(i => i.CreatedAt)?.CreatedAt,
                MostAccessedKey = items.MaxBy(i => i.AccessCount)?.ToString() ?? "None"
            };
        }
    }

    /// <summary>
    /// Gets all cache keys.
    /// </summary>
    public List<string> GetKeys()
    {
        lock (_cacheLock)
        {
            return new List<string>(_cache.Keys);
        }
    }

    /// <summary>
    /// Gets cache items that match a predicate.
    /// </summary>
    public Dictionary<string, T> GetItems<T>(Func<string, CacheItem, bool> predicate)
    {
        lock (_cacheLock)
        {
            var result = new Dictionary<string, T>();
            var now = DateTime.Now;

            foreach (var kvp in _cache)
            {
                if (kvp.Value.ExpiresAt > now && predicate(kvp.Key, kvp.Value) && kvp.Value.Value is T)
                {
                    result[kvp.Key] = (T)kvp.Value.Value;
                }
            }

            return result;
        }
    }

    /// <summary>
    /// Cleanup timer callback to remove expired items.
    /// </summary>
    private void CleanupExpiredItems(object? state)
    {
        lock (_cacheLock)
        {
            var now = DateTime.Now;
            var expiredKeys = _cache.Where(kvp => kvp.Value.ExpiresAt <= now).Select(kvp => kvp.Key).ToList();

            foreach (var key in expiredKeys)
            {
                _cache.Remove(key);
            }

            if (expiredKeys.Count > 0)
            {
                Logger.Logger.Instance.Info($"Cleaned up {expiredKeys.Count} expired cache items", "Cache");
            }
        }
    }

    public override string ToString()
    {
        return $"ApplicationCache: {Count} items, Default expiration: {_defaultExpirationMinutes} minutes";
    }
}