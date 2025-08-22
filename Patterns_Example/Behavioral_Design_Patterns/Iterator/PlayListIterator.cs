namespace Patterns_Example.Behavioral_Design_Patterns.Iterator;

public class PlayListIterator : IIterator<Song>
{
    private readonly PlayList _playlist;
    private int _current = 0;
    
    public PlayListIterator(PlayList playList)
    {
        _playlist = playList;
    }

    public bool HasNext()
    {
        return _current < _playlist.Count;
    }

    public Song Next()
    {
        return _playlist[_current++];
    }
}