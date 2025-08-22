namespace Patterns_Example.Behavioral_Design_Patterns.Mediator;

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