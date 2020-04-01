<Query Kind="Program">
  <NuGetReference Version="2.2.2">Rx-Core</NuGetReference>
  <NuGetReference Version="2.2.2">Rx-Interfaces</NuGetReference>
  <NuGetReference Version="2.2.2">Rx-Linq</NuGetReference>
  <NuGetReference Version="2.2.2">Rx-Main</NuGetReference>
  <NuGetReference Version="2.2.2">Rx-PlatformServices</NuGetReference>
  <NuGetReference Version="2.2.2">Rx-Xaml</NuGetReference>
  <Namespace>System</Namespace>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Concurrency</Namespace>
  <Namespace>System.Reactive.Disposables</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

void Main()
{
	var quotes = new TickerPlant().GetQuotes();

	quotes
		.GroupBy(q => q.Symbol)
		.Select(g => g.Window(10)
			.Select(w => 
			w.Aggregate(new {Symbol = "", WeightedPrice = 0m, Volume = 0m, Time = DateTime.Now}, (acc, next) =>
			{
				return new { Symbol = next.Symbol, WeightedPrice = acc.WeightedPrice + next.Price*next.Volume, Volume = next.Volume, Time = next.Time };
			})
			.Select(x => new { Symbol = x.Symbol, VWAP=((int)(x.WeightedPrice/ x.Volume*100))/100m, Time = x.Time })
			))
			.Merge()
			.Merge()
			
		.Dump("VWAP");		

	quotes.Dump("Quotes");
}

public class Quote
{
	public string Symbol { get; set; }
	public decimal Price { get; set; }
	public decimal Volume { get; set; }
	public DateTime Time { get; set; }
}

public class TickerPlant
{
	private string[] universe = {"IBM", "CSCO", "GOOG", "AAPL"};
	private Random rand = new Random();
	
	public IObservable<Quote> GetQuotes()
	{
		return Observable.Interval(TimeSpan.FromMilliseconds(50))
			.Select(_ => {
				return GetQuote();
			});
	}
	
	public Quote GetQuote()
	{
		var r1 = rand.Next(0, universe.Count());
		var r2 = rand.Next(100, 1000);
		var r3 = rand.Next(1, 1000);
		return new Quote(){ Symbol = universe[r1], Price = r2/100m, Volume = r3, Time = DateTime.Now };
	}
}