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
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{
	var r = new Random();
	var count = 0;
	Func<CancellationToken, Task<string>> asyncFunc = async _ =>
	{
		if (count == 3) throw new Exception("Ah, snap!");
		var sleepDuration = r.Next(0, 1000);
		await Task.Delay(TimeSpan.FromMilliseconds(sleepDuration));
		return string.Format("Task {0}. Duration {1}. ThreadId {2}", count++, sleepDuration, Thread.CurrentThread.ManagedThreadId);
	};
	
	PeriodicTaskObservable(asyncFunc, TimeSpan.FromMilliseconds(300), Scheduler.Default)
		.Dump("PeriodicTasks");
	
//	Observable.Interval(TimeSpan.FromMilliseconds(300))
//		.Dump("Interval");
}

// Define other methods and classes here
public static IObservable<T> PeriodicTaskObservable<T>(Func<CancellationToken, Task<T>> asyncFunc, TimeSpan period, IScheduler scheduler)
{
	return Observable.Interval(period, scheduler)
		.StartWith(-1)
		.Select(_ => Observable.FromAsync(asyncFunc))
		.Concat();
}