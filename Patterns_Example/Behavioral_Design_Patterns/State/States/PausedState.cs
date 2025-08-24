using Patterns_Example.Behavioral_Design_Patterns.State.Abstractions;
using Patterns_Example.Behavioral_Design_Patterns.State.Contexts;

namespace Patterns_Example.Behavioral_Design_Patterns.State.States;

public class PausedState : IMediaPlayerState
{
    public async Task Play(MediaPlayer player)
    {
        Console.WriteLine("Resuming playback...");
        player.SetState(new PlayingState());
        await Task.CompletedTask;
    }

    public async Task Pause(MediaPlayer player)
    {
        Console.WriteLine("Already paused...");
        await Task.CompletedTask;
    }

    public async Task Stop(MediaPlayer player)
    {
        Console.WriteLine("Stopping playback...");
        player.SetState(new StoppedState());
        await Task.CompletedTask;
    }

    public async Task Lock(MediaPlayer player)
    {
        Console.WriteLine("Locking player controls while paused...");
        player.SetState(new LockedState(this));
        await Task.CompletedTask;
    }

    public async Task Speed(MediaPlayer player, double speed)
    {
        Console.WriteLine("Cannot change speed while paused.");
        await Task.CompletedTask;
    }

    public string GetCurrentState() => "Paused";
}