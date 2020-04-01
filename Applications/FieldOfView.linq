<Query Kind="Program">
  <NuGetReference Version="2.2.2">Rx-Core</NuGetReference>
  <NuGetReference Version="2.2.2">Rx-Interfaces</NuGetReference>
  <NuGetReference Version="2.2.2">Rx-Linq</NuGetReference>
  <NuGetReference Version="2.2.2">Rx-Main</NuGetReference>
  <NuGetReference Version="2.2.2">Rx-PlatformServices</NuGetReference>
  <NuGetReference Version="2.2.2">Rx-Xaml</NuGetReference>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
  <Namespace>System.Reactive.Subjects</Namespace>
</Query>

void Main()
{
	IObservable<Security> securities = SecurityMaster.Instance.GetSecurityFeed();
	Observable.Return(securities.ToEnumerable().ToDisplay()).DumpLive("Universe of securities");
	var view = new View(SecurityMaster.Instance);
	view.VisibleSecurities.Select(l => l.ToDisplay()).DumpLive("Securities of interest");
	// view query
	securities.SelectMany(s => view.InView(s)
		     	                   .SelectMany(_ => s.PriceFeed.TakeUntil(view.OutOfView(s)))
			                       .DumpLive())
		.Dump();
}
public class Security : IComparable<Security>
{
public string Name { get; private set; }
public IObservable<Price> PriceFeed { get { return PriceRepository.Instance.GetPriceFeed(Name); } }
public Security(string name)
{
Name = name;
}
public override string ToString()
{
return Name;
}
public int CompareTo(Security other)
{
return Name.CompareTo(other.Name);
}
public override bool Equals(Object obj)
{
// If parameter is null return false.
if (obj == null)
{
return false;
}
// If parameter cannot be cast to Point return false.
Security s = obj as Security;
if ((System.Object)s == null)
{
return false;
}
// Return true if the fields match:
return s.Name == Name;
}
public override int GetHashCode()
{
return Name.GetHashCode();
}
public static Security Create(string name)
{
return new Security(name);
} 
}
public static class SecurityExtensions
{
public static string ToDisplay(this IEnumerable<Security> securities)
{
string output = "[";
foreach (Security s in securities)
{
output += s + ", ";
}
output = output.TrimEnd(',', ' ');
output += "]";
return output;
}
public static IEnumerable<Security> CreateSecurities(params string[] list)
{
foreach (string name in list)
{
yield return Security.Create(name);
}
} 
}
public class Price
{
public string Symbol { get; private set; }
public decimal Value { get; private set; }
public Price(string symbol, decimal value)
{
Symbol = symbol;
Value = value;
}
public override string ToString()
{
return string.Format("{0}: {1}", Symbol, Value);
}
}
public class SecurityMaster
{
private static SecurityMaster instance = new SecurityMaster();
private Random rand = new Random();
private Security[] securities = new Security[]{new Security("IBM"), new Security("CSCO"), new Security("AAPL"), new Security("GOOG"), new Security("FB")};
public static SecurityMaster Instance { get { return instance; } }
public IEnumerable<Security> GetSecurityList()
{
var list = securities.ToList();
list.Sort();
return list;
}
public IObservable<Security> GetSecurityFeed()
{
return GetSecurityList().ToObservable();
}
public Security GetRandomSecurity()
{
var securityList = new List<Security>(securities);
return securityList[rand.Next(securityList.Count)];
} 
}
public class PriceRepository
{
private static PriceRepository instance = new PriceRepository();
Random rand = new Random();
public static PriceRepository Instance { get { return instance; } }
public IObservable<Price> GetPriceFeed(string symbol)
{
return Observable.Interval(TimeSpan.FromMilliseconds(50)).Select(i => GetRandomPrice(symbol));
}
private Price GetRandomPrice(string symbol)
{
return new Price(
symbol,
rand.Next(10000)/100m
);
}
}
public class View
{
private readonly SecurityMaster securityMaster;
public View(SecurityMaster securityMaster)
{
this.securityMaster = securityMaster;
}
public IObservable<IEnumerable<Security>> VisibleSecurities
{
get
{
var t0List = SecurityExtensions.CreateSecurities("AAPL","CSCO", "FB", "GOOG","IBM");
var seq0 = Observable.Timer(TimeSpan.FromMilliseconds(1)).Select(_ => t0List);
var t1List = SecurityExtensions.CreateSecurities("AAPL", "CSCO", "FB");
var seq1 = Observable.Timer(TimeSpan.FromSeconds(2)).Select(_ => t1List);
var t2List = SecurityExtensions.CreateSecurities("AAPL");
var seq2 = Observable.Timer(TimeSpan.FromSeconds(3)).Select(_ => t2List);
var t3List = SecurityExtensions.CreateSecurities("");
var seq3 = Observable.Timer(TimeSpan.FromSeconds(4)).Select(_ => t3List);
return seq0.Concat(seq1).Concat(seq2).Concat(seq3).Publish().RefCount();
}
}
public IObservable<bool> IsSecurityVisible(Security security)
{
return VisibleSecurities.Select(l => l.Contains(security)).DistinctUntilChanged();
}
public IObservable<Unit> InView(Security security)
{
return IsSecurityVisible(security).Where(b => b).Select(b => Unit.Default);
}
public IObservable<Unit> OutOfView(Security security)
{
return IsSecurityVisible(security).Where(b => !b).Select(b => Unit.Default);
}
}

