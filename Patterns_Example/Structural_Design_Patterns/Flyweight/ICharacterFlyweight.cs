namespace Patterns_Example.Structural_Design_Patterns.Flyweight;

/// <summary>
/// Flyweight interface that defines operations that can be performed on flyweight objects.
/// The flyweight receives extrinsic state as parameters to its methods.
/// </summary>
public interface ICharacterFlyweight
{
    /// <summary>
    /// Renders the character at the specified position with the given formatting.
    /// </summary>
    /// <param name="context">Extrinsic state containing position, color, and other context-specific data</param>
    void Render(CharacterContext context);
    
    /// <summary>
    /// Gets the intrinsic properties of this character flyweight.
    /// </summary>
    CharacterIntrinsicState GetIntrinsicState();
    
    /// <summary>
    /// Calculates the display width of the character with the given formatting.
    /// </summary>
    /// <param name="fontSize">Font size for width calculation</param>
    /// <param name="fontStyle">Font style affecting width</param>
    double GetWidth(int fontSize, FontStyle fontStyle);
    
    /// <summary>
    /// Calculates the display height of the character with the given formatting.
    /// </summary>
    /// <param name="fontSize">Font size for height calculation</param>
    /// <param name="fontStyle">Font style affecting height</param>
    double GetHeight(int fontSize, FontStyle fontStyle);
    
    /// <summary>
    /// Checks if this character can be combined with another for ligatures or special rendering.
    /// </summary>
    /// <param name="nextChar">The next character to check for combination</param>
    bool CanCombineWith(char nextChar);
}

/// <summary>
/// Intrinsic state of a character - shared among all instances of the same character.
/// This data is stored in the flyweight and doesn't change based on context.
/// </summary>
public class CharacterIntrinsicState
{
    public char Character { get; }
    public CharacterType Type { get; }
    public System.Globalization.UnicodeCategory Category { get; }
    public bool IsWhitespace { get; }
    public bool IsPunctuation { get; }
    public bool IsDigit { get; }
    public bool IsLetter { get; }
    public string FontFamily { get; }
    public byte[] GlyphData { get; }
    public Dictionary<int, CharacterMetrics> SizeMetrics { get; }

    public CharacterIntrinsicState(char character, string fontFamily = "Arial")
    {
        Character = character;
        FontFamily = fontFamily;
        Type = DetermineCharacterType(character);
        Category = char.GetUnicodeCategory(character);
        IsWhitespace = char.IsWhiteSpace(character);
        IsPunctuation = char.IsPunctuation(character);
        IsDigit = char.IsDigit(character);
        IsLetter = char.IsLetter(character);
        GlyphData = GenerateGlyphData(character, fontFamily);
        SizeMetrics = GenerateSizeMetrics(character, fontFamily);
    }

    private static CharacterType DetermineCharacterType(char character)
    {
        if (char.IsLetter(character))
            return CharacterType.Letter;
        if (char.IsDigit(character))
            return CharacterType.Digit;
        if (char.IsWhiteSpace(character))
            return CharacterType.Whitespace;
        if (char.IsPunctuation(character))
            return CharacterType.Punctuation;
        if (char.IsSymbol(character))
            return CharacterType.Symbol;
        return CharacterType.Other;
    }

    private static byte[] GenerateGlyphData(char character, string fontFamily)
    {
        // Simulate glyph data generation (in real implementation, this would load from font files)
        var random = new Random(character.GetHashCode() ^ fontFamily.GetHashCode());
        var glyphSize = character switch
        {
            ' ' => 10,  // Space has minimal glyph data
            >= 'A' and <= 'Z' => 150, // Uppercase letters
            >= 'a' and <= 'z' => 120, // Lowercase letters
            >= '0' and <= '9' => 100, // Digits
            _ => 80 // Other characters
        };
        
        var data = new byte[glyphSize];
        random.NextBytes(data);
        return data;
    }

    private static Dictionary<int, CharacterMetrics> GenerateSizeMetrics(char character, string fontFamily)
    {
        var metrics = new Dictionary<int, CharacterMetrics>();
        
        // Generate metrics for common font sizes
        var commonSizes = new[] { 8, 10, 12, 14, 16, 18, 20, 24, 28, 32, 36, 48, 72 };
        
        foreach (var size in commonSizes)
        {
            metrics[size] = new CharacterMetrics
            {
                Width = CalculateWidth(character, size),
                Height = size, // Height typically matches font size
                Baseline = size * 0.8, // Baseline is typically 80% of font size
                Ascent = size * 0.8,
                Descent = size * 0.2
            };
        }
        
        return metrics;
    }

    private static double CalculateWidth(char character, int fontSize)
    {
        // Simulate character width calculation based on character and font size
        var baseWidth = character switch
        {
            ' ' => 0.25, // Space is narrow
            'i' or 'l' or 'I' => 0.3, // Narrow letters
            'w' or 'W' or 'm' or 'M' => 0.8, // Wide letters
            >= 'A' and <= 'Z' => 0.6, // Uppercase letters
            >= 'a' and <= 'z' => 0.5, // Lowercase letters
            >= '0' and <= '9' => 0.5, // Digits
            _ => 0.4 // Other characters
        };
        
        return baseWidth * fontSize;
    }

    public override string ToString()
    {
        return $"'{Character}' ({Type}, {FontFamily})";
    }
}

/// <summary>
/// Extrinsic state that varies for each character instance in the document.
/// This data is passed to flyweight methods and is not stored in the flyweight.
/// </summary>
public class CharacterContext
{
    public int X { get; set; }
    public int Y { get; set; }
    public ConsoleColor ForegroundColor { get; set; } = ConsoleColor.Black;
    public ConsoleColor BackgroundColor { get; set; } = ConsoleColor.White;
    public int FontSize { get; set; } = 12;
    public FontStyle FontStyle { get; set; } = FontStyle.Normal;
    public bool IsSelected { get; set; }
    public bool IsHighlighted { get; set; }
    public double Rotation { get; set; } = 0.0;
    public double ScaleX { get; set; } = 1.0;
    public double ScaleY { get; set; } = 1.0;
    public int LineNumber { get; set; }
    public int ColumnNumber { get; set; }

    public CharacterContext(int x, int y)
    {
        X = x;
        Y = y;
    }

    public CharacterContext Clone()
    {
        return new CharacterContext(X, Y)
        {
            ForegroundColor = ForegroundColor,
            BackgroundColor = BackgroundColor,
            FontSize = FontSize,
            FontStyle = FontStyle,
            IsSelected = IsSelected,
            IsHighlighted = IsHighlighted,
            Rotation = Rotation,
            ScaleX = ScaleX,
            ScaleY = ScaleY,
            LineNumber = LineNumber,
            ColumnNumber = ColumnNumber
        };
    }

    public override string ToString()
    {
        return $"Pos({X},{Y}) Size:{FontSize} Style:{FontStyle} Color:{ForegroundColor}";
    }
}

/// <summary>
/// Character metrics for a specific font size.
/// </summary>
public class CharacterMetrics
{
    public double Width { get; set; }
    public double Height { get; set; }
    public double Baseline { get; set; }
    public double Ascent { get; set; }
    public double Descent { get; set; }

    public override string ToString()
    {
        return $"W:{Width:F1} H:{Height:F1} B:{Baseline:F1}";
    }
}

/// <summary>
/// Types of characters for categorization.
/// </summary>
public enum CharacterType
{
    Letter,
    Digit,
    Whitespace,
    Punctuation,
    Symbol,
    Other
}

/// <summary>
/// Font styles that affect character rendering.
/// </summary>
[Flags]
public enum FontStyle
{
    Normal = 0,
    Bold = 1,
    Italic = 2,
    Underline = 4,
    Strikethrough = 8,
    BoldItalic = Bold | Italic
}


