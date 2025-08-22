namespace Patterns_Example.Behavioral_Design_Patterns.Iterator;

public interface IAggregate <T>
{
    IIterator<T> CreateIterator();
}