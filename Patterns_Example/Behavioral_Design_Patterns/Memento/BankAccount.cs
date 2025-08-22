namespace Patterns_Example.Behavioral_Design_Patterns.Memento;

public class BankAccount
{
    public decimal Balance { get; set; }

    public BankAccount(decimal balance)
    {
        Balance = balance;
    }

    public void Deposit(decimal amount)
    {
        if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));
        Balance += amount;
        Console.WriteLine($"Deposited {amount} balance. Balance: {Balance}");
    }

    public void Withdraw(decimal amount)
    {
        if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));

        if (Balance > amount)
        {
            Balance -= amount;
            Console.WriteLine($"Withdrawn {amount} balance. Balance: {Balance}");
        }
        else
        {
            Console.WriteLine($"Insufficient balance. Balance: {Balance}");
        }
    }

    public BankAccountMemento Save(string transaction)
    {
        return new BankAccountMemento(Balance, transaction);
    }

    public void Restore(BankAccountMemento memento)
    {
        Balance = memento.Balance;
        Console.WriteLine($"Restored {Balance} balance. Balance: {Balance}");
    }
}