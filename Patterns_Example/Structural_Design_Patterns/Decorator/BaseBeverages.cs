namespace Patterns_Example.Structural_Design_Patterns.Decorator;

/// <summary>
/// Abstract base class for concrete beverages (ConcreteComponent in Decorator pattern).
/// Provides common functionality for all base beverages.
/// </summary>
public abstract class BaseBeverage : IBeverage
{
    protected string _name;
    protected string _description;
    protected decimal _basePrice;
    protected int _baseCalories;
    protected BeverageSize _size;
    protected List<string> _ingredients;
    protected List<string> _allergens;
    protected NutritionalInfo _baseNutrition;

    public virtual string Name => _name;
    public virtual string Description => _description;
    public virtual decimal Price => _basePrice * _size.GetSizeMultiplier();
    public virtual int CaloriesPerServing => (int)(_baseCalories * _size.GetSizeMultiplier());
    public virtual BeverageSize Size => _size;
    public virtual List<string> Ingredients => new List<string>(_ingredients);
    public virtual List<string> Allergens => new List<string>(_allergens);

    protected BaseBeverage(string name, string description, decimal basePrice, int baseCalories)
    {
        _name = name;
        _description = description;
        _basePrice = basePrice;
        _baseCalories = baseCalories;
        _size = BeverageSize.Medium;
        _ingredients = new List<string>();
        _allergens = new List<string>();
        _baseNutrition = new NutritionalInfo();
    }

    public abstract string Prepare();
    public abstract TimeSpan GetPreparationTime();

    public virtual NutritionalInfo GetNutritionalInfo()
    {
        var multiplier = _size.GetSizeMultiplier();
        return new NutritionalInfo
        {
            Calories = (int)(_baseNutrition.Calories * multiplier),
            Protein = _baseNutrition.Protein * multiplier,
            Carbohydrates = _baseNutrition.Carbohydrates * multiplier,
            Fat = _baseNutrition.Fat * multiplier,
            Sugar = _baseNutrition.Sugar * multiplier,
            Caffeine = _baseNutrition.Caffeine * multiplier,
            Sodium = _baseNutrition.Sodium * multiplier
        };
    }

    public virtual IBeverage WithSize(BeverageSize size)
    {
        _size = size;
        return this;
    }

    public virtual bool IsVegan()
    {
        var nonVeganIngredients = new[] { "milk", "cream", "butter", "honey", "whey" };
        return !_ingredients.Any(ingredient => 
            nonVeganIngredients.Any(nonVegan => 
                ingredient.Contains(nonVegan, StringComparison.OrdinalIgnoreCase)));
    }

    public virtual bool IsGlutenFree()
    {
        var glutenIngredients = new[] { "wheat", "barley", "rye", "malt" };
        return !_ingredients.Any(ingredient => 
            glutenIngredients.Any(gluten => 
                ingredient.Contains(gluten, StringComparison.OrdinalIgnoreCase)));
    }

    public virtual bool IsDairyFree()
    {
        var dairyIngredients = new[] { "milk", "cream", "butter", "cheese", "whey", "lactose" };
        return !_ingredients.Any(ingredient => 
            dairyIngredients.Any(dairy => 
                ingredient.Contains(dairy, StringComparison.OrdinalIgnoreCase)));
    }

    public virtual string GetReceipt()
    {
        return $"{Name} ({Size.GetDisplayName()}) - ${Price:F2}";
    }

    public virtual string GetDetailedDescription()
    {
        var ingredients = string.Join(", ", Ingredients);
        var allergens = Allergens.Count > 0 ? string.Join(", ", Allergens) : "None";
        
        return $"""
            {Name} ({Size.GetDisplayName()})
            {Description}
            
            Price: ${Price:F2}
            Calories: {CaloriesPerServing}
            
            Ingredients: {ingredients}
            Allergens: {allergens}
            
            Vegan: {(IsVegan() ? "Yes" : "No")}
            Gluten-Free: {(IsGlutenFree() ? "Yes" : "No")}
            Dairy-Free: {(IsDairyFree() ? "Yes" : "No")}
            """;
    }
}

/// <summary>
/// Concrete component - Espresso beverage.
/// </summary>
public class Espresso : BaseBeverage
{
    public Espresso() : base("Espresso", "Rich and bold espresso shot", 2.50m, 5)
    {
        _ingredients.AddRange(new[] { "espresso beans", "water" });
        _baseNutrition = new NutritionalInfo
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

    public override string Prepare()
    {
        return "1. Grind espresso beans\n2. Tamp grounds in portafilter\n3. Extract shot for 25-30 seconds\n4. Serve immediately";
    }

    public override TimeSpan GetPreparationTime()
    {
        return TimeSpan.FromMinutes(2);
    }
}

/// <summary>
/// Concrete component - House Blend Coffee.
/// </summary>
public class HouseBlend : BaseBeverage
{
    public HouseBlend() : base("House Blend Coffee", "Our signature medium roast coffee blend", 3.00m, 10)
    {
        _ingredients.AddRange(new[] { "house blend coffee beans", "water" });
        _baseNutrition = new NutritionalInfo
        {
            Calories = 10,
            Protein = 0.3m,
            Carbohydrates = 1.2m,
            Fat = 0.1m,
            Sugar = 0.0m,
            Caffeine = 95m,
            Sodium = 5m
        };
    }

    public override string Prepare()
    {
        return "1. Grind house blend beans\n2. Add to drip coffee maker\n3. Brew with hot water\n4. Serve hot";
    }

    public override TimeSpan GetPreparationTime()
    {
        return TimeSpan.FromMinutes(5);
    }
}

/// <summary>
/// Concrete component - Green Tea.
/// </summary>
public class GreenTea : BaseBeverage
{
    public GreenTea() : base("Green Tea", "Delicate and refreshing green tea", 2.75m, 0)
    {
        _ingredients.AddRange(new[] { "green tea leaves", "hot water" });
        _baseNutrition = new NutritionalInfo
        {
            Calories = 0,
            Protein = 0.0m,
            Carbohydrates = 0.0m,
            Fat = 0.0m,
            Sugar = 0.0m,
            Caffeine = 25m,
            Sodium = 2m
        };
    }

    public override string Prepare()
    {
        return "1. Heat water to 175°F\n2. Steep green tea leaves for 2-3 minutes\n3. Strain and serve";
    }

    public override TimeSpan GetPreparationTime()
    {
        return TimeSpan.FromMinutes(4);
    }
}

/// <summary>
/// Concrete component - Hot Chocolate.
/// </summary>
public class HotChocolate : BaseBeverage
{
    public HotChocolate() : base("Hot Chocolate", "Rich and creamy hot chocolate", 3.50m, 200)
    {
        _ingredients.AddRange(new[] { "cocoa powder", "milk", "sugar", "vanilla extract" });
        _allergens.AddRange(new[] { "milk" });
        _baseNutrition = new NutritionalInfo
        {
            Calories = 200,
            Protein = 8.0m,
            Carbohydrates = 30.0m,
            Fat = 6.0m,
            Sugar = 25.0m,
            Caffeine = 5m,
            Sodium = 100m
        };
    }

    public override string Prepare()
    {
        return "1. Heat milk in saucepan\n2. Whisk in cocoa powder and sugar\n3. Add vanilla extract\n4. Serve with whipped cream";
    }

    public override TimeSpan GetPreparationTime()
    {
        return TimeSpan.FromMinutes(6);
    }
}
