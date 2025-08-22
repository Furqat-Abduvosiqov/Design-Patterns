namespace Patterns_Example.Behavioral_Design_Patterns.Iterator;

public class PlayList : IAggregate<Song>
{
    private readonly List<Song> _songs = new List<Song>();
    
    public void AddSong(Song song) => _songs.Add(song);
    
    public IIterator<Song> CreateIterator()
    {
        return new PlayListIterator(this);
    }
    
    // access for iterator only.
    public int Count => _songs.Count;
    
    public Song this[int index] => _songs[index];
}