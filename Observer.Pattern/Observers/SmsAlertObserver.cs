using Observer.Pattern.Models;

namespace Observer.Pattern.Observers;

public class SmsAlertObserver : INotificationObserver, IObserver<StockPrice>
{
    private readonly string _phone;
    private readonly decimal _threshold;

    public SmsAlertObserver(string phone, decimal threshold)
    {
        _phone = phone;
        _threshold = threshold;
    }
    
    public async Task OnPriceChangedAsync(StockPrice price)
    {
        if (price.Price > _threshold)
        {
            await Task.Delay(5); // Simulate SMS
            Console.WriteLine($"📱 SMS to {_phone}: {price.Symbol} crossed ${_threshold}! Now ${price.Price}");
        }
    }

    public bool IsInterestedIn(string symbol) => true;

    public void OnCompleted() { }

    public void OnError(Exception error) { }

    public void OnNext(StockPrice value) => _ = OnPriceChangedAsync(value);
   
}
