using Microsoft.AspNetCore.SignalR;
using Observer.Pattern.Hubs;
using Observer.Pattern.Models;
using Observer.Pattern.Observers;
using Observer.Pattern.Publisher_Subject_;

var builder = WebApplication.CreateBuilder(args);

// Add SignalR
builder.Services.AddSignalR();

// Add Observers
builder.Services.AddScoped<INotificationObserver, EmailAlertObserver>(
    sp => new EmailAlertObserver("alice@invest.com", "AAPL", "TSLA"));

builder.Services.AddScoped<INotificationObserver, SmsAlertObserver>(
    sp => new SmsAlertObserver("+1234567890", 150.0m));

// Register publisher as singleton
builder.Services.AddSingleton<StockPricePublisher>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:51694")  // 🔥 Exact match
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // ✅ Required for SignalR (uses cookies/auth by default)
    });
});

// Register real-time observer with SignalR
builder.Services.AddSingleton<IObserver<StockPrice>>(sp =>
{
    var hubContext = sp.GetRequiredService<IHubContext<StockHub>>();
    return new RealTimeDashboardObserver(hubContext);
});

var app = builder.Build();


app.UseCors("AllowFrontend");

app.UseRouting(); 

app.UseDefaultFiles();

app.UseStaticFiles();

// Map SignalR hub
app.MapHub<StockHub>("/stockhub");

// ✅ Get service references after building the app
using (var scope = app.Services.CreateScope())
{
    var publisher = scope.ServiceProvider.GetRequiredService<StockPricePublisher>();
    var dashboardObserver = scope.ServiceProvider.GetRequiredService<IObserver<StockPrice>>();

    // 🔥 CRITICAL: Subscribe the observer to the publisher
    publisher.Subscribe(dashboardObserver);
}


// Simulate stock updates
app.MapGet("/trigger", async (StockPricePublisher publisher) =>
{
    var random = new Random();
    var symbols = new[] { "AAPL", "GOOGL", "TSLA", "MSFT" };

    for (int i = 0; i < 10; i++)
    {
        var symbol = symbols[random.Next(symbols.Length)];
        var price = (decimal)(100 + random.NextDouble() * 150);
        publisher.PublishPrice(new StockPrice(symbol, Math.Round(price, 2), DateTime.UtcNow));
        await Task.Delay(500);
    }

    return "Stock updates triggered.";
});

app.Run();
