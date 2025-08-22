using Microsoft.AspNetCore.SignalR;
using Observer.Pattern.Hubs;
using Observer.Pattern.Models;

namespace Observer.Pattern.Observers;

public class RealTimeDashboardObserver : IObserver<StockPrice>
{
    private readonly IHubContext<StockHub> _hubContext;

    public RealTimeDashboardObserver(IHubContext<StockHub> hubContext)
    {
        _hubContext = hubContext;
    }
    
    public void OnCompleted() => Console.WriteLine("Web observer completed.");

    public void OnError(Exception error) => Console.WriteLine($"Web observer error: {error.Message}");

    public void OnNext(StockPrice price)
    {
        // ✅ This must be called when price is published
        Console.WriteLine($"🌐 Broadcasting to group '{price.Symbol}': {price.Symbol} = ${price.Price}");

        // Send to all clients in the stock symbol group
        _hubContext.Clients.Group(price.Symbol).SendAsync("OnPriceUpdate", price);
    }
}
