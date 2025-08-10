using System.Text;

namespace Patterns_Example.Structural_Design_Patterns.Flyweight;

/// <summary>
/// Context class that uses flyweights to represent a text document.
/// This class manages the extrinsic state (position, formatting) while
/// delegating character rendering to flyweights that contain intrinsic state.
/// </summary>
public class TextDocument
{
    private readonly List<DocumentCharacter> _characters;
    private readonly CharacterFlyweightFactory _flyweightFactory;
    private readonly DocumentSettings _settings;
    private int _currentLine;
    private int _currentColumn;

    public string Title { get; set; }
    public int CharacterCount => _characters.Count;
    public int LineCount => _currentLine + 1;
    public DocumentSettings Settings => _settings;

    public TextDocument(string title = "Untitled Document")
    {
        Title = title;
        _characters = new List<DocumentCharacter>();
        _flyweightFactory = CharacterFlyweightFactory.Instance;
        _settings = new DocumentSettings();
        _currentLine = 0;
        _currentColumn = 0;
    }

    /// <summary>
    /// Adds text to the document, creating flyweights for each character.
    /// </summary>
    public void AddText(string text, TextFormatting? formatting = null)
    {
        if (string.IsNullOrEmpty(text))
            return;

        formatting ??= new TextFormatting();

        foreach (char character in text)
        {
            AddCharacter(character, formatting);
        }
    }

    /// <summary>
    /// Adds a single character to the document.
    /// </summary>
    public void AddCharacter(char character, TextFormatting? formatting = null)
    {
        formatting ??= new TextFormatting();

        // Get or create flyweight for this character
        var flyweight = _flyweightFactory.GetFlyweight(character, formatting.FontFamily);

        // Calculate position
        var position = CalculatePosition(character, formatting);

        // Create document character with extrinsic state
        var documentChar = new DocumentCharacter
        {
            Flyweight = flyweight,
            Context = new CharacterContext(position.X, position.Y)
            {
                ForegroundColor = formatting.ForegroundColor,
                BackgroundColor = formatting.BackgroundColor,
                FontSize = formatting.FontSize,
                FontStyle = formatting.FontStyle,
                LineNumber = _currentLine,
                ColumnNumber = _currentColumn
            }
        };

        _characters.Add(documentChar);

        // Update position for next character
        UpdatePosition(character, formatting);
    }

    /// <summary>
    /// Renders the entire document to the console.
    /// </summary>
    public void Render()
    {
        Console.Clear();
        Console.WriteLine($"Document: {Title}");
        Console.WriteLine(new string('=', Math.Min(Title.Length + 10, Console.WindowWidth - 1)));
        Console.WriteLine();

        foreach (var documentChar in _characters)
        {
            documentChar.Flyweight.Render(documentChar.Context);
        }

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine($"Characters: {CharacterCount:N0} | Lines: {LineCount} | Flyweights: {_flyweightFactory.TotalFlyweights}");
    }

    /// <summary>
    /// Renders a specific range of characters.
    /// </summary>
    public void RenderRange(int startIndex, int endIndex)
    {
        if (startIndex < 0 || endIndex >= _characters.Count || startIndex > endIndex)
            return;

        for (int i = startIndex; i <= endIndex; i++)
        {
            var documentChar = _characters[i];
            documentChar.Flyweight.Render(documentChar.Context);
        }
    }

    /// <summary>
    /// Applies formatting to a range of characters.
    /// </summary>
    public void ApplyFormatting(int startIndex, int endIndex, TextFormatting formatting)
    {
        if (startIndex < 0 || endIndex >= _characters.Count || startIndex > endIndex)
            return;

        for (int i = startIndex; i <= endIndex; i++)
        {
            var context = _characters[i].Context;
            context.ForegroundColor = formatting.ForegroundColor;
            context.BackgroundColor = formatting.BackgroundColor;
            context.FontSize = formatting.FontSize;
            context.FontStyle = formatting.FontStyle;
        }
    }

    /// <summary>
    /// Selects a range of characters.
    /// </summary>
    public void SelectRange(int startIndex, int endIndex)
    {
        // Clear previous selection
        foreach (var documentChar in _characters)
        {
            documentChar.Context.IsSelected = false;
        }

        // Apply new selection
        if (startIndex >= 0 && endIndex < _characters.Count && startIndex <= endIndex)
        {
            for (int i = startIndex; i <= endIndex; i++)
            {
                _characters[i].Context.IsSelected = true;
            }
        }
    }

    /// <summary>
    /// Highlights a range of characters.
    /// </summary>
    public void HighlightRange(int startIndex, int endIndex)
    {
        if (startIndex < 0 || endIndex >= _characters.Count || startIndex > endIndex)
            return;

        for (int i = startIndex; i <= endIndex; i++)
        {
            _characters[i].Context.IsHighlighted = true;
        }
    }

    /// <summary>
    /// Finds all occurrences of a specific character or string.
    /// </summary>
    public List<int> FindText(string searchText, bool caseSensitive = false)
    {
        var results = new List<int>();
        var documentText = GetText();
        
        var comparison = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
        int index = 0;
        
        while ((index = documentText.IndexOf(searchText, index, comparison)) != -1)
        {
            results.Add(index);
            index++;
        }
        
        return results;
    }

    /// <summary>
    /// Gets the text content of the document.
    /// </summary>
    public string GetText()
    {
        var text = new StringBuilder(_characters.Count);
        
        foreach (var documentChar in _characters)
        {
            text.Append(documentChar.Flyweight.GetIntrinsicState().Character);
        }
        
        return text.ToString();
    }

    /// <summary>
    /// Gets statistics about the document and flyweight usage.
    /// </summary>
    public DocumentStatistics GetStatistics()
    {
        var flyweightStats = _flyweightFactory.GetStatistics();
        var characterCounts = new Dictionary<char, int>();
        var typeCounts = new Dictionary<CharacterType, int>();
        
        foreach (var documentChar in _characters)
        {
            var intrinsicState = documentChar.Flyweight.GetIntrinsicState();
            var character = intrinsicState.Character;
            var type = intrinsicState.Type;
            
            characterCounts[character] = characterCounts.GetValueOrDefault(character, 0) + 1;
            typeCounts[type] = typeCounts.GetValueOrDefault(type, 0) + 1;
        }

        return new DocumentStatistics
        {
            Title = Title,
            TotalCharacters = CharacterCount,
            TotalLines = LineCount,
            UniqueCharacters = characterCounts.Count,
            CharacterFrequency = characterCounts,
            CharacterTypeDistribution = typeCounts,
            FlyweightStatistics = flyweightStats,
            EstimatedMemoryUsage = CalculateMemoryUsage(),
            EstimatedMemoryWithoutFlyweight = CalculateMemoryWithoutFlyweight()
        };
    }

    /// <summary>
    /// Inserts text at a specific position.
    /// </summary>
    public void InsertText(int position, string text, TextFormatting? formatting = null)
    {
        if (position < 0 || position > _characters.Count)
            return;

        formatting ??= new TextFormatting();
        var insertedChars = new List<DocumentCharacter>();

        foreach (char character in text)
        {
            var flyweight = _flyweightFactory.GetFlyweight(character, formatting.FontFamily);
            var documentChar = new DocumentCharacter
            {
                Flyweight = flyweight,
                Context = new CharacterContext(0, 0) // Position will be recalculated
                {
                    ForegroundColor = formatting.ForegroundColor,
                    BackgroundColor = formatting.BackgroundColor,
                    FontSize = formatting.FontSize,
                    FontStyle = formatting.FontStyle
                }
            };
            insertedChars.Add(documentChar);
        }

        _characters.InsertRange(position, insertedChars);
        RecalculatePositions();
    }

    /// <summary>
    /// Deletes a range of characters.
    /// </summary>
    public void DeleteRange(int startIndex, int count)
    {
        if (startIndex < 0 || startIndex >= _characters.Count || count <= 0)
            return;

        var endIndex = Math.Min(startIndex + count - 1, _characters.Count - 1);
        var deleteCount = endIndex - startIndex + 1;
        
        _characters.RemoveRange(startIndex, deleteCount);
        RecalculatePositions();
    }

    private (int X, int Y) CalculatePosition(char character, TextFormatting formatting)
    {
        if (character == '\n')
        {
            return (0, _currentLine + 1);
        }

        var width = _flyweightFactory.GetFlyweight(character, formatting.FontFamily)
            .GetWidth(formatting.FontSize, formatting.FontStyle);
        
        return ((int)(_currentColumn * width), _currentLine);
    }

    private void UpdatePosition(char character, TextFormatting formatting)
    {
        if (character == '\n')
        {
            _currentLine++;
            _currentColumn = 0;
        }
        else
        {
            _currentColumn++;
        }
    }

    private void RecalculatePositions()
    {
        _currentLine = 0;
        _currentColumn = 0;

        foreach (var documentChar in _characters)
        {
            var character = documentChar.Flyweight.GetIntrinsicState().Character;
            var position = CalculatePosition(character, new TextFormatting
            {
                FontSize = documentChar.Context.FontSize,
                FontStyle = documentChar.Context.FontStyle
            });

            documentChar.Context.X = position.X;
            documentChar.Context.Y = position.Y;
            documentChar.Context.LineNumber = _currentLine;
            documentChar.Context.ColumnNumber = _currentColumn;

            UpdatePosition(character, new TextFormatting());
        }
    }

    private long CalculateMemoryUsage()
    {
        // Estimate memory usage with flyweight pattern
        const long contextSize = 64; // Estimated bytes per character context
        const long flyweightOverhead = 8; // Reference to flyweight
        return _characters.Count * (contextSize + flyweightOverhead) + _flyweightFactory.GetStatistics().EstimatedMemoryUsage;
    }

    private long CalculateMemoryWithoutFlyweight()
    {
        // Estimate memory usage without flyweight pattern
        const long fullCharacterSize = 2048; // Full character data per instance
        return _characters.Count * fullCharacterSize;
    }
}

/// <summary>
/// Represents a character in the document with its flyweight and extrinsic state.
/// </summary>
public class DocumentCharacter
{
    public ICharacterFlyweight Flyweight { get; set; } = null!;
    public CharacterContext Context { get; set; } = null!;

    public override string ToString()
    {
        var character = Flyweight.GetIntrinsicState().Character;
        return $"'{character}' at {Context}";
    }
}

/// <summary>
/// Text formatting options that represent extrinsic state.
/// </summary>
public class TextFormatting
{
    public ConsoleColor ForegroundColor { get; set; } = ConsoleColor.Black;
    public ConsoleColor BackgroundColor { get; set; } = ConsoleColor.White;
    public int FontSize { get; set; } = 12;
    public FontStyle FontStyle { get; set; } = FontStyle.Normal;
    public string FontFamily { get; set; } = "Arial";

    public TextFormatting Clone()
    {
        return new TextFormatting
        {
            ForegroundColor = ForegroundColor,
            BackgroundColor = BackgroundColor,
            FontSize = FontSize,
            FontStyle = FontStyle,
            FontFamily = FontFamily
        };
    }
}

/// <summary>
/// Document settings and configuration.
/// </summary>
public class DocumentSettings
{
    public int TabSize { get; set; } = 4;
    public bool ShowWhitespace { get; set; } = false;
    public bool WordWrap { get; set; } = true;
    public int LineHeight { get; set; } = 16;
    public int CharacterSpacing { get; set; } = 1;
    public ConsoleColor DefaultForegroundColor { get; set; } = ConsoleColor.Black;
    public ConsoleColor DefaultBackgroundColor { get; set; } = ConsoleColor.White;
}

/// <summary>
/// Statistics about the document and flyweight usage.
/// </summary>
public class DocumentStatistics
{
    public string Title { get; set; } = string.Empty;
    public int TotalCharacters { get; set; }
    public int TotalLines { get; set; }
    public int UniqueCharacters { get; set; }
    public Dictionary<char, int> CharacterFrequency { get; set; } = new();
    public Dictionary<CharacterType, int> CharacterTypeDistribution { get; set; } = new();
    public FlyweightStatistics FlyweightStatistics { get; set; } = new();
    public long EstimatedMemoryUsage { get; set; }
    public long EstimatedMemoryWithoutFlyweight { get; set; }

    public override string ToString()
    {
        var memoryUsageMB = EstimatedMemoryUsage / (1024.0 * 1024.0);
        var memoryWithoutMB = EstimatedMemoryWithoutFlyweight / (1024.0 * 1024.0);
        var memorySavedMB = (EstimatedMemoryWithoutFlyweight - EstimatedMemoryUsage) / (1024.0 * 1024.0);
        var memorySavedPercent = EstimatedMemoryWithoutFlyweight > 0 
            ? (double)(EstimatedMemoryWithoutFlyweight - EstimatedMemoryUsage) / EstimatedMemoryWithoutFlyweight 
            : 0;

        var topChars = CharacterFrequency
            .OrderByDescending(kvp => kvp.Value)
            .Take(5)
            .Select(kvp => $"'{kvp.Key}': {kvp.Value}")
            .ToList();

        return $"""
            Document Statistics: {Title}
              Total Characters: {TotalCharacters:N0}
              Total Lines: {TotalLines:N0}
              Unique Characters: {UniqueCharacters:N0}
              Memory Usage: {memoryUsageMB:F2} MB
              Memory Without Flyweight: {memoryWithoutMB:F2} MB
              Memory Saved: {memorySavedMB:F2} MB ({memorySavedPercent:P1})
              Top Characters: {string.Join(", ", topChars)}
              
            {FlyweightStatistics}
            """;
    }
}
