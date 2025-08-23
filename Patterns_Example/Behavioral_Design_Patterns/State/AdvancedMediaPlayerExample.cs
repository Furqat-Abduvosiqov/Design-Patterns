namespace Patterns_Example.Behavioral_Design_Patterns.State;

public static class AdvancedMediaPlayerExample
{
    public static async Task Demonstrate()
    {
        var player = new MediaPlayer();
            
        Console.WriteLine("Initial state:");
        Console.WriteLine(player.GetPlayerStatus());
        Console.WriteLine();

        // Demonstrate various state transitions
        await player.Play();
        await Task.Delay(2000); // Simulate some playback time
        Console.WriteLine(player.GetPlayerStatus());
        Console.WriteLine();

        await player.Speed(1.5);
        await Task.Delay(1000);
        Console.WriteLine(player.GetPlayerStatus());
        Console.WriteLine();

        await player.Lock();
        await player.Play();  // Should show locked message
        Console.WriteLine(player.GetPlayerStatus());
        Console.WriteLine();

        await player.Lock();  // Unlock
        Console.WriteLine(player.GetPlayerStatus());
        Console.WriteLine();

        await player.Pause();
        await Task.Delay(1000);
        Console.WriteLine(player.GetPlayerStatus());
        Console.WriteLine();

        await player.Stop();
        Console.WriteLine(player.GetPlayerStatus());
    }
}