namespace Patterns_Example.Structural_Design_Patterns.Facade;

/// <summary>
/// Complex subsystem for home entertainment management.
/// This represents another complex subsystem that the Facade will simplify.
/// </summary>
public class EntertainmentSystem
{
    private readonly Dictionary<string, AudioZone> _audioZones;
    private readonly Dictionary<string, VideoDevice> _videoDevices;
    private readonly Dictionary<string, EntertainmentScene> _scenes;
    private bool _partyMode;
    private int _globalVolume;

    public bool IsPartyMode => _partyMode;
    public int GlobalVolume => _globalVolume;

    public EntertainmentSystem()
    {
        _audioZones = new Dictionary<string, AudioZone>
        {
            ["living_room"] = new AudioZone("Living Room", AudioType.Surround, 8),
            ["kitchen"] = new AudioZone("Kitchen", AudioType.Stereo, 4),
            ["bedroom"] = new AudioZone("Bedroom", AudioType.Stereo, 2),
            ["bathroom"] = new AudioZone("Bathroom", AudioType.Mono, 1),
            ["outdoor"] = new AudioZone("Outdoor", AudioType.Weather, 6)
        };

        _videoDevices = new Dictionary<string, VideoDevice>
        {
            ["main_tv"] = new VideoDevice("Main TV", VideoType.SmartTV, "Living Room"),
            ["bedroom_tv"] = new VideoDevice("Bedroom TV", VideoType.SmartTV, "Bedroom"),
            ["kitchen_display"] = new VideoDevice("Kitchen Display", VideoType.Tablet, "Kitchen"),
            ["projector"] = new VideoDevice("Projector", VideoType.Projector, "Living Room")
        };

        _scenes = new Dictionary<string, EntertainmentScene>
        {
            ["movie"] = new EntertainmentScene("Movie Night", new Dictionary<string, int>
            {
                ["living_room"] = 60
            }, new List<string> { "main_tv" }),
            ["party"] = new EntertainmentScene("Party", new Dictionary<string, int>
            {
                ["living_room"] = 80,
                ["kitchen"] = 70,
                ["outdoor"] = 75
            }, new List<string> { "main_tv", "kitchen_display" }),
            ["dinner"] = new EntertainmentScene("Dinner", new Dictionary<string, int>
            {
                ["kitchen"] = 40,
                ["living_room"] = 30
            }, new List<string> { "kitchen_display" }),
            ["bedtime"] = new EntertainmentScene("Bedtime", new Dictionary<string, int>
            {
                ["bedroom"] = 20
            }, new List<string> { "bedroom_tv" })
        };

        _partyMode = false;
        _globalVolume = 50;
    }

    public void TurnOnAllAudio()
    {
        Console.WriteLine("[Entertainment] Turning on all audio zones...");
        
        foreach (var zone in _audioZones.Values)
        {
            zone.TurnOn();
            Console.WriteLine($"[Entertainment]   {zone.Name}: ON at volume {zone.Volume}");
            Thread.Sleep(15);
        }
        
        Console.WriteLine("[Entertainment] All audio zones are now on");
    }

    public void TurnOffAllAudio()
    {
        Console.WriteLine("[Entertainment] Turning off all audio zones...");
        
        foreach (var zone in _audioZones.Values)
        {
            zone.TurnOff();
            Console.WriteLine($"[Entertainment]   {zone.Name}: OFF");
            Thread.Sleep(10);
        }
        
        Console.WriteLine("[Entertainment] All audio zones are now off");
    }

    public void TurnOnAllVideo()
    {
        Console.WriteLine("[Entertainment] Turning on all video devices...");
        
        foreach (var device in _videoDevices.Values)
        {
            device.TurnOn();
            Console.WriteLine($"[Entertainment]   {device.Name}: ON");
            Thread.Sleep(20);
        }
        
        Console.WriteLine("[Entertainment] All video devices are now on");
    }

    public void TurnOffAllVideo()
    {
        Console.WriteLine("[Entertainment] Turning off all video devices...");
        
        foreach (var device in _videoDevices.Values)
        {
            device.TurnOff();
            Console.WriteLine($"[Entertainment]   {device.Name}: OFF");
            Thread.Sleep(15);
        }
        
        Console.WriteLine("[Entertainment] All video devices are now off");
    }

    public void SetGlobalVolume(int volume)
    {
        Console.WriteLine($"[Entertainment] Setting global volume to {volume}%...");
        _globalVolume = Math.Max(0, Math.Min(100, volume));
        
        foreach (var zone in _audioZones.Values)
        {
            if (zone.IsOn)
            {
                zone.SetVolume(_globalVolume);
                Console.WriteLine($"[Entertainment]   {zone.Name}: {_globalVolume}%");
            }
        }
        
        Console.WriteLine("[Entertainment] Global volume set");
    }

    public void SetZoneVolume(string zoneName, int volume)
    {
        if (_audioZones.TryGetValue(zoneName, out var zone))
        {
            Console.WriteLine($"[Entertainment] Setting {zone.Name} volume to {volume}%");
            zone.SetVolume(volume);
        }
        else
        {
            Console.WriteLine($"[Entertainment] ❌ Audio zone '{zoneName}' not found");
        }
    }

    public void PlayMusic(string source, string zoneName = "all")
    {
        Console.WriteLine($"[Entertainment] Playing music from {source}...");
        
        if (zoneName == "all")
        {
            foreach (var zone in _audioZones.Values)
            {
                zone.PlaySource(source);
                Console.WriteLine($"[Entertainment]   {zone.Name}: Playing {source}");
            }
        }
        else if (_audioZones.TryGetValue(zoneName, out var zone))
        {
            zone.PlaySource(source);
            Console.WriteLine($"[Entertainment]   {zone.Name}: Playing {source}");
        }
        
        Console.WriteLine("[Entertainment] Music playback started");
    }

    public void StopMusic(string zoneName = "all")
    {
        Console.WriteLine("[Entertainment] Stopping music...");
        
        if (zoneName == "all")
        {
            foreach (var zone in _audioZones.Values)
            {
                zone.Stop();
                Console.WriteLine($"[Entertainment]   {zone.Name}: Stopped");
            }
        }
        else if (_audioZones.TryGetValue(zoneName, out var zone))
        {
            zone.Stop();
            Console.WriteLine($"[Entertainment]   {zone.Name}: Stopped");
        }
        
        Console.WriteLine("[Entertainment] Music stopped");
    }

    public void ActivateScene(string sceneName)
    {
        if (_scenes.TryGetValue(sceneName, out var scene))
        {
            Console.WriteLine($"[Entertainment] Activating '{scene.Name}' scene...");
            
            // Turn off all devices first
            TurnOffAllAudio();
            TurnOffAllVideo();
            Thread.Sleep(100);
            
            // Apply audio settings
            foreach (var audioSetting in scene.AudioSettings)
            {
                if (_audioZones.TryGetValue(audioSetting.Key, out var zone))
                {
                    zone.TurnOn();
                    zone.SetVolume(audioSetting.Value);
                    Console.WriteLine($"[Entertainment]   Audio {zone.Name}: {audioSetting.Value}%");
                }
            }
            
            // Turn on specified video devices
            foreach (var deviceName in scene.ActiveVideoDevices)
            {
                if (_videoDevices.TryGetValue(deviceName, out var device))
                {
                    device.TurnOn();
                    Console.WriteLine($"[Entertainment]   Video {device.Name}: ON");
                }
            }
            
            Console.WriteLine($"[Entertainment] '{scene.Name}' scene activated");
        }
        else
        {
            Console.WriteLine($"[Entertainment] ❌ Scene '{sceneName}' not found");
        }
    }

    public void EnablePartyMode()
    {
        Console.WriteLine("[Entertainment] Enabling party mode...");
        _partyMode = true;
        
        // Activate party scene
        ActivateScene("party");
        
        // Set upbeat music
        PlayMusic("Party Playlist");
        
        Console.WriteLine("[Entertainment] 🎉 Party mode enabled!");
    }

    public void DisablePartyMode()
    {
        Console.WriteLine("[Entertainment] Disabling party mode...");
        _partyMode = false;
        
        // Stop all music and reduce volume
        StopMusic();
        SetGlobalVolume(30);
        
        Console.WriteLine("[Entertainment] Party mode disabled");
    }

    public void StartMovieMode()
    {
        Console.WriteLine("[Entertainment] Starting movie mode...");
        
        ActivateScene("movie");
        
        // Optimize audio for movies
        if (_audioZones.TryGetValue("living_room", out var livingRoom))
        {
            livingRoom.SetAudioMode(AudioMode.Movie);
        }
        
        Console.WriteLine("[Entertainment] 🎬 Movie mode ready!");
    }

    public void MuteAll()
    {
        Console.WriteLine("[Entertainment] Muting all audio...");
        
        foreach (var zone in _audioZones.Values)
        {
            zone.Mute();
            Console.WriteLine($"[Entertainment]   {zone.Name}: MUTED");
        }
        
        Console.WriteLine("[Entertainment] All audio muted");
    }

    public void UnmuteAll()
    {
        Console.WriteLine("[Entertainment] Unmuting all audio...");
        
        foreach (var zone in _audioZones.Values)
        {
            zone.Unmute();
            Console.WriteLine($"[Entertainment]   {zone.Name}: UNMUTED");
        }
        
        Console.WriteLine("[Entertainment] All audio unmuted");
    }

    public EntertainmentStatus GetSystemStatus()
    {
        var activeAudioZones = _audioZones.Values.Where(z => z.IsOn).ToList();
        var activeVideoDevices = _videoDevices.Values.Where(d => d.IsOn).ToList();
        var totalPowerConsumption = activeAudioZones.Sum(z => z.GetPowerConsumption()) + 
                                   activeVideoDevices.Sum(d => d.GetPowerConsumption());
        
        return new EntertainmentStatus
        {
            PartyMode = _partyMode,
            GlobalVolume = _globalVolume,
            ActiveAudioZones = activeAudioZones.Count,
            TotalAudioZones = _audioZones.Count,
            ActiveVideoDevices = activeVideoDevices.Count,
            TotalVideoDevices = _videoDevices.Count,
            PowerConsumption = totalPowerConsumption,
            AudioZoneDetails = _audioZones.Values.ToList(),
            VideoDeviceDetails = _videoDevices.Values.ToList()
        };
    }

    public List<string> GetAvailableScenes()
    {
        return _scenes.Keys.ToList();
    }

    public List<string> GetAvailableAudioZones()
    {
        return _audioZones.Keys.ToList();
    }

    public List<string> GetAvailableVideoDevices()
    {
        return _videoDevices.Keys.ToList();
    }
}

/// <summary>
/// Represents an audio zone in the entertainment system.
/// </summary>
public class AudioZone
{
    public string Name { get; }
    public AudioType Type { get; }
    public int SpeakerCount { get; }
    public bool IsOn { get; private set; }
    public int Volume { get; private set; }
    public bool IsMuted { get; private set; }
    public string CurrentSource { get; private set; }
    public AudioMode Mode { get; private set; }

    public AudioZone(string name, AudioType type, int speakerCount)
    {
        Name = name;
        Type = type;
        SpeakerCount = speakerCount;
        IsOn = false;
        Volume = 50;
        IsMuted = false;
        CurrentSource = "None";
        Mode = AudioMode.Music;
    }

    public void TurnOn()
    {
        IsOn = true;
    }

    public void TurnOff()
    {
        IsOn = false;
        CurrentSource = "None";
    }

    public void SetVolume(int volume)
    {
        Volume = Math.Max(0, Math.Min(100, volume));
        if (Volume > 0 && !IsOn)
        {
            TurnOn();
        }
    }

    public void Mute()
    {
        IsMuted = true;
    }

    public void Unmute()
    {
        IsMuted = false;
    }

    public void PlaySource(string source)
    {
        if (!IsOn) TurnOn();
        CurrentSource = source;
    }

    public void Stop()
    {
        CurrentSource = "None";
    }

    public void SetAudioMode(AudioMode mode)
    {
        Mode = mode;
    }

    public double GetPowerConsumption()
    {
        if (!IsOn) return 0;
        
        var basePower = Type switch
        {
            AudioType.Mono => 15.0,      // 15W per speaker
            AudioType.Stereo => 25.0,    // 25W per speaker
            AudioType.Surround => 40.0,  // 40W per speaker
            AudioType.Weather => 30.0,   // 30W per weather-resistant speaker
            _ => 20.0
        };
        
        return SpeakerCount * basePower * (Volume / 100.0);
    }

    public override string ToString()
    {
        var status = IsOn ? $"ON ({Volume}%)" : "OFF";
        if (IsMuted && IsOn) status += " [MUTED]";
        return $"{Name}: {status} | {Type} ({SpeakerCount} speakers) | Playing: {CurrentSource}";
    }
}

/// <summary>
/// Represents a video device in the entertainment system.
/// </summary>
public class VideoDevice
{
    public string Name { get; }
    public VideoType Type { get; }
    public string Location { get; }
    public bool IsOn { get; private set; }
    public string CurrentInput { get; private set; }

    public VideoDevice(string name, VideoType type, string location)
    {
        Name = name;
        Type = type;
        Location = location;
        IsOn = false;
        CurrentInput = "None";
    }

    public void TurnOn()
    {
        IsOn = true;
        CurrentInput = "HDMI1"; // Default input
    }

    public void TurnOff()
    {
        IsOn = false;
        CurrentInput = "None";
    }

    public void SetInput(string input)
    {
        if (IsOn)
        {
            CurrentInput = input;
        }
    }

    public double GetPowerConsumption()
    {
        if (!IsOn) return 0;
        
        return Type switch
        {
            VideoType.SmartTV => 120.0,    // 120W for smart TV
            VideoType.Projector => 200.0,  // 200W for projector
            VideoType.Tablet => 15.0,      // 15W for tablet
            VideoType.Monitor => 50.0,     // 50W for monitor
            _ => 75.0
        };
    }

    public override string ToString()
    {
        var status = IsOn ? $"ON ({CurrentInput})" : "OFF";
        return $"{Name}: {status} | {Type} in {Location}";
    }
}

/// <summary>
/// Represents an entertainment scene with predefined settings.
/// </summary>
public class EntertainmentScene
{
    public string Name { get; }
    public Dictionary<string, int> AudioSettings { get; }
    public List<string> ActiveVideoDevices { get; }

    public EntertainmentScene(string name, Dictionary<string, int> audioSettings, List<string> activeVideoDevices)
    {
        Name = name;
        AudioSettings = new Dictionary<string, int>(audioSettings);
        ActiveVideoDevices = new List<string>(activeVideoDevices);
    }
}

/// <summary>
/// Current status of the entertainment system.
/// </summary>
public class EntertainmentStatus
{
    public bool PartyMode { get; set; }
    public int GlobalVolume { get; set; }
    public int ActiveAudioZones { get; set; }
    public int TotalAudioZones { get; set; }
    public int ActiveVideoDevices { get; set; }
    public int TotalVideoDevices { get; set; }
    public double PowerConsumption { get; set; }
    public List<AudioZone> AudioZoneDetails { get; set; } = new();
    public List<VideoDevice> VideoDeviceDetails { get; set; } = new();

    public override string ToString()
    {
        var mode = PartyMode ? "PARTY" : "NORMAL";
        return $"Entertainment: {mode} | Audio: {ActiveAudioZones}/{TotalAudioZones} zones | Video: {ActiveVideoDevices}/{TotalVideoDevices} devices | Volume: {GlobalVolume}% | Power: {PowerConsumption:F0}W";
    }
}

/// <summary>
/// Types of audio systems.
/// </summary>
public enum AudioType
{
    Mono,       // Single speaker
    Stereo,     // Two speakers
    Surround,   // Multi-speaker surround sound
    Weather     // Weather-resistant outdoor speakers
}

/// <summary>
/// Types of video devices.
/// </summary>
public enum VideoType
{
    SmartTV,    // Smart television
    Projector,  // Video projector
    Tablet,     // Tablet display
    Monitor     // Computer monitor
}

/// <summary>
/// Audio playback modes.
/// </summary>
public enum AudioMode
{
    Music,      // Optimized for music
    Movie,      // Optimized for movies/TV
    Voice,      // Optimized for voice/calls
    Gaming      // Optimized for gaming
}
