namespace Patterns_Example.Structural_Design_Patterns.Facade;

/// <summary>
/// Complex subsystem for home lighting management.
/// This represents another complex subsystem that the Facade will simplify.
/// </summary>
public class LightingSystem
{
    private readonly Dictionary<string, LightZone> _zones;
    private readonly Dictionary<string, LightingScene> _scenes;
    private bool _automaticMode;
    private int _globalBrightness;

    public bool IsAutomaticMode => _automaticMode;
    public int GlobalBrightness => _globalBrightness;

    public LightingSystem()
    {
        _zones = new Dictionary<string, LightZone>
        {
            ["living_room"] = new LightZone("Living Room", 4, LightType.LED),
            ["kitchen"] = new LightZone("Kitchen", 6, LightType.LED),
            ["bedroom"] = new LightZone("Bedroom", 2, LightType.Warm),
            ["bathroom"] = new LightZone("Bathroom", 3, LightType.Bright),
            ["hallway"] = new LightZone("Hallway", 2, LightType.LED),
            ["outdoor"] = new LightZone("Outdoor", 8, LightType.Security)
        };

        _scenes = new Dictionary<string, LightingScene>
        {
            ["morning"] = new LightingScene("Morning", new Dictionary<string, int>
            {
                ["living_room"] = 70,
                ["kitchen"] = 90,
                ["bedroom"] = 50,
                ["bathroom"] = 80,
                ["hallway"] = 60
            }),
            ["evening"] = new LightingScene("Evening", new Dictionary<string, int>
            {
                ["living_room"] = 60,
                ["kitchen"] = 70,
                ["bedroom"] = 40,
                ["bathroom"] = 60,
                ["hallway"] = 40
            }),
            ["night"] = new LightingScene("Night", new Dictionary<string, int>
            {
                ["living_room"] = 20,
                ["kitchen"] = 30,
                ["bedroom"] = 10,
                ["bathroom"] = 40,
                ["hallway"] = 20
            }),
            ["party"] = new LightingScene("Party", new Dictionary<string, int>
            {
                ["living_room"] = 100,
                ["kitchen"] = 80,
                ["hallway"] = 70
            }),
            ["movie"] = new LightingScene("Movie", new Dictionary<string, int>
            {
                ["living_room"] = 15,
                ["hallway"] = 10
            })
        };

        _automaticMode = false;
        _globalBrightness = 100;
    }

    public void TurnOnAllLights()
    {
        Console.WriteLine("[Lighting] Turning on all lights...");
        
        foreach (var zone in _zones.Values)
        {
            zone.TurnOn();
            Console.WriteLine($"[Lighting]   {zone.Name}: ON at {zone.Brightness}%");
            Thread.Sleep(10);
        }
        
        Console.WriteLine("[Lighting] All lights are now on");
    }

    public void TurnOffAllLights()
    {
        Console.WriteLine("[Lighting] Turning off all lights...");
        
        foreach (var zone in _zones.Values)
        {
            zone.TurnOff();
            Console.WriteLine($"[Lighting]   {zone.Name}: OFF");
            Thread.Sleep(10);
        }
        
        Console.WriteLine("[Lighting] All lights are now off");
    }

    public void SetZoneBrightness(string zoneName, int brightness)
    {
        if (_zones.TryGetValue(zoneName, out var zone))
        {
            Console.WriteLine($"[Lighting] Setting {zone.Name} brightness to {brightness}%");
            zone.SetBrightness(brightness);
        }
        else
        {
            Console.WriteLine($"[Lighting] ❌ Zone '{zoneName}' not found");
        }
    }

    public void SetGlobalBrightness(int brightness)
    {
        Console.WriteLine($"[Lighting] Setting global brightness to {brightness}%");
        _globalBrightness = Math.Max(0, Math.Min(100, brightness));
        
        foreach (var zone in _zones.Values)
        {
            if (zone.IsOn)
            {
                var adjustedBrightness = (int)(zone.Brightness * (_globalBrightness / 100.0));
                zone.SetBrightness(adjustedBrightness);
            }
        }
    }

    public void ActivateScene(string sceneName)
    {
        if (_scenes.TryGetValue(sceneName, out var scene))
        {
            Console.WriteLine($"[Lighting] Activating '{scene.Name}' scene...");
            
            // First turn off all lights
            foreach (var zone in _zones.Values)
            {
                zone.TurnOff();
            }
            
            Thread.Sleep(50);
            
            // Then apply scene settings
            foreach (var setting in scene.ZoneSettings)
            {
                if (_zones.TryGetValue(setting.Key, out var zone))
                {
                    zone.TurnOn();
                    zone.SetBrightness(setting.Value);
                    Console.WriteLine($"[Lighting]   {zone.Name}: {setting.Value}%");
                    Thread.Sleep(20);
                }
            }
            
            Console.WriteLine($"[Lighting] '{scene.Name}' scene activated");
        }
        else
        {
            Console.WriteLine($"[Lighting] ❌ Scene '{sceneName}' not found");
        }
    }

    public void EnableAutomaticMode()
    {
        Console.WriteLine("[Lighting] Enabling automatic lighting mode...");
        _automaticMode = true;
        
        // Simulate automatic adjustment based on time of day
        var hour = DateTime.Now.Hour;
        string sceneToActivate = hour switch
        {
            >= 6 and < 12 => "morning",
            >= 12 and < 18 => "evening",
            >= 18 and < 22 => "evening",
            _ => "night"
        };
        
        ActivateScene(sceneToActivate);
        Console.WriteLine("[Lighting] Automatic mode enabled");
    }

    public void DisableAutomaticMode()
    {
        Console.WriteLine("[Lighting] Disabling automatic lighting mode...");
        _automaticMode = false;
        Console.WriteLine("[Lighting] Manual control restored");
    }

    public void DimLightsForSecurity()
    {
        Console.WriteLine("[Lighting] Dimming lights for security mode...");
        
        foreach (var zone in _zones.Values)
        {
            if (zone.Name != "Outdoor") // Keep outdoor lights bright for security
            {
                if (zone.IsOn)
                {
                    zone.SetBrightness(20); // Dim to 20%
                }
            }
            else
            {
                zone.TurnOn();
                zone.SetBrightness(100); // Outdoor lights at full brightness
            }
        }
        
        Console.WriteLine("[Lighting] Security lighting mode activated");
    }

    public void SetWelcomeMode()
    {
        Console.WriteLine("[Lighting] Setting welcome lighting...");
        
        // Turn on entry lights
        SetZoneBrightness("hallway", 80);
        SetZoneBrightness("living_room", 60);
        SetZoneBrightness("outdoor", 100);
        
        Console.WriteLine("[Lighting] Welcome lighting activated");
    }

    public LightingStatus GetSystemStatus()
    {
        var activeZones = _zones.Values.Where(z => z.IsOn).ToList();
        var totalPowerConsumption = activeZones.Sum(z => z.GetPowerConsumption());
        
        return new LightingStatus
        {
            AutomaticMode = _automaticMode,
            GlobalBrightness = _globalBrightness,
            ActiveZones = activeZones.Count,
            TotalZones = _zones.Count,
            PowerConsumption = totalPowerConsumption,
            ZoneDetails = _zones.Values.ToList()
        };
    }

    public List<string> GetAvailableScenes()
    {
        return _scenes.Keys.ToList();
    }

    public List<string> GetAvailableZones()
    {
        return _zones.Keys.ToList();
    }
}

/// <summary>
/// Represents a lighting zone in the home.
/// </summary>
public class LightZone
{
    public string Name { get; }
    public int LightCount { get; }
    public LightType Type { get; }
    public bool IsOn { get; private set; }
    public int Brightness { get; private set; }

    public LightZone(string name, int lightCount, LightType type)
    {
        Name = name;
        LightCount = lightCount;
        Type = type;
        IsOn = false;
        Brightness = 100;
    }

    public void TurnOn()
    {
        IsOn = true;
    }

    public void TurnOff()
    {
        IsOn = false;
    }

    public void SetBrightness(int brightness)
    {
        Brightness = Math.Max(0, Math.Min(100, brightness));
        if (Brightness > 0 && !IsOn)
        {
            TurnOn();
        }
        else if (Brightness == 0 && IsOn)
        {
            TurnOff();
        }
    }

    public double GetPowerConsumption()
    {
        if (!IsOn) return 0;
        
        var basePower = Type switch
        {
            LightType.LED => 8.0,      // 8W per LED bulb
            LightType.Warm => 12.0,    // 12W per warm bulb
            LightType.Bright => 15.0,  // 15W per bright bulb
            LightType.Security => 20.0, // 20W per security light
            _ => 10.0
        };
        
        return LightCount * basePower * (Brightness / 100.0);
    }

    public override string ToString()
    {
        var status = IsOn ? $"ON ({Brightness}%)" : "OFF";
        return $"{Name}: {status} [{LightCount} {Type} lights]";
    }
}

/// <summary>
/// Represents a lighting scene with predefined settings.
/// </summary>
public class LightingScene
{
    public string Name { get; }
    public Dictionary<string, int> ZoneSettings { get; }

    public LightingScene(string name, Dictionary<string, int> zoneSettings)
    {
        Name = name;
        ZoneSettings = new Dictionary<string, int>(zoneSettings);
    }
}

/// <summary>
/// Current status of the lighting system.
/// </summary>
public class LightingStatus
{
    public bool AutomaticMode { get; set; }
    public int GlobalBrightness { get; set; }
    public int ActiveZones { get; set; }
    public int TotalZones { get; set; }
    public double PowerConsumption { get; set; }
    public List<LightZone> ZoneDetails { get; set; } = new();

    public override string ToString()
    {
        var mode = AutomaticMode ? "AUTO" : "MANUAL";
        return $"Lighting: {mode} | Active: {ActiveZones}/{TotalZones} zones | Power: {PowerConsumption:F1}W | Brightness: {GlobalBrightness}%";
    }
}

/// <summary>
/// Types of lights available in the system.
/// </summary>
public enum LightType
{
    LED,        // Energy efficient LED lights
    Warm,       // Warm white lights for bedrooms
    Bright,     // Bright white lights for work areas
    Security    // High-intensity security lights
}
