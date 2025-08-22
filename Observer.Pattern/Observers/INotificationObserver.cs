using Observer.Pattern.Models;

namespace Observer.Pattern.Observers;

public interface INotificationObserver
{
    Task OnPriceChangedAsync(StockPrice price);
    
    bool IsInterestedIn(string symbol);
}