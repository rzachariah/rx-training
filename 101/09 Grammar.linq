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
	// The Rx grammar specifies that an Observable will have zero or more OnNext notifications
	// followed by an optional OnCompleted or OnError, which can be expressed like this.
	//
	// 		OnNext* (OnCompleted | OnError)?
	//
	// This means that OnNext calls are serialized. We can update state in a Subscribe delegate without locks.
	
	var count = 0;
	var source = Observable.Interval(TimeSpan.FromMilliseconds(2), Scheduler.Default)
		.Take(105);
	var subscription = source.Subscribe(i =>
			{
				// count can be updated safely without locks
				if (count > 100)
				{
					// Note the OnNext notifications are coming from multiple threads (Scheduler.Default behavior)
					Console.WriteLine(new { Iteration = i, Count = count, Thread = System.Threading.Thread.CurrentThread.ManagedThreadId });
				}
				Thread.Sleep(3);	// next OnNext won't start until the handler completes
				count++;
			});
}