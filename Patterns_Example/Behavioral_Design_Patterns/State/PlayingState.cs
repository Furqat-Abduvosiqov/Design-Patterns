namespace Patterns_Example.Behavioral_Design_Patterns.State;

public class PlayingState : IMediaPlayerState
{
    public async Task Play(MediaPlayer player)
    {
        Console.WriteLine("Already playing...");
    }

    public async Task Pause(MediaPlayer player)
    {
        Console.WriteLine("Pausing playback...");
        await player.SetCurrentTime();
        player.SetState(new PausedState());
    }

    public async Task Stop(MediaPlayer player)
    {
        Console.WriteLine("Stopping playback...");
        player.ResetProgress();
        player.SetState(new StoppedState());
    }

    public async Task Lock(MediaPlayer player)
    {
        Console.WriteLine("Locking player controls while playing...");
        player.SetState(new LockedState(this));
    }

    public async Task Speed(MediaPlayer player, double speed)
    {
        player.SetPlaybackSpeed(speed);
        Console.WriteLine($"Changed playback speed to {speed}x");
    }

    public string GetCurrentState() => "Playing";
}