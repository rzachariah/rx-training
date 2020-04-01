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
	var taskCount = 0;
	Action<long> onNext = x =>
		{
			Task.Run(() =>
			{
				taskCount++;
				Console.WriteLine("[{2}]Task {0} started on thread {1}.", taskCount, x, Thread.CurrentThread.ManagedThreadId);
				Thread.Sleep(r.Next(500, 1000));
				Console.WriteLine("[{2}]		Task {0} completed on thread {1}.", taskCount, x, Thread.CurrentThread.ManagedThreadId);
				taskCount--;
			});
		};	
	Observable.Interval(TimeSpan.FromMilliseconds(50), Scheduler.NewThread)
		.Do(x => Console.WriteLine("Interval produced {0} on thread {1}.", x, Thread.CurrentThread.ManagedThreadId))
		.Subscribe(onNext);
}