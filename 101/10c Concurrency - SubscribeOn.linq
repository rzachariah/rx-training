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
	
	Action<IObserver<string>> blockingAction = observer =>
	{
		for (int i = 0; i < 500000000; i++);
		observer.OnNext("a");
		for (int i = 0; i < 200000000; i++);
		observer.OnNext("b");
		for (int i = 0; i < 200000000; i++);
		observer.OnCompleted();
	};

	// This blocks
	SynchronousSubscribe(blockingAction)
		.Select(x => new {Thread = Thread.CurrentThread.ManagedThreadId, Value = x})
		.Subscribe(x => Console.WriteLine(x));	
	
	// This runs async
//	AsynchronousSubscribe(blockingAction, Scheduler.Default)
//		.Select(x => new {Thread = Thread.CurrentThread.ManagedThreadId, Value = x})
//		.Subscribe(x => Console.WriteLine(x));

	// This makes the originally blocking subscribe run async
//	SynchronousSubscribe(blockingAction)
//		.Select(x => new {Thread = Thread.CurrentThread.ManagedThreadId, Value = x})
//		.SubscribeOn(Scheduler.Default)
//		.Subscribe(x => Console.WriteLine(x));	
		
	Console.WriteLine("Ending on thread {0}", Thread.CurrentThread.ManagedThreadId);
}

private IObservable<string> SynchronousSubscribe(Action<IObserver<string>> action)
{
	return Observable.Create<string>(
	(IObserver<string> observer) =>
	{
		action(observer);
		return Disposable.Create(() => Console.WriteLine("Blocking observer has unsubscribed"));
	});
}

private IObservable<string> AsynchronousSubscribe(Action<IObserver<string>> action, IScheduler scheduler)
{
	return Observable.Create<string>(
	(IObserver<string> observer) =>
	{
		return scheduler.Schedule(observer,
			(_, state) =>
				{
					action(state);
					return Disposable.Create(() => Console.WriteLine("Non-blocking observer has unsubscribed"));
				});
	});
}