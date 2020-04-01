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
	Console.WriteLine("Program starting on thread {0}.", Thread.CurrentThread.ManagedThreadId);
	Action a = () =>
		{
			Console.WriteLine("Task starting on thread {0}.", Thread.CurrentThread.ManagedThreadId);
			Observable.Interval(TimeSpan.FromMilliseconds(50), Scheduler.NewThread)
				.Subscribe(x => Console.WriteLine("Received {0} on thread {1}.", x, Thread.CurrentThread.ManagedThreadId));
			Console.WriteLine("Task ending on thread {0}.", Thread.CurrentThread.ManagedThreadId);
		};	
	Task.Run(a);
	Console.WriteLine("End of program on thread {0}.", Thread.CurrentThread.ManagedThreadId);
}