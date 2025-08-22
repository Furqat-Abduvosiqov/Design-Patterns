namespace Patterns_Example.Behavioral_Design_Patterns.Mediator;

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