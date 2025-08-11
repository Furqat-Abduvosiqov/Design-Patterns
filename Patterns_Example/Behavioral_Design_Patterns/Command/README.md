# Command Pattern Example

This folder contains a comprehensive implementation of the Command design pattern using a real-world smart home automation system that demonstrates how to encapsulate requests as objects and enable powerful features like undo/redo, macro operations, and command queuing.

## Files Overview

- **`ICommand.cs`** - Command interface, base classes, and command history management
- **`SmartHomeCommands.cs`** - Concrete command implementations and smart home devices
- **`CommandExample.cs`** - Comprehensive usage demonstrations
- **`CommandPatternDemo.cs`** - Main demo program
- **`CommandPatternGuide.md`** - Complete pattern explanation

## What Problem Does This Solve?

The Command pattern addresses the tight coupling between operation invokers and receivers, and the lack of operation management features. Instead of:

```csharp
// ❌ Direct coupling - inflexible and limited
public class RemoteControl
{
    private Light _light;
    private Fan _fan;
    
    public void PressButton1() => _light.TurnOn();    // Hard-coded
    public void PressButton2() => _light.TurnOff();   // No undo
    public void PressButton3() => _fan.TurnOn();      // No queuing
    // Problems: tight coupling, no undo, no macro operations
}
```

You can use:

```csharp
// ✅ Command pattern - flexible and powerful
var remote = new SmartHomeRemote();
remote.SetCommand("1", new TurnOnCommand(light));    // Configurable
remote.SetCommand("2", new TurnOffCommand(light));   // Undoable
remote.SetCommand("3", movieNightMacro);             // Macro support

remote.PressButton("1");  // Execute command
remote.Undo();           // Reverse operation
```

## Key Benefits Demonstrated

1. **Decoupled Invoker and Receiver**: Remote control doesn't know about specific devices
2. **Commands as First-Class Objects**: Can store, pass around, and manipulate commands
3. **Undo/Redo Functionality**: Easy to implement operation reversal
4. **Macro Commands**: Combine multiple operations into complex scenarios
5. **Command Queuing**: Schedule and batch operations
6. **Logging and Auditing**: Track all operations for debugging and compliance

## Real-World Use Cases

This pattern is commonly used for:

- **GUI Applications**: Menu items, toolbar buttons, keyboard shortcuts
- **Text Editors**: Undo/redo functionality, macro recording
- **Database Transactions**: Rollback and commit operations
- **Home Automation**: Device control with scheduling and scenes
- **Game Development**: Player actions, replay systems, AI commands
- **Web Applications**: Request handling, operation logging

## Components Demonstrated

### 1. Command Interface (`ICommand`)
Defines the contract for all commands:
```csharp
public interface ICommand
{
    void Execute();           // Perform the operation
    void Undo();             // Reverse the operation
    bool CanUndo { get; }    // Whether operation is reversible
    CommandInfo GetCommandInfo(); // Metadata about the command
}
```

### 2. Concrete Commands
Specific implementations for different operations:

**TurnOnCommand**: Turns a device on (remembers previous state for undo)
**TurnOffCommand**: Turns a device off (remembers previous state for undo)
**SetPropertyCommand**: Sets device properties like brightness, temperature
**DelayCommand**: Adds delays between operations (useful for sequences)
**MacroCommand**: Executes multiple commands as a single operation

### 3. Smart Home Devices (Receivers)
Objects that perform the actual work:
```csharp
public class SmartHomeDevice
{
    public string Name { get; }
    public string Type { get; }      // Light, Fan, TV, etc.
    public string Location { get; }  // Living Room, Bedroom, etc.
    public bool IsOn { get; private set; }
    public Dictionary<string, object> Properties { get; }
    
    public void TurnOn() { /* Implementation */ }
    public void TurnOff() { /* Implementation */ }
    public void SetProperty(string property, object value) { /* Implementation */ }
}
```

### 4. Smart Home Remote (Invoker)
Controls command execution and provides advanced features:
```csharp
public class SmartHomeRemote
{
    public void SetCommand(string slot, ICommand command);  // Assign commands
    public void PressButton(string slot);                   // Execute commands
    public void Undo();                                     // Reverse last operation
    public void Redo();                                     // Replay undone operation
    public void ShowHistory();                              // Display command history
}
```

## Advanced Features

### Command History with Undo/Redo
Comprehensive operation tracking and reversal:
```csharp
public class CommandHistory
{
    private readonly Stack<ICommand> _undoStack;
    private readonly Stack<ICommand> _redoStack;
    
    public void ExecuteCommand(ICommand command);
    public void Undo();
    public void Redo();
    public List<CommandInfo> GetUndoHistory();
    public List<CommandInfo> GetRedoHistory();
}
```

### Macro Commands (Composite Pattern Integration)
Complex operations combining multiple commands:
```csharp
var movieNightMacro = new MacroCommand("Movie Night", "Perfect cinema setup");
movieNightMacro.AddCommands(
    new SetPropertyCommand(lights, "brightness", 30),
    new DelayCommand(TimeSpan.FromMilliseconds(200)),
    new TurnOnCommand(tv),
    new SetPropertyCommand(tv, "channel", "Netflix"),
    new TurnOnCommand(soundSystem),
    new SetPropertyCommand(soundSystem, "volume", 40)
);
```

### Command Information and Metadata
Rich information about commands for logging and display:
```csharp
public class CommandInfo
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public Dictionary<string, object> Parameters { get; set; }
    public TimeSpan EstimatedExecutionTime { get; set; }
    public bool RequiresConfirmation { get; set; }
}
```

### Command Queuing and Scheduling
Batch operations and delayed execution:
```csharp
// Create a sequence of commands for automated coffee brewing
var commandQueue = new Queue<ICommand>();
commandQueue.Enqueue(new TurnOnCommand(coffeeMaker));
commandQueue.Enqueue(new DelayCommand(TimeSpan.FromMilliseconds(300), "Warming up"));
commandQueue.Enqueue(new SetPropertyCommand(coffeeMaker, "brew_strength", "strong"));
commandQueue.Enqueue(new SetPropertyCommand(coffeeMaker, "cup_size", "large"));
```

## How to Run

1. Navigate to the project root directory
2. Update Program.cs to use the Command example:
   ```csharp
   using Patterns_Example.Behavioral_Design_Patterns.Command;
   CommandExample.RunExample();
   ```
3. Run the demo:
   ```bash
   dotnet run --project Patterns_Example
   ```

## Example Output

The demo will show:

1. **Problems Without Command**: Direct coupling issues and limitations
2. **Basic Command Usage**: Command assignment and execution
3. **Undo/Redo Functionality**: Operation reversal and replay
4. **Macro Commands**: Complex multi-step operations
5. **Command Queuing**: Scheduled and batched operations
6. **Command History**: Logging and auditing capabilities

## Learning Objectives

After studying this example, you should understand:

- When to encapsulate requests as objects
- How to implement undo/redo functionality effectively
- The relationship between Command and other patterns (Composite, Memento)
- How to design flexible command hierarchies
- The trade-offs between flexibility and performance
- How to implement macro operations and command queuing

## Command Pattern Scenarios

### Basic Device Control:
```csharp
// Simple on/off commands
var turnOnLight = new TurnOnCommand(livingRoomLight);
var turnOffLight = new TurnOffCommand(livingRoomLight);

remote.SetCommand("1", turnOnLight);
remote.SetCommand("2", turnOffLight);
```

### Property Configuration:
```csharp
// Setting device properties
var setBrightness = new SetPropertyCommand(light, "brightness", 75);
var setTemperature = new SetPropertyCommand(thermostat, "temperature", 72);
var setVolume = new SetPropertyCommand(soundSystem, "volume", 40);
```

### Complex Scenarios (Macros):
```csharp
// "Good Morning" scenario
var goodMorningMacro = new MacroCommand("Good Morning", "Start the day right");
goodMorningMacro.AddCommands(
    new TurnOnCommand(lights),
    new SetPropertyCommand(lights, "brightness", 100),
    new SetPropertyCommand(thermostat, "temperature", 72),
    new TurnOnCommand(coffeeMaker)
);
```

### Undo/Redo Operations:
```csharp
remote.PressButton("1");        // Turn on light
remote.PressButton("brightness"); // Set brightness to 75
remote.Undo();                  // Undo brightness change
remote.Undo();                  // Undo light turn on
remote.Redo();                  // Redo light turn on
```

## Design Decisions

### Command Interface Design
Simple, consistent interface for all commands:
- `Execute()`: Perform the operation
- `Undo()`: Reverse the operation (if possible)
- `CanUndo`: Indicates if operation is reversible
- `GetCommandInfo()`: Provides metadata for logging/display

### State Management for Undo
Commands store previous state to enable proper undo:
- `TurnOnCommand` remembers if device was already on
- `SetPropertyCommand` stores previous property value
- `MacroCommand` undoes constituent commands in reverse order

### Command History Management
Bounded history to prevent memory issues:
- Configurable maximum history size
- Automatic cleanup of old commands
- Separate undo and redo stacks

## Extension Ideas

Try extending this example by:

1. **Adding Voice Commands**: Voice recognition integration with command execution
2. **Implementing Command Persistence**: Save/load command history to/from storage
3. **Creating Conditional Execution**: Commands that execute based on device state
4. **Adding Command Validation**: Validate commands before execution
5. **Implementing Command Batching**: Group related commands for efficiency
6. **Creating Smart Macros**: AI-powered command sequence optimization
7. **Adding Remote Execution**: Execute commands on remote devices over network

## Best Practices Demonstrated

1. **Clear Command Interface**: Simple, consistent interface for all commands
2. **Proper Undo Implementation**: Careful state management for reversible operations
3. **Command Information**: Rich metadata for logging and display purposes
4. **Error Handling**: Graceful handling of command execution failures
5. **Memory Management**: Bounded command history to prevent memory leaks
6. **Separation of Concerns**: Clear separation between command logic and device logic
7. **Extensible Design**: Easy to add new commands without changing existing code
8. **Comprehensive Logging**: Detailed audit trail for debugging and compliance

## Performance Considerations

1. **Command Object Creation**: Each operation creates a command object (minimal overhead)
2. **Memory Usage**: Command history can consume memory (bounded by configuration)
3. **Undo Stack Size**: Configurable limits prevent excessive memory usage
4. **Macro Command Complexity**: Large macros may impact performance
5. **Command Serialization**: Consider for persistent undo/redo across sessions
