using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFin
{
	namespace Securities
	{
		public readonly struct StockPrice
		{
			public DateTime Timestamp { get; }
			public double Price { get; }

			public StockPrice(DateTime timestamp, double price)
			{
				Timestamp = timestamp;
				Price = price;
			}

			public override string ToString() => $"{Price:C} @ {Timestamp:f}";
		}
		public class Stock
		{
			public string Exchange { get; set; }
			public string Symbol { get; set; }
			public string Name { get; set; }
			public double StockPrice { get; set; }
		}
	}
	
}
