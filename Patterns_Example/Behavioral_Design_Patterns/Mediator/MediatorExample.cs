namespace Patterns_Example.Behavioral_Design_Patterns.Mediator;

public class MediatorExample
{
    public static void Demonstrate()
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