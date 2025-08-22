namespace Patterns_Example.Behavioral_Design_Patterns.Mediator;

public class NotificationService
{
    private readonly IMediator _mediator;
    public NotificationService(IMediator mediator) => _mediator = mediator;

    public void SendNotification()
    {
        Console.WriteLine("NotificationService: Confirmation email sent.");
    }
}