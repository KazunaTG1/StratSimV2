using QFin.Models.OptionPricing;
using QFin.Securities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFin
{
	[Serializable]
	public class Trade
	{
		public Trade() { }
		public Trade(StockPath path, bool isOption, double quantity, TradeDirection direction)
		{
			Path = path;
			IsOption = isOption;
			Quantity = quantity;
			if (direction == TradeDirection.Short) Quantity *= -1;
			if (isOption) Quantity *= 100;
			Direction = direction;
		}
		public double PL => GetPL(Path.StartPrice, Path.EndPrice);
		public double PlPerc => GetPlPerc(Path.StartPrice, Path.EndPrice);
		public double NetCashFlow => GetNetCashFlow();
		public double CapitalAtRisk => GetCapitalAtRisk();
		public TradeDirection Direction { get; set; }
		public StockPath Path { get; set; }
		public bool IsOption { get; set; }
		public List<StockPrice> PathPL
		{
			get
			{

				return Path.Path.Select(x => new StockPrice(x.Timestamp, GetPL(Path.StartPrice, x.Price))).ToList();
			}
		}
		public List<AmountPoint> PathPLPerc
		{
			get
			{
				return Path.Path.Select(x => new AmountPoint(x.Timestamp, GetPlPerc(Path.StartPrice, x.Price))).ToList();
			}
		}
		private double GetCapitalAtRisk()
		{
			return Math.Abs(Quantity * Path.StartPrice);
		}
		private double GetNetCashFlow()
		{
			return Path.StartPrice * -Quantity;
		}
		private double GetPlPerc(double startPrice, double currPrice)
		{
			double perc = (currPrice / startPrice) - 1;
			if (Direction == TradeDirection.Short) perc *= -1;
			return perc; 
		}
		private double GetPL(double startPrice, double currPrice)
		{
			double pl = (currPrice - startPrice) * Quantity;
			return pl;
		}
		public double Quantity { get; set; }
	}
}
