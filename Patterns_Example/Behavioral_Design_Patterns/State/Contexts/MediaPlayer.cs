using Patterns_Example.Behavioral_Design_Patterns.State.Abstractions;
using Patterns_Example.Behavioral_Design_Patterns.State.States;

namespace Patterns_Example.Behavioral_Design_Patterns.State.Contexts;

public class MediaPlayer
{
    private IMediaPlayerState _state;
    private DateTime _startTime;
    private TimeSpan _currentTime;
    private double _playbackSpeed;
    private bool _isPlaying;

    public MediaPlayer()
    {
        _state = new StoppedState();
        _playbackSpeed = 1.0;
        _isPlaying = false;
        ResetProgress();
    }

    public void SetState(IMediaPlayerState state)
    {
        UpdateCurrentTime();
        _state = state ?? throw new ArgumentNullException(nameof(state));
    }

    public async Task Play()
    {
        await _state.Play(this);
        _isPlaying = true;
        _startTime = DateTime.Now;
    }

    public async Task Pause()
    {
        UpdateCurrentTime();
        await _state.Pause(this);
        _isPlaying = false;
    }

    public async Task Stop()
    {
        UpdateCurrentTime();
        await _state.Stop(this);
        _isPlaying = false;
        ResetProgress();
    }

    public async Task Lock()
    {
        UpdateCurrentTime();
        await _state.Lock(this);
        _isPlaying = false;
    }

    public async Task Speed(double speed)
    {
        if (speed <= 0 || speed > 4.0)
            throw new ArgumentOutOfRangeException(nameof(speed), "Playback speed must be between 0.1 and 4.0");

        await _state.Speed(this, speed);
        UpdateCurrentTime();
        SetPlaybackSpeed(speed);
    }

    public void ResetProgress()
    {
        _startTime = DateTime.Now;
        _currentTime = TimeSpan.Zero;
    }

    private void UpdateCurrentTime()
    {
        if (_isPlaying)
        {
            var elapsed = DateTime.Now - _startTime;
            _currentTime += TimeSpan.FromTicks((long)(elapsed.Ticks * _playbackSpeed));
            _startTime = DateTime.Now;
        }
    }

    public void SetPlaybackSpeed(double speed)
    {
        _playbackSpeed = speed;
    }

    public string GetPlayerStatus()
    {
        UpdateCurrentTime();
        return $"Player Status: {_state.GetCurrentState()}\n" +
               $"Current Time: {_currentTime:hh\\:mm\\:ss}\n" +
               $"Playback Speed: {_playbackSpeed:F1}x";
    }
}