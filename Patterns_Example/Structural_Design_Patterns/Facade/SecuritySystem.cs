namespace Patterns_Example.Structural_Design_Patterns.Facade;

/// <summary>
/// Complex subsystem for home security management.
/// This represents one of the complex subsystems that the Facade will simplify.
/// </summary>
public class SecuritySystem
{
    private bool _isArmed;
    private bool _motionDetectionEnabled;
    private bool _doorSensorEnabled;
    private bool _windowSensorEnabled;
    private bool _camerasRecording;
    private List<SecurityEvent> _eventLog;
    private Dictionary<string, SecurityZone> _zones;

    public bool IsArmed => _isArmed;
    public bool IsMotionDetectionEnabled => _motionDetectionEnabled;
    public List<SecurityEvent> EventLog => new List<SecurityEvent>(_eventLog);

    public SecuritySystem()
    {
        _isArmed = false;
        _motionDetectionEnabled = false;
        _doorSensorEnabled = false;
        _windowSensorEnabled = false;
        _camerasRecording = false;
        _eventLog = new List<SecurityEvent>();
        _zones = new Dictionary<string, SecurityZone>
        {
            ["entry"] = new SecurityZone("Entry", SecurityZoneType.Entry),
            ["living"] = new SecurityZone("Living Room", SecurityZoneType.Interior),
            ["bedroom"] = new SecurityZone("Bedroom", SecurityZoneType.Interior),
            ["perimeter"] = new SecurityZone("Perimeter", SecurityZoneType.Perimeter)
        };
    }

    public void ArmSystem(SecurityMode mode)
    {
        Console.WriteLine($"[Security] Arming system in {mode} mode...");
        
        switch (mode)
        {
            case SecurityMode.Home:
                _doorSensorEnabled = true;
                _windowSensorEnabled = true;
                _motionDetectionEnabled = false; // Don't detect motion when home
                break;
            case SecurityMode.Away:
                _doorSensorEnabled = true;
                _windowSensorEnabled = true;
                _motionDetectionEnabled = true;
                break;
            case SecurityMode.Night:
                _doorSensorEnabled = true;
                _windowSensorEnabled = true;
                _motionDetectionEnabled = true; // Only in entry zones
                break;
        }
        
        _isArmed = true;
        StartCameraRecording();
        LogEvent(new SecurityEvent("System Armed", $"Armed in {mode} mode", SecurityEventType.SystemArmed));
        
        Console.WriteLine($"[Security] System armed successfully in {mode} mode");
        Thread.Sleep(100); // Simulate arming delay
    }

    public void DisarmSystem(string code)
    {
        Console.WriteLine("[Security] Disarming system...");
        
        if (ValidateDisarmCode(code))
        {
            _isArmed = false;
            _motionDetectionEnabled = false;
            _doorSensorEnabled = false;
            _windowSensorEnabled = false;
            StopCameraRecording();
            LogEvent(new SecurityEvent("System Disarmed", "System disarmed by user", SecurityEventType.SystemDisarmed));
            Console.WriteLine("[Security] System disarmed successfully");
        }
        else
        {
            LogEvent(new SecurityEvent("Invalid Disarm Code", $"Failed disarm attempt with code: {code}", SecurityEventType.SecurityBreach));
            Console.WriteLine("[Security] ❌ Invalid disarm code!");
            throw new UnauthorizedAccessException("Invalid disarm code");
        }
        
        Thread.Sleep(50);
    }

    public void EnableMotionDetection()
    {
        Console.WriteLine("[Security] Enabling motion detection...");
        _motionDetectionEnabled = true;
        LogEvent(new SecurityEvent("Motion Detection Enabled", "Motion sensors activated", SecurityEventType.ConfigurationChange));
    }

    public void DisableMotionDetection()
    {
        Console.WriteLine("[Security] Disabling motion detection...");
        _motionDetectionEnabled = false;
        LogEvent(new SecurityEvent("Motion Detection Disabled", "Motion sensors deactivated", SecurityEventType.ConfigurationChange));
    }

    public void StartCameraRecording()
    {
        Console.WriteLine("[Security] Starting camera recording...");
        _camerasRecording = true;
        LogEvent(new SecurityEvent("Camera Recording Started", "All cameras now recording", SecurityEventType.CameraActivated));
    }

    public void StopCameraRecording()
    {
        Console.WriteLine("[Security] Stopping camera recording...");
        _camerasRecording = false;
        LogEvent(new SecurityEvent("Camera Recording Stopped", "Camera recording deactivated", SecurityEventType.CameraDeactivated));
    }

    public SecurityStatus GetSystemStatus()
    {
        return new SecurityStatus
        {
            IsArmed = _isArmed,
            MotionDetectionEnabled = _motionDetectionEnabled,
            DoorSensorEnabled = _doorSensorEnabled,
            WindowSensorEnabled = _windowSensorEnabled,
            CamerasRecording = _camerasRecording,
            ActiveZones = _zones.Values.Where(z => z.IsActive).ToList(),
            RecentEvents = _eventLog.TakeLast(5).ToList()
        };
    }

    public void TestAllSensors()
    {
        Console.WriteLine("[Security] Testing all sensors...");
        
        foreach (var zone in _zones.Values)
        {
            Console.WriteLine($"[Security]   Testing {zone.Name} zone... ✓");
            Thread.Sleep(20);
        }
        
        LogEvent(new SecurityEvent("Sensor Test", "All sensors tested successfully", SecurityEventType.SystemTest));
        Console.WriteLine("[Security] All sensors tested successfully");
    }

    private bool ValidateDisarmCode(string code)
    {
        // Simple validation - in real system would be more secure
        return code == "1234" || code == "admin";
    }

    private void LogEvent(SecurityEvent securityEvent)
    {
        _eventLog.Add(securityEvent);
        
        // Keep only last 100 events
        if (_eventLog.Count > 100)
        {
            _eventLog.RemoveAt(0);
        }
    }
}

/// <summary>
/// Represents a security event in the system.
/// </summary>
public class SecurityEvent
{
    public DateTime Timestamp { get; }
    public string Title { get; }
    public string Description { get; }
    public SecurityEventType Type { get; }

    public SecurityEvent(string title, string description, SecurityEventType type)
    {
        Timestamp = DateTime.Now;
        Title = title;
        Description = description;
        Type = type;
    }

    public override string ToString()
    {
        return $"{Timestamp:HH:mm:ss} - {Title}: {Description}";
    }
}

/// <summary>
/// Security zone configuration.
/// </summary>
public class SecurityZone
{
    public string Name { get; }
    public SecurityZoneType Type { get; }
    public bool IsActive { get; set; }

    public SecurityZone(string name, SecurityZoneType type)
    {
        Name = name;
        Type = type;
        IsActive = true;
    }
}

/// <summary>
/// Current status of the security system.
/// </summary>
public class SecurityStatus
{
    public bool IsArmed { get; set; }
    public bool MotionDetectionEnabled { get; set; }
    public bool DoorSensorEnabled { get; set; }
    public bool WindowSensorEnabled { get; set; }
    public bool CamerasRecording { get; set; }
    public List<SecurityZone> ActiveZones { get; set; } = new();
    public List<SecurityEvent> RecentEvents { get; set; } = new();

    public override string ToString()
    {
        var status = IsArmed ? "ARMED" : "DISARMED";
        var features = new List<string>();
        if (MotionDetectionEnabled) features.Add("Motion");
        if (DoorSensorEnabled) features.Add("Doors");
        if (WindowSensorEnabled) features.Add("Windows");
        if (CamerasRecording) features.Add("Cameras");
        
        return $"Security: {status} | Active: {string.Join(", ", features)} | Zones: {ActiveZones.Count}";
    }
}

/// <summary>
/// Security system modes.
/// </summary>
public enum SecurityMode
{
    Home,    // Armed but allows movement inside
    Away,    // Fully armed with all sensors
    Night    // Armed with limited motion detection
}

/// <summary>
/// Types of security events.
/// </summary>
public enum SecurityEventType
{
    SystemArmed,
    SystemDisarmed,
    SecurityBreach,
    MotionDetected,
    DoorOpened,
    WindowOpened,
    CameraActivated,
    CameraDeactivated,
    ConfigurationChange,
    SystemTest
}

/// <summary>
/// Types of security zones.
/// </summary>
public enum SecurityZoneType
{
    Entry,      // Entry points (doors)
    Interior,   // Interior rooms
    Perimeter   // Windows and exterior
}
