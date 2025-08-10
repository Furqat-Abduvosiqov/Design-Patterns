namespace Patterns_Example.Structural_Design_Patterns.Decorator;

/// <summary>
/// Concrete decorator for making beverages iced.
/// </summary>
public class IcedDecorator : PreparationDecorator
{
    public IcedDecorator(IBeverage beverage) : base(beverage, "Iced", 0.00m)
    {
        _additionalIngredients.Add("ice cubes");
    }

    public override string Name => $"Iced {_beverage.Name.Replace(" with ", " with ")}";

    protected override string GetDecoratorPreparationSteps()
    {
        return "5. Fill glass with ice cubes\n6. Pour beverage over ice\n7. Serve immediately";
    }

    protected override TimeSpan GetAdditionalPreparationTime()
    {
        return TimeSpan.FromSeconds(30); // Adding ice is quick
    }
}

/// <summary>
/// Concrete decorator for making beverages decaffeinated.
/// </summary>
public class DecafDecorator : PreparationDecorator
{
    public DecafDecorator(IBeverage beverage) : base(beverage, "Decaffeinated", 0.00m)
    {
        // Replace caffeinated ingredients with decaf versions
        var ingredients = _beverage.Ingredients.ToList();
        for (int i = 0; i < ingredients.Count; i++)
        {
            if (ingredients[i].Contains("coffee") || ingredients[i].Contains("espresso"))
            {
                ingredients[i] = $"decaffeinated {ingredients[i]}";
            }
        }
        _additionalIngredients.AddRange(ingredients);
    }

    public override string Name => $"Decaf {_beverage.Name.Replace(" with ", " with ")}";

    public override NutritionalInfo GetNutritionalInfo()
    {
        var nutrition = _beverage.GetNutritionalInfo();
        // Remove most caffeine (decaf still has trace amounts)
        nutrition.Caffeine = nutrition.Caffeine * 0.03m; // Decaf has ~3% of original caffeine
        return nutrition;
    }

    protected override string GetDecoratorPreparationSteps()
    {
        return "5. Use decaffeinated beans/leaves for preparation";
    }

    protected override TimeSpan GetAdditionalPreparationTime()
    {
        return TimeSpan.Zero; // No additional time needed
    }
}

/// <summary>
/// Concrete decorator for making beverages extra hot.
/// </summary>
public class ExtraHotDecorator : PreparationDecorator
{
    public ExtraHotDecorator(IBeverage beverage) : base(beverage, "Extra Hot", 0.00m)
    {
    }

    public override string Name => $"Extra Hot {_beverage.Name.Replace(" with ", " with ")}";

    protected override string GetDecoratorPreparationSteps()
    {
        return "5. Heat beverage to 180°F (instead of standard 160°F)\n6. Serve immediately in pre-warmed cup";
    }

    protected override TimeSpan GetAdditionalPreparationTime()
    {
        return TimeSpan.FromSeconds(45); // Extra heating time
    }
}

/// <summary>
/// Concrete decorator for making beverages with half the caffeine.
/// </summary>
public class HalfCafDecorator : PreparationDecorator
{
    public HalfCafDecorator(IBeverage beverage) : base(beverage, "Half Caffeine", 0.00m)
    {
        // Mix regular and decaf ingredients
        var ingredients = _beverage.Ingredients.ToList();
        for (int i = 0; i < ingredients.Count; i++)
        {
            if (ingredients[i].Contains("coffee") || ingredients[i].Contains("espresso"))
            {
                ingredients[i] = $"half regular, half decaffeinated {ingredients[i]}";
            }
        }
        _additionalIngredients.AddRange(ingredients);
    }

    public override string Name => $"Half-Caf {_beverage.Name.Replace(" with ", " with ")}";

    public override NutritionalInfo GetNutritionalInfo()
    {
        var nutrition = _beverage.GetNutritionalInfo();
        // Reduce caffeine by half
        nutrition.Caffeine = nutrition.Caffeine * 0.5m;
        return nutrition;
    }

    protected override string GetDecoratorPreparationSteps()
    {
        return "5. Use 50% regular and 50% decaffeinated beans/leaves for preparation";
    }

    protected override TimeSpan GetAdditionalPreparationTime()
    {
        return TimeSpan.FromSeconds(30); // Slight additional time for mixing
    }
}

/// <summary>
/// Concrete decorator for making beverages with double strength.
/// </summary>
public class DoubleStrengthDecorator : PreparationDecorator
{
    public DoubleStrengthDecorator(IBeverage beverage) : base(beverage, "Double Strength", 0.50m)
    {
        // Double the coffee/tea ingredients
        var ingredients = _beverage.Ingredients.ToList();
        for (int i = 0; i < ingredients.Count; i++)
        {
            if (ingredients[i].Contains("coffee") || ingredients[i].Contains("espresso") || ingredients[i].Contains("tea"))
            {
                ingredients[i] = $"double {ingredients[i]}";
            }
        }
        _additionalIngredients.AddRange(ingredients);
    }

    public override string Name => $"Double Strength {_beverage.Name.Replace(" with ", " with ")}";

    public override NutritionalInfo GetNutritionalInfo()
    {
        var nutrition = _beverage.GetNutritionalInfo();
        // Double the caffeine and slightly increase other values
        nutrition.Caffeine = nutrition.Caffeine * 2.0m;
        nutrition.Calories = (int)(nutrition.Calories * 1.1m); // Slight increase due to more grounds
        return nutrition;
    }

    protected override string GetDecoratorPreparationSteps()
    {
        return "5. Use double the amount of coffee beans/tea leaves\n6. Adjust brewing time for optimal extraction";
    }

    protected override TimeSpan GetAdditionalPreparationTime()
    {
        return TimeSpan.FromMinutes(1); // Additional brewing time
    }
}

/// <summary>
/// Concrete decorator for making beverages with foam art.
/// </summary>
public class FoamArtDecorator : PreparationDecorator
{
    private readonly string _artPattern;

    public FoamArtDecorator(IBeverage beverage, string artPattern = "heart") : base(beverage, "Foam Art", 1.00m)
    {
        _artPattern = artPattern;
    }

    public override string Name => $"{_beverage.Name.Replace(" with ", " with ")} with {_artPattern} foam art";

    protected override string GetDecoratorPreparationSteps()
    {
        return $"5. Steam milk to create microfoam\n6. Pour milk with latte art technique\n7. Create {_artPattern} pattern in foam";
    }

    protected override TimeSpan GetAdditionalPreparationTime()
    {
        return TimeSpan.FromMinutes(2); // Foam art takes skill and time
    }

    protected override string GetDecoratorDescription()
    {
        return $"Foam Art ({_artPattern} pattern): +${_additionalPrice:F2} - Artistic milk foam design";
    }
}

/// <summary>
/// Concrete decorator for making beverages organic.
/// </summary>
public class OrganicDecorator : PreparationDecorator
{
    public OrganicDecorator(IBeverage beverage) : base(beverage, "Organic", 0.75m)
    {
        // Replace ingredients with organic versions
        var ingredients = _beverage.Ingredients.ToList();
        for (int i = 0; i < ingredients.Count; i++)
        {
            if (!ingredients[i].Contains("organic"))
            {
                ingredients[i] = $"organic {ingredients[i]}";
            }
        }
        _additionalIngredients.AddRange(ingredients);
    }

    public override string Name => $"Organic {_beverage.Name.Replace(" with ", " with ")}";

    protected override string GetDecoratorPreparationSteps()
    {
        return "5. Use certified organic ingredients throughout preparation";
    }

    protected override TimeSpan GetAdditionalPreparationTime()
    {
        return TimeSpan.Zero; // No additional time needed
    }

    protected override string GetDecoratorDescription()
    {
        return $"Organic: +${_additionalPrice:F2} - Made with certified organic ingredients";
    }
}
