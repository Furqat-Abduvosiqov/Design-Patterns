namespace Patterns_Example.Behavioral_Design_Patterns.State;

public class MediaPlayer
{
    private IMediaPlayerState _state;
    private DateTime _startTime;
    private TimeSpan _currentTime;
    private double _playbackSpeed;

    public MediaPlayer()
    {
        _state = new StoppedState();
        _playbackSpeed = 1.0;
        ResetProgress();
    }

    public void SetState(IMediaPlayerState state)
    {
        _state = state;
    }

    public async Task Play() => await _state.Play(this);
    public async Task Pause() => await _state.Pause(this);
    public async Task Stop() => await _state.Stop(this);
    public async Task Lock() => await _state.Lock(this);
    public async Task Speed(double speed) => await _state.Speed(this, speed);

    public void ResetProgress()
    {
        _startTime = DateTime.Now;
        _currentTime = TimeSpan.Zero;
    }

    public async Task SetCurrentTime()
    {
        _currentTime += DateTime.Now - _startTime;
        _startTime = DateTime.Now;
    }

    public void SetPlaybackSpeed(double speed)
    {
        _playbackSpeed = speed;
    }

    public string GetPlayerStatus()
    {
        return $"Player Status: {_state.GetCurrentState()}\n" +
               $"Current Time: {_currentTime:hh\\:mm\\:ss}\n" +
               $"Playback Speed: {_playbackSpeed}x";
    }
}