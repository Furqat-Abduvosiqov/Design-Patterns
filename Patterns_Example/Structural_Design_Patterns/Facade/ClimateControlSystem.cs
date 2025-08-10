namespace Patterns_Example.Structural_Design_Patterns.Facade;

/// <summary>
/// Complex subsystem for climate control management.
/// This represents another complex subsystem that the Facade will simplify.
/// </summary>
public class ClimateControlSystem
{
    private readonly Dictionary<string, ClimateZone> _zones;
    private readonly Dictionary<string, ClimateSchedule> _schedules;
    private bool _ecoMode;
    private bool _awayMode;
    private double _globalTemperatureOffset;

    public bool IsEcoMode => _ecoMode;
    public bool IsAwayMode => _awayMode;
    public double GlobalTemperatureOffset => _globalTemperatureOffset;

    public ClimateControlSystem()
    {
        _zones = new Dictionary<string, ClimateZone>
        {
            ["living_room"] = new ClimateZone("Living Room", 72.0, 45.0),
            ["bedroom"] = new ClimateZone("Bedroom", 68.0, 50.0),
            ["kitchen"] = new ClimateZone("Kitchen", 70.0, 40.0),
            ["office"] = new ClimateZone("Office", 71.0, 45.0),
            ["basement"] = new ClimateZone("Basement", 65.0, 55.0)
        };

        _schedules = new Dictionary<string, ClimateSchedule>
        {
            ["weekday"] = new ClimateSchedule("Weekday", new Dictionary<int, double>
            {
                [6] = 70.0,   // 6 AM - Wake up
                [8] = 68.0,   // 8 AM - Leave for work
                [17] = 72.0,  // 5 PM - Return home
                [22] = 68.0   // 10 PM - Sleep
            }),
            ["weekend"] = new ClimateSchedule("Weekend", new Dictionary<int, double>
            {
                [8] = 72.0,   // 8 AM - Wake up
                [22] = 68.0   // 10 PM - Sleep
            }),
            ["vacation"] = new ClimateSchedule("Vacation", new Dictionary<int, double>
            {
                [0] = 60.0    // Maintain minimal temperature
            })
        };

        _ecoMode = false;
        _awayMode = false;
        _globalTemperatureOffset = 0.0;
    }

    public void SetTemperature(double temperature)
    {
        Console.WriteLine($"[Climate] Setting global temperature to {temperature:F1}°F...");
        
        foreach (var zone in _zones.Values)
        {
            zone.SetTargetTemperature(temperature);
            Console.WriteLine($"[Climate]   {zone.Name}: {temperature:F1}°F");
            Thread.Sleep(20);
        }
        
        Console.WriteLine("[Climate] Global temperature set");
    }

    public void SetZoneTemperature(string zoneName, double temperature)
    {
        if (_zones.TryGetValue(zoneName, out var zone))
        {
            Console.WriteLine($"[Climate] Setting {zone.Name} to {temperature:F1}°F");
            zone.SetTargetTemperature(temperature);
        }
        else
        {
            Console.WriteLine($"[Climate] ❌ Zone '{zoneName}' not found");
        }
    }

    public void SetHumidity(string zoneName, double humidity)
    {
        if (_zones.TryGetValue(zoneName, out var zone))
        {
            Console.WriteLine($"[Climate] Setting {zone.Name} humidity to {humidity:F1}%");
            zone.SetTargetHumidity(humidity);
        }
        else
        {
            Console.WriteLine($"[Climate] ❌ Zone '{zoneName}' not found");
        }
    }

    public void EnableEcoMode()
    {
        Console.WriteLine("[Climate] Enabling eco mode...");
        _ecoMode = true;
        _globalTemperatureOffset = -2.0; // Reduce temperature by 2°F for energy savings
        
        foreach (var zone in _zones.Values)
        {
            var newTemp = zone.TargetTemperature + _globalTemperatureOffset;
            zone.SetTargetTemperature(newTemp);
            Console.WriteLine($"[Climate]   {zone.Name}: {newTemp:F1}°F (eco)");
        }
        
        Console.WriteLine("[Climate] Eco mode enabled - saving energy");
    }

    public void DisableEcoMode()
    {
        Console.WriteLine("[Climate] Disabling eco mode...");
        _ecoMode = false;
        
        foreach (var zone in _zones.Values)
        {
            var newTemp = zone.TargetTemperature - _globalTemperatureOffset;
            zone.SetTargetTemperature(newTemp);
        }
        
        _globalTemperatureOffset = 0.0;
        Console.WriteLine("[Climate] Eco mode disabled - normal operation");
    }

    public void SetAwayMode(bool enabled)
    {
        Console.WriteLine($"[Climate] {(enabled ? "Enabling" : "Disabling")} away mode...");
        _awayMode = enabled;
        
        if (enabled)
        {
            // Reduce temperature for energy savings when away
            foreach (var zone in _zones.Values)
            {
                var awayTemp = zone.TargetTemperature - 5.0;
                zone.SetTargetTemperature(Math.Max(60.0, awayTemp)); // Don't go below 60°F
                Console.WriteLine($"[Climate]   {zone.Name}: {zone.TargetTemperature:F1}°F (away)");
            }
        }
        else
        {
            // Restore normal temperatures
            ActivateSchedule("weekday"); // Default to weekday schedule
        }
        
        Console.WriteLine($"[Climate] Away mode {(enabled ? "enabled" : "disabled")}");
    }

    public void ActivateSchedule(string scheduleName)
    {
        if (_schedules.TryGetValue(scheduleName, out var schedule))
        {
            Console.WriteLine($"[Climate] Activating '{schedule.Name}' schedule...");
            
            var currentHour = DateTime.Now.Hour;
            var targetTemp = schedule.GetTemperatureForHour(currentHour);
            
            SetTemperature(targetTemp);
            Console.WriteLine($"[Climate] Schedule activated - current setting: {targetTemp:F1}°F");
        }
        else
        {
            Console.WriteLine($"[Climate] ❌ Schedule '{scheduleName}' not found");
        }
    }

    public void StartHeating()
    {
        Console.WriteLine("[Climate] Starting heating system...");
        
        foreach (var zone in _zones.Values)
        {
            zone.StartHeating();
            Console.WriteLine($"[Climate]   {zone.Name}: Heating to {zone.TargetTemperature:F1}°F");
            Thread.Sleep(15);
        }
        
        Console.WriteLine("[Climate] Heating system started");
    }

    public void StartCooling()
    {
        Console.WriteLine("[Climate] Starting cooling system...");
        
        foreach (var zone in _zones.Values)
        {
            zone.StartCooling();
            Console.WriteLine($"[Climate]   {zone.Name}: Cooling to {zone.TargetTemperature:F1}°F");
            Thread.Sleep(15);
        }
        
        Console.WriteLine("[Climate] Cooling system started");
    }

    public void StopAllSystems()
    {
        Console.WriteLine("[Climate] Stopping all climate systems...");
        
        foreach (var zone in _zones.Values)
        {
            zone.StopSystem();
            Console.WriteLine($"[Climate]   {zone.Name}: System stopped");
        }
        
        Console.WriteLine("[Climate] All climate systems stopped");
    }

    public void OptimizeForSleep()
    {
        Console.WriteLine("[Climate] Optimizing climate for sleep...");
        
        // Bedrooms slightly cooler, reduce humidity
        SetZoneTemperature("bedroom", 66.0);
        SetHumidity("bedroom", 45.0);
        
        // Other zones slightly warmer to save energy
        SetZoneTemperature("living_room", 68.0);
        SetZoneTemperature("kitchen", 65.0);
        
        Console.WriteLine("[Climate] Sleep optimization complete");
    }

    public ClimateStatus GetSystemStatus()
    {
        var activeZones = _zones.Values.Where(z => z.IsActive).ToList();
        var averageTemp = _zones.Values.Average(z => z.CurrentTemperature);
        var averageHumidity = _zones.Values.Average(z => z.CurrentHumidity);
        var totalEnergyUsage = _zones.Values.Sum(z => z.GetEnergyUsage());
        
        return new ClimateStatus
        {
            EcoMode = _ecoMode,
            AwayMode = _awayMode,
            ActiveZones = activeZones.Count,
            TotalZones = _zones.Count,
            AverageTemperature = averageTemp,
            AverageHumidity = averageHumidity,
            EnergyUsage = totalEnergyUsage,
            ZoneDetails = _zones.Values.ToList()
        };
    }

    public List<string> GetAvailableSchedules()
    {
        return _schedules.Keys.ToList();
    }

    public List<string> GetAvailableZones()
    {
        return _zones.Keys.ToList();
    }
}

/// <summary>
/// Represents a climate zone in the home.
/// </summary>
public class ClimateZone
{
    public string Name { get; }
    public double CurrentTemperature { get; private set; }
    public double TargetTemperature { get; private set; }
    public double CurrentHumidity { get; private set; }
    public double TargetHumidity { get; private set; }
    public bool IsActive { get; private set; }
    public ClimateMode Mode { get; private set; }

    public ClimateZone(string name, double initialTemp, double initialHumidity)
    {
        Name = name;
        CurrentTemperature = initialTemp;
        TargetTemperature = initialTemp;
        CurrentHumidity = initialHumidity;
        TargetHumidity = initialHumidity;
        IsActive = false;
        Mode = ClimateMode.Off;
    }

    public void SetTargetTemperature(double temperature)
    {
        TargetTemperature = temperature;
        // Simulate gradual temperature change
        CurrentTemperature = temperature + (Random.Shared.NextDouble() - 0.5) * 2;
    }

    public void SetTargetHumidity(double humidity)
    {
        TargetHumidity = Math.Max(30, Math.Min(70, humidity));
        // Simulate gradual humidity change
        CurrentHumidity = TargetHumidity + (Random.Shared.NextDouble() - 0.5) * 5;
    }

    public void StartHeating()
    {
        IsActive = true;
        Mode = ClimateMode.Heating;
    }

    public void StartCooling()
    {
        IsActive = true;
        Mode = ClimateMode.Cooling;
    }

    public void StopSystem()
    {
        IsActive = false;
        Mode = ClimateMode.Off;
    }

    public double GetEnergyUsage()
    {
        if (!IsActive) return 0;
        
        var tempDifference = Math.Abs(TargetTemperature - CurrentTemperature);
        var baseUsage = Mode switch
        {
            ClimateMode.Heating => 2.5, // kW for heating
            ClimateMode.Cooling => 3.0, // kW for cooling
            _ => 0.1 // Standby power
        };
        
        return baseUsage * (1 + tempDifference * 0.1);
    }

    public override string ToString()
    {
        var status = IsActive ? $"{Mode}" : "OFF";
        return $"{Name}: {CurrentTemperature:F1}°F → {TargetTemperature:F1}°F | {CurrentHumidity:F0}% humidity | {status}";
    }
}

/// <summary>
/// Represents a climate schedule with temperature settings by hour.
/// </summary>
public class ClimateSchedule
{
    public string Name { get; }
    private readonly Dictionary<int, double> _hourlySettings;

    public ClimateSchedule(string name, Dictionary<int, double> hourlySettings)
    {
        Name = name;
        _hourlySettings = new Dictionary<int, double>(hourlySettings);
    }

    public double GetTemperatureForHour(int hour)
    {
        // Find the most recent setting for the given hour
        var applicableHour = _hourlySettings.Keys
            .Where(h => h <= hour)
            .DefaultIfEmpty(0)
            .Max();
            
        return _hourlySettings.TryGetValue(applicableHour, out var temp) ? temp : 70.0;
    }
}

/// <summary>
/// Current status of the climate control system.
/// </summary>
public class ClimateStatus
{
    public bool EcoMode { get; set; }
    public bool AwayMode { get; set; }
    public int ActiveZones { get; set; }
    public int TotalZones { get; set; }
    public double AverageTemperature { get; set; }
    public double AverageHumidity { get; set; }
    public double EnergyUsage { get; set; }
    public List<ClimateZone> ZoneDetails { get; set; } = new();

    public override string ToString()
    {
        var modes = new List<string>();
        if (EcoMode) modes.Add("ECO");
        if (AwayMode) modes.Add("AWAY");
        var modeStr = modes.Count > 0 ? $" ({string.Join(", ", modes)})" : "";
        
        return $"Climate: {AverageTemperature:F1}°F | {AverageHumidity:F0}% humidity | {ActiveZones}/{TotalZones} zones | {EnergyUsage:F1}kW{modeStr}";
    }
}

/// <summary>
/// Climate control modes.
/// </summary>
public enum ClimateMode
{
    Off,
    Heating,
    Cooling,
    Auto
}
