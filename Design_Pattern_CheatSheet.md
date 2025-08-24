# 🧠 Design Patterns Cheat Sheet
### *A Practical Guide to Solving Common Problems with Design Patterns*

> **Target Audience**: C#/.NET Developers, Architects, Senior Engineers  
> **Purpose**: Map real-world problems to proven design patterns

---

## 🗂 Table of Contents

1. [Creational Patterns](#-creational-patterns)
2. [Structural Patterns](#-structural-patterns)
3. [Behavioral Patterns](#-behavioral-patterns)
4. [Architectural & Cross-Cutting Patterns](#-architectural--cross-cutting-patterns)
5. [Anti-Patterns to Avoid](#-anti-patterns-to-avoid)

---

## ✅ Problem → Pattern Mapping

| Problem | Solution (Design Pattern) |
|--------|----------------------------|
| Object creation is complex or varies by environment | **Factory Method**, **Abstract Factory**, **Builder** |
| Need a single instance of a class (e.g., logger, config) | **Singleton** |
| Object setup requires many optional parameters | **Builder** |
| Want to hide object creation logic | **Factory Method**, **Simple Factory** |
| Need to compose objects into tree structures | **Composite** |
| Want to add functionality without modifying source | **Decorator**, **Adapter** |
| Need to change object behavior based on state | **State** |
| Want to encapsulate a request as an object | **Command** |
| Need to traverse a collection without exposing its structure | **Iterator** |
| Want to define a subscription mechanism for events | **Observer** |
| Need to centralize complex conditional logic | **Strategy**, **State** |
| Want to reduce dependencies between objects | **Mediator**, **Observer** |
| Need to undo/redo operations | **Command**, **Memento** |
| Want to separate algorithm from object structure | **Visitor** |

---

## 🔨 Creational Patterns

> **Purpose**: Control object creation logic.

### 1. **Singleton**
> Ensures a class has only one instance and provides a global point of access.

```csharp
public sealed class Logger
{
    private static readonly Logger _instance = new();
    public static Logger Instance => _instance;
    private Logger() { }
}
```
✅ Use when:
- One instance needed (logging, configuration, cache)
- Shared resource across app

⚠️ Caution: Avoid overuse; breaks testability and DI.

---

### 2. **Factory Method**
> Defines an interface for creating an object, but lets subclasses alter the type of objects created.

```csharp
public abstract class PaymentProcessor { public abstract void Process(); }
public class PayPalProcessor : PaymentProcessor { ... }
public class StripeProcessor : PaymentProcessor { ... }

public abstract class PaymentFactory { public abstract PaymentProcessor Create(); }
```

✅ Use when:
- Object type depends on runtime logic
- Want to delegate creation to subclasses

---

### 3. **Abstract Factory**
> Creates families of related or dependent objects without specifying their concrete classes.

```csharp
interface IUiFactory
{
    IButton CreateButton();
    ICheckbox CreateCheckbox();
}
```

✅ Use when:
- Creating themed UIs (e.g., Dark/Light mode)
- Multiple related object types

---

### 4. **Builder**
> Separates construction of a complex object from its representation.

```csharp
var car = new CarBuilder()
    .WithEngine("V8")
    .WithColor("Red")
    .Build();
```

✅ Use when:
- Many optional parameters
- Immutable object with fluent API
- Avoid telescoping constructors

---

### 5. **Prototype**
> Copies an existing object without depending on its class.

```csharp
public class Person : ICloneable
{
    public object Clone() => MemberwiseClone();
}
```

✅ Use when:
- Costly object creation
- Need many similar objects

---

## 🔗 Structural Patterns

> **Purpose**: Simplify design by identifying relationships between entities.

### 6. **Adapter**
> Allows incompatible interfaces to work together.

```csharp
public class LegacyPrinter { public void PrintOld(string text) => Console.WriteLine(text); }
public class ModernPrinterAdapter : IPrinter
{
    private readonly LegacyPrinter _legacy;
    public void Print(string text) => _legacy.PrintOld(text);
}
```

✅ Use when:
- Integrating legacy or third-party code
- Wrapping external APIs

---

### 7. **Decorator**
> Adds behavior to objects dynamically without altering their class.

```csharp
public interface IService { string Execute(); }
public class LoggingDecorator : IService
{
    private readonly IService _inner;
    public string Execute()
    {
        Console.WriteLine("Before");
        var result = _inner.Execute();
        Console.WriteLine("After");
        return result;
    }
}
```

✅ Use when:
- Add cross-cutting concerns (logging, caching, auth)
- Avoid subclass explosion

💡 In .NET: Often replaced by **middleware** or **interceptors**.

---

### 8. **Facade**
> Provides a simplified interface to a complex subsystem.

```csharp
public class OrderFacade
{
    private readonly InventoryService _inv;
    private readonly PaymentService _pay;
    private readonly ShippingService _ship;

    public void PlaceOrder(Order order)
    {
        _inv.CheckStock(order);
        _pay.Process(order);
        _ship.Schedule(order);
    }
}
```

✅ Use when:
- Hiding complexity of multiple services
- Onboarding new developers

---

### 9. **Composite**
> Composes objects into tree structures to represent part-whole hierarchies.

```csharp
public interface IComponent { void Render(); }
public class Leaf : IComponent { public void Render() => Console.WriteLine("Leaf"); }
public class Composite : IComponent
{
    private List<IComponent> _children = new();
    public void Render()
    {
        foreach (var c in _children) c.Render();
    }
}
```

✅ Use when:
- Building UI components, file systems, menus
- Treat individual and group objects uniformly

---

### 10. **Proxy**
> Controls access to another object (e.g., lazy loading, security).

```csharp
public class ImageProxy : IImage
{
    private RealImage? _real;
    public void Display()
    {
        _real ??= new RealImage();
        _real.Display();
    }
}
```

✅ Use when:
- Lazy initialization
- Access control
- Logging or monitoring

---

### 11. **Bridge**
> Separates abstraction from implementation so both can vary independently.

```csharp
public abstract class RemoteControl
{
    protected IDevice _device;
    public RemoteControl(IDevice device) => _device = device;
    public virtual void TurnOn() => _device.TurnOn();
}

public interface IDevice { void TurnOn(); }
```

✅ Use when:
- Multiple platforms + multiple device types
- Avoid binding abstraction to implementation at compile time

---

## 🔄 Behavioral Patterns

> **Purpose**: Manage communication and responsibilities between objects.

### 12. **Observer**
> Defines a subscription mechanism to notify multiple objects about events.

```csharp
public interface IObserver<in T> { void Update(T data); }
public interface IObservable<out T> { void Subscribe(IObserver<T> observer); }
```

✅ Use when:
- Event-driven systems (UI, messaging)
- Decouple publishers and subscribers

💡 In .NET: Use `IObservable<T>` or **event/action delegates**.

---

### 13. **Strategy**
> Encapsulates interchangeable algorithms and makes them interchangeable.

```csharp
public interface IPaymentStrategy { void Pay(decimal amount); }
public class CreditCardStrategy : IPaymentStrategy { ... }
public class PayPalStrategy : IPaymentStrategy { ... }

public class PaymentContext
{
    private IPaymentStrategy _strategy;
    public void SetStrategy(IPaymentStrategy s) => _strategy = s;
    public void ExecutePayment(decimal amount) => _strategy.Pay(amount);
}
```

✅ Use when:
- Multiple ways to do the same thing (sorting, validation, payment)
- Avoid `if-else` or `switch` chains

---

### 14. **Command**
> Encapsulates a request as an object, enabling parameterization and queuing.

```csharp
public interface ICommand { void Execute(); void Undo(); }
public class LightOnCommand : ICommand { ... }
```

✅ Use when:
- Implementing undo/redo
- Command queuing (e.g., task scheduling)
- Remote control systems

---

### 15. **State**
> Allows an object to alter its behavior when its internal state changes.

```csharp
public interface IState { void Handle(Context ctx); }
public class DraftState : IState { ... }
public class PublishedState : IState { ... }

public class Document
{
    public IState State { get; set; } = new DraftState();
    public void Publish() => State.Handle(this);
}
```

✅ Use when:
- Object behavior depends on state
- Replace complex conditionals

---

### 16. **Mediator**
> Reduces direct dependencies between objects by routing communication through a central mediator.

```csharp
public class ChatRoom : IMediator
{
    public void SendMessage(string msg, User user) { /* broadcast */ }
}
```

✅ Use when:
- Many-to-many object communication (e.g., chat, GUI components)
- Avoid tight coupling

---

### 17. **Iterator**
> Provides a way to access elements of a collection without exposing its internal structure.

```csharp
public interface IIterator<T>
{
    T Current();
    bool MoveNext();
    void Reset();
}
```

✅ Use when:
- Traverse custom collections
- Hide internal data structure

💡 In .NET: Use `IEnumerable<T>` and `IEnumerator<T>`.

---

### 18. **Visitor**
> Separates algorithm from object structure, allowing new operations without changing classes.

```csharp
public interface IElement
{
    void Accept(IVisitor visitor);
}

public interface IVisitor
{
    void Visit(ConcreteElementA a);
    void Visit(ConcreteElementB b);
}
```

✅ Use when:
- Add operations to object hierarchies
- Avoid polluting classes with unrelated logic

⚠️ Rare in .NET; consider alternatives like extension methods.

---

### 19. **Memento**
> Captures and externalizes an object’s internal state for later restoration.

```csharp
public class Memento { public string State { get; } }
public class Originator { public Memento Save(); public void Restore(Memento m); }
```

✅ Use when:
- Implement undo/rollback
- Snapshot-based systems

---

## 🏗 Architectural & Cross-Cutting Patterns

> **Purpose**: High-level structure and system-wide concerns.

### 20. **Repository Pattern**
> Abstracts data access logic behind an interface.

```csharp
public interface IUserRepository
{
    Task<User> GetByIdAsync(int id);
    Task AddAsync(User user);
}
```

✅ Use with: Entity Framework, Dapper, etc.  
✅ Enables unit testing and swapping data sources

---

### 21. **Unit of Work**
> Maintains a list of objects affected by a business transaction and coordinates writes.

```csharp
public interface IUnitOfWork
{
    IUserRepository Users { get; }
    Task<int> SaveChangesAsync();
}
```

✅ Use with: EF Core (built-in via `DbContext`)

---

### 22. **Dependency Injection (DI)**
> Injects dependencies instead of hardcoding them.

```csharp
public class OrderService
{
    public OrderService(IPaymentGateway payment) { ... }
}
```

✅ Use in all modern .NET apps  
✅ Promotes loose coupling and testability

---

### 23. **Pipeline / Middleware**
> Chains components that process a request sequentially.

```csharp
app.UseAuthentication();
app.UseAuthorization();
```

✅ Use in ASP.NET Core  
✅ Great for cross-cutting concerns

---

### 24. **Specification Pattern**
> Encapsulates business rules as reusable, composable predicates.

```csharp
public interface ISpecification<T>
{
    Expression<Func<T, bool>> IsSatisfied();
}
```

✅ Use when:
- Complex filtering logic
- Reusable business rules

---

## 🚫 Anti-Patterns to Avoid

| Anti-Pattern | Problem | Better Alternative |
|-------------|--------|-------------------|
| **God Object** | One class does everything | SRP + Modular design |
| **Spaghetti Code** | Unstructured, tangled logic | Clean layers, patterns |
| **Primitive Obsession** | Using primitives instead of value objects | Create rich types |
| **Magic Numbers/Strings** | Hardcoded values | Constants, enums, config |
| **Anemic Model** | Classes with data but no behavior | Domain-Driven Design |
| **Busy Constructor** | Too many dependencies | Facade, Mediator, refactor |
| **Async Overuse** | `async/await` everywhere | Use only when needed |

---

## 📚 Summary Table

| Category | Patterns |
|--------|----------|
| **Creational** | Singleton, Factory Method, Abstract Factory, Builder, Prototype |
| **Structural** | Adapter, Decorator, Facade, Composite, Proxy, Bridge, Flyweight |
| **Behavioral** | Observer, Strategy, Command, State, Mediator, Iterator, Visitor, Memento |
| **Architectural** | Repository, Unit of Work, DI, Specification, Pipeline |

