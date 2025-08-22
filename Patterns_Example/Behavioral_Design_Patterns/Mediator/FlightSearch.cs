namespace Patterns_Example.Behavioral_Design_Patterns.Mediator;

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