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
	Observable.Return(4).Dump("Observable.Return");
	
	Observable.Empty<int>().Dump("Observable.Empty");
	
	Observable.Never<int>().Dump("Observable.Never");
	
	Observable.Throw<int>(new Exception("Ah, snap!")).Dump("Observable.Throw");
}