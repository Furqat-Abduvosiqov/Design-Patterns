namespace Patterns_Example.Structural_Design_Patterns.Decorator;

/// <summary>
/// Concrete decorator for adding milk to beverages.
/// </summary>
public class MilkDecorator : AddOnDecorator
{
    public MilkDecorator(IBeverage beverage) : base(beverage, "Milk", 0.60m, 50)
    {
        _additionalIngredients.Add("whole milk");
        _additionalAllergens.Add("milk");
        _additionalNutrition = new NutritionalInfo
        {
            Calories = 50,
            Protein = 3.0m,
            Carbohydrates = 4.0m,
            Fat = 3.0m,
            Sugar = 4.0m,
            Caffeine = 0m,
            Sodium = 40m
        };
    }

    protected override string GetDecoratorPreparationSteps()
    {
        return "5. Steam milk to 150°F\n6. Pour steamed milk into beverage";
    }
}

/// <summary>
/// Concrete decorator for adding soy milk to beverages.
/// </summary>
public class SoyMilkDecorator : AddOnDecorator
{
    public SoyMilkDecorator(IBeverage beverage) : base(beverage, "Soy Milk", 0.65m, 40)
    {
        _additionalIngredients.Add("soy milk");
        _additionalAllergens.Add("soy");
        _additionalNutrition = new NutritionalInfo
        {
            Calories = 40,
            Protein = 3.5m,
            Carbohydrates = 2.0m,
            Fat = 2.5m,
            Sugar = 1.0m,
            Caffeine = 0m,
            Sodium = 50m
        };
    }

    protected override string GetDecoratorPreparationSteps()
    {
        return "5. Steam soy milk to 150°F\n6. Pour steamed soy milk into beverage";
    }
}

/// <summary>
/// Concrete decorator for adding whipped cream to beverages.
/// </summary>
public class WhippedCreamDecorator : AddOnDecorator
{
    public WhippedCreamDecorator(IBeverage beverage) : base(beverage, "Whipped Cream", 0.75m, 80)
    {
        _additionalIngredients.AddRange(new[] { "heavy cream", "sugar", "vanilla extract" });
        _additionalAllergens.Add("milk");
        _additionalNutrition = new NutritionalInfo
        {
            Calories = 80,
            Protein = 1.0m,
            Carbohydrates = 2.0m,
            Fat = 8.0m,
            Sugar = 2.0m,
            Caffeine = 0m,
            Sodium = 10m
        };
    }

    protected override string GetDecoratorPreparationSteps()
    {
        return "5. Whip heavy cream with sugar and vanilla\n6. Top beverage with whipped cream";
    }

    protected override TimeSpan GetAdditionalPreparationTime()
    {
        return TimeSpan.FromMinutes(1); // Whipping cream takes a bit longer
    }
}

/// <summary>
/// Concrete decorator for adding chocolate syrup to beverages.
/// </summary>
public class ChocolateSyrupDecorator : AddOnDecorator
{
    public ChocolateSyrupDecorator(IBeverage beverage) : base(beverage, "Chocolate Syrup", 0.50m, 60)
    {
        _additionalIngredients.AddRange(new[] { "chocolate syrup", "cocoa", "sugar", "corn syrup" });
        _additionalNutrition = new NutritionalInfo
        {
            Calories = 60,
            Protein = 1.0m,
            Carbohydrates = 15.0m,
            Fat = 0.5m,
            Sugar = 14.0m,
            Caffeine = 2m,
            Sodium = 15m
        };
    }

    protected override string GetDecoratorPreparationSteps()
    {
        return "5. Add chocolate syrup to beverage\n6. Stir gently to combine";
    }
}

/// <summary>
/// Concrete decorator for adding vanilla syrup to beverages.
/// </summary>
public class VanillaSyrupDecorator : AddOnDecorator
{
    public VanillaSyrupDecorator(IBeverage beverage) : base(beverage, "Vanilla Syrup", 0.50m, 50)
    {
        _additionalIngredients.AddRange(new[] { "vanilla syrup", "sugar", "vanilla extract", "water" });
        _additionalNutrition = new NutritionalInfo
        {
            Calories = 50,
            Protein = 0.0m,
            Carbohydrates = 13.0m,
            Fat = 0.0m,
            Sugar = 13.0m,
            Caffeine = 0m,
            Sodium = 5m
        };
    }

    protected override string GetDecoratorPreparationSteps()
    {
        return "5. Add vanilla syrup to beverage\n6. Stir gently to combine";
    }
}

/// <summary>
/// Concrete decorator for adding caramel syrup to beverages.
/// </summary>
public class CaramelSyrupDecorator : AddOnDecorator
{
    public CaramelSyrupDecorator(IBeverage beverage) : base(beverage, "Caramel Syrup", 0.55m, 65)
    {
        _additionalIngredients.AddRange(new[] { "caramel syrup", "sugar", "butter", "cream", "salt" });
        _additionalAllergens.Add("milk");
        _additionalNutrition = new NutritionalInfo
        {
            Calories = 65,
            Protein = 0.5m,
            Carbohydrates = 16.0m,
            Fat = 1.0m,
            Sugar = 15.0m,
            Caffeine = 0m,
            Sodium = 25m
        };
    }

    protected override string GetDecoratorPreparationSteps()
    {
        return "5. Add caramel syrup to beverage\n6. Stir gently to combine";
    }
}

/// <summary>
/// Concrete decorator for adding cinnamon to beverages.
/// </summary>
public class CinnamonDecorator : AddOnDecorator
{
    public CinnamonDecorator(IBeverage beverage) : base(beverage, "Cinnamon", 0.25m, 5)
    {
        _additionalIngredients.Add("ground cinnamon");
        _additionalNutrition = new NutritionalInfo
        {
            Calories = 5,
            Protein = 0.1m,
            Carbohydrates = 1.5m,
            Fat = 0.0m,
            Sugar = 0.0m,
            Caffeine = 0m,
            Sodium = 1m
        };
    }

    protected override string GetDecoratorPreparationSteps()
    {
        return "5. Sprinkle ground cinnamon on top of beverage";
    }

    protected override TimeSpan GetAdditionalPreparationTime()
    {
        return TimeSpan.FromSeconds(15); // Sprinkling spices is quick
    }
}

/// <summary>
/// Concrete decorator for adding extra espresso shot to beverages.
/// </summary>
public class ExtraEspressoShotDecorator : AddOnDecorator
{
    public ExtraEspressoShotDecorator(IBeverage beverage) : base(beverage, "Extra Espresso Shot", 1.00m, 5)
    {
        _additionalIngredients.AddRange(new[] { "espresso beans", "water" });
        _additionalNutrition = new NutritionalInfo
        {
            Calories = 5,
            Protein = 0.1m,
            Carbohydrates = 0.8m,
            Fat = 0.0m,
            Sugar = 0.0m,
            Caffeine = 63m,
            Sodium = 5m
        };
    }

    protected override string GetDecoratorPreparationSteps()
    {
        return "5. Grind additional espresso beans\n6. Extract extra shot\n7. Add to beverage";
    }

    protected override TimeSpan GetAdditionalPreparationTime()
    {
        return TimeSpan.FromMinutes(2); // Extracting an espresso shot takes time
    }
}

/// <summary>
/// Concrete decorator for adding honey to beverages.
/// </summary>
public class HoneyDecorator : AddOnDecorator
{
    public HoneyDecorator(IBeverage beverage) : base(beverage, "Honey", 0.40m, 45)
    {
        _additionalIngredients.Add("organic honey");
        _additionalNutrition = new NutritionalInfo
        {
            Calories = 45,
            Protein = 0.1m,
            Carbohydrates = 12.0m,
            Fat = 0.0m,
            Sugar = 11.0m,
            Caffeine = 0m,
            Sodium = 1m
        };
    }

    protected override string GetDecoratorPreparationSteps()
    {
        return "5. Drizzle honey into beverage\n6. Stir well to dissolve";
    }
}

/// <summary>
/// Concrete decorator for adding coconut milk to beverages.
/// </summary>
public class CoconutMilkDecorator : AddOnDecorator
{
    public CoconutMilkDecorator(IBeverage beverage) : base(beverage, "Coconut Milk", 0.70m, 35)
    {
        _additionalIngredients.Add("coconut milk");
        _additionalNutrition = new NutritionalInfo
        {
            Calories = 35,
            Protein = 0.5m,
            Carbohydrates = 1.5m,
            Fat = 3.5m,
            Sugar = 1.0m,
            Caffeine = 0m,
            Sodium = 15m
        };
    }

    protected override string GetDecoratorPreparationSteps()
    {
        return "5. Steam coconut milk to 150°F\n6. Pour steamed coconut milk into beverage";
    }
}
