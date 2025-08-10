namespace Patterns_Example.Structural_Design_Patterns.Flyweight;

/// <summary>
/// Concrete flyweight implementation for characters.
/// Stores intrinsic state (character data that doesn't change based on context)
/// and implements operations that use extrinsic state passed as parameters.
/// </summary>
public class CharacterFlyweight : ICharacterFlyweight
{
    private readonly CharacterIntrinsicState _intrinsicState;
    private static readonly Dictionary<char, string[]> _ligatureMap;
    private static readonly Dictionary<FontStyle, double> _styleWidthMultipliers;
    private static readonly Dictionary<FontStyle, double> _styleHeightMultipliers;

    static CharacterFlyweight()
    {
        // Initialize ligature mappings (character combinations that render as single glyphs)
        _ligatureMap = new Dictionary<char, string[]>
        {
            ['f'] = new[] { "fi", "fl", "ff", "ffi", "ffl" },
            ['A'] = new[] { "AE" },
            ['O'] = new[] { "OE" },
            ['"'] = new[] { "\"\"" },
            ['\''] = new[] { "''" },
            ['-'] = new[] { "--", "---" }
        };

        // Style multipliers for width and height calculations
        _styleWidthMultipliers = new Dictionary<FontStyle, double>
        {
            [FontStyle.Normal] = 1.0,
            [FontStyle.Bold] = 1.1,
            [FontStyle.Italic] = 1.0,
            [FontStyle.BoldItalic] = 1.1,
            [FontStyle.Underline] = 1.0,
            [FontStyle.Strikethrough] = 1.0
        };

        _styleHeightMultipliers = new Dictionary<FontStyle, double>
        {
            [FontStyle.Normal] = 1.0,
            [FontStyle.Bold] = 1.0,
            [FontStyle.Italic] = 1.0,
            [FontStyle.BoldItalic] = 1.0,
            [FontStyle.Underline] = 1.2, // Extra space for underline
            [FontStyle.Strikethrough] = 1.0
        };
    }

    public CharacterFlyweight(char character, string fontFamily = "Arial")
    {
        _intrinsicState = new CharacterIntrinsicState(character, fontFamily);
    }

    public virtual void Render(CharacterContext context)
    {
        // Simulate character rendering with the given context
        var originalForeground = Console.ForegroundColor;
        var originalBackground = Console.BackgroundColor;

        try
        {
            // Apply context-specific formatting
            Console.ForegroundColor = context.ForegroundColor;
            Console.BackgroundColor = context.BackgroundColor;

            // Position cursor (in a real implementation, this would be more sophisticated)
            if (context.X >= 0 && context.Y >= 0 && context.X < Console.WindowWidth && context.Y < Console.WindowHeight)
            {
                Console.SetCursorPosition(context.X, context.Y);
            }

            // Apply visual effects based on context
            var displayChar = _intrinsicState.Character;
            
            if (context.IsSelected)
            {
                // Invert colors for selection
                (Console.ForegroundColor, Console.BackgroundColor) = (Console.BackgroundColor, Console.ForegroundColor);
            }

            if (context.IsHighlighted)
            {
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.ForegroundColor = ConsoleColor.Black;
            }

            // Render the character
            Console.Write(displayChar);

            // Add style indicators (simplified for console)
            if (context.FontStyle.HasFlag(FontStyle.Bold))
            {
                // In a real implementation, this would affect the glyph rendering
                // For console demo, we'll just add a visual indicator
            }

            if (context.FontStyle.HasFlag(FontStyle.Underline))
            {
                // In a real implementation, this would draw an underline
                // For console demo, we might use a different character or color
            }
        }
        finally
        {
            // Restore original console colors
            Console.ForegroundColor = originalForeground;
            Console.BackgroundColor = originalBackground;
        }
    }

    public CharacterIntrinsicState GetIntrinsicState()
    {
        return _intrinsicState;
    }

    public double GetWidth(int fontSize, FontStyle fontStyle)
    {
        // Get base width from cached metrics
        var baseWidth = GetBaseWidth(fontSize);
        
        // Apply style multiplier
        var styleMultiplier = _styleWidthMultipliers.GetValueOrDefault(fontStyle, 1.0);
        
        return baseWidth * styleMultiplier;
    }

    public double GetHeight(int fontSize, FontStyle fontStyle)
    {
        // Get base height (typically equals font size)
        var baseHeight = fontSize;
        
        // Apply style multiplier
        var styleMultiplier = _styleHeightMultipliers.GetValueOrDefault(fontStyle, 1.0);
        
        return baseHeight * styleMultiplier;
    }

    public virtual bool CanCombineWith(char nextChar)
    {
        // Check if this character can form a ligature with the next character
        if (_ligatureMap.TryGetValue(_intrinsicState.Character, out var ligatures))
        {
            var combination = $"{_intrinsicState.Character}{nextChar}";
            return ligatures.Any(ligature => ligature.StartsWith(combination));
        }
        
        return false;
    }

    private double GetBaseWidth(int fontSize)
    {
        // Try to get cached metrics for the exact font size
        if (_intrinsicState.SizeMetrics.TryGetValue(fontSize, out var metrics))
        {
            return metrics.Width;
        }
        
        // If exact size not cached, interpolate from nearest sizes
        var availableSizes = _intrinsicState.SizeMetrics.Keys.OrderBy(s => s).ToArray();
        
        if (availableSizes.Length == 0)
        {
            // Fallback calculation
            return CalculateFallbackWidth(fontSize);
        }
        
        // Find the closest cached size
        var closestSize = availableSizes.OrderBy(s => Math.Abs(s - fontSize)).First();
        var closestMetrics = _intrinsicState.SizeMetrics[closestSize];
        
        // Scale proportionally
        return closestMetrics.Width * ((double)fontSize / closestSize);
    }

    private double CalculateFallbackWidth(int fontSize)
    {
        // Fallback width calculation when no cached metrics are available
        var baseWidth = _intrinsicState.Character switch
        {
            ' ' => 0.25, // Space
            'i' or 'l' or 'I' or 'j' or 't' => 0.3, // Narrow characters
            'w' or 'W' or 'm' or 'M' => 0.8, // Wide characters
            >= 'A' and <= 'Z' => 0.6, // Uppercase letters
            >= 'a' and <= 'z' => 0.5, // Lowercase letters
            >= '0' and <= '9' => 0.5, // Digits
            _ => 0.4 // Other characters
        };
        
        return baseWidth * fontSize;
    }

    public override string ToString()
    {
        return $"CharacterFlyweight[{_intrinsicState}]";
    }

    public override bool Equals(object? obj)
    {
        if (obj is CharacterFlyweight other)
        {
            return _intrinsicState.Character == other._intrinsicState.Character &&
                   _intrinsicState.FontFamily == other._intrinsicState.FontFamily;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_intrinsicState.Character, _intrinsicState.FontFamily);
    }
}

/// <summary>
/// Specialized flyweight for whitespace characters with optimized rendering.
/// </summary>
public class WhitespaceFlyweight : CharacterFlyweight
{
    public WhitespaceFlyweight(char character, string fontFamily = "Arial") 
        : base(character, fontFamily)
    {
        if (!char.IsWhiteSpace(character))
        {
            throw new ArgumentException($"Character '{character}' is not whitespace", nameof(character));
        }
    }

    public override void Render(CharacterContext context)
    {
        // Optimized rendering for whitespace - often just cursor movement
        var intrinsicState = GetIntrinsicState();
        
        if (intrinsicState.Character == ' ')
        {
            // For spaces, we might just move the cursor or render a visible space if selected
            if (context.IsSelected || context.IsHighlighted)
            {
                base.Render(context); // Render visible space when selected
            }
            else
            {
                // Just advance cursor position (no actual rendering needed)
                var width = GetWidth(context.FontSize, context.FontStyle);
                // In a real implementation, this would advance the rendering position
            }
        }
        else
        {
            // For other whitespace (tabs, newlines), use base rendering
            base.Render(context);
        }
    }
}

/// <summary>
/// Specialized flyweight for punctuation characters with enhanced ligature support.
/// </summary>
public class PunctuationFlyweight : CharacterFlyweight
{
    private static readonly Dictionary<char, char[]> _punctuationCombinations;

    static PunctuationFlyweight()
    {
        _punctuationCombinations = new Dictionary<char, char[]>
        {
            ['"'] = new[] { '"' }, // Smart quotes
            ['\''] = new[] { '\'' }, // Smart apostrophes
            ['-'] = new[] { '-' }, // En dash, em dash
            ['.'] = new[] { '.', '.' }, // Ellipsis
            ['<'] = new[] { '<', '=' }, // Less than or equal
            ['>'] = new[] { '>', '=' }, // Greater than or equal
            ['!'] = new[] { '=' }, // Not equal
            ['='] = new[] { '=' } // Double equals
        };
    }

    public PunctuationFlyweight(char character, string fontFamily = "Arial") 
        : base(character, fontFamily)
    {
        if (!char.IsPunctuation(character) && !char.IsSymbol(character))
        {
            throw new ArgumentException($"Character '{character}' is not punctuation or symbol", nameof(character));
        }
    }

    public override bool CanCombineWith(char nextChar)
    {
        // Enhanced ligature detection for punctuation
        var intrinsicState = GetIntrinsicState();
        
        if (_punctuationCombinations.TryGetValue(intrinsicState.Character, out var combinations))
        {
            return combinations.Contains(nextChar);
        }
        
        return base.CanCombineWith(nextChar);
    }
}
