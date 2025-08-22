namespace Patterns_Example.Behavioral_Design_Patterns.Mediator;

public interface IMediator
{
    void Notify(object sender, string ev);
}