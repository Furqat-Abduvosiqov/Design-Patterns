namespace Patterns_Example.Creational_Design_Patterns.Singleton.ConfigurationManager;

/// <summary>
/// Interface for configuration management.
/// This allows for easier testing and dependency injection when needed.
/// </summary>
public interface IConfigurationManager
{
    string GetSetting(string key);
    void SetSetting(string key, string value);
    bool HasSetting(string key);
    void RemoveSetting(string key);
    void LoadFromFile(string filePath);
    void SaveToFile(string filePath);
    void ReloadConfiguration();
    Dictionary<string, string> GetAllSettings();
    int SettingsCount { get; }
    DateTime LastModified { get; }
}
