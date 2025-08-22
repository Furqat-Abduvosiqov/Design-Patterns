# 🏦 Memento Pattern (Advanced Example)

## 📖 Definition
The **Memento Pattern** is a behavioral design pattern that allows capturing and restoring an object’s state without exposing its internal details.  
It is mainly used to implement **undo/redo** functionality.

---

## 🔥 Real-World Scenario — Banking System with Transaction Rollback

We’re designing a **banking system** where:  
- `BankAccount` can **deposit** and **withdraw** money.  
- Every transaction’s state should be **saved as a memento** so we can:  
  - Undo last transaction(s).  
  - Redo transactions if necessary (like a rollback/commit system).  

This is realistic in:  
- Financial apps (undo transaction, rollback).  
- Games (save/restore player state).  
- Editors (undo/redo).  
- Workflow engines.  

---

## 1. Memento Class

```csharp
public class BankAccountMemento
{
    public decimal Balance { get; }
    public string LastTransaction { get; }

    public BankAccountMemento(decimal Balance, string lastTransaction)
    {
        Balance = balance;
        LastTransaction = lastTransaction;
    }
}
```

---

## 2. Originator (BankAccount)

```csharp
public class BankAccount
{
    public decimal Balance { get; private set; }

    public BankAccount(decimal initialBalance)
    {
        Balance = initialBalance;
    }

    public void Deposit(decimal amount)
    {
        Balance += amount;
        Console.WriteLine($"Deposited {amount}, Balance = {Balance}");
    }

    public void Withdraw(decimal amount)
    {
        if (Balance >= amount)
        {
            Balance -= amount;
            Console.WriteLine($"Withdrew {amount}, Balance = {Balance}");
        }
        else
        {
            Console.WriteLine("Insufficient funds!");
        }
    }

    public BankAccountMemento Save(string transaction)
    {
        return new BankAccountMemento(Balance, transaction);
    }

    public void Restore(BankAccountMemento memento)
    {
        Balance = memento.Balance;
        Console.WriteLine($"Restored state: Balance = {Balance} (after undo: {memento.LastTransaction})");
    }
}
```

---

## 3. Caretaker (TransactionHistory)

```csharp
public class TransactionHistory
{
    private readonly Stack<BankAccountMemento> _undo = new();
    private readonly Stack<BankAccountMemento> _redo = new();

    public void SaveState(BankAccountMemento memento)
    {
        _undo.Push(memento);
        _redo.Clear(); // once new transaction happens, redo history is cleared
    }

    public BankAccountMemento? Undo()
    {
        if (_undo.Count > 1) 
        {
            var current = _undo.Pop();                 
            _redo.Push(current);                       
            return _undo.Peek();                    
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
```

---

## 4. Client Code

```csharp
class Program
{
    static void Main()
    {
        var history = new TransactionHistory();
        var account = new BankAccount(1000);

        // Save initial state
        history.SaveState(account.Save("Initial"));

        account.Deposit(200);
        history.SaveState(account.Save("Deposit 200"));

        account.Withdraw(100);
        history.SaveState(account.Save("Withdraw 100"));

        Console.WriteLine("---- Undo Last Transaction ----");
        var prevState = history.Undo();
        if (prevState != null) account.Restore(prevState);

        Console.WriteLine("---- Redo Last Transaction ----");
        var redoState = history.Redo();
        if (redoState != null) account.Restore(redoState);
    }
}
```

---

## ✅ Example Output

```
Deposited 200, Balance = 1200
Withdrew 100, Balance = 1100
---- Undo Last Transaction ----
Restored state: Balance = 1200 (after undo: Withdraw 100)
---- Redo Last Transaction ----
Restored state: Balance = 1100 (after undo: Redo)
```

---

## 🔑 Key Advanced Concepts

- **Two-way undo/redo stack**: useful in editors, banking, and workflow engines.  
- **Immutable mementos**: ensures integrity of historical states.  
- **Encapsulation**: `BankAccount` decides what state to save, not the caretaker.  
- **Realistic use case**: Transaction rollback in financial systems.  

---

## ⚖️ Pros & Cons

✅ Advantages:  
- Easy undo/redo.  
- Keeps history without exposing internal details.  
- Fits real-world transactional systems.  

❌ Disadvantages:  
- High memory usage if many states are stored.  
- Can be slow for large/complex objects.  
