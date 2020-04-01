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
	var source = Observable.Interval(TimeSpan.FromMilliseconds(200))
		.Take(10);

	source.Dump("source");
	
	source
		.Aggregate((cur, next) => cur + next)
		.Dump("Aggregate");
		
	source
		.Sum()
		.Dump("Sum");
		
	source.Scan((cur, next) => cur + next)
		.Dump("Scan");
		
	source.GroupBy(x => x%2 == 0)
		.Select(g => g.Scan((cur, next) => cur + next))
		.Dump("Grouped Scan");

	source.GroupBy(x => x%2 == 0)
		.Select(g => g.Scan((cur, next) => cur + next))
		.Merge()
		.Dump("Grouped Scan Merged");		
}