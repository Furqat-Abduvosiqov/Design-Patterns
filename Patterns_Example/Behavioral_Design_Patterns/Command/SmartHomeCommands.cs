namespace Patterns_Example.Behavioral_Design_Patterns.Command;

/// <summary>
/// Smart home device that can be controlled by commands.
/// This represents the Receiver in the Command pattern.
/// </summary>
public class SmartHomeDevice
{
    public string Name { get; }
    public string Type { get; }
    public string Location { get; }
    public bool IsOn { get; private set; }
    public Dictionary<string, object> Properties { get; }
    public List<string> ActionHistory { get; }

    public SmartHomeDevice(string name, string type, string location)
    {
        Name = name;
        Type = type;
        Location = location;
        Properties = new Dictionary<string, object>();
        ActionHistory = new List<string>();
    }

    public void TurnOn()
    {
        IsOn = true;
        LogAction("Turned ON");
        Console.WriteLine($"🔌 {Name} ({Type}) in {Location} is now ON");
    }

    public void TurnOff()
    {
        IsOn = false;
        LogAction("Turned OFF");
        Console.WriteLine($"⚫ {Name} ({Type}) in {Location} is now OFF");
    }

    public void SetProperty(string property, object value)
    {
        var oldValue = Properties.GetValueOrDefault(property, "none");
        Properties[property] = value;
        LogAction($"Set {property} from {oldValue} to {value}");
        Console.WriteLine($"⚙️ {Name}: {property} set to {value}");
    }

    public object GetProperty(string property)
    {
        return Properties.GetValueOrDefault(property, null);
    }

    private void LogAction(string action)
    {
        ActionHistory.Add($"{DateTime.Now:HH:mm:ss} - {action}");
    }

    public override string ToString()
    {
        var status = IsOn ? "ON" : "OFF";
        var props = Properties.Any() 
            ? $" [{string.Join(", ", Properties.Select(kvp => $"{kvp.Key}={kvp.Value}"))}]"
            : "";
        return $"{Name} ({Type}) in {Location}: {status}{props}";
    }
}

/// <summary>
/// Command to turn a device on.
/// </summary>
public class TurnOnCommand : BaseCommand
{
    private readonly SmartHomeDevice _device;
    private bool _wasOn;

    public override bool CanUndo => true;

    public TurnOnCommand(SmartHomeDevice device)
    {
        _device = device ?? throw new ArgumentNullException(nameof(device));
    }

    public override void Execute()
    {
        _wasOn = _device.IsOn;
        _device.TurnOn();
        LogExecution("EXECUTED");
    }

    public override void Undo()
    {
        if (_wasOn)
        {
            _device.TurnOn();
        }
        else
        {
            _device.TurnOff();
        }
        LogExecution("UNDONE");
    }

    public override CommandInfo GetCommandInfo()
    {
        return new CommandInfo
        {
            Name = "Turn On",
            Description = $"Turn on {_device.Name}",
            Category = "Device Control",
            Parameters = new Dictionary<string, object>
            {
                ["device"] = _device.Name,
                ["location"] = _device.Location
            },
            EstimatedExecutionTime = TimeSpan.FromMilliseconds(100)
        };
    }
}

/// <summary>
/// Command to turn a device off.
/// </summary>
public class TurnOffCommand : BaseCommand
{
    private readonly SmartHomeDevice _device;
    private bool _wasOn;

    public override bool CanUndo => true;

    public TurnOffCommand(SmartHomeDevice device)
    {
        _device = device ?? throw new ArgumentNullException(nameof(device));
    }

    public override void Execute()
    {
        _wasOn = _device.IsOn;
        _device.TurnOff();
        LogExecution("EXECUTED");
    }

    public override void Undo()
    {
        if (_wasOn)
        {
            _device.TurnOn();
        }
        else
        {
            _device.TurnOff();
        }
        LogExecution("UNDONE");
    }

    public override CommandInfo GetCommandInfo()
    {
        return new CommandInfo
        {
            Name = "Turn Off",
            Description = $"Turn off {_device.Name}",
            Category = "Device Control",
            Parameters = new Dictionary<string, object>
            {
                ["device"] = _device.Name,
                ["location"] = _device.Location
            },
            EstimatedExecutionTime = TimeSpan.FromMilliseconds(100)
        };
    }
}

/// <summary>
/// Command to set a device property (like brightness, temperature, etc.).
/// </summary>
public class SetPropertyCommand : BaseCommand
{
    private readonly SmartHomeDevice _device;
    private readonly string _property;
    private readonly object _newValue;
    private object _oldValue;

    public override bool CanUndo => true;

    public SetPropertyCommand(SmartHomeDevice device, string property, object value)
    {
        _device = device ?? throw new ArgumentNullException(nameof(device));
        _property = property ?? throw new ArgumentNullException(nameof(property));
        _newValue = value ?? throw new ArgumentNullException(nameof(value));
    }

    public override void Execute()
    {
        _oldValue = _device.GetProperty(_property);
        _device.SetProperty(_property, _newValue);
        LogExecution("EXECUTED");
    }

    public override void Undo()
    {
        if (_oldValue != null)
        {
            _device.SetProperty(_property, _oldValue);
        }
        LogExecution("UNDONE");
    }

    public override CommandInfo GetCommandInfo()
    {
        return new CommandInfo
        {
            Name = "Set Property",
            Description = $"Set {_property} of {_device.Name} to {_newValue}",
            Category = "Device Configuration",
            Parameters = new Dictionary<string, object>
            {
                ["device"] = _device.Name,
                ["property"] = _property,
                ["value"] = _newValue
            },
            EstimatedExecutionTime = TimeSpan.FromMilliseconds(150)
        };
    }
}

/// <summary>
/// Command to delay execution (useful for scheduling).
/// </summary>
public class DelayCommand : BaseCommand
{
    private readonly TimeSpan _delay;
    private readonly string _reason;

    public override bool CanUndo => false; // Cannot undo time

    public DelayCommand(TimeSpan delay, string reason = "")
    {
        _delay = delay;
        _reason = reason;
    }

    public override void Execute()
    {
        LogExecution("EXECUTING");
        Console.WriteLine($"⏳ Waiting {_delay.TotalMilliseconds}ms{(_reason != "" ? $" ({_reason})" : "")}...");
        Thread.Sleep(_delay);
        Console.WriteLine("✅ Delay completed");
    }

    public override void Undo()
    {
        throw new InvalidOperationException("Cannot undo a delay command");
    }

    public override CommandInfo GetCommandInfo()
    {
        return new CommandInfo
        {
            Name = "Delay",
            Description = $"Wait for {_delay.TotalMilliseconds}ms",
            Category = "Timing",
            Parameters = new Dictionary<string, object>
            {
                ["delay_ms"] = _delay.TotalMilliseconds,
                ["reason"] = _reason
            },
            EstimatedExecutionTime = _delay
        };
    }
}

/// <summary>
/// Remote control that acts as the Invoker in the Command pattern.
/// </summary>
public class SmartHomeRemote
{
    private readonly Dictionary<string, ICommand> _commands;
    private readonly CommandHistory _history;

    public SmartHomeRemote()
    {
        _commands = new Dictionary<string, ICommand>();
        _history = new CommandHistory();
    }

    public void SetCommand(string slot, ICommand command)
    {
        _commands[slot] = command;
        Console.WriteLine($"📱 Remote: Command '{command.GetCommandInfo().Name}' assigned to slot '{slot}'");
    }

    public void PressButton(string slot)
    {
        if (_commands.TryGetValue(slot, out var command))
        {
            Console.WriteLine($"📱 Remote: Pressing button '{slot}'");
            _history.ExecuteCommand(command);
        }
        else
        {
            Console.WriteLine($"❌ Remote: No command assigned to slot '{slot}'");
        }
    }

    public void Undo()
    {
        if (_history.CanUndo)
        {
            Console.WriteLine("📱 Remote: Pressing UNDO button");
            _history.Undo();
        }
        else
        {
            Console.WriteLine("❌ Remote: Nothing to undo");
        }
    }

    public void Redo()
    {
        if (_history.CanRedo)
        {
            Console.WriteLine("📱 Remote: Pressing REDO button");
            _history.Redo();
        }
        else
        {
            Console.WriteLine("❌ Remote: Nothing to redo");
        }
    }

    public void ShowHistory()
    {
        Console.WriteLine("📱 Remote: Command History");
        Console.WriteLine("Undo Stack:");
        var undoHistory = _history.GetUndoHistory();
        for (int i = 0; i < undoHistory.Count; i++)
        {
            Console.WriteLine($"  {i + 1}. {undoHistory[i]}");
        }

        Console.WriteLine("Redo Stack:");
        var redoHistory = _history.GetRedoHistory();
        for (int i = 0; i < redoHistory.Count; i++)
        {
            Console.WriteLine($"  {i + 1}. {redoHistory[i]}");
        }
    }

    public void ShowCommands()
    {
        Console.WriteLine("📱 Remote: Available Commands");
        foreach (var kvp in _commands)
        {
            Console.WriteLine($"  [{kvp.Key}] {kvp.Value.GetCommandInfo()}");
        }
    }

    public CommandHistory GetHistory() => _history;
}
