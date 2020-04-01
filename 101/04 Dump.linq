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
	//LINQPad includes useful extension methods for visualizing observables: .Dump() and .DumpLive()
	IObservable<long> source = Observable.Interval(TimeSpan.FromMilliseconds(300))
		.Take(5)
		.Concat(Observable.Throw<long>(new Exception("Ah, snap!")));
		
	source.Subscribe(x => Console.WriteLine("OnNext: {0}", x),
		(e) => Console.WriteLine("OnError: {0}", e),
		() => Console.WriteLine("OnCompleted."));
	
	source.Dump("Dump");
	// source.DumpLive("Dump Live!");
}