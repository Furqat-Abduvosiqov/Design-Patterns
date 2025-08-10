namespace Patterns_Example.Structural_Design_Patterns.Decorator;

/// <summary>
/// Abstract decorator class (Decorator in Decorator pattern).
/// Maintains a reference to a Component object and defines an interface that conforms to Component's interface.
/// </summary>
public abstract class BeverageDecorator : IBeverage
{
    public IBeverage _beverage;
    protected string _decoratorName;
    protected decimal _additionalPrice;
    protected int _additionalCalories;
    protected List<string> _additionalIngredients;
    protected List<string> _additionalAllergens;
    protected NutritionalInfo _additionalNutrition;

    protected BeverageDecorator(IBeverage beverage, string decoratorName, decimal additionalPrice, int additionalCalories)
    {
        _beverage = beverage ?? throw new ArgumentNullException(nameof(beverage));
        _decoratorName = decoratorName;
        _additionalPrice = additionalPrice;
        _additionalCalories = additionalCalories;
        _additionalIngredients = new List<string>();
        _additionalAllergens = new List<string>();
        _additionalNutrition = new NutritionalInfo();
    }

    public virtual string Name => $"{_beverage.Name} with {_decoratorName}";
    public virtual string Description => $"{_beverage.Description} Enhanced with {_decoratorName.ToLower()}.";
    public virtual decimal Price => _beverage.Price + (_additionalPrice * _beverage.Size.GetSizeMultiplier());
    public virtual int CaloriesPerServing => _beverage.CaloriesPerServing + (int)(_additionalCalories * _beverage.Size.GetSizeMultiplier());
    public virtual BeverageSize Size => _beverage.Size;

    public virtual List<string> Ingredients
    {
        get
        {
            var ingredients = new List<string>(_beverage.Ingredients);
            ingredients.AddRange(_additionalIngredients);
            return ingredients;
        }
    }

    public virtual List<string> Allergens
    {
        get
        {
            var allergens = new List<string>(_beverage.Allergens);
            allergens.AddRange(_additionalAllergens);
            return allergens.Distinct().ToList();
        }
    }

    public virtual string Prepare()
    {
        var basePreparation = _beverage.Prepare();
        var decoratorPreparation = GetDecoratorPreparationSteps();
        return $"{basePreparation}\n{decoratorPreparation}";
    }

    public virtual TimeSpan GetPreparationTime()
    {
        return _beverage.GetPreparationTime().Add(GetAdditionalPreparationTime());
    }

    public virtual NutritionalInfo GetNutritionalInfo()
    {
        var baseNutrition = _beverage.GetNutritionalInfo();
        var multiplier = _beverage.Size.GetSizeMultiplier();
        
        return new NutritionalInfo
        {
            Calories = baseNutrition.Calories + (int)(_additionalNutrition.Calories * multiplier),
            Protein = baseNutrition.Protein + (_additionalNutrition.Protein * multiplier),
            Carbohydrates = baseNutrition.Carbohydrates + (_additionalNutrition.Carbohydrates * multiplier),
            Fat = baseNutrition.Fat + (_additionalNutrition.Fat * multiplier),
            Sugar = baseNutrition.Sugar + (_additionalNutrition.Sugar * multiplier),
            Caffeine = baseNutrition.Caffeine + (_additionalNutrition.Caffeine * multiplier),
            Sodium = baseNutrition.Sodium + (_additionalNutrition.Sodium * multiplier)
        };
    }

    public virtual IBeverage WithSize(BeverageSize size)
    {
        _beverage = _beverage.WithSize(size);
        return this;
    }

    public virtual bool IsVegan()
    {
        var nonVeganIngredients = new[] { "milk", "cream", "butter", "honey", "whey" };
        var hasNonVeganDecorator = _additionalIngredients.Any(ingredient => 
            nonVeganIngredients.Any(nonVegan => 
                ingredient.Contains(nonVegan, StringComparison.OrdinalIgnoreCase)));
        
        return _beverage.IsVegan() && !hasNonVeganDecorator;
    }

    public virtual bool IsGlutenFree()
    {
        var glutenIngredients = new[] { "wheat", "barley", "rye", "malt" };
        var hasGlutenDecorator = _additionalIngredients.Any(ingredient => 
            glutenIngredients.Any(gluten => 
                ingredient.Contains(gluten, StringComparison.OrdinalIgnoreCase)));
        
        return _beverage.IsGlutenFree() && !hasGlutenDecorator;
    }

    public virtual bool IsDairyFree()
    {
        var dairyIngredients = new[] { "milk", "cream", "butter", "cheese", "whey", "lactose" };
        var hasDairyDecorator = _additionalIngredients.Any(ingredient => 
            dairyIngredients.Any(dairy => 
                ingredient.Contains(dairy, StringComparison.OrdinalIgnoreCase)));
        
        return _beverage.IsDairyFree() && !hasDairyDecorator;
    }

    public virtual string GetReceipt()
    {
        var baseReceipt = _beverage.GetReceipt();
        var decoratorPrice = _additionalPrice * _beverage.Size.GetSizeMultiplier();
        return $"{baseReceipt}\n  + {_decoratorName} - ${decoratorPrice:F2}";
    }

    public virtual string GetDetailedDescription()
    {
        var baseDescription = _beverage.GetDetailedDescription();
        var decoratorInfo = GetDecoratorDescription();
        return $"{baseDescription}\n\nDecorator Added:\n{decoratorInfo}";
    }

    // Abstract methods for concrete decorators to implement
    protected abstract string GetDecoratorPreparationSteps();
    protected abstract TimeSpan GetAdditionalPreparationTime();
    protected abstract string GetDecoratorDescription();
}

/// <summary>
/// Base class for add-on decorators (ingredients that can be added to beverages).
/// </summary>
public abstract class AddOnDecorator : BeverageDecorator
{
    protected AddOnDecorator(IBeverage beverage, string addOnName, decimal price, int calories)
        : base(beverage, addOnName, price, calories)
    {
    }

    protected override string GetDecoratorDescription()
    {
        return $"{_decoratorName}: +${_additionalPrice:F2}, +{_additionalCalories} calories";
    }

    protected override TimeSpan GetAdditionalPreparationTime()
    {
        return TimeSpan.FromSeconds(30); // Most add-ons take about 30 seconds
    }
}

/// <summary>
/// Base class for preparation method decorators (different ways to prepare the beverage).
/// </summary>
public abstract class PreparationDecorator : BeverageDecorator
{
    protected PreparationDecorator(IBeverage beverage, string preparationName, decimal additionalPrice = 0)
        : base(beverage, preparationName, additionalPrice, 0)
    {
    }

    protected override string GetDecoratorDescription()
    {
        var priceInfo = _additionalPrice > 0 ? $" (+${_additionalPrice:F2})" : "";
        return $"Preparation: {_decoratorName}{priceInfo}";
    }

    protected override TimeSpan GetAdditionalPreparationTime()
    {
        return TimeSpan.FromMinutes(1); // Preparation modifications typically add a minute
    }
}
