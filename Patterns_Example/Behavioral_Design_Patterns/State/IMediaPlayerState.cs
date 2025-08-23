namespace Patterns_Example.Behavioral_Design_Patterns.State;

public interface IMediaPlayerState
{
    Task Play(MediaPlayer player);
    Task Pause(MediaPlayer player);
    Task Stop(MediaPlayer player);
    Task Lock(MediaPlayer player);
    Task Speed(MediaPlayer player, double speed);
    string GetCurrentState();
}