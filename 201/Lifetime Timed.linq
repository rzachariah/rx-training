<Query Kind="Program">
  <NuGetReference Version="2.2.2">Rx-Core</NuGetReference>
  <NuGetReference Version="2.2.2">Rx-Interfaces</NuGetReference>
  <NuGetReference Version="2.2.2">Rx-Linq</NuGetReference>
  <NuGetReference Version="2.2.2">Rx-Main</NuGetReference>
  <NuGetReference Version="2.2.2">Rx-PlatformServices</NuGetReference>
  <NuGetReference Version="2.2.2">Rx-Xaml</NuGetReference>
  <Namespace>System</Namespace>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Disposables</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

void Main()
{
	Observable.Interval(TimeSpan.FromMilliseconds(100))
		.Take(10)
		.Timed("Interval")
		.Dump("Interval");
}

// Define other methods and classes here
public static class ObservableEx
{
	public static IObservable<T> Timed<T>(this IObservable<T> source, string description)
	{
		return Observable.Using(() => new TimeIt(description), timeIt => source);
	}
}

public class TimeIt : IDisposable
{
	private readonly string _name;
	private readonly Stopwatch _watch;
	public TimeIt(string name)
	{
		Console.WriteLine("{0}: Starting at {1}", name, DateTime.Now);
		_name = name;
		_watch = Stopwatch.StartNew();
	}
	public void Dispose()
	{
		_watch.Stop();
		Console.WriteLine("{0}: Completed in {1}", _name, _watch.Elapsed);
	}
}