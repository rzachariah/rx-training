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
	IObservable<long> observable = Observable.Interval(TimeSpan.FromMilliseconds(300))
		.Take(10)
		.Concat(Observable.Throw<long>(new Exception("Ah, snap!")));

	// You indicate your interest in an Observable by subscribing to it.
	// We provide delegate handlers to Subscribe (no need to implement IObserver)
	IDisposable subscription = observable.Subscribe(x => Console.WriteLine(x),
		e => Console.WriteLine("ERROR {0}", e),
		() => Console.WriteLine("Sequence completed."));
		
	Thread.Sleep(2000);
	// You indicate you are no longer interested in an Observable subscription by disposing it!
	//subscription.Dispose();
}