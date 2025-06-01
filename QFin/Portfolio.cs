using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using QFin.Securities;
namespace QFin
{
	public class Portfolio
	{
		public Portfolio(List<Trade> trades) => Trades = trades;
		public Portfolio() { }
		public List<Trade> Trades { get; set; }
		public double PL => FinalPL - StartPL;
		public double FinalPL => PathPL.Last().Price;
		public double StartPL => PathPL.First().Price;
		public double PercPL => ((FinalPL - CapitalAtRisk) / CapitalAtRisk)+1;
		public double CapitalAtRisk => GetCapitalAtRisk();
		public double GetCapitalAtRisk() => Trades.Sum(x => x.CapitalAtRisk);
		public List<StockPrice> PathPL
		{
			get
			{
				var trades = Trades.OrderBy(x => x.PathPL.Count).ToList();
				List<StockPrice> results = new List<StockPrice>();
				for (int i = 0; i < trades[0].PathPL.Count; i++)
				{
					DateTime timestamp = trades[0].PathPL[i].Timestamp;
					double accum = 0;
					StockPrice price = new StockPrice();
					for (int j = 0; j < trades.Count; j++)
					{
						var temp = trades[j].PathPL[i];
						accum += temp.Price;
					}
					results.Add(new StockPrice(timestamp, accum));
				}
				return results;
			}
		}
	}
}
