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
	Func<CancellationToken, Task<TaskDescriptor>> asyncFunc = async token =>
	{
		count++;
		Console.WriteLine("Task {0} starting.", count);
		var sleepDuration = r.Next(1, 1000);
		await Task.Delay(TimeSpan.FromMilliseconds(sleepDuration), token);
		
		return new TaskDescriptor{Count=count, Duration=sleepDuration, ThreadId=Thread.CurrentThread.ManagedThreadId};
	};
	
	Observable.FromAsync(asyncFunc)
		.Do(x => Console.WriteLine("	Task {0} completed.", x.Count))
		.Timeout(TimeSpan.FromMilliseconds(500))
		.Retry(3)
		.Dump();
}

public class TaskDescriptor
{
	public int Count { get; set; }
	public int Duration {get; set; }
	public int ThreadId { get; set; }
}