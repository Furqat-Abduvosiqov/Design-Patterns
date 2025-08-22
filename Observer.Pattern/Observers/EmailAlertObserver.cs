using Observer.Pattern.Models;

namespace Observer.Pattern.Observers;

public class EmailAlertObserver : INotificationObserver, IObserver<StockPrice>
{
    private readonly string _email;
    private readonly HashSet<string> _watchedSymbols;

    public EmailAlertObserver(string emial, params string[] symbols)
    {
        _email = emial;
        _watchedSymbols = new HashSet<string>(symbols);
    }
    
    public async Task OnPriceChangedAsync(StockPrice price)
    {
        if (IsInterestedIn(price.Symbol))
        {
            await Task.Delay(10); // Simulate email send
            Console.WriteLine($"📧 Email to {_email}: {price.Symbol} is now ${price.Price}");
        }
    }

    public bool IsInterestedIn(string symbol) => _watchedSymbols.Contains(symbol);
        

    public void OnCompleted() => Console.WriteLine("Email observer unsubscribed.");

    public void OnError(Exception error) => Console.WriteLine($"Email observer error: {error.Message}");

    public void OnNext(StockPrice value) => _ = OnPriceChangedAsync(value);
}
