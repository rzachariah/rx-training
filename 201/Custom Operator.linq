<Query Kind="Program">
  <NuGetReference Version="2.2.2">Rx-Core</NuGetReference>
  <NuGetReference Version="2.2.2">Rx-Interfaces</NuGetReference>
  <NuGetReference Version="2.2.2">Rx-Linq</NuGetReference>
  <NuGetReference Version="2.2.2">Rx-Main</NuGetReference>
  <NuGetReference Version="2.2.2">Rx-PlatformServices</NuGetReference>
  <NuGetReference Version="2.2.2">Rx-Xaml</NuGetReference>
  <Namespace>System</Namespace>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Disposables</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
</Query>

void Main()
{
	Observable.Interval(TimeSpan.FromMilliseconds(200))
		.MySelect(x => x*1000)
		.Dump();
}

public static class ObservableEx
{
	// Note: there is no reason to reimplement Select. This is just for demonstration.
	public static IObservable<TResult> MySelect<TSource, TResult>(this IObservable<TSource> source, Func<TSource, TResult> selector)
	{
		return Observable.Create<TResult>(
			observer => source.Subscribe(x =>
			{
				TResult result;
				try
				{
					result = selector(x);
				}
				catch (Exception exception)
				{
					observer.OnError(exception);
					return;
				}
				observer.OnNext(result);
			},
			observer.OnError,
			observer.OnCompleted));
			}
}