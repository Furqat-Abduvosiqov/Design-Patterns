
# Iterator Pattern in C#

## Overview

The **Iterator Pattern** is a behavioral design pattern that allows sequential access to elements of a collection **without exposing its underlying representation**.

In C#, this pattern is essentially what `IEnumerable<T>` and `IEnumerator<T>` provide under the hood.

---

## When to Use
- When you want to **traverse a collection** without exposing its internal structure.
- When you need multiple ways of traversing (normal order, reverse order, filtering, shuffling).
- When you want to follow **Single Responsibility Principle** (collection holds data, iterator controls traversal).

---

## Structure

1. **Iterator Interface** — defines methods for traversing (e.g., `HasNext()`, `Next()`).
2. **Concrete Iterator** — implements the iterator logic for a specific collection.
3. **Aggregate Interface** — defines method for creating an iterator.
4. **Concrete Aggregate** — collection class that implements iterator creation.
5. **Client** — uses the iterator to traverse the collection.

---

## Real-World Example — Music Playlist

### Interfaces

```csharp
public interface IIterator<T>
{
    bool HasNext();
    T Next();
}

public interface IAggregate<T>
{
    IIterator<T> CreateIterator();
}
```

### Domain Model

```csharp
public class Song
{
    public string Title { get; }
    public string Artist { get; }

    public Song(string title, string artist)
    {
        Title = title;
        Artist = artist;
    }

    public override string ToString() => $"{Title} by {Artist}";
}
```

### Concrete Collection (Playlist)

```csharp
public class Playlist : IAggregate<Song>
{
    private readonly List<Song> _songs = new();

    public void AddSong(Song song) => _songs.Add(song);

    public IIterator<Song> CreateIterator()
    {
        return new PlaylistIterator(this);
    }

    public int Count => _songs.Count;
    public Song this[int index] => _songs[index];
}
```

### Concrete Iterator (PlaylistIterator)

```csharp
public class PlaylistIterator : IIterator<Song>
{
    private readonly Playlist _playlist;
    private int _current = 0;

    public PlaylistIterator(Playlist playlist)
    {
        _playlist = playlist;
    }

    public bool HasNext() => _current < _playlist.Count;

    public Song Next() => _playlist[_current++];
}
```

### Client Code

```csharp
class Program
{
    static void Main()
    {
        Playlist playlist = new();
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
```

### Output

```
🎵 Playlist:
Bohemian Rhapsody by Queen
Imagine by John Lennon
Hotel California by Eagles
```

---

## Advantages
- Encapsulates iteration logic.
- Supports multiple traversal strategies.
- Promotes **Single Responsibility Principle**.

## Disadvantages
- Extra classes required (more boilerplate).
- Slight overhead compared to direct iteration.

---

## Iterator in .NET
In practice, C# already provides iterator functionality via `IEnumerable<T>` and `IEnumerator<T>`.

Example:

```csharp
foreach (var song in playlist)
{
    Console.WriteLine(song);
}
```

Behind the scenes, the compiler generates an **iterator state machine** very similar to the custom implementation above.

---

## Extended Example — Shuffle Iterator (Optional)

You can create a **ShuffleIterator** to play songs in random order without changing the original `Playlist` class.

---

# ✅ Summary
The **Iterator Pattern** decouples data storage from traversal, allowing flexible iteration strategies.  
In C#, this is heavily used in LINQ, `foreach`, and all collection types.
