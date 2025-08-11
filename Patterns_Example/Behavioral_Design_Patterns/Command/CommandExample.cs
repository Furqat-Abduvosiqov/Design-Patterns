namespace Patterns_Example.Behavioral_Design_Patterns.Command;

/// <summary>
/// Example class demonstrating the Command pattern usage
/// </summary>
public static class CommandExample
{
    public static void RunExample()
    {
        Console.WriteLine("=== Command Pattern Example: Smart Home Automation ===\n");
        
        // Demonstrate the problem without command pattern
        DemonstrateProblemsWithoutCommand();
        
        // Demonstrate basic command usage
        DemonstrateBasicCommandUsage();
        
        // Demonstrate undo/redo functionality
        DemonstrateUndoRedo();
        
        // Demonstrate macro commands
        DemonstrateMacroCommands();
        
        // Demonstrate command queuing and scheduling
        DemonstrateCommandQueuing();
        
        // Demonstrate command logging and history
        DemonstrateCommandHistory();
        
        Console.WriteLine("=== Command Pattern Benefits Demonstrated ===");
        Console.WriteLine("✓ Decoupled invoker from receiver");
        Console.WriteLine("✓ Commands as first-class objects");
        Console.WriteLine("✓ Undo/Redo functionality");
        Console.WriteLine("✓ Macro commands (composite operations)");
        Console.WriteLine("✓ Command queuing and scheduling");
        Console.WriteLine("✓ Command logging and auditing");
        Console.WriteLine("✓ Parameterizable objects with operations");
    }

    private static void DemonstrateProblemsWithoutCommand()
    {
        Console.WriteLine("1. Problems WITHOUT Command Pattern:");
        Console.WriteLine(new string('=', 50));
        Console.WriteLine("❌ Tight coupling between invoker and receiver:");
        Console.WriteLine();
        
        Console.WriteLine("// Without command pattern - tight coupling");
        Console.WriteLine("public class RemoteControl");
        Console.WriteLine("{");
        Console.WriteLine("    private Light _light;");
        Console.WriteLine("    private Fan _fan;");
        Console.WriteLine("    ");
        Console.WriteLine("    public void PressButton1() => _light.TurnOn();");
        Console.WriteLine("    public void PressButton2() => _light.TurnOff();");
        Console.WriteLine("    public void PressButton3() => _fan.TurnOn();");
        Console.WriteLine("    // Hard-coded operations, no flexibility");
        Console.WriteLine("}");
        Console.WriteLine();
        Console.WriteLine("❌ Problems:");
        Console.WriteLine("   • Tight coupling between remote and devices");
        Console.WriteLine("   • No undo functionality");
        Console.WriteLine("   • Cannot queue or schedule operations");
        Console.WriteLine("   • No logging or auditing");
        Console.WriteLine("   • Hard to add new operations");
        Console.WriteLine("   • Cannot create macro operations");
        Console.WriteLine();
    }

    private static void DemonstrateBasicCommandUsage()
    {
        Console.WriteLine("2. Basic Command Usage:");
        Console.WriteLine(new string('=', 50));
        
        // Create smart home devices (receivers)
        var livingRoomLight = new SmartHomeDevice("Living Room Light", "Light", "Living Room");
        var bedroomFan = new SmartHomeDevice("Bedroom Fan", "Fan", "Bedroom");
        var kitchenLight = new SmartHomeDevice("Kitchen Light", "Light", "Kitchen");
        
        // Create commands
        var turnOnLivingRoomLight = new TurnOnCommand(livingRoomLight);
        var turnOffLivingRoomLight = new TurnOffCommand(livingRoomLight);
        var turnOnBedroomFan = new TurnOnCommand(bedroomFan);
        var setBrightness = new SetPropertyCommand(kitchenLight, "brightness", 75);
        
        // Create remote control (invoker)
        var remote = new SmartHomeRemote();
        
        // Assign commands to remote buttons
        remote.SetCommand("1", turnOnLivingRoomLight);
        remote.SetCommand("2", turnOffLivingRoomLight);
        remote.SetCommand("3", turnOnBedroomFan);
        remote.SetCommand("4", setBrightness);
        
        Console.WriteLine("\n✅ Command Pattern Benefits:");
        Console.WriteLine("   • Decoupled remote from specific devices");
        Console.WriteLine("   • Commands are objects that can be stored, passed around");
        Console.WriteLine("   • Easy to add new commands without changing remote");
        Console.WriteLine();
        
        // Show available commands
        remote.ShowCommands();
        Console.WriteLine();
        
        // Execute commands
        Console.WriteLine("Executing commands:");
        remote.PressButton("1");
        remote.PressButton("4");
        remote.PressButton("3");
        Console.WriteLine();
    }

    private static void DemonstrateUndoRedo()
    {
        Console.WriteLine("3. Undo/Redo Functionality:");
        Console.WriteLine(new string('=', 50));
        
        var device = new SmartHomeDevice("Smart Thermostat", "Thermostat", "Hallway");
        var remote = new SmartHomeRemote();
        
        // Create various commands
        var turnOn = new TurnOnCommand(device);
        var setTemp1 = new SetPropertyCommand(device, "temperature", 72);
        var setTemp2 = new SetPropertyCommand(device, "temperature", 75);
        var setMode = new SetPropertyCommand(device, "mode", "cooling");
        
        remote.SetCommand("on", turnOn);
        remote.SetCommand("temp1", setTemp1);
        remote.SetCommand("temp2", setTemp2);
        remote.SetCommand("mode", setMode);
        
        Console.WriteLine("Executing a sequence of commands:");
        remote.PressButton("on");
        remote.PressButton("temp1");
        remote.PressButton("temp2");
        remote.PressButton("mode");
        
        Console.WriteLine($"\nDevice state: {device}");
        Console.WriteLine();
        
        Console.WriteLine("Demonstrating undo functionality:");
        remote.Undo(); // Undo set mode
        remote.Undo(); // Undo set temp to 75
        Console.WriteLine($"Device state after 2 undos: {device}");
        Console.WriteLine();
        
        Console.WriteLine("Demonstrating redo functionality:");
        remote.Redo(); // Redo set temp to 75
        Console.WriteLine($"Device state after 1 redo: {device}");
        Console.WriteLine();
        
        remote.ShowHistory();
        Console.WriteLine();
    }

    private static void DemonstrateMacroCommands()
    {
        Console.WriteLine("4. Macro Commands (Composite Operations):");
        Console.WriteLine(new string('=', 50));
        
        // Create devices
        var livingRoomLight = new SmartHomeDevice("Living Room Light", "Light", "Living Room");
        var tv = new SmartHomeDevice("Smart TV", "TV", "Living Room");
        var soundSystem = new SmartHomeDevice("Sound System", "Audio", "Living Room");
        var curtains = new SmartHomeDevice("Smart Curtains", "Curtains", "Living Room");
        
        // Create individual commands
        var dimLights = new SetPropertyCommand(livingRoomLight, "brightness", 30);
        var turnOnTV = new TurnOnCommand(tv);
        var setTVChannel = new SetPropertyCommand(tv, "channel", "Netflix");
        var turnOnSound = new TurnOnCommand(soundSystem);
        var setSoundVolume = new SetPropertyCommand(soundSystem, "volume", 40);
        var closeCurtains = new SetPropertyCommand(curtains, "position", "closed");
        
        // Create macro command for "Movie Night"
        var movieNightMacro = new MacroCommand("Movie Night", "Perfect setup for watching movies");
        movieNightMacro.AddCommands(
            dimLights,
            new DelayCommand(TimeSpan.FromMilliseconds(200), "Let lights adjust"),
            turnOnTV,
            setTVChannel,
            turnOnSound,
            setSoundVolume,
            closeCurtains
        );
        
        // Create macro command for "Good Morning"
        var goodMorningMacro = new MacroCommand("Good Morning", "Start the day right");
        goodMorningMacro.AddCommands(
            new TurnOnCommand(livingRoomLight),
            new SetPropertyCommand(livingRoomLight, "brightness", 100),
            new SetPropertyCommand(curtains, "position", "open"),
            new TurnOnCommand(soundSystem),
            new SetPropertyCommand(soundSystem, "playlist", "Morning Jazz")
        );
        
        var remote = new SmartHomeRemote();
        remote.SetCommand("movie", movieNightMacro);
        remote.SetCommand("morning", goodMorningMacro);
        
        Console.WriteLine("Executing 'Movie Night' macro:");
        remote.PressButton("movie");
        Console.WriteLine();
        
        Console.WriteLine("Device states after movie night setup:");
        Console.WriteLine($"  {livingRoomLight}");
        Console.WriteLine($"  {tv}");
        Console.WriteLine($"  {soundSystem}");
        Console.WriteLine($"  {curtains}");
        Console.WriteLine();
        
        Console.WriteLine("Undoing movie night macro:");
        remote.Undo();
        Console.WriteLine();
        
        Console.WriteLine("Executing 'Good Morning' macro:");
        remote.PressButton("morning");
        Console.WriteLine();
    }

    private static void DemonstrateCommandQueuing()
    {
        Console.WriteLine("5. Command Queuing and Scheduling:");
        Console.WriteLine(new string('=', 50));
        
        var device = new SmartHomeDevice("Smart Coffee Maker", "Appliance", "Kitchen");
        
        // Create a queue of commands to execute
        var commandQueue = new Queue<ICommand>();
        
        commandQueue.Enqueue(new TurnOnCommand(device));
        commandQueue.Enqueue(new DelayCommand(TimeSpan.FromMilliseconds(300), "Warming up"));
        commandQueue.Enqueue(new SetPropertyCommand(device, "brew_strength", "strong"));
        commandQueue.Enqueue(new SetPropertyCommand(device, "cup_size", "large"));
        commandQueue.Enqueue(new DelayCommand(TimeSpan.FromMilliseconds(500), "Brewing coffee"));
        commandQueue.Enqueue(new SetPropertyCommand(device, "status", "ready"));
        
        Console.WriteLine("Executing queued commands for automated coffee brewing:");
        var history = new CommandHistory();
        
        while (commandQueue.Count > 0)
        {
            var command = commandQueue.Dequeue();
            history.ExecuteCommand(command);
        }
        
        Console.WriteLine($"\nFinal device state: {device}");
        Console.WriteLine();
        
        // Show command execution history
        Console.WriteLine("Command execution history:");
        var executedCommands = history.GetUndoHistory();
        for (int i = 0; i < executedCommands.Count; i++)
        {
            Console.WriteLine($"  {i + 1}. {executedCommands[i]}");
        }
        Console.WriteLine();
    }

    private static void DemonstrateCommandHistory()
    {
        Console.WriteLine("6. Command Logging and History:");
        Console.WriteLine(new string('=', 50));
        
        var securitySystem = new SmartHomeDevice("Security System", "Security", "Main Panel");
        var remote = new SmartHomeRemote();
        
        // Create security-related commands
        var armSystem = new SetPropertyCommand(securitySystem, "mode", "armed");
        var disarmSystem = new SetPropertyCommand(securitySystem, "mode", "disarmed");
        var setAlarmCode = new SetPropertyCommand(securitySystem, "alarm_code", "1234");
        var enableMotionDetection = new SetPropertyCommand(securitySystem, "motion_detection", true);
        
        remote.SetCommand("arm", armSystem);
        remote.SetCommand("disarm", disarmSystem);
        remote.SetCommand("code", setAlarmCode);
        remote.SetCommand("motion", enableMotionDetection);
        
        Console.WriteLine("Executing security system commands:");
        remote.PressButton("code");
        remote.PressButton("motion");
        remote.PressButton("arm");
        
        // Simulate some time passing
        Thread.Sleep(100);
        
        remote.PressButton("disarm");
        Console.WriteLine();
        
        // Show detailed history
        Console.WriteLine("Detailed command history:");
        remote.ShowHistory();
        Console.WriteLine();
        
        // Show device action history
        Console.WriteLine("Device action history:");
        foreach (var action in securitySystem.ActionHistory)
        {
            Console.WriteLine($"  {action}");
        }
        Console.WriteLine();
        
        // Demonstrate history snapshot
        var snapshot = remote.GetHistory().GetSnapshot();
        Console.WriteLine($"History snapshot: {snapshot}");
        Console.WriteLine();
    }
}
