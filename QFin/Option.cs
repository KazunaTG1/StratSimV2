using MathNet.Numerics;
using MathNet.Numerics.RootFinding;
using QFin.MarketData.Transformations;
using QFin.Models.OptionPricing;
using QFin.Models.Stochastic;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QFin
{
	namespace Securities
	{
		public enum ExitStatus
		{
			TakeProfit,
			StopLoss,
			CloseDTE,
			Expiration
		}
		public enum OptionType
		{
			Put,
			Call
		}
		public class Option : Stock
		{
			public Dictionary<double, double> GetThetaMap(out Dictionary<double, double> prices, out Dictionary<double, double> rates)
			{
				prices = new Dictionary<double, double>();
				rates = new Dictionary<double, double> ();
				var result = new Dictionary<double, double>();
				double dte = (Expiration - DateTime.Now).TotalDays;
				for (double d = dte; d > 1; d--)
				{
					if (d < 0) continue;
					double newPrice = BlackScholes.OptionPrice(Type, StockPrice, Strike, d / 365, InterestRate, ImpliedVolatility);
					if (newPrice <= 0.01)
						continue;
					prices.Add(d.Round(0), newPrice);
					result.Add(d.Round(0), BlackScholes.OptionTheta(Type, StockPrice, Strike, d, InterestRate, ImpliedVolatility));
					rates.Add(d.Round(0), BlackScholes.OptionThetaRate(Type, StockPrice, Strike, d, InterestRate, ImpliedVolatility));
					
					
				}
				return result;
			}
			public Dictionary<double, double> GetDeltaMap(int scale, out Dictionary<double, double> prices, out Dictionary<double, double> rates)
			{
				rates = new Dictionary<double, double>();
				prices = new Dictionary<double, double>();
				var result = new Dictionary<double, double>();
				double limit = StockPrice * ((scale / 10.0) + 1);
				for (int i = 0; i <= limit; i++)
				{
					double newPrice = BlackScholes.OptionPrice(Type, i, Strike, YearsToExpiry, InterestRate, ImpliedVolatility);
					if (newPrice <= 0.01)
						continue;
					prices.Add(i, newPrice);
					rates.Add(i, BlackScholes.OptionDeltaRate(Type, i, Strike, YearsToExpiry, InterestRate, ImpliedVolatility));
					result.Add(i, BlackScholes.OptionDelta(Type, i, Strike, YearsToExpiry, InterestRate, ImpliedVolatility));
				}
				return result;
			}
			public Dictionary<double, double> GetGammaMap(int scale, out Dictionary<double, double> prices, out Dictionary<double, double> rates)
			{
				rates = new Dictionary<double, double>();
				prices = new Dictionary<double, double>();
				var result = new Dictionary<double, double>();
				double limit = StockPrice * ((scale / 10.0) + 1);
				for (int i = 0; i <= limit; i++)
				{
					double newPrice = BlackScholes.OptionPrice(Type, i, Strike, YearsToExpiry, InterestRate, ImpliedVolatility);
					if (newPrice <= 0.01)
						continue;
					prices.Add(i, newPrice);
					rates.Add(i, BlackScholes.OptionGammaRate(Type, i, Strike, YearsToExpiry, InterestRate, ImpliedVolatility));
					result.Add(i, BlackScholes.OptionGamma(Type, i, Strike, YearsToExpiry, InterestRate, ImpliedVolatility));
					
				}
				return result;
			}
			public Dictionary<double, double> GetRhoMap(int scale, out Dictionary<double, double> prices, out Dictionary<double, double> rates)
			{
				rates = new Dictionary<double, double>();
				prices = new Dictionary<double, double>();
				var result = new Dictionary<double, double>();
				double limit = 10 * scale;
				for (int r = 0; r < limit; r++)
				{
					double rate = (double)r / 100;
					double newPrice = BlackScholes.OptionPrice(Type, StockPrice, Strike, YearsToExpiry, rate, ImpliedVolatility);
					if (newPrice <= 0.01)
						continue;
					prices.Add(r, newPrice);
					rates.Add(r, BlackScholes.OptionRhoRate(Type, StockPrice, Strike, YearsToExpiry, rate, ImpliedVolatility));
					result.Add(r, BlackScholes.OptionRho(Type, StockPrice, Strike, YearsToExpiry, rate, ImpliedVolatility));
				}
				return result;
			}
			public Dictionary<double, double> GetVegaMap(int scale, out Dictionary<double, double> prices, out Dictionary<double, double> rates)
			{
				rates = new Dictionary<double, double>();
				prices = new Dictionary<double, double>();
				var result = new Dictionary<double, double>();
				double limit = 10 * scale;
				for (int v = 0; v < limit; v++)
				{
					double vol = (double)v / 100;
					double newPrice = BlackScholes.OptionPrice(Type, StockPrice, Strike, YearsToExpiry, InterestRate, vol);
					if (newPrice <= 0.01)
						continue;
					prices.Add(v, newPrice);
					rates.Add(v, BlackScholes.OptionVegaRate(Type, StockPrice, Strike, YearsToExpiry, InterestRate, vol));
					result.Add(v, BlackScholes.OptionVega(Type, StockPrice, Strike, YearsToExpiry, InterestRate, vol));
				}
				return result;
			}
			
			public double FindWinProbability( out double tpProb, out double slProb, out double dteProb, out double expProb, double tp = -1, double sl = -1, int closeDTE = -1)
			{
				int simCount = 10_000;
				int totalWins = 0, totalTP = 0, totalSL = 0, totalDTE = 0, totalEXP = 0;
				Parallel.For(0, simCount,
					() => (wins: 0, tps:0,sls:0,dtes:0,exps:0),
					(i,loop,local) =>
				{
					double localTP, localSL, localDTE, localEXP;
					var exit = TradeOption(TradeDirection.Long, tp, sl, closeDTE, out var prices, out var optionPrices, out var status);
					double pl = exit.Price - Price;
					
					if (pl > 0) local.wins++;

					switch (status)
					{
						case ExitStatus.TakeProfit: local.tps++; break;
						case ExitStatus.StopLoss: local.sls++; break;
						case ExitStatus.CloseDTE: local.dtes++; break;
						case ExitStatus.Expiration: local.exps++; break;
					}
					return local;
				},
				local =>
				{
					Interlocked.Add(ref totalWins, local.wins);
					Interlocked.Add(ref totalTP, local.tps);
					Interlocked.Add(ref totalSL, local.sls);
					Interlocked.Add(ref totalDTE, local.dtes);
					Interlocked.Add(ref totalEXP, local.exps);
				});
				tpProb = (double)totalTP / simCount;
				slProb = (double)totalSL / simCount;
				dteProb = (double)totalDTE / simCount;
				expProb = (double)totalEXP / simCount;
				return (double)totalWins / simCount;
			}
			public static Color GetExitColor(ExitStatus exitStatus)
			{
				switch (exitStatus)
				{
					case ExitStatus.TakeProfit:
						return Color.LightGreen;
					case ExitStatus.StopLoss:
						return Color.MistyRose;
					case ExitStatus.CloseDTE:
						return Color.Gold;
					case ExitStatus.Expiration:
						return Color.PeachPuff;
					default:
						return Color.LightBlue;
				}
			}
			public int GetContracts(string quantityType, double quantity, double currBal)
			{
				int contracts = 1;
				switch (quantityType)
				{
					case "% of Balance":
						contracts = (int)Math.Max(1, Math.Round((currBal * quantity) / (MarketPrice * 100), 0));
						break;
					case "$ USD":
						contracts = (int)Math.Max(1, Math.Floor((quantity / (MarketPrice * 100))));
						break;
					case "# of Contracts":
						contracts = (int)Math.Max(1, quantity);
						break;
				}
				return contracts;
			}
			#region Methods
			public Dictionary<double, double> GetPriceMap(int percLength)
			{
				return GetMap(percLength, GetOptionPrice);
			}
			public Dictionary<double, double> GetExpiredPriceMap(int percLength)
			{
				return GetMap(percLength, GetExpiredOptionPrice);
			}
			public Dictionary<double, double> GetMap(int percLength, Func<double, double> func)
			{
				var mapPrices = new Dictionary<double, double>
				{
					{ StockPrice, func(StockPrice) }
				};
				int c = 1;
				while (StockPrice - c > 0)
				{
					double uPrice = StockPrice + c;
					double lPrice = StockPrice - c;
					mapPrices.Add(uPrice, func(uPrice).Round(5));
					mapPrices.Add(lPrice, func(lPrice).Round(5));
					c++;
				}
				return mapPrices.OrderBy(x => x.Key)
					.Where(x => x.Key > StockPrice - (StockPrice / percLength) &&
					x.Key < StockPrice + (StockPrice / percLength))
					.ToDictionary(x => x.Key, x => x.Value);
			}
			public Dictionary<double, double> GetDtePriceMap(int closeDTE, int percLength)
			{
				DateTime date = Expiration.AddDays(-closeDTE);
				var mapPrices = new Dictionary<double, double>
				{
					{ StockPrice, GetPriceAtDate(StockPrice, date) }
				};
				int c = 1;
				while (StockPrice - c > 0)
				{
					double uPrice = StockPrice + c;
					double lPrice = StockPrice - c;
					mapPrices.Add(uPrice, GetPriceAtDate(uPrice, date).Round(5));
					mapPrices.Add(lPrice, GetPriceAtDate(lPrice, date).Round(5));
					c++;
				}
				return mapPrices.OrderBy(x => x.Key)
					.Where(x => x.Key > StockPrice - (StockPrice / percLength) &&
					x.Key < StockPrice + (StockPrice / percLength))
					.ToDictionary(x => x.Key, x => x.Value);
			}
			public Dictionary<double, double> GetDtePlMap(int closeDTE, TradeDirection direction, int percLength, int quantity)
			{
				var mapPL = new Dictionary<double, double>();
				foreach (var pair in GetDtePriceMap(closeDTE, percLength))
				{
					double pl = (pair.Value - Price) * 100 * quantity;
					if (direction == TradeDirection.Short)
						pl *= -1;
					mapPL.Add(pair.Key, pl.Round(5));
				}
				return mapPL.OrderBy(x => x.Key)
					.ToDictionary(x => x.Key, x => x.Value);
			}
			public Dictionary<double, double> GetPLMap(TradeDirection direction, int percLength, double tp = -1, double sl = -1, int quantity = 1)
			{
				var mapPL = new Dictionary<double, double>();
				foreach (var pair in GetPriceMap(percLength))
				{
					double pl = (pair.Value - Price) * 100 * quantity;
					if (tp != -1)
					{
						double tTp = (1+tp);
						if (direction == TradeDirection.Short)
						{
							tTp = 1-tp;
						}
						double tpPrice = Price * tTp;
						if (direction == TradeDirection.Long && pair.Value > tpPrice)
						{
							pl = (tpPrice-Price) * 100 * quantity;
						}
						if (direction == TradeDirection.Short && pair.Value < tpPrice)
						{
							pl = (tpPrice - Price) * 100 * quantity;
						}
					}
					if (sl != -1)
					{
						double tSl = sl;
						if (direction == TradeDirection.Short)
						{
							tSl += 1;
						}
						double slPrice = Price * tSl;
						if (direction == TradeDirection.Long && pair.Value < slPrice)
						{
							pl = (slPrice-Price) * 100 * quantity;
						}
						if (direction == TradeDirection.Short && pair.Value > slPrice)
						{
							pl = (slPrice - Price) * 100 * quantity;
						}
					}
					if (direction == TradeDirection.Short)
						pl *= -1;
					mapPL.Add(pair.Key, pl.Round(5));
				}
				return mapPL.OrderBy(x => x.Key)
					.ToDictionary(x => x.Key, x => x.Value);
			}
			private double GetExpiredPL(TradeDirection direction, double s0, int quantity, out double price)
			{
				price = GetExpiredOptionPrice(s0).Round(5);
				double pl = (price - Price) * 100 * quantity;
				if (direction == TradeDirection.Short)
					pl *= -1;
				return pl;
			}
			private double GetPriceAtDate(double s0, DateTime date)
			{
				double years = (double)(Expiration - date).TotalDays / 365;
				return BlackScholes.OptionPrice(Type, s0, Strike, years, InterestRate, ImpliedVolatility);
			}
			public Dictionary<double, double> GetExpiredPLMap(TradeDirection direction, int percLength, int quantity = 1)
			{
				var mapPL = new Dictionary<double, double>() 
				{ { StockPrice, GetExpiredPL(direction, StockPrice, quantity, out double opPrice) } };
				int c = 1;
				while (StockPrice - c > 0)
				{
					double uPrice = StockPrice + c;
					double lPrice = StockPrice - c;
					double uexPay = GetExpiredPL(direction, uPrice, quantity, out double uOpPrice);
					double lexPay = GetExpiredPL(direction, lPrice, quantity, out double lOpPrice);
					
					
					mapPL.Add(uPrice, uexPay);
					mapPL.Add(lPrice, lexPay);
					c++;
				}
				return mapPL.OrderBy(x => x.Key)
					.Where(x => x.Key > StockPrice - (StockPrice / percLength) &&
					x.Key < StockPrice + (StockPrice / percLength))
					.ToDictionary(x => x.Key, x => x.Value);
			}
			public StockPrice TradeOption(TradeDirection direction, double tp, double sl, int closeDTE, 
				out List<StockPrice> prices, out List<StockPrice> optionPrices, out ExitStatus exitStatus, Interval interval = Interval.Day)
			{
				prices = GeometricBrownianMotion.GBM(StockPrice, InterestRate, YearsToExpiry, ImpliedVolatility, DateTime.Now, interval);
				return TradeOption(direction, tp, sl, closeDTE, prices, out optionPrices, out exitStatus);
			}
			/// <summary>
			/// Trades an option
			/// </summary>
			/// <param name="tp"></param>
			/// <param name="sl"></param>
			/// <param name="closeDTE"></param>
			/// <param name="prices"></param>
			/// <param name="optionPrices"></param>
			/// <returns>The exit price of the option</returns>
			public StockPrice TradeOption(TradeDirection direction, double tp, double sl, int closeDTE, List<StockPrice> prices, out List<StockPrice> optionPrices, out ExitStatus status)
			{
				optionPrices = prices.Select(x => new StockPrice(x.Timestamp, GetOptionPrice(x.Price, x.Timestamp))).ToList();
				for (int i = 0; i < prices.Count; i++)
				{
					StockPrice price = optionPrices[i];

					double perc = (price.Price / MarketPrice) - 1;
					if (direction == TradeDirection.Long)
					{
						if (perc > tp && tp != -1)
						{
							status = ExitStatus.TakeProfit;
							return price;
						}
						if (perc < -sl && sl != -1)
						{
							status = ExitStatus.StopLoss;
							return price;
						}
					}
					else
					{
						if (perc < -tp && tp != -1)
						{
							status = ExitStatus.TakeProfit;
							return price;
						}
						if (perc > sl && sl != -1)
						{
							status = ExitStatus.StopLoss;
							return price;
						}
					}
					
					if ((Expiration - price.Timestamp).TotalDays < closeDTE && closeDTE != -1)
					{
						status = ExitStatus.CloseDTE;
						return price;
					}
				}
				status = ExitStatus.Expiration;
				return new StockPrice(Expiration, IntrinsicPrice(prices));
			}
			public double IntrinsicPrice(List<StockPrice> prices)
			{
				switch (Type)
				{
					case OptionType.Call:
						return Math.Max((prices.Last().Price - Strike), 0);
					case OptionType.Put:
						return Math.Max((Strike - prices.Last().Price), 0);
				}
				return 0;

			}
			#endregion
			#region Constructors
			public Option(string symbol, OptionType type, DateTime expiry, double strike, double stockPrice, double marketPrice, double iv, double interest)
			{
				Symbol = symbol;
				Type = type;
				Strike = strike;
				StockPrice = stockPrice;
				ImpliedVolatility = iv;
				Expiration = expiry;
				InterestRate = interest;
				if (marketPrice == 0)
				{
					MarketPrice = marketPrice;
				}
				MarketPrice = marketPrice == 0 ? Price : marketPrice;
				
			}
			public Option(OptionDTO opDto)
			{
				Symbol = opDto.Symbol;
				Type = opDto.Type;
				Strike = opDto.Strike;
				StockPrice = opDto.StockPrice;
				ImpliedVolatility = opDto.ImpliedVolatility;
				Expiration = opDto.Expiration;
				InterestRate = opDto.InterestRate;
				MarketPrice = opDto.MarketPrice;
			}
			#endregion
			public double Strike { get; set; }
			public OptionType Type { get; set; }
			public double ImpliedVolatility { get; set; }
			#region Expiration
			public DateTime Expiration { get; set; }
			public double DaysToExpiry
			{
				get
				{
					return (Expiration - DateTime.Now).TotalDays;
				}
			}
			public double YearsToExpiry
			{
				get
				{
					return DaysToExpiry / 365;
				}
			}
			#endregion
			public double InterestRate { get; set; }
			#region Pricing
			public double Price
			{
				get
				{
					return GetOptionPrice(StockPrice);
				}
			}
			public double MarketValue { get
				{
					return MarketPrice * 100;
				} }
			public double Edge
			{
				get
				{
					return Price - MarketPrice;
				}
			}
			public double EdgeRatio
			{
				get
				{
					return (Price / MarketPrice) - 1;
				}
			}
			public double GetOptionPrice(double price)
			{
				switch (Type)
				{
					case OptionType.Call:
						return BlackScholes.CallPrice(price, Strike, YearsToExpiry, InterestRate, ImpliedVolatility);
					case OptionType.Put:
						return BlackScholes.PutPrice(price, Strike, YearsToExpiry, InterestRate, ImpliedVolatility);
					default: return 0;
				}
			}
			public double GetOptionPrice(double price, DateTime date)
			{
				double t = (Expiration - date).TotalDays / 365.0;
				switch (Type)
				{
					case OptionType.Call:
						return BlackScholes.CallPrice(price, Strike, t, InterestRate, ImpliedVolatility);
					case OptionType.Put:
						return BlackScholes.PutPrice(price, Strike, t, InterestRate, ImpliedVolatility);
					default: return 0;
				}
			}
			public double GetExpiredOptionPrice(double price)
			{
				switch (Type)
				{
					case OptionType.Call:
						return BlackScholes.CallPrice(price, Strike, 0, InterestRate, ImpliedVolatility);
					case OptionType.Put:
						return BlackScholes.PutPrice(price, Strike, 0, InterestRate, ImpliedVolatility);
					default: return 0;
				}
			}
			public Dictionary<double, double> GetPricesByUnderlying()
			{
				Dictionary<double, double> pl = new Dictionary<double, double>();
				int length = (int)Price / 4,
					mid = length / 2;
				for (int i = -mid; i < mid; i++)
				{
					double currPrice = Price + i;
					pl.Add(currPrice, Math.Round(GetOptionPrice(currPrice), 3));
				}
				return pl;
			}
			public Dictionary<double, double> GetCurrentPL(double quantity, TradeDirection dir)
			{
				switch (dir)
				{
					case TradeDirection.Long:
						return GetPL(quantity, GetOptionPrice);
					case TradeDirection.Short:
						return GetPL(-quantity, GetOptionPrice);
				}
				return GetPL(quantity, GetOptionPrice);
			}
			private Dictionary<double, double> GetPL(double quantity, Func<double, double> optionPrice)
			{
				Dictionary<double, double> pl = new Dictionary<double, double>();
				int length = (int)(Price / 5),
					mid = length / 2;
				for (int i = -mid; i < mid; i++)
				{
					double currPrice = Price + i,
						entry = Price * 100 * quantity,
						value = Math.Round(optionPrice(currPrice), 5) * 100 * quantity;
					pl[currPrice] = value - entry;
				}
				return pl;
			}
			public Dictionary<double, double> GetExpiredPL(double quantity, TradeDirection dir)
			{
				switch (dir)
				{
					case TradeDirection.Long:
						return GetPL(quantity, GetExpiredOptionPrice);
					case TradeDirection.Short:
						return GetPL(-quantity, GetExpiredOptionPrice);
				}
				return null;
			}
			public override string ToString()
			{
				return $"{Type}{Expiration:d}{Strike:N2}";
			}
			public double MarketPrice { get; set; } = 0;
			public double Breakeven
			{
				get
				{
					return Brent.FindRoot(s => (GetExpiredOptionPrice(s) - Price), 0, 10000);
				}
			}
			#endregion
			public string Status
			{
				get
				{
					switch (Type)
					{
						case OptionType.Call:
							if (Strike - 1 < Price && Strike + 1 > Price)
								return "ATM";
							else if (Strike > Price)
								return "OTM";
							else if (Strike < Price)
								return "ITM";
							break;
						case OptionType.Put:
							if (Strike - 1 < Price && Strike + 1 > Price)
								return "ATM";
							else if (Strike < Price)
								return "OTM";
							else if (Strike > Price)
								return "ITM";
							break;
					}
					return "NULL";
				}

			}
			public string ToLongString()
			{
				return $"{Type} {Expiration:d} @ {Strike:C}";
			}
		}
		[Serializable]
		public class OptionDTO
		{
			public OptionDTO(Option option, int quantity)
			{
				Symbol = option.Symbol;
				Type = option.Type;
				Strike = option.Strike;
				StockPrice = option.StockPrice;
				ImpliedVolatility = option.ImpliedVolatility;
				Expiration = option.Expiration;
				InterestRate = option.InterestRate;
				MarketPrice = option.MarketPrice;
				TheorPrice = option.Price;
				Quantity = quantity;
			}
			public string Symbol { get; set; }
			public OptionType Type { get; set; }
			public double Strike { get; set; }
			public double StockPrice { get; set; }
			public double ImpliedVolatility { get; set; }
			public DateTime Expiration { get; set; }
			public double InterestRate { get; set; }
			public double MarketPrice { get; set; }
			public double TheorPrice { get; set; }
			public int Quantity { get; set; }
		}
	}
}
