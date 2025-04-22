using QFin.Securities;
using System;
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
