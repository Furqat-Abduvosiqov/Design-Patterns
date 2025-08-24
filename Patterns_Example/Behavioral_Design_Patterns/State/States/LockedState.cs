using Patterns_Example.Behavioral_Design_Patterns.State.Abstractions;
using Patterns_Example.Behavioral_Design_Patterns.State.Contexts;

namespace Patterns_Example.Behavioral_Design_Patterns.State.States;

public class LockedState : IMediaPlayerState
{
    private readonly IMediaPlayerState _previousState;

    public LockedState(IMediaPlayerState previousState)
    {
        _previousState = previousState;
    }

    public async Task Play(MediaPlayer player)
    {
        Console.WriteLine("Player is locked! Unlock first.");
        await Task.CompletedTask;
    }

    public async Task Pause(MediaPlayer player)
    {
        Console.WriteLine("Player is locked! Unlock first.");
        await Task.CompletedTask;
    }

    public async Task Stop(MediaPlayer player)
    {
        Console.WriteLine("Player is locked! Unlock first.");
        await Task.CompletedTask;
    }

    public async Task Lock(MediaPlayer player)
    {
        Console.WriteLine("Unlocking player...");
        player.SetState(_previousState);
        await Task.CompletedTask;
    }

    public async Task Speed(MediaPlayer player, double speed)
    {
        Console.WriteLine("Player is locked! Unlock first.");
        await Task.CompletedTask;
    }

    public string GetCurrentState() => "Locked";
}