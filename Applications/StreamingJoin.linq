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
	var rand = new Random();
	var trades = Observable.Interval(TimeSpan.FromSeconds(1)).Select(_ => new Trade(){Symbol = "IBM", Amount = rand.Next(0, 100)});
	trades.Dump("Trades");

	var rand2 = new Random();
	var prices = Observable.Interval(TimeSpan.FromMilliseconds(700)).Select(_ => new Price(){Symbol = "IBM", Value = rand2.Next(0, 1000)/100m})
						   .Publish()
						   .RefCount()
						   ;
	prices.Dump("Prices");

	var positions = trades.Scan(new Position(), (current, next) => new Position() {Symbol = next.Symbol, Amount = current.Amount + next.Amount})
						  .Publish()
						  .RefCount()
						  ;
	positions.Dump("Positions");

	var rows = positions.CombineLatest(prices, (pos, price) => new {Symbol = pos.Symbol, Amount = pos.Amount, Price = price.Value, MarketValue = pos.Amount * price.Value});
	rows.Dump("Rows");

	/*
	var rows2 = positions.CombineLatest(prices, (pos, price) => new {Symbol = pos.Symbol, Amount = pos.Amount, Price = price.Value});
	rows2.Dump("Rows2");	

	var calcService = new CalculationService();
	var rows3 = positions.CombineLatest(prices, (pos, price) => new {Symbol = pos.Symbol, Amount = pos.Amount, Price = price.Value, CalculatedFields = calcService.KnownCalculationsEnumerable.Select(c => c.Calculate(pos.Amount, price.Value))});
	rows3.Dump("Rows3");	
	*/
}


public class CalculationService
{
	public Dictionary<string, ICalculation> KnownCalculations { get; private set; }
	
	public IEnumerable<ICalculation> KnownCalculationsEnumerable
	{
		get
		{
			return KnownCalculations.Values;
		}
	}

	public CalculationService()
	{
		KnownCalculations = new Dictionary<string, ICalculation>();
		KnownCalculations.Add(typeof(MarketValue).Name, new MarketValue());
		KnownCalculations.Add(typeof(LastPrice).Name, new LastPrice());
	}
	
	
	public CalculationResult Calculate (string calcName, decimal input1, decimal input2)
	{
		var calc = KnownCalculations[calcName];
		return calc.Calculate(input1, input2);
	}
}

public class CalculationResult
{
	public string Name{get;set;}
	public decimal Value{get;set;}
	
	public override string ToString()
	{
		return Name + " = " + Value.ToString(); 
	}
}

public interface ICalculation
{
	CalculationResult Calculate(decimal input1, decimal input2);
}

public class MarketValue : ICalculation
{
	public CalculationResult Calculate(decimal amount, decimal price)
	{
		return new CalculationResult(){Name = GetType().Name, Value = amount * price};
	}
}

public class LastPrice : ICalculation
{
	public CalculationResult Calculate(decimal amount, decimal price)
	{
		return new CalculationResult(){Name = GetType().Name, Value = price};
	}
}


// Define other methods and classes here
public class Trade
{
	public string Symbol { get; set; }
	public decimal Amount { get; set; }
}

public class Position
{
	public string Symbol { get; set; }
	public decimal Amount { get; set; }
}


public class Price
{
	public string Symbol { get; set; }
	public decimal Value { get; set; }
}