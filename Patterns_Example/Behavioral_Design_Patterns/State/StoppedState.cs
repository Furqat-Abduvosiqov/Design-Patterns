namespace Patterns_Example.Behavioral_Design_Patterns.State;

public class StoppedState : IMediaPlayerState
{
    public async Task Play(MediaPlayer player)
    {
        Console.WriteLine("Starting playback from beginning...");
        player.SetState(new PlayingState());
    }

    public async Task Pause(MediaPlayer player)
    {
        Console.WriteLine("Cannot pause when stopped");
    }

    public async Task Stop(MediaPlayer player)
    {
        Console.WriteLine("Already stopped");
    }

    public async Task Lock(MediaPlayer player)
    {
        Console.WriteLine("Locking player controls while stopped...");
        player.SetState(new LockedState(this));
    }

    public async Task Speed(MediaPlayer player, double speed)
    {
        Console.WriteLine("Cannot change speed while stopped");
    }

    public string GetCurrentState() => "Stopped";
}