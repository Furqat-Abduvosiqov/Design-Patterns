using Microsoft.AspNetCore.SignalR;

namespace Observer.Pattern.Hubs;

public class StockHub : Hub
{
    public async Task SubscribeToStock(string symbol)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, symbol);
        await Clients.Caller.SendAsync("Subscribed", symbol);
        Console.WriteLine($"Client {Context.ConnectionId} subscribed to {symbol}");
    }
    
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // Optional: cleanup
        await base.OnDisconnectedAsync(exception);
    }
}
