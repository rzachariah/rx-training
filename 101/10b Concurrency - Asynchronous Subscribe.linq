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
	Console.WriteLine("Starting on thread {0}", Thread.CurrentThread.ManagedThreadId);
	
	var source = Observable.Interval(TimeSpan.FromMilliseconds(300)
		//, Scheduler.Default
		)
		.Select(x => new {Thread = Thread.CurrentThread.ManagedThreadId, Value = x})
		.Take(5);
	var subscription = source.Subscribe(x => Console.WriteLine(x));
		
	Console.WriteLine("Ending on thread {0}", Thread.CurrentThread.ManagedThreadId);
}