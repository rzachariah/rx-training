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
	// Rx contains a rich library of operators for filtering observable sequences. Learn them!
	
	var source = Observable.Interval(TimeSpan.FromMilliseconds(200));

	source.Dump("source");
	
	source.Where(x => x%5 == 0)
		.Dump("Where x is divisible by 5");

	source.Take(10)
		.Dump("Take first 10");
		
	source.TakeWhile(x => x < 10)
		.Dump("Take while x < 10");
		
	source.TakeUntil(Observable.Timer(TimeSpan.FromSeconds(1)))
		.Dump("Take for 1 seconds");
		
	source.Sample(TimeSpan.FromSeconds(1))
		.Dump("Sample once a second");

	// Beware. First is blocking!
	source.Skip(5).First()
		.Dump("First");		
}