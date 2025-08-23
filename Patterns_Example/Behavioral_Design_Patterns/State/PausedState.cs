namespace Patterns_Example.Behavioral_Design_Patterns.State;

public class PausedState : IMediaPlayerState
{
    public async Task Play(MediaPlayer player)
    {
        Console.WriteLine("Resuming playback...");
        player.SetState(new PlayingState());
    }

    public async Task Pause(MediaPlayer player)
    {
        Console.WriteLine("Already paused...");
    }

    public async Task Stop(MediaPlayer player)
    {
        Console.WriteLine("Stopping playback...");
        player.ResetProgress();
        player.SetState(new StoppedState());
    }

    public async Task Lock(MediaPlayer player)
    {
        Console.WriteLine("Locking player controls while paused...");
        player.SetState(new LockedState(this));
    }

    public async Task Speed(MediaPlayer player, double speed)
    {
        Console.WriteLine("Cannot change speed while paused");
    }

    public string GetCurrentState() => "Paused";
}