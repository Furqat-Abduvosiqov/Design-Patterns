namespace Patterns_Example.Creational_Design_Patterns.Singleton.ConfigurationManager;

/// <summary>
/// Thread-safe Singleton implementation of a Configuration Manager.
/// This class demonstrates the Singleton pattern with a real-world use case
/// where you need exactly one instance to manage application configuration.
/// </summary>
public sealed class ConfigurationManager : IConfigurationManager
{
    private static readonly object _lock = new();
    private static ConfigurationManager? _instance;
    private readonly Dictionary<string, string> _settings;
    private readonly object _settingsLock = new();
    
    public DateTime LastModified { get; private set; }
    public int SettingsCount 
    { 
        get 
        { 
            lock (_settingsLock) 
            { 
                return _settings.Count; 
            } 
        } 
    }

    /// <summary>
    /// Private constructor prevents direct instantiation.
    /// This is a key characteristic of the Singleton pattern.
    /// </summary>
    private ConfigurationManager()
    {
        _settings = new Dictionary<string, string>();
        LastModified = DateTime.Now;
        LoadDefaultSettings();
    }

    /// <summary>
    /// Thread-safe singleton instance access.
    /// Uses double-checked locking pattern for performance.
    /// </summary>
    public static ConfigurationManager Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    _instance ??= new ConfigurationManager();
                }
            }
            return _instance;
        }
    }

    /// <summary>
    /// Gets a configuration setting by key.
    /// </summary>
    public string GetSetting(string key)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentException("Key cannot be null or empty", nameof(key));

        lock (_settingsLock)
        {
            return _settings.TryGetValue(key, out var value) ? value : string.Empty;
        }
    }

    /// <summary>
    /// Sets a configuration setting.
    /// </summary>
    public void SetSetting(string key, string value)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentException("Key cannot be null or empty", nameof(key));

        lock (_settingsLock)
        {
            _settings[key] = value ?? string.Empty;
            LastModified = DateTime.Now;
        }
    }

    /// <summary>
    /// Checks if a setting exists.
    /// </summary>
    public bool HasSetting(string key)
    {
        if (string.IsNullOrEmpty(key))
            return false;

        lock (_settingsLock)
        {
            return _settings.ContainsKey(key);
        }
    }

    /// <summary>
    /// Removes a setting.
    /// </summary>
    public void RemoveSetting(string key)
    {
        if (string.IsNullOrEmpty(key))
            return;

        lock (_settingsLock)
        {
            if (_settings.Remove(key))
            {
                LastModified = DateTime.Now;
            }
        }
    }

    /// <summary>
    /// Loads configuration from a file.
    /// In a real application, this might load from JSON, XML, or INI files.
    /// </summary>
    public void LoadFromFile(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
            throw new ArgumentException("File path cannot be null or empty", nameof(filePath));

        // Simulate loading from file
        // In real implementation, you would parse actual file formats
        lock (_settingsLock)
        {
            _settings.Clear();
            
            // Simulate file content
            var fileSettings = new Dictionary<string, string>
            {
                ["DatabaseConnectionString"] = "Server=localhost;Database=MyApp;Trusted_Connection=true;",
                ["ApiEndpoint"] = "https://api.myapp.com/v1",
                ["LogLevel"] = "Information",
                ["MaxRetryAttempts"] = "3",
                ["TimeoutSeconds"] = "30",
                ["EnableCaching"] = "true",
                ["CacheExpirationMinutes"] = "60"
            };

            foreach (var setting in fileSettings)
            {
                _settings[setting.Key] = setting.Value;
            }
            
            LastModified = DateTime.Now;
        }
    }

    /// <summary>
    /// Saves configuration to a file.
    /// </summary>
    public void SaveToFile(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
            throw new ArgumentException("File path cannot be null or empty", nameof(filePath));

        lock (_settingsLock)
        {
            // In a real implementation, you would serialize to actual file formats
            Console.WriteLine($"Saving {_settings.Count} settings to {filePath}");
            foreach (var setting in _settings)
            {
                Console.WriteLine($"  {setting.Key} = {setting.Value}");
            }
        }
    }

    /// <summary>
    /// Reloads configuration from the default source.
    /// </summary>
    public void ReloadConfiguration()
    {
        lock (_settingsLock)
        {
            _settings.Clear();
            LoadDefaultSettings();
            LastModified = DateTime.Now;
        }
    }

    /// <summary>
    /// Gets all settings as a copy.
    /// </summary>
    public Dictionary<string, string> GetAllSettings()
    {
        lock (_settingsLock)
        {
            return new Dictionary<string, string>(_settings);
        }
    }

    /// <summary>
    /// Loads default application settings.
    /// </summary>
    private void LoadDefaultSettings()
    {
        _settings["ApplicationName"] = "Design Patterns Demo";
        _settings["Version"] = "1.0.0";
        _settings["Environment"] = "Development";
        _settings["DatabaseConnectionString"] = "Server=localhost;Database=DemoApp;Trusted_Connection=true;";
        _settings["LogLevel"] = "Debug";
        _settings["MaxConcurrentUsers"] = "100";
        _settings["SessionTimeoutMinutes"] = "30";
        _settings["EnableFeatureX"] = "false";
        _settings["ApiRateLimit"] = "1000";
        _settings["DefaultLanguage"] = "en-US";
    }

    /// <summary>
    /// Gets a strongly-typed setting value.
    /// </summary>
    public T GetSetting<T>(string key, T defaultValue = default!)
    {
        var stringValue = GetSetting(key);
        
        if (string.IsNullOrEmpty(stringValue))
            return defaultValue;

        try
        {
            return (T)Convert.ChangeType(stringValue, typeof(T));
        }
        catch
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// Sets a strongly-typed setting value.
    /// </summary>
    public void SetSetting<T>(string key, T value)
    {
        SetSetting(key, value?.ToString() ?? string.Empty);
    }

    public override string ToString()
    {
        lock (_settingsLock)
        {
            return $"ConfigurationManager: {_settings.Count} settings, Last Modified: {LastModified:yyyy-MM-dd HH:mm:ss}";
        }
    }
}
