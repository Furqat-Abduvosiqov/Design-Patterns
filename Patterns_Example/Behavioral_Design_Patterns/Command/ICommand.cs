namespace Patterns_Example.Behavioral_Design_Patterns.Command;

/// <summary>
/// Command interface that defines the contract for all commands.
/// This is the core of the Command pattern - encapsulating requests as objects.
/// </summary>
public interface ICommand
{
    /// <summary>
    /// Executes the command.
    /// </summary>
    void Execute();
    
    /// <summary>
    /// Undoes the command (if possible).
    /// </summary>
    void Undo();
    
    /// <summary>
    /// Gets information about this command.
    /// </summary>
    CommandInfo GetCommandInfo();
    
    /// <summary>
    /// Checks if this command can be undone.
    /// </summary>
    bool CanUndo { get; }
    
    /// <summary>
    /// Gets the timestamp when the command was created.
    /// </summary>
    DateTime CreatedAt { get; }
}

/// <summary>
/// Information about a command for logging and display purposes.
/// </summary>
public class CommandInfo
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public Dictionary<string, object> Parameters { get; set; } = new();
    public TimeSpan EstimatedExecutionTime { get; set; }
    public bool RequiresConfirmation { get; set; }
    public string[] RequiredPermissions { get; set; } = Array.Empty<string>();

    public override string ToString()
    {
        var paramStr = Parameters.Any() 
            ? $" ({string.Join(", ", Parameters.Select(kvp => $"{kvp.Key}={kvp.Value}"))})"
            : "";
        return $"{Name}{paramStr}";
    }
}

/// <summary>
/// Base abstract command that provides common functionality.
/// </summary>
public abstract class BaseCommand : ICommand
{
    public DateTime CreatedAt { get; }
    public abstract bool CanUndo { get; }

    protected BaseCommand()
    {
        CreatedAt = DateTime.Now;
    }

    public abstract void Execute();
    public abstract void Undo();
    public abstract CommandInfo GetCommandInfo();

    protected virtual void LogExecution(string action)
    {
        var info = GetCommandInfo();
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {action}: {info}");
    }
}

/// <summary>
/// Macro command that can execute multiple commands as a single unit.
/// This demonstrates the Composite pattern working with Command pattern.
/// </summary>
public class MacroCommand : BaseCommand
{
    private readonly List<ICommand> _commands;
    private readonly string _name;
    private readonly string _description;

    public override bool CanUndo => _commands.All(c => c.CanUndo);

    public MacroCommand(string name, string description)
    {
        _name = name;
        _description = description;
        _commands = new List<ICommand>();
    }

    public void AddCommand(ICommand command)
    {
        _commands.Add(command);
    }

    public void AddCommands(params ICommand[] commands)
    {
        _commands.AddRange(commands);
    }

    public override void Execute()
    {
        LogExecution("EXECUTING MACRO");
        
        foreach (var command in _commands)
        {
            try
            {
                command.Execute();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error executing command in macro: {ex.Message}");
                // In a real implementation, you might want to rollback previous commands
                throw;
            }
        }
        
        Console.WriteLine($"✅ Macro '{_name}' completed successfully");
    }

    public override void Undo()
    {
        if (!CanUndo)
        {
            throw new InvalidOperationException("Cannot undo macro - some commands are not undoable");
        }

        LogExecution("UNDOING MACRO");
        
        // Undo commands in reverse order
        for (int i = _commands.Count - 1; i >= 0; i--)
        {
            try
            {
                _commands[i].Undo();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error undoing command in macro: {ex.Message}");
                throw;
            }
        }
        
        Console.WriteLine($"↩️ Macro '{_name}' undone successfully");
    }

    public override CommandInfo GetCommandInfo()
    {
        return new CommandInfo
        {
            Name = _name,
            Description = _description,
            Category = "Macro",
            Parameters = new Dictionary<string, object>
            {
                ["command_count"] = _commands.Count,
                ["can_undo"] = CanUndo
            },
            EstimatedExecutionTime = TimeSpan.FromMilliseconds(_commands.Sum(c => c.GetCommandInfo().EstimatedExecutionTime.TotalMilliseconds))
        };
    }
}

/// <summary>
/// Command history manager that tracks executed commands and supports undo/redo.
/// </summary>
public class CommandHistory
{
    private readonly Stack<ICommand> _undoStack;
    private readonly Stack<ICommand> _redoStack;
    private readonly int _maxHistorySize;

    public int UndoCount => _undoStack.Count;
    public int RedoCount => _redoStack.Count;
    public bool CanUndo => _undoStack.Count > 0;
    public bool CanRedo => _redoStack.Count > 0;

    public CommandHistory(int maxHistorySize = 100)
    {
        _maxHistorySize = maxHistorySize;
        _undoStack = new Stack<ICommand>();
        _redoStack = new Stack<ICommand>();
    }

    public void ExecuteCommand(ICommand command)
    {
        try
        {
            command.Execute();
            
            // Add to undo stack if the command can be undone
            if (command.CanUndo)
            {
                _undoStack.Push(command);
                
                // Limit history size
                if (_undoStack.Count > _maxHistorySize)
                {
                    var commands = _undoStack.ToArray();
                    _undoStack.Clear();
                    for (int i = commands.Length - _maxHistorySize; i < commands.Length; i++)
                    {
                        _undoStack.Push(commands[i]);
                    }
                }
            }
            
            // Clear redo stack since we executed a new command
            _redoStack.Clear();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Command execution failed: {ex.Message}");
            throw;
        }
    }

    public void Undo()
    {
        if (!CanUndo)
        {
            throw new InvalidOperationException("No commands to undo");
        }

        var command = _undoStack.Pop();
        try
        {
            command.Undo();
            _redoStack.Push(command);
        }
        catch (Exception ex)
        {
            // If undo fails, put the command back on the undo stack
            _undoStack.Push(command);
            Console.WriteLine($"❌ Undo failed: {ex.Message}");
            throw;
        }
    }

    public void Redo()
    {
        if (!CanRedo)
        {
            throw new InvalidOperationException("No commands to redo");
        }

        var command = _redoStack.Pop();
        try
        {
            command.Execute();
            _undoStack.Push(command);
        }
        catch (Exception ex)
        {
            // If redo fails, put the command back on the redo stack
            _redoStack.Push(command);
            Console.WriteLine($"❌ Redo failed: {ex.Message}");
            throw;
        }
    }

    public List<CommandInfo> GetUndoHistory()
    {
        return _undoStack.Select(c => c.GetCommandInfo()).ToList();
    }

    public List<CommandInfo> GetRedoHistory()
    {
        return _redoStack.Select(c => c.GetCommandInfo()).ToList();
    }

    public void Clear()
    {
        _undoStack.Clear();
        _redoStack.Clear();
    }

    public CommandHistorySnapshot GetSnapshot()
    {
        return new CommandHistorySnapshot
        {
            UndoCommands = GetUndoHistory(),
            RedoCommands = GetRedoHistory(),
            Timestamp = DateTime.Now
        };
    }
}

/// <summary>
/// Snapshot of command history state.
/// </summary>
public class CommandHistorySnapshot
{
    public List<CommandInfo> UndoCommands { get; set; } = new();
    public List<CommandInfo> RedoCommands { get; set; } = new();
    public DateTime Timestamp { get; set; }

    public override string ToString()
    {
        return $"History Snapshot ({Timestamp:HH:mm:ss}): {UndoCommands.Count} undo, {RedoCommands.Count} redo";
    }
}
