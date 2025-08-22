namespace Patterns_Example.Behavioral_Design_Patterns.Memento;

public class TransactionHistory
{
    private readonly Stack<BankAccountMemento> _undo = new();
    private readonly Stack<BankAccountMemento> _redo = new();

    public void SaveState(BankAccountMemento memento)
    {
        _undo.Push(memento);
        _redo.Clear();
    }

    public BankAccountMemento? Undo()
    {
        if (_undo.Count > 1) // keep at least initial state
        {
            var current = _undo.Pop();                 // current state
            _redo.Push(current);                       // save it for redo
            return _undo.Peek();                       // return previous state
        }
        
        return null;
    }

    public BankAccountMemento? Redo()
    {
        if (_redo.Count > 0)
        {
            var memento = _redo.Pop();
            _undo.Push(memento);
            return memento;
        }
        
        return null;
    }

}