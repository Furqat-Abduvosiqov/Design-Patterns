namespace Patterns_Example.Behavioral_Design_Patterns.Iterator;

public static class IteratorExample
{
    public static void Demonstrate()
    {
        PlayList playlist = new();
        playlist.AddSong(new Song("Bohemian Rhapsody", "Queen"));
        playlist.AddSong(new Song("Imagine", "John Lennon"));
        playlist.AddSong(new Song("Hotel California", "Eagles"));

        IIterator<Song> iterator = playlist.CreateIterator();

        Console.WriteLine("🎵 Playlist:");
        
        while (iterator.HasNext())
        {
            Console.WriteLine(iterator.Next());
        }
    }
}