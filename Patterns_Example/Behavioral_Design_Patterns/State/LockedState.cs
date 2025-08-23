namespace Patterns_Example.Behavioral_Design_Patterns.State;

public class LockedState : IMediaPlayerState
{
    private readonly IMediaPlayerState _previousState;

    public LockedState(IMediaPlayerState state)
    {
        _previousState = state;
    }

    public async Task Play(MediaPlayer player)
    {
        Console.WriteLine("Player is locked. Unlock first.");
    }

    public async Task Pause(MediaPlayer player)
    {
        Console.WriteLine("Player is locked. Unlock first.");
    }

    public async Task Stop(MediaPlayer player)
    {
        Console.WriteLine("Player is locked. Unlock first.");
    }

    public async Task Lock(MediaPlayer player)
    {
        Console.WriteLine("Unlocking player controls...");
        player.SetState(_previousState);
    }

    public async Task Speed(MediaPlayer player, double speed)
    {
        Console.WriteLine("Cannot change speed while locked");
    }

    public string GetCurrentState() => $"Locked ({_previousState.GetCurrentState()})";
}