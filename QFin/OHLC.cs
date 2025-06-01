using MathNet.Numerics.Interpolation;
using QFin.Securities;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFin
{
	namespace MarketData
	{
		namespace Transformations
		{
			public enum Interval
			{
				Minutes15,
				Minutes30,
				Hour,
				Hours2,
				Hours4,
				Day,
				Days2,
				Days3,
				Week,
				Weeks2,
				Month,
				Months2,
				Months4,
				Months6,
				Year
			}
			public class ChartInterval
			{
				public static Dictionary<Interval, string> IntervalStrings
				{
					get => new Dictionary<Interval, string>()
						{
							{ Interval.Minutes15, "Minutes 15" },
							{ Interval.Minutes30, "Minutes 30" },
							{ Interval.Hour, "Hour" },
							{ Interval.Hours2, "2 Hours" },
							{ Interval.Hours4, "4 Hours" },
							{ Interval.Day, "Day" },
							{ Interval.Days2, "2 Days" },
							{ Interval.Days3, "3 Days" },
							{ Interval.Week, "Week" },
							{ Interval.Weeks2, "2 Weeks" },
							{ Interval.Month, "Month" },
							{ Interval.Months2, "2 Months" },
							{ Interval.Months4, "4 Months" },
							{ Interval.Months6, "6 Months" },
							{ Interval.Year, "Year" }
						};
				}
				public static int GetSteps(Interval interval, int days)
				{
					if (days <= 0) return 0;
					switch (interval)
					{
						case Interval.Minutes15:
							return (days * 24 * 60) / 15;
						case Interval.Minutes30:
							return (days * 24 * 60) / 30;
						case Interval.Hour:
							return days * 24;
						case Interval.Hours2:
							return (days * 24) / 2;
						case Interval.Hours4:
							return (days * 24) / 4;
						case Interval.Day:
							return days;
						case Interval.Days2:
							return days / 2;
						case Interval.Days3:
							return days / 3;
						case Interval.Week:
							return (int)(days / 7.0);
						case Interval.Weeks2:
							return (int)(days / 14.0) ;
						case Interval.Month:
							return (int)(days / 30.0);
						case Interval.Months2:
							return (int)(days / 60.0);
						case Interval.Months4:
							return (int)(days / 12.0);
						case Interval.Months6:
							return (int)(days /180.0);
						case Interval.Year:
							return (int)(days / 365.0);
						default:
							return 10;
					}
				}
			}
			public class OHLC
			{
				public DateTime IntervalStart { get; set; }
				public double Open { get; set; }
				public double High { get; set; }
				public double Low { get; set; }
				public double Close { get; set; }
			}
			public class OHLCBuilder
			{
				public static List<OHLC> GetOHLC(List<StockPrice> prices, TimeSpan interval)
				{
					return prices
						.OrderBy(p => p.Timestamp)
						.GroupBy(p => new DateTime((p.Timestamp.Ticks / interval.Ticks) * interval.Ticks))
						.Select(g =>
						{
							var list = g.OrderBy(p => p.Timestamp).ToList();
							return new OHLC
							{
								IntervalStart = g.Key,
								Open = list.First().Price,
								High = list.Max(p => p.Price),
								Low = list.Min(p => p.Price),
								Close = list.Last().Price
							};
						})
						.ToList();
				}
			}
		}
	}
}
