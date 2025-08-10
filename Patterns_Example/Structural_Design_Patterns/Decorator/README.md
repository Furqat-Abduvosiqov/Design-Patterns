# Decorator Pattern Example

This folder contains a comprehensive implementation of the Decorator design pattern using a real-world coffee shop ordering system that demonstrates dynamic behavior addition to objects.

## Files Overview

- **`IBeverage.cs`** - Component interface defining the contract for all beverage objects
- **`BaseBeverages.cs`** - Concrete components representing base beverage types
- **`BeverageDecorator.cs`** - Abstract decorator class maintaining component reference
- **`AddOnDecorators.cs`** - Concrete decorators for beverage add-ons (milk, syrups, toppings)
- **`PreparationDecorators.cs`** - Concrete decorators for preparation methods (iced, decaf, organic)
- **`CoffeeShop.cs`** - Context class with ordering system and builder pattern integration
- **`DecoratorExample.cs`** - Comprehensive usage demonstrations
- **`DecoratorPatternDemo.cs`** - Main demo program
- **`DecoratorPatternGuide.md`** - Complete pattern explanation

## What Problem Does This Solve?

The Decorator pattern addresses the class explosion problem that occurs when you need many combinations of features. Instead of:

```csharp
// ❌ Class explosion - exponential growth of classes
class Espresso { }
class EspressoWithMilk { }
class EspressoWithMilkAndSugar { }
class EspressoWithMilkAndSugarAndWhippedCream { }
class IcedEspresso { }
class IcedEspressoWithMilk { }
class IcedEspressoWithMilkAndSugar { }
class IcedEspressoWithMilkAndSugarAndWhippedCream { }
// ... hundreds of combinations for just a few options!
```

You can use:

```csharp
// ✅ Decorator pattern - dynamic composition
IBeverage beverage = new Espresso();
beverage = new MilkDecorator(beverage);
beverage = new VanillaSyrupDecorator(beverage);
beverage = new WhippedCreamDecorator(beverage);
beverage = new IcedDecorator(beverage);
// Any combination possible at runtime!
```

## Key Benefits Demonstrated

1. **Dynamic Composition**: Add functionality to objects at runtime
2. **Flexible Extension**: Easy to add new decorators without changing existing code
3. **Composition over Inheritance**: Avoids deep inheritance hierarchies
4. **Open/Closed Principle**: Open for extension, closed for modification
5. **Single Responsibility**: Each decorator has one specific purpose
6. **Transparent Interface**: Decorated objects look like original objects

## Real-World Use Cases

This pattern is commonly used for:

- **UI Component Enhancement**: Adding borders, scrollbars, shadows to UI elements
- **Stream Processing**: Adding compression, encryption, buffering to data streams
- **Text Formatting**: Adding bold, italic, underline, color to text
- **Web Request Processing**: Adding authentication, logging, caching to HTTP requests
- **Game Character Abilities**: Adding temporary buffs, equipment effects
- **Food/Beverage Customization**: Adding ingredients, preparation methods

## Components Demonstrated

### 1. Component Interface (`IBeverage`)
Defines the contract for all beverage objects:
```csharp
public interface IBeverage
{
    string Name { get; }
    decimal Price { get; }
    int CaloriesPerServing { get; }
    List<string> Ingredients { get; }
    string Prepare();
    NutritionalInfo GetNutritionalInfo();
    bool IsVegan();
}
```

### 2. Concrete Components (Base Beverages)
**Espresso**: Rich espresso shot (2.50, 5 calories, 63mg caffeine)
**House Blend**: Medium roast coffee (3.00, 10 calories, 95mg caffeine)
**Green Tea**: Delicate tea (2.75, 0 calories, 25mg caffeine)
**Hot Chocolate**: Rich chocolate drink (3.50, 200 calories, 5mg caffeine)

### 3. Abstract Decorator (`BeverageDecorator`)
Base class for all decorators:
- Maintains reference to wrapped component
- Delegates calls to wrapped component
- Provides extension points for concrete decorators

### 4. Concrete Decorators

**Add-On Decorators** (ingredients):
- `MilkDecorator`: Adds steamed milk (+$0.60, +50 cal)
- `SoyMilkDecorator`: Adds soy milk (+$0.65, +40 cal)
- `WhippedCreamDecorator`: Adds whipped cream (+$0.75, +80 cal)
- `ChocolateSyrupDecorator`: Adds chocolate syrup (+$0.50, +60 cal)
- `ExtraEspressoShotDecorator`: Adds extra shot (+$1.00, +63mg caffeine)

**Preparation Decorators** (how it's made):
- `IcedDecorator`: Makes beverage iced (no extra cost)
- `DecafDecorator`: Removes 97% of caffeine (no extra cost)
- `OrganicDecorator`: Uses organic ingredients (+$0.75)
- `DoubleStrengthDecorator`: Doubles coffee/tea strength (+$0.50)
- `FoamArtDecorator`: Adds artistic foam design (+$1.00)

## Advanced Features

### Nutritional Information Tracking
Comprehensive nutritional data with automatic calculation:
```csharp
public class NutritionalInfo
{
    public int Calories { get; set; }
    public decimal Protein { get; set; }
    public decimal Carbohydrates { get; set; }
    public decimal Fat { get; set; }
    public decimal Sugar { get; set; }
    public decimal Caffeine { get; set; }
    public decimal Sodium { get; set; }
}
```

### Dietary Restriction Support
Automatic detection of dietary compatibility:
```csharp
public bool IsVegan() // Checks for animal products
public bool IsGlutenFree() // Checks for gluten-containing ingredients
public bool IsDairyFree() // Checks for dairy products
```

### Size Scaling
Automatic price and nutrition scaling based on size:
```csharp
public enum BeverageSize
{
    Small = 1,    // ×0.8 multiplier
    Medium = 2,   // ×1.0 multiplier
    Large = 3,    // ×1.3 multiplier
    ExtraLarge = 4 // ×1.6 multiplier
}
```

### Builder Pattern Integration
Fluent interface for easy beverage construction:
```csharp
var beverage = coffeeShop.CreateOrder("Alice")
    .AddBeverage(new Espresso())
        .WithSize(BeverageSize.Large)
        .AddMilk()
        .AddVanillaSyrup()
        .AddWhippedCream()
        .MakeIced()
        .AddToOrder()
    .Build();
```

## How to Run

1. Navigate to the project root directory
2. Update Program.cs to use the Decorator example:
   ```csharp
   using Patterns_Example.Structural_Design_Patterns.Decorator;
   await DecoratorExample.RunExample();
   ```
3. Run the demo:
   ```bash
   dotnet run --project Patterns_Example
   ```

## Example Output

The demo will show:

1. **Basic Decorator Usage**: Adding milk and whipped cream to espresso
2. **Multiple Decorators Chaining**: Complex beverage with 5+ decorators
3. **Different Decorator Types**: Add-ons vs preparation methods
4. **Coffee Shop System**: Complete ordering system with menu and processing
5. **Nutritional Information**: Dietary restrictions and nutritional tracking

## Learning Objectives

After studying this example, you should understand:

- When the Decorator pattern solves real composition problems
- How to design flexible decorator hierarchies
- The difference between inheritance and composition for extending behavior
- How to implement transparent interfaces in decorators
- The relationship between Decorator and other patterns (Builder, Strategy)
- Performance and design trade-offs of the pattern

## Design Patterns Integration

This example demonstrates integration with other patterns:

### Builder Pattern
Fluent interface for constructing decorated objects:
```csharp
public class BeverageBuilder
{
    public BeverageBuilder AddMilk() { /* ... */ }
    public BeverageBuilder MakeIced() { /* ... */ }
    public OrderBuilder AddToOrder() { /* ... */ }
}
```

### Strategy Pattern
Different preparation strategies as decorators:
```csharp
public abstract class PreparationDecorator : BeverageDecorator
{
    // Different preparation strategies
}
```

### Template Method Pattern
Consistent preparation workflow with customization points:
```csharp
public virtual string Prepare()
{
    var basePreparation = _beverage.Prepare();
    var decoratorPreparation = GetDecoratorPreparationSteps();
    return $"{basePreparation}\n{decoratorPreparation}";
}
```

## Performance Considerations

1. **Memory Overhead**: Each decorator creates a wrapper object
2. **Method Call Chain**: Deep decorator chains can impact performance
3. **Object Creation**: Many decorators can be expensive to create
4. **Caching**: Consider caching expensive calculations

## Extension Ideas

Try extending this example by:

1. **Adding New Beverage Types**: Smoothies, frappés, specialty teas, cold brew
2. **Implementing Seasonal Decorators**: Pumpkin spice, peppermint, holiday flavors
3. **Adding Temperature Control**: Precise temperature decorators (140°F, 160°F, 180°F)
4. **Creating Loyalty Programs**: Discount decorators based on customer status
5. **Implementing Recipe Suggestions**: AI-powered decorator recommendations
6. **Adding Inventory Management**: Track ingredient availability
7. **Creating Mobile App Integration**: QR code ordering, payment processing

## Best Practices Demonstrated

1. **Interface Consistency**: All decorators implement the same interface
2. **Transparent Delegation**: Decorators properly delegate to wrapped components
3. **Single Responsibility**: Each decorator has one clear purpose
4. **Immutable Decoration**: Decorators don't modify the original object
5. **Composition over Inheritance**: Flexible composition instead of rigid inheritance
6. **Open/Closed Principle**: Easy to add new decorators without changing existing code
