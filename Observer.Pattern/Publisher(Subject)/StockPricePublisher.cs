using System.Reactive.Subjects;
using Observer.Pattern.Models;

namespace Observer.Pattern.Publisher_Subject_;

public class StockPricePublisher : IDisposable
{
    private readonly Subject<StockPrice> _subject = new();
    private readonly List<IObserver<StockPrice>> _observers = new();

    public IDisposable Subscribe(IObserver<StockPrice> observer)
    {
        _observers.Add(observer);
        return _subject.Subscribe(observer);
    }
    
    public void PublishPrice(StockPrice price)
    {
        Console.WriteLine($"[Publisher] Broadcasting {price.Symbol} = ${price.Price}");
        _subject.OnNext(price);
    }
    
    public void Dispose()
    {
        _subject.OnCompleted();
        _subject.Dispose();
    }
}