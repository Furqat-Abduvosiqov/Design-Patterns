# Decorator Design Pattern

The Decorator pattern is a structural design pattern that allows behavior to be added to objects dynamically without altering their structure. It provides a flexible alternative to subclassing for extending functionality and follows the principle of composition over inheritance.

## Problem

Imagine you're building a coffee shop ordering system. You have different types of beverages (espresso, house blend, tea) and customers want to customize them with various add-ons (milk, syrups, whipped cream) and preparation methods (iced, decaf, extra hot). Without the Decorator pattern, you'd face several issues:

```csharp
// ❌ Without Decorator pattern - class explosion
class Espresso { }
class EspressoWithMilk { }
class EspressoWithMilkAndSugar { }
class EspressoWithMilkAndSugarAndWhippedCream { }
class IcedEspresso { }
class IcedEspressoWithMilk { }
class IcedEspressoWithMilkAndSugar { }
// ... hundreds of combinations!
```

### Issues with Subclassing Approach:

1. **Class Explosion**: N base types × M customizations = N×M classes
2. **Inflexible Design**: Cannot add new customizations without modifying existing code
3. **Runtime Limitations**: Cannot change object behavior after creation
4. **Code Duplication**: Similar functionality repeated across many classes
5. **Maintenance Nightmare**: Changes require updating multiple classes

## Solution

The Decorator pattern suggests wrapping objects in decorator objects that contain the additional behavior:

1. **Component Interface**: Defines the interface for objects that can have responsibilities added
2. **Concrete Component**: Basic implementation of the component interface
3. **Decorator**: Abstract class that implements the component interface and contains a reference to a component
4. **Concrete Decorators**: Add specific responsibilities to the component

### Key Components:

1. **Component (IBeverage)**: Interface for objects that can be decorated
2. **ConcreteComponent (Espresso, HouseBlend)**: Basic beverage implementations
3. **Decorator (BeverageDecorator)**: Abstract decorator maintaining component reference
4. **ConcreteDecorators (MilkDecorator, SyrupDecorator)**: Add specific functionality

## Real-World Example: Coffee Shop Ordering System

Our example demonstrates a coffee shop where:

### Base Beverages (ConcreteComponents):
- **Espresso**: Rich espresso shot with high caffeine
- **House Blend**: Medium roast coffee blend
- **Green Tea**: Delicate tea with moderate caffeine
- **Hot Chocolate**: Rich chocolate beverage with milk

### Add-On Decorators:
- **Milk Varieties**: Regular milk, soy milk, coconut milk
- **Syrups**: Vanilla, chocolate, caramel
- **Toppings**: Whipped cream, cinnamon
- **Extras**: Extra espresso shots, honey

### Preparation Decorators:
- **Temperature**: Iced, extra hot
- **Strength**: Decaf, half-caf, double strength
- **Special**: Organic, foam art

## Benefits Demonstrated

1. **Dynamic Composition**: Add functionality at runtime
2. **Flexible Extension**: Easy to add new decorators without changing existing code
3. **Single Responsibility**: Each decorator has one specific purpose
4. **Open/Closed Principle**: Open for extension, closed for modification
5. **Composition over Inheritance**: Avoids deep inheritance hierarchies
6. **Transparent Interface**: Decorated objects look like original objects

## Implementation Structure

```csharp
// Component interface
public interface IBeverage
{
    string Name { get; }
    decimal Price { get; }
    string Prepare();
}

// Concrete component
public class Espresso : IBeverage
{
    public string Name => "Espresso";
    public decimal Price => 2.50m;
    public string Prepare() => "Extract espresso shot";
}

// Abstract decorator
public abstract class BeverageDecorator : IBeverage
{
    protected IBeverage _beverage;
    
    protected BeverageDecorator(IBeverage beverage)
    {
        _beverage = beverage;
    }
    
    public virtual string Name => _beverage.Name;
    public virtual decimal Price => _beverage.Price;
    public virtual string Prepare() => _beverage.Prepare();
}

// Concrete decorator
public class MilkDecorator : BeverageDecorator
{
    public MilkDecorator(IBeverage beverage) : base(beverage) { }
    
    public override string Name => $"{_beverage.Name} with Milk";
    public override decimal Price => _beverage.Price + 0.60m;
    public override string Prepare() => $"{_beverage.Prepare()}\nAdd steamed milk";
}
```

## When to Use Decorator Pattern

✅ **Use Decorator when:**
- You want to add responsibilities to objects dynamically and transparently
- Extension by subclassing is impractical or would result in class explosion
- You want to add functionality that can be withdrawn
- You need different combinations of behaviors
- You want to follow the Open/Closed Principle

❌ **Don't use Decorator when:**
- You have a simple system with few variations
- The component interface is unstable and changes frequently
- You need to remove specific decorators from the middle of a chain
- Performance is critical and the extra indirection is costly

## Advanced Features

### 1. Nutritional Information Tracking
```csharp
public class NutritionalInfo
{
    public int Calories { get; set; }
    public decimal Protein { get; set; }
    public decimal Caffeine { get; set; }
    // ... other nutritional data
}
```

### 2. Dietary Restriction Support
```csharp
public bool IsVegan()
{
    var nonVeganIngredients = new[] { "milk", "cream", "honey" };
    return !Ingredients.Any(ingredient => 
        nonVeganIngredients.Any(nonVegan => 
            ingredient.Contains(nonVegan, StringComparison.OrdinalIgnoreCase)));
}
```

### 3. Builder Pattern Integration
```csharp
public class BeverageBuilder
{
    private IBeverage _beverage;
    
    public BeverageBuilder AddMilk()
    {
        _beverage = new MilkDecorator(_beverage);
        return this;
    }
    
    public BeverageBuilder AddSyrup(string flavor)
    {
        _beverage = new SyrupDecorator(_beverage, flavor);
        return this;
    }
}
```

## Code Structure

```
Decorator/
├── IBeverage.cs                 # Component interface
├── BaseBeverages.cs             # Concrete components
├── BeverageDecorator.cs         # Abstract decorator
├── AddOnDecorators.cs           # Concrete add-on decorators
├── PreparationDecorators.cs     # Concrete preparation decorators
├── CoffeeShop.cs               # Context and ordering system
├── DecoratorExample.cs         # Usage demonstrations
└── DecoratorPatternDemo.cs     # Main demo program
```

## Performance Considerations

1. **Memory Overhead**: Each decorator adds a wrapper object
2. **Method Call Chain**: Deep decorator chains can impact performance
3. **Object Creation**: Creating many decorators can be expensive
4. **Caching**: Consider caching expensive calculations like nutritional info

## Testing Benefits

1. **Isolated Testing**: Test each decorator independently
2. **Combination Testing**: Test various decorator combinations
3. **Mock Decorators**: Easy to create test doubles
4. **Behavior Verification**: Verify decorators add expected functionality

## Common Variations

### 1. Transparent Decorator
All methods delegated to the wrapped component:
```csharp
public virtual string Prepare() => _beverage.Prepare();
```

### 2. Semi-Transparent Decorator
Some methods add behavior, others delegate:
```csharp
public override string Prepare() => $"{_beverage.Prepare()}\nAdd milk";
```

### 3. Opaque Decorator
Completely replaces component behavior:
```csharp
public override string Prepare() => "Steam milk and add to cup";
```

## Learning Objectives

After studying this example, you should understand:

- When the Decorator pattern solves real composition problems
- How to design flexible decorator hierarchies
- The difference between inheritance and composition for extending behavior
- How to implement transparent interfaces in decorators
- The relationship between Decorator and other patterns (Builder, Strategy)
- Performance and design trade-offs of the pattern

## Extension Ideas

Try extending this example by:

1. **Adding New Beverage Types**: Smoothies, frappés, specialty teas
2. **Implementing Seasonal Decorators**: Pumpkin spice, peppermint, holiday flavors
3. **Adding Temperature Control**: Precise temperature decorators
4. **Creating Loyalty Programs**: Discount decorators based on customer status
5. **Implementing Allergen Tracking**: Comprehensive allergen management
6. **Adding Nutritional Goals**: Decorators that adjust for dietary requirements
7. **Creating Recipe Suggestions**: AI-powered decorator recommendations
