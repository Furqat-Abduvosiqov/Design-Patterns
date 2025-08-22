namespace Patterns_Example.Behavioral_Design_Patterns.Mediator;

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