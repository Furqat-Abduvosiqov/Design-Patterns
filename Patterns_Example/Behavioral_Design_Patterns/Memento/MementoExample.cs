namespace Patterns_Example.Behavioral_Design_Patterns.Memento;

public static class MementoExample
{
    public static void Demonstrate()
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