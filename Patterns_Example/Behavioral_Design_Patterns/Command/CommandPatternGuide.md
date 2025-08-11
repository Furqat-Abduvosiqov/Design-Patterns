# Command Design Pattern

The Command pattern is a behavioral design pattern that turns a request into a stand-alone object containing all information about the request. This transformation lets you parameterize methods with different requests, delay or queue a request's execution, and support undoable operations.

## Problem

Imagine you're building a smart home automation system where you need to control various devices through a remote control. Without the Command pattern, you'd face several issues:

```csharp
// ❌ Without Command pattern - tight coupling
public class RemoteControl
{
    private Light _light;
    private Fan _fan;
    private TV _tv;
    
    public void PressButton1() => _light.TurnOn();
    public void PressButton2() => _light.TurnOff();
    public void PressButton3() => _fan.TurnOn();
    public void PressButton4() => _tv.TurnOn();
    // Hard-coded operations, no flexibility
}
```

### Issues with Direct Coupling:

1. **Tight Coupling**: Remote control directly depends on specific device classes
2. **No Undo Functionality**: Cannot reverse operations
3. **No Queuing**: Cannot schedule or batch operations
4. **No Logging**: No audit trail of operations
5. **Inflexibility**: Hard to add new operations or change button assignments
6. **No Macro Operations**: Cannot combine multiple operations

## Solution

The Command pattern suggests encapsulating requests as objects, allowing you to:

1. **Command Interface**: Defines the contract for executing operations
2. **Concrete Commands**: Implement specific operations
3. **Receiver**: The object that performs the actual work
4. **Invoker**: Asks the command to carry out the request
5. **Client**: Creates command objects and sets their receivers

### Key Components:

1. **Command (ICommand)**: Interface for executing operations
2. **ConcreteCommand**: Implements command interface for specific operations
3. **Receiver (SmartHomeDevice)**: Knows how to perform operations
4. **Invoker (SmartHomeRemote)**: Asks command to execute
5. **Client**: Creates and configures commands

## Real-World Example: Smart Home Automation

Our example demonstrates a smart home system where:

### Smart Home Devices (Receivers):
- **Lights**: Turn on/off, adjust brightness, change color
- **Fans**: Turn on/off, adjust speed, set timer
- **TV**: Turn on/off, change channel, adjust volume
- **Thermostat**: Set temperature, change mode
- **Security System**: Arm/disarm, set codes

### Commands:
- **TurnOnCommand**: Turn any device on
- **TurnOffCommand**: Turn any device off
- **SetPropertyCommand**: Set device properties (brightness, temperature, etc.)
- **DelayCommand**: Add delays between operations
- **MacroCommand**: Execute multiple commands as one operation

### Remote Control (Invoker):
- **Button Assignment**: Assign commands to remote buttons
- **Command Execution**: Execute commands when buttons are pressed
- **Undo/Redo**: Reverse and replay operations
- **History Tracking**: Keep track of executed commands

## Benefits Demonstrated

1. **Decoupled Invoker and Receiver**: Remote doesn't know about specific devices
2. **Commands as Objects**: Can store, pass around, and manipulate commands
3. **Undo/Redo Functionality**: Easy to implement operation reversal
4. **Macro Commands**: Combine multiple operations into complex scenarios
5. **Command Queuing**: Schedule and batch operations
6. **Logging and Auditing**: Track all operations for debugging and compliance

## Implementation Structure

```csharp
// Command interface
public interface ICommand
{
    void Execute();
    void Undo();
    bool CanUndo { get; }
    CommandInfo GetCommandInfo();
}

// Concrete command
public class TurnOnCommand : ICommand
{
    private readonly SmartHomeDevice _device;
    private bool _wasOn;
    
    public void Execute()
    {
        _wasOn = _device.IsOn;
        _device.TurnOn();
    }
    
    public void Undo()
    {
        if (!_wasOn) _device.TurnOff();
    }
}

// Receiver
public class SmartHomeDevice
{
    public void TurnOn() { /* Implementation */ }
    public void TurnOff() { /* Implementation */ }
    public void SetProperty(string prop, object value) { /* Implementation */ }
}

// Invoker
public class SmartHomeRemote
{
    private readonly Dictionary<string, ICommand> _commands;
    private readonly CommandHistory _history;
    
    public void SetCommand(string slot, ICommand command)
    {
        _commands[slot] = command;
    }
    
    public void PressButton(string slot)
    {
        if (_commands.TryGetValue(slot, out var command))
            _history.ExecuteCommand(command);
    }
}
```

## When to Use Command Pattern

✅ **Use Command when:**
- You want to parameterize objects with operations
- You need to queue operations, schedule their execution, or execute them remotely
- You want to support undo operations
- You need to log changes so you can reapply them in case of a system crash
- You want to structure a system around high-level operations built on primitive operations

❌ **Don't use Command when:**
- The operation is simple and doesn't benefit from the pattern's complexity
- You don't need undo functionality, queuing, or logging
- The relationship between invoker and receiver is stable and unlikely to change
- Performance is critical and the command overhead is significant

## Advanced Features

### 1. Macro Commands (Composite Pattern Integration)
```csharp
public class MacroCommand : ICommand
{
    private readonly List<ICommand> _commands = new();
    
    public void AddCommand(ICommand command) => _commands.Add(command);
    
    public void Execute()
    {
        foreach (var command in _commands)
            command.Execute();
    }
    
    public void Undo()
    {
        // Undo in reverse order
        for (int i = _commands.Count - 1; i >= 0; i--)
            _commands[i].Undo();
    }
}
```

### 2. Command History with Undo/Redo
```csharp
public class CommandHistory
{
    private readonly Stack<ICommand> _undoStack = new();
    private readonly Stack<ICommand> _redoStack = new();
    
    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        if (command.CanUndo)
        {
            _undoStack.Push(command);
            _redoStack.Clear(); // Clear redo stack
        }
    }
    
    public void Undo()
    {
        if (_undoStack.Count > 0)
        {
            var command = _undoStack.Pop();
            command.Undo();
            _redoStack.Push(command);
        }
    }
    
    public void Redo()
    {
        if (_redoStack.Count > 0)
        {
            var command = _redoStack.Pop();
            command.Execute();
            _undoStack.Push(command);
        }
    }
}
```

### 3. Command Queuing and Scheduling
```csharp
public class CommandScheduler
{
    private readonly Queue<ICommand> _commandQueue = new();
    
    public void ScheduleCommand(ICommand command)
    {
        _commandQueue.Enqueue(command);
    }
    
    public void ExecuteScheduledCommands()
    {
        while (_commandQueue.Count > 0)
        {
            var command = _commandQueue.Dequeue();
            command.Execute();
        }
    }
}
```

## Code Structure

```
Command/
├── ICommand.cs                    # Command interface and base classes
├── SmartHomeCommands.cs           # Concrete command implementations
├── CommandExample.cs              # Main demo program
└── CommandPatternGuide.md         # This documentation
```

## Performance Considerations

1. **Command Object Creation**: Each operation creates a command object
2. **Memory Usage**: Command history can consume significant memory
3. **Undo Stack Size**: Limit history size to prevent memory issues
4. **Macro Command Complexity**: Large macros can impact performance
5. **Command Serialization**: Consider serialization for persistent undo/redo

## Testing Benefits

1. **Command Isolation**: Test commands independently of invokers
2. **Mock Receivers**: Easy to mock devices for testing
3. **History Testing**: Verify undo/redo functionality
4. **Macro Testing**: Test complex command combinations
5. **State Verification**: Check device state after command execution

## Common Variations

### 1. Parameterized Commands
Commands that accept parameters at execution time:
```csharp
public class ParameterizedCommand : ICommand
{
    private readonly Action<object[]> _action;
    private object[] _parameters;
    
    public void Execute(params object[] parameters)
    {
        _parameters = parameters;
        _action(parameters);
    }
}
```

### 2. Asynchronous Commands
Commands that support async operations:
```csharp
public interface IAsyncCommand
{
    Task ExecuteAsync();
    Task UndoAsync();
}
```

### 3. Conditional Commands
Commands that execute based on conditions:
```csharp
public class ConditionalCommand : ICommand
{
    private readonly Func<bool> _condition;
    private readonly ICommand _command;
    
    public void Execute()
    {
        if (_condition())
            _command.Execute();
    }
}
```

## Learning Objectives

After studying this example, you should understand:

- When to encapsulate requests as objects
- How to implement undo/redo functionality effectively
- The relationship between Command and other patterns (Composite, Memento)
- How to design flexible command hierarchies
- The trade-offs between flexibility and performance
- How to implement macro operations and command queuing

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
3. **Command Information**: Rich metadata for logging and display
4. **Error Handling**: Graceful handling of command execution failures
5. **Memory Management**: Bounded command history to prevent memory leaks
6. **Separation of Concerns**: Clear separation between command logic and device logic
7. **Extensible Design**: Easy to add new commands without changing existing code
8. **Comprehensive Logging**: Detailed audit trail for debugging and compliance
