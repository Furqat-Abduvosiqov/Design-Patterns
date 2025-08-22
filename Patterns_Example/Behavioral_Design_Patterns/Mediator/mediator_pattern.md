
# Mediator Pattern in C#

## Overview

The **Mediator Pattern** is a behavioral design pattern that reduces coupling between components by introducing a **mediator object**.  
Instead of components referencing each other directly, they communicate **through the mediator**.

This pattern is commonly used in **complex workflows**, GUIs, and in enterprise .NET applications (e.g., via the MediatR library).

---

## When to Use
- When a system has **many interacting components** with tangled dependencies.
- When you want to **centralize complex orchestration logic**.
- When adding/removing components should not break other components.

---

## Structure

1. **Mediator Interface** — declares communication methods.
2. **Concrete Mediator** — orchestrates interactions between components.
3. **Colleague Components** — interact only through the mediator.
4. **Client** — configures and uses the system.

---

## Real-World Example — Flight Booking System

In this system we have multiple services:  
- `FlightSearch`  
- `PaymentProcessor`  
- `BookingService`  
- `NotificationService`  

These are coordinated by a **BookingMediator**.

---

### Mediator Interface

```csharp
public interface IMediator
{
    void Notify(object sender, string ev);
}
```

### Concrete Mediator

```csharp
public class BookingMediator : IMediator
{
    public FlightSearch FlightSearch { get; set; }
    public PaymentProcessor PaymentProcessor { get; set; }
    public BookingService BookingService { get; set; }
    public NotificationService NotificationService { get; set; }

    public void Notify(object sender, string ev)
    {
        if (sender is FlightSearch && ev == "FlightSelected")
        {
            Console.WriteLine("Mediator: Flight selected → request payment.");
            PaymentProcessor.ProcessPayment();
        }
        else if (sender is PaymentProcessor && ev == "PaymentSuccessful")
        {
            Console.WriteLine("Mediator: Payment done → create booking.");
            BookingService.CreateBooking();
        }
        else if (sender is BookingService && ev == "BookingCreated")
        {
            Console.WriteLine("Mediator: Booking created → send confirmation.");
            NotificationService.SendNotification();
        }
    }
}
```

### Components

```csharp
public class FlightSearch
{
    private readonly IMediator _mediator;
    public FlightSearch(IMediator mediator) => _mediator = mediator;

    public void SelectFlight()
    {
        Console.WriteLine("FlightSearch: User selected a flight.");
        _mediator.Notify(this, "FlightSelected");
    }
}

public class PaymentProcessor
{
    private readonly IMediator _mediator;
    public PaymentProcessor(IMediator mediator) => _mediator = mediator;

    public void ProcessPayment()
    {
        Console.WriteLine("PaymentProcessor: Processing payment...");
        _mediator.Notify(this, "PaymentSuccessful");
    }
}

public class BookingService
{
    private readonly IMediator _mediator;
    public BookingService(IMediator mediator) => _mediator = mediator;

    public void CreateBooking()
    {
        Console.WriteLine("BookingService: Booking created.");
        _mediator.Notify(this, "BookingCreated");
    }
}

public class NotificationService
{
    private readonly IMediator _mediator;
    public NotificationService(IMediator mediator) => _mediator = mediator;

    public void SendNotification()
    {
        Console.WriteLine("NotificationService: Confirmation email sent.");
    }
}
```

### Client Code

```csharp
class Program
{
    static void Main()
    {
        var mediator = new BookingMediator();

        var flightSearch = new FlightSearch(mediator);
        var paymentProcessor = new PaymentProcessor(mediator);
        var bookingService = new BookingService(mediator);
        var notificationService = new NotificationService(mediator);

        mediator.FlightSearch = flightSearch;
        mediator.PaymentProcessor = paymentProcessor;
        mediator.BookingService = bookingService;
        mediator.NotificationService = notificationService;

        // User starts by selecting a flight
        flightSearch.SelectFlight();
    }
}
```

### Output

```
FlightSearch: User selected a flight.
Mediator: Flight selected → request payment.
PaymentProcessor: Processing payment...
Mediator: Payment done → create booking.
BookingService: Booking created.
Mediator: Booking created → send confirmation.
NotificationService: Confirmation email sent.
```

---

## Advantages
- Reduces **tight coupling** between services/components.
- Centralizes **workflow logic**.
- Easier to extend (add/remove services without breaking others).

## Disadvantages
- Mediator can grow into a **God Object** if it holds too much logic.
- Adds one more indirection layer.

---

## Mediator in .NET (Real Use Case)

In enterprise .NET applications, a **Mediator pattern implementation is used via the MediatR library**.

- `IRequest` → represents a command/query.
- `IRequestHandler` → handles requests.
- `IMediator` → dispatches requests to handlers.

Example:

```csharp
public record BookFlightCommand(string FlightId, string UserId) : IRequest<bool>;

public class BookFlightHandler : IRequestHandler<BookFlightCommand, bool>
{
    public Task<bool> Handle(BookFlightCommand request, CancellationToken cancellationToken)
    {
        // Handle booking logic here
        return Task.FromResult(true);
    }
}
```

This way, instead of services calling each other directly, **requests are sent to the mediator**, which dispatches them.

---

# ✅ Summary

The **Mediator Pattern** is essential when multiple components need to coordinate without tight coupling.  
In C#, it is often used **directly** (custom mediator classes) or **indirectly** (via the MediatR library for CQRS and messaging).

