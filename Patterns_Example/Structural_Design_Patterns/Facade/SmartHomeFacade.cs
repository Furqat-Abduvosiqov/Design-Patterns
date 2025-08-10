namespace Patterns_Example.Structural_Design_Patterns.Facade;

/// <summary>
/// Facade class that provides a simplified interface to the complex smart home subsystems.
/// This is the main Facade in the Facade pattern - it hides the complexity of multiple subsystems
/// behind a simple, unified interface.
/// </summary>
public class SmartHomeFacade
{
    private readonly SecuritySystem _securitySystem;
    private readonly LightingSystem _lightingSystem;
    private readonly ClimateControlSystem _climateSystem;
    private readonly EntertainmentSystem _entertainmentSystem;
    private readonly Dictionary<string, HomeScene> _homeScenes;

    public SmartHomeFacade()
    {
        // Initialize all subsystems
        _securitySystem = new SecuritySystem();
        _lightingSystem = new LightingSystem();
        _climateSystem = new ClimateControlSystem();
        _entertainmentSystem = new EntertainmentSystem();
        
        // Define home scenes that coordinate multiple subsystems
        _homeScenes = new Dictionary<string, HomeScene>
        {
            ["good_morning"] = new HomeScene("Good Morning", "Start your day with optimal settings"),
            ["leaving_home"] = new HomeScene("Leaving Home", "Secure and optimize the home for absence"),
            ["coming_home"] = new HomeScene("Coming Home", "Welcome you back with comfortable settings"),
            ["movie_night"] = new HomeScene("Movie Night", "Perfect environment for watching movies"),
            ["dinner_party"] = new HomeScene("Dinner Party", "Entertaining guests with style"),
            ["bedtime"] = new HomeScene("Bedtime", "Prepare the home for a good night's sleep"),
            ["vacation_mode"] = new HomeScene("Vacation Mode", "Long-term absence security and efficiency")
        };
    }

    #region Simple High-Level Operations

    /// <summary>
    /// Activates "Good Morning" scene - gradually wake up the home.
    /// </summary>
    public void GoodMorning()
    {
        Console.WriteLine("🌅 Good Morning! Starting your day...");
        Console.WriteLine(new string('=', 50));
        
        // Disarm security (assuming user is home)
        _securitySystem.DisarmSystem("1234");
        
        // Gradual lighting
        _lightingSystem.ActivateScene("morning");
        
        // Comfortable temperature
        _climateSystem.SetTemperature(72.0);
        _climateSystem.DisableEcoMode();
        
        // Gentle music in kitchen and living room
        _entertainmentSystem.SetZoneVolume("kitchen", 30);
        _entertainmentSystem.SetZoneVolume("living_room", 25);
        _entertainmentSystem.PlayMusic("Morning Playlist", "kitchen");
        
        Console.WriteLine("🌅 Good morning routine complete! Have a great day!");
    }

    /// <summary>
    /// Activates "Leaving Home" scene - secure and optimize for absence.
    /// </summary>
    public void LeavingHome()
    {
        Console.WriteLine("🚪 Leaving Home - Securing and optimizing...");
        Console.WriteLine(new string('=', 50));
        
        // Turn off entertainment
        _entertainmentSystem.TurnOffAllAudio();
        _entertainmentSystem.TurnOffAllVideo();
        
        // Optimize lighting (turn off most, keep some for security)
        _lightingSystem.TurnOffAllLights();
        _lightingSystem.SetZoneBrightness("outdoor", 100); // Keep outdoor lights
        _lightingSystem.SetZoneBrightness("hallway", 20);  // Dim hallway light
        
        // Set climate to away mode
        _climateSystem.SetAwayMode(true);
        _climateSystem.EnableEcoMode();
        
        // Arm security system
        _securitySystem.ArmSystem(SecurityMode.Away);
        
        Console.WriteLine("🚪 Home secured! Safe travels!");
    }

    /// <summary>
    /// Activates "Coming Home" scene - welcome back with comfort.
    /// </summary>
    public void ComingHome()
    {
        Console.WriteLine("🏠 Welcome Home! Preparing comfortable environment...");
        Console.WriteLine(new string('=', 50));
        
        // Disarm security
        _securitySystem.DisarmSystem("1234");
        
        // Welcome lighting
        _lightingSystem.SetWelcomeMode();
        
        // Restore comfortable temperature
        _climateSystem.SetAwayMode(false);
        _climateSystem.SetTemperature(72.0);
        
        // Gentle background music
        _entertainmentSystem.SetGlobalVolume(40);
        _entertainmentSystem.PlayMusic("Welcome Home Playlist", "living_room");
        
        Console.WriteLine("🏠 Welcome home! Everything is ready for you!");
    }

    /// <summary>
    /// Activates "Movie Night" scene - perfect cinema experience.
    /// </summary>
    public void MovieNight()
    {
        Console.WriteLine("🎬 Movie Night! Creating cinema experience...");
        Console.WriteLine(new string('=', 50));
        
        // Dim lights for movie watching
        _lightingSystem.ActivateScene("movie");
        
        // Optimize temperature for extended sitting
        _climateSystem.SetZoneTemperature("living_room", 70.0);
        
        // Set up entertainment system
        _entertainmentSystem.StartMovieMode();
        
        // Ensure security is on but motion detection is limited
        if (!_securitySystem.IsArmed)
        {
            _securitySystem.ArmSystem(SecurityMode.Home);
        }
        _securitySystem.DisableMotionDetection(); // Avoid false alarms during movie
        
        Console.WriteLine("🎬 Movie night setup complete! Enjoy the show!");
    }

    /// <summary>
    /// Activates "Dinner Party" scene - entertaining guests.
    /// </summary>
    public void DinnerParty()
    {
        Console.WriteLine("🍽️ Dinner Party! Setting up for guests...");
        Console.WriteLine(new string('=', 50));
        
        // Warm, welcoming lighting
        _lightingSystem.ActivateScene("evening");
        _lightingSystem.SetZoneBrightness("kitchen", 80);
        _lightingSystem.SetZoneBrightness("living_room", 70);
        
        // Comfortable temperature
        _climateSystem.SetTemperature(71.0);
        
        // Background music for ambiance
        _entertainmentSystem.ActivateScene("dinner");
        _entertainmentSystem.PlayMusic("Dinner Jazz Playlist");
        
        // Security on but guest-friendly
        if (!_securitySystem.IsArmed)
        {
            _securitySystem.ArmSystem(SecurityMode.Home);
        }
        
        Console.WriteLine("🍽️ Dinner party setup complete! Enjoy your evening with guests!");
    }

    /// <summary>
    /// Activates "Bedtime" scene - prepare for sleep.
    /// </summary>
    public void Bedtime()
    {
        Console.WriteLine("🌙 Bedtime! Preparing for a good night's sleep...");
        Console.WriteLine(new string('=', 50));
        
        // Dim lighting gradually
        _lightingSystem.ActivateScene("night");
        
        // Optimize climate for sleep
        _climateSystem.OptimizeForSleep();
        
        // Turn off entertainment except bedroom
        _entertainmentSystem.TurnOffAllAudio();
        _entertainmentSystem.TurnOffAllVideo();
        _entertainmentSystem.SetZoneVolume("bedroom", 15); // Very quiet
        
        // Arm security for night
        _securitySystem.ArmSystem(SecurityMode.Night);
        
        Console.WriteLine("🌙 Sweet dreams! The home is secured for the night.");
    }

    /// <summary>
    /// Activates "Vacation Mode" - long-term absence optimization.
    /// </summary>
    public void VacationMode()
    {
        Console.WriteLine("✈️ Vacation Mode! Optimizing for extended absence...");
        Console.WriteLine(new string('=', 50));
        
        // Turn off all entertainment
        _entertainmentSystem.TurnOffAllAudio();
        _entertainmentSystem.TurnOffAllVideo();
        
        // Minimal lighting with security focus
        _lightingSystem.TurnOffAllLights();
        _lightingSystem.SetZoneBrightness("outdoor", 100);
        _lightingSystem.EnableAutomaticMode(); // Automatic day/night cycle
        
        // Minimal climate control
        _climateSystem.ActivateSchedule("vacation");
        _climateSystem.EnableEcoMode();
        
        // Maximum security
        _securitySystem.ArmSystem(SecurityMode.Away);
        _securitySystem.StartCameraRecording();
        
        Console.WriteLine("✈️ Vacation mode activated! Have a wonderful trip!");
    }

    #endregion

    #region Emergency and Safety Operations

    /// <summary>
    /// Emergency mode - maximum security and safety.
    /// </summary>
    public void EmergencyMode()
    {
        Console.WriteLine("🚨 EMERGENCY MODE ACTIVATED!");
        Console.WriteLine(new string('=', 50));
        
        // Turn on all lights for visibility
        _lightingSystem.TurnOnAllLights();
        _lightingSystem.SetGlobalBrightness(100);
        
        // Stop all entertainment
        _entertainmentSystem.TurnOffAllAudio();
        _entertainmentSystem.TurnOffAllVideo();
        
        // Maintain climate for safety
        _climateSystem.StopAllSystems(); // Prevent any potential hazards
        
        // Maximum security
        _securitySystem.ArmSystem(SecurityMode.Away);
        _securitySystem.StartCameraRecording();
        
        Console.WriteLine("🚨 Emergency mode active! All systems secured!");
    }

    /// <summary>
    /// Power saving mode - minimize energy consumption.
    /// </summary>
    public void PowerSavingMode()
    {
        Console.WriteLine("⚡ Power Saving Mode! Minimizing energy consumption...");
        Console.WriteLine(new string('=', 50));
        
        // Minimal lighting
        _lightingSystem.TurnOffAllLights();
        _lightingSystem.SetZoneBrightness("hallway", 20); // Safety lighting only
        
        // Eco climate settings
        _climateSystem.EnableEcoMode();
        _climateSystem.SetTemperature(68.0); // Lower temperature to save energy
        
        // Turn off entertainment
        _entertainmentSystem.TurnOffAllAudio();
        _entertainmentSystem.TurnOffAllVideo();
        
        // Basic security only
        _securitySystem.DisableMotionDetection(); // Save power on sensors
        
        Console.WriteLine("⚡ Power saving mode activated! Energy consumption minimized.");
    }

    #endregion

    #region Status and Information

    /// <summary>
    /// Gets comprehensive status of all home systems.
    /// </summary>
    public SmartHomeStatus GetHomeStatus()
    {
        return new SmartHomeStatus
        {
            SecurityStatus = _securitySystem.GetSystemStatus(),
            LightingStatus = _lightingSystem.GetSystemStatus(),
            ClimateStatus = _climateSystem.GetSystemStatus(),
            EntertainmentStatus = _entertainmentSystem.GetSystemStatus(),
            Timestamp = DateTime.Now
        };
    }

    /// <summary>
    /// Displays a comprehensive status report.
    /// </summary>
    public void DisplayStatusReport()
    {
        Console.WriteLine("📊 Smart Home Status Report");
        Console.WriteLine(new string('=', 60));
        Console.WriteLine($"Report generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        Console.WriteLine();
        
        var status = GetHomeStatus();
        
        Console.WriteLine("🔒 Security System:");
        Console.WriteLine($"   {status.SecurityStatus}");
        Console.WriteLine();
        
        Console.WriteLine("💡 Lighting System:");
        Console.WriteLine($"   {status.LightingStatus}");
        Console.WriteLine();
        
        Console.WriteLine("🌡️ Climate Control:");
        Console.WriteLine($"   {status.ClimateStatus}");
        Console.WriteLine();
        
        Console.WriteLine("🎵 Entertainment System:");
        Console.WriteLine($"   {status.EntertainmentStatus}");
        Console.WriteLine();
        
        // Calculate total power consumption
        var totalPower = status.LightingStatus.PowerConsumption + 
                        status.ClimateStatus.EnergyUsage + 
                        status.EntertainmentStatus.PowerConsumption;
        
        Console.WriteLine($"⚡ Total Power Consumption: {totalPower:F1}W");
        Console.WriteLine(new string('=', 60));
    }

    /// <summary>
    /// Gets available home scenes.
    /// </summary>
    public List<string> GetAvailableScenes()
    {
        return _homeScenes.Keys.ToList();
    }

    /// <summary>
    /// Gets description of a specific scene.
    /// </summary>
    public string GetSceneDescription(string sceneName)
    {
        return _homeScenes.TryGetValue(sceneName, out var scene) 
            ? scene.Description 
            : "Scene not found";
    }

    #endregion

    #region Advanced Operations

    /// <summary>
    /// Activates a custom scene by name.
    /// </summary>
    public void ActivateScene(string sceneName)
    {
        if (_homeScenes.ContainsKey(sceneName))
        {
            Console.WriteLine($"🎭 Activating '{sceneName}' scene...");
            
            switch (sceneName.ToLower())
            {
                case "good_morning":
                    GoodMorning();
                    break;
                case "leaving_home":
                    LeavingHome();
                    break;
                case "coming_home":
                    ComingHome();
                    break;
                case "movie_night":
                    MovieNight();
                    break;
                case "dinner_party":
                    DinnerParty();
                    break;
                case "bedtime":
                    Bedtime();
                    break;
                case "vacation_mode":
                    VacationMode();
                    break;
                default:
                    Console.WriteLine($"❌ Scene '{sceneName}' not implemented yet");
                    break;
            }
        }
        else
        {
            Console.WriteLine($"❌ Scene '{sceneName}' not found");
        }
    }

    /// <summary>
    /// Performs a system-wide test of all subsystems.
    /// </summary>
    public void RunSystemTest()
    {
        Console.WriteLine("🔧 Running Smart Home System Test...");
        Console.WriteLine(new string('=', 50));
        
        Console.WriteLine("Testing Security System...");
        _securitySystem.TestAllSensors();
        
        Console.WriteLine("\nTesting Lighting System...");
        _lightingSystem.TurnOnAllLights();
        Thread.Sleep(500);
        _lightingSystem.TurnOffAllLights();
        
        Console.WriteLine("\nTesting Climate System...");
        _climateSystem.SetTemperature(72.0);
        
        Console.WriteLine("\nTesting Entertainment System...");
        _entertainmentSystem.SetGlobalVolume(20);
        _entertainmentSystem.TurnOnAllAudio();
        Thread.Sleep(500);
        _entertainmentSystem.TurnOffAllAudio();
        
        Console.WriteLine("\n✅ System test completed successfully!");
    }

    #endregion
}

/// <summary>
/// Represents a home scene configuration.
/// </summary>
public class HomeScene
{
    public string Name { get; }
    public string Description { get; }

    public HomeScene(string name, string description)
    {
        Name = name;
        Description = description;
    }
}

/// <summary>
/// Comprehensive status of the entire smart home system.
/// </summary>
public class SmartHomeStatus
{
    public SecurityStatus SecurityStatus { get; set; } = new();
    public LightingStatus LightingStatus { get; set; } = new();
    public ClimateStatus ClimateStatus { get; set; } = new();
    public EntertainmentStatus EntertainmentStatus { get; set; } = new();
    public DateTime Timestamp { get; set; }

    public override string ToString()
    {
        return $"""
            Smart Home Status ({Timestamp:HH:mm:ss}):
              Security: {SecurityStatus}
              Lighting: {LightingStatus}
              Climate: {ClimateStatus}
              Entertainment: {EntertainmentStatus}
            """;
    }
}
