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
	Observable.Interval(TimeSpan.FromMilliseconds(300))
		.Do(x => Console.WriteLine("[{0}] Interval produced {0} on thread {1}", x, Thread.CurrentThread.ManagedThreadId))
//		.SubscribeOn(Scheduler.NewThread)
//		.ObserveOn(Scheduler.NewThread)
		.Subscribe(x => 
		{
			Console.WriteLine("------>[{0}] Observed {0} on thread {1}", x, Thread.CurrentThread.ManagedThreadId);
			for (int i = 0; i < 500000000; i++);
			Console.WriteLine("------>[{0}] Completed work for {0} on thread {1}", x, Thread.CurrentThread.ManagedThreadId);
		});
}