namespace Patterns_Example.Behavioral_Design_Patterns.Iterator;

public interface IIterator<T>
{
    bool HasNext();
    T Next();
}