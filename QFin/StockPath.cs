using MathNet.Numerics.Financial;
using MathNet.Numerics.Statistics;
using QFin.MarketData.Transformations;
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
	public class StockPath
	{
		public StockPath()
		{
			Path = new List<StockPrice>();
			Interval = Interval.Day;
		}
		public StockPath(List<StockPrice> path, Interval interval)
		{
			Path = path;
			Interval = interval;
		}
		public List<double> LogReturns => 
			Path.Select(x => x.Price)
			.Zip(Path.Select(x => x.Price).Skip(1), (prev, next) => Math.Log(next / prev))
			.ToList();
		public List<StockPrice> Path { get; set; }
		public Interval Interval { get; set; }
		public StockPrice Start => Path.First();
		public StockPrice End => Path.Last();
		public double StartPrice => Start.Price;
		public double EndPrice => End.Price;
		public DateTime StartDate => Start.Timestamp;
		public DateTime EndDate => End.Timestamp;
		public TimeSpan TimeSpan => (End.Timestamp - Start.Timestamp);
		public double Years => TimeSpan.TotalDays / 365.0;
		public double Change => End.Price - Start.Price;
		public double ChangePercent => (End.Price / Start.Price) - 1;
		public double GainLossRatio => LogReturns.GainLossRatio();
		public double Max => Path.Max(x => x.Price);
		public double Min => Path.Min(x => x.Price);
		public double StandardDeviation => LogReturns.StandardDeviation();
		public double MeanStandardDeviation => LogReturns.MeanStandardDeviation().StandardDeviation;
		public double Variance => LogReturns.Variance();
		public double MeanVariance => LogReturns.MeanVariance().Variance;
		public double Volatility => StandardDeviation * Math.Sqrt(GetStepsPerYear());
		public double MeanVolatility => MeanVariance * Math.Sqrt(GetStepsPerYear());
		public int GetStepsPerYear()
		{
			return ChartInterval.GetSteps(Interval, 252);
		}
	}
}
