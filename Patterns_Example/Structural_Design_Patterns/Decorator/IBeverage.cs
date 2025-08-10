namespace Patterns_Example.Structural_Design_Patterns.Decorator;

/// <summary>
/// Component interface for the Decorator pattern.
/// This defines the interface for objects that can have responsibilities added to them dynamically.
/// </summary>
public interface IBeverage
{
    string Name { get; }
    string Description { get; }
    decimal Price { get; }
    int CaloriesPerServing { get; }
    BeverageSize Size { get; }
    List<string> Ingredients { get; }
    List<string> Allergens { get; }
    
    // Preparation methods
    string Prepare();
    TimeSpan GetPreparationTime();
    
    // Nutritional information
    NutritionalInfo GetNutritionalInfo();
    
    // Customization methods
    IBeverage WithSize(BeverageSize size);
    bool IsVegan();
    bool IsGlutenFree();
    bool IsDairyFree();
    
    // Display methods
    string GetReceipt();
    string GetDetailedDescription();
}

/// <summary>
/// Nutritional information for beverages.
/// </summary>
public class NutritionalInfo
{
    public int Calories { get; set; }
    public decimal Protein { get; set; } // grams
    public decimal Carbohydrates { get; set; } // grams
    public decimal Fat { get; set; } // grams
    public decimal Sugar { get; set; } // grams
    public decimal Caffeine { get; set; } // mg
    public decimal Sodium { get; set; } // mg

    public override string ToString()
    {
        return $"""
            Nutritional Information (per serving):
              Calories: {Calories}
              Protein: {Protein:F1}g
              Carbohydrates: {Carbohydrates:F1}g
              Fat: {Fat:F1}g
              Sugar: {Sugar:F1}g
              Caffeine: {Caffeine:F1}mg
              Sodium: {Sodium:F1}mg
            """;
    }
}

/// <summary>
/// Beverage sizes with corresponding multipliers.
/// </summary>
public enum BeverageSize
{
    Small = 1,
    Medium = 2,
    Large = 3,
    ExtraLarge = 4
}

/// <summary>
/// Extension methods for BeverageSize enum.
/// </summary>
public static class BeverageSizeExtensions
{
    public static decimal GetSizeMultiplier(this BeverageSize size)
    {
        return size switch
        {
            BeverageSize.Small => 0.8m,
            BeverageSize.Medium => 1.0m,
            BeverageSize.Large => 1.3m,
            BeverageSize.ExtraLarge => 1.6m,
            _ => 1.0m
        };
    }

    public static string GetDisplayName(this BeverageSize size)
    {
        return size switch
        {
            BeverageSize.Small => "Small (8oz)",
            BeverageSize.Medium => "Medium (12oz)",
            BeverageSize.Large => "Large (16oz)",
            BeverageSize.ExtraLarge => "Extra Large (20oz)",
            _ => "Medium (12oz)"
        };
    }

    public static int GetVolumeOz(this BeverageSize size)
    {
        return size switch
        {
            BeverageSize.Small => 8,
            BeverageSize.Medium => 12,
            BeverageSize.Large => 16,
            BeverageSize.ExtraLarge => 20,
            _ => 12
        };
    }
}
