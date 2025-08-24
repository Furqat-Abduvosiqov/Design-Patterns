using Patterns_Example.Behavioral_Design_Patterns.State.Abstractions;
using Patterns_Example.Behavioral_Design_Patterns.State.Contexts;

namespace Patterns_Example.Behavioral_Design_Patterns.State.States;

public class PlayingState : IMediaPlayerState
{
    public async Task Play(MediaPlayer player)
    {
        await Console.Out.WriteLineAsync("Already playing...");
    }

    public async Task Pause(MediaPlayer player)
    {
        await Console.Out.WriteLineAsync("Pausing playback...");
        player.SetState(new PausedState());
    }

    public async Task Stop(MediaPlayer player)
    {
        await Console.Out.WriteLineAsync("Stopping playback...");
        player.SetState(new StoppedState());
    }

    public async Task Lock(MediaPlayer player)
    {
        await Console.Out.WriteLineAsync("Locking player controls while playing...");
        player.SetState(new LockedState(this));
    }

    public async Task Speed(MediaPlayer player, double speed)
    {
        await Console.Out.WriteLineAsync($"Changing playback speed to {speed:F1}x");
    }

    public string GetCurrentState() => "Playing";
}