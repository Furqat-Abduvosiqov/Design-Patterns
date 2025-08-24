using Patterns_Example.Behavioral_Design_Patterns.State.Abstractions;
using Patterns_Example.Behavioral_Design_Patterns.State.Contexts;

namespace Patterns_Example.Behavioral_Design_Patterns.State.States;

public class StoppedState : IMediaPlayerState
{
    public async Task Play(MediaPlayer player)
    {
        Console.WriteLine("Starting playback from the beginning...");
        player.SetState(new PlayingState());
        await Task.CompletedTask;
    }

    public async Task Pause(MediaPlayer player)
    {
        Console.WriteLine("Cannot pause - player is stopped");
        await Task.CompletedTask;
    }

    public async Task Stop(MediaPlayer player)
    {
        Console.WriteLine("Already stopped");
        await Task.CompletedTask;
    }

    public async Task Lock(MediaPlayer player)
    {
        Console.WriteLine("Locking player controls while stopped...");
        player.SetState(new LockedState(this));
        await Task.CompletedTask;
    }

    public async Task Speed(MediaPlayer player, double speed)
    {
        Console.WriteLine("Cannot change speed while stopped");
        await Task.CompletedTask;
    }

    public string GetCurrentState() => "Stopped";
}