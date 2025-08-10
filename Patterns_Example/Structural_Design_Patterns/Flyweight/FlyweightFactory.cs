namespace Patterns_Example.Structural_Design_Patterns.Flyweight;

/// <summary>
/// Factory that manages flyweight instances and ensures that flyweights are shared properly.
/// This is a crucial component of the Flyweight pattern - it ensures that identical flyweights
/// are reused rather than creating new instances.
/// </summary>
public class CharacterFlyweightFactory
{
    private readonly Dictionary<string, ICharacterFlyweight> _flyweights;
    private readonly object _lock = new object();
    private static CharacterFlyweightFactory? _instance;
    private static readonly object _instanceLock = new object();

    // Statistics for monitoring flyweight usage
    public int TotalFlyweights => _flyweights.Count;
    public int TotalRequests { get; private set; }
    public int CacheHits { get; private set; }
    public int CacheMisses { get; private set; }

    private CharacterFlyweightFactory()
    {
        _flyweights = new Dictionary<string, ICharacterFlyweight>();
    }

    /// <summary>
    /// Gets the singleton instance of the flyweight factory.
    /// </summary>
    public static CharacterFlyweightFactory Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_instanceLock)
                {
                    _instance ??= new CharacterFlyweightFactory();
                }
            }
            return _instance;
        }
    }

    /// <summary>
    /// Gets or creates a flyweight for the specified character and font family.
    /// This method ensures that identical flyweights are reused.
    /// </summary>
    public ICharacterFlyweight GetFlyweight(char character, string fontFamily = "Arial")
    {
        var key = CreateKey(character, fontFamily);
        
        lock (_lock)
        {
            TotalRequests++;
            
            if (_flyweights.TryGetValue(key, out var existingFlyweight))
            {
                CacheHits++;
                return existingFlyweight;
            }
            
            // Create new flyweight based on character type
            ICharacterFlyweight newFlyweight = CreateFlyweight(character, fontFamily);
            _flyweights[key] = newFlyweight;
            CacheMisses++;
            
            return newFlyweight;
        }
    }

    /// <summary>
    /// Gets flyweights for an entire string, reusing existing flyweights where possible.
    /// </summary>
    public IEnumerable<ICharacterFlyweight> GetFlyweights(string text, string fontFamily = "Arial")
    {
        if (string.IsNullOrEmpty(text))
        {
            return Enumerable.Empty<ICharacterFlyweight>();
        }

        var flyweights = new List<ICharacterFlyweight>(text.Length);
        
        foreach (char character in text)
        {
            flyweights.Add(GetFlyweight(character, fontFamily));
        }
        
        return flyweights;
    }

    /// <summary>
    /// Preloads flyweights for common characters to improve performance.
    /// </summary>
    public void PreloadCommonCharacters(string fontFamily = "Arial")
    {
        // Preload ASCII printable characters (32-126)
        for (int i = 32; i <= 126; i++)
        {
            GetFlyweight((char)i, fontFamily);
        }
        
        // Preload common Unicode characters
        var commonChars = new[]
        {
            // Common punctuation and symbols
            '\u2026', '\u2013', '\u2014', '\u2018', '\u2019', '\u201C', '\u201D', '\u2022', '\u00A9', '\u00AE', '\u2122',
            // Common accented characters
            'á', 'à', 'â', 'ä', 'ã', 'å', 'æ', 'ç', 'é', 'è', 'ê', 'ë',
            'í', 'ì', 'î', 'ï', 'ñ', 'ó', 'ò', 'ô', 'ö', 'õ', 'ø', 'œ',
            'ú', 'ù', 'û', 'ü', 'ý', 'ÿ', 'ß',
            // Currency symbols
            '€', '£', '¥', '¢', '\u20B9', '\u20BD'
        };
        
        foreach (char character in commonChars)
        {
            GetFlyweight(character, fontFamily);
        }
    }

    /// <summary>
    /// Gets statistics about flyweight usage and memory efficiency.
    /// </summary>
    public FlyweightStatistics GetStatistics()
    {
        lock (_lock)
        {
            var stats = new FlyweightStatistics
            {
                TotalFlyweights = TotalFlyweights,
                TotalRequests = TotalRequests,
                CacheHits = CacheHits,
                CacheMisses = CacheMisses,
                CacheHitRatio = TotalRequests > 0 ? (double)CacheHits / TotalRequests : 0,
                MemorySavingsRatio = TotalRequests > 0 ? 1.0 - ((double)TotalFlyweights / TotalRequests) : 0
            };

            // Calculate memory usage estimates
            stats.EstimatedMemoryUsage = CalculateEstimatedMemoryUsage();
            stats.EstimatedMemoryWithoutFlyweight = CalculateMemoryWithoutFlyweight();
            stats.MemorySaved = stats.EstimatedMemoryWithoutFlyweight - stats.EstimatedMemoryUsage;

            // Analyze character distribution
            stats.CharacterDistribution = AnalyzeCharacterDistribution();

            return stats;
        }
    }

    /// <summary>
    /// Clears all cached flyweights. Use with caution as this will force recreation of flyweights.
    /// </summary>
    public void ClearCache()
    {
        lock (_lock)
        {
            _flyweights.Clear();
            TotalRequests = 0;
            CacheHits = 0;
            CacheMisses = 0;
        }
    }

    /// <summary>
    /// Gets all currently cached flyweights grouped by character type.
    /// </summary>
    public Dictionary<CharacterType, List<ICharacterFlyweight>> GetFlyweightsByType()
    {
        lock (_lock)
        {
            var result = new Dictionary<CharacterType, List<ICharacterFlyweight>>();
            
            foreach (var flyweight in _flyweights.Values)
            {
                var intrinsicState = flyweight.GetIntrinsicState();
                var type = intrinsicState.Type;
                
                if (!result.ContainsKey(type))
                {
                    result[type] = new List<ICharacterFlyweight>();
                }
                
                result[type].Add(flyweight);
            }
            
            return result;
        }
    }

    private static string CreateKey(char character, string fontFamily)
    {
        return $"{character}|{fontFamily}";
    }

    private static ICharacterFlyweight CreateFlyweight(char character, string fontFamily)
    {
        // Create specialized flyweights based on character type
        if (char.IsWhiteSpace(character))
        {
            return new WhitespaceFlyweight(character, fontFamily);
        }
        
        if (char.IsPunctuation(character) || char.IsSymbol(character))
        {
            return new PunctuationFlyweight(character, fontFamily);
        }
        
        // Default flyweight for letters, digits, and other characters
        return new CharacterFlyweight(character, fontFamily);
    }

    private long CalculateEstimatedMemoryUsage()
    {
        // Estimate memory usage with flyweight pattern
        // Each flyweight contains intrinsic state (glyph data, metrics, etc.)
        const long averageFlyweightSize = 2048; // Estimated bytes per flyweight
        return TotalFlyweights * averageFlyweightSize;
    }

    private long CalculateMemoryWithoutFlyweight()
    {
        // Estimate memory usage without flyweight pattern
        // Each character instance would contain full character data
        const long averageCharacterInstanceSize = 2048; // Same data per instance
        return TotalRequests * averageCharacterInstanceSize;
    }

    private Dictionary<CharacterType, int> AnalyzeCharacterDistribution()
    {
        var distribution = new Dictionary<CharacterType, int>();
        
        foreach (var flyweight in _flyweights.Values)
        {
            var type = flyweight.GetIntrinsicState().Type;
            distribution[type] = distribution.GetValueOrDefault(type, 0) + 1;
        }
        
        return distribution;
    }
}

/// <summary>
/// Statistics about flyweight usage and memory efficiency.
/// </summary>
public class FlyweightStatistics
{
    public int TotalFlyweights { get; set; }
    public int TotalRequests { get; set; }
    public int CacheHits { get; set; }
    public int CacheMisses { get; set; }
    public double CacheHitRatio { get; set; }
    public double MemorySavingsRatio { get; set; }
    public long EstimatedMemoryUsage { get; set; }
    public long EstimatedMemoryWithoutFlyweight { get; set; }
    public long MemorySaved { get; set; }
    public Dictionary<CharacterType, int> CharacterDistribution { get; set; } = new();

    public override string ToString()
    {
        var memoryUsageMB = EstimatedMemoryUsage / (1024.0 * 1024.0);
        var memoryWithoutMB = EstimatedMemoryWithoutFlyweight / (1024.0 * 1024.0);
        var memorySavedMB = MemorySaved / (1024.0 * 1024.0);
        
        var distributionStr = string.Join(", ", 
            CharacterDistribution.Select(kvp => $"{kvp.Key}: {kvp.Value}"));

        return $"""
            Flyweight Statistics:
              Total Flyweights: {TotalFlyweights:N0}
              Total Requests: {TotalRequests:N0}
              Cache Hits: {CacheHits:N0} ({CacheHitRatio:P1})
              Cache Misses: {CacheMisses:N0}
              Memory Usage: {memoryUsageMB:F2} MB
              Memory Without Flyweight: {memoryWithoutMB:F2} MB
              Memory Saved: {memorySavedMB:F2} MB ({MemorySavingsRatio:P1})
              Character Distribution: {distributionStr}
            """;
    }
}

/// <summary>
/// Configuration for flyweight factory behavior.
/// </summary>
public class FlyweightFactoryConfig
{
    public bool PreloadCommonCharacters { get; set; } = true;
    public string DefaultFontFamily { get; set; } = "Arial";
    public int MaxCacheSize { get; set; } = 10000;
    public bool EnableStatistics { get; set; } = true;
    public TimeSpan CacheExpirationTime { get; set; } = TimeSpan.FromHours(1);
}
