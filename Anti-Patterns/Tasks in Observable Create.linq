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
	IObservable<int> o = Observable.Create<int>(
		observer => 
		{
			Console.WriteLine("	Observable.Create starting on thread {0}.", Thread.CurrentThread.ManagedThreadId);
			Action action = () => 
			{
				Console.WriteLine("		Task starting on thread {0}.", Thread.CurrentThread.ManagedThreadId);
				Thread.Sleep(3000);
				observer.OnNext(7);
				observer.OnCompleted();
				Console.WriteLine("		Task ending on thread {0}.", Thread.CurrentThread.ManagedThreadId);
			};
			Task.Run(action);
			return Disposable.Create(() => Console.WriteLine("	Observable.Create ending on thread {0}.", Thread.CurrentThread.ManagedThreadId));
		});
	o.Dump();
	Console.WriteLine("End of program on thread {0}.", Thread.CurrentThread.ManagedThreadId);
}