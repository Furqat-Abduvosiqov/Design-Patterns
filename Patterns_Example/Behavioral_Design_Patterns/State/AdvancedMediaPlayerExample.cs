using Patterns_Example.Behavioral_Design_Patterns.State.Contexts;

namespace Patterns_Example.Behavioral_Design_Patterns.State;

public class AdvancedMediaPlayerExample
{
    public static async Task RunExample()
    {
        Console.WriteLine("Advanced Media Player Example - State Pattern");
        Console.WriteLine("--------------------------------------------");

        var player = new MediaPlayer();

        // Initial state
        Console.WriteLine("\nInitial state:");
        Console.WriteLine(player.GetPlayerStatus());

        // Start playing
        Console.WriteLine("\nStarting playback:");
        await player.Play();
        Console.WriteLine(player.GetPlayerStatus());

        // Simulate some time passing
        await Task.Delay(2000);

        // Change speed
        Console.WriteLine("\nChanging speed to 2x:");
        await player.Speed(2.0);
        Console.WriteLine(player.GetPlayerStatus());

        // Wait a bit more
        await Task.Delay(2000);

        // Pause
        Console.WriteLine("\nPausing playback:");
        await player.Pause();
        Console.WriteLine(player.GetPlayerStatus());

        // Try to change speed while paused
        Console.WriteLine("\nTrying to change speed while paused:");
        await player.Speed(1.5);
        Console.WriteLine(player.GetPlayerStatus());

        // Lock the player
        Console.WriteLine("\nLocking the player:");
        await player.Lock();
        Console.WriteLine(player.GetPlayerStatus());

        // Try to play while locked
        Console.WriteLine("\nTrying to play while locked:");
        await player.Play();
        Console.WriteLine(player.GetPlayerStatus());

        // Unlock
        Console.WriteLine("\nUnlocking the player:");
        await player.Lock();
        Console.WriteLine(player.GetPlayerStatus());

        // Stop
        Console.WriteLine("\nStopping playback:");
        await player.Stop();
        Console.WriteLine(player.GetPlayerStatus());
    }
}