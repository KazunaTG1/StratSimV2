using QFin.MarketData.Transformations;
using QFin.Models.Stochastic;
using QFin.Securities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StratSimV2
{
	public enum OutputType
	{
		Paths,
		MinMaxGainLoss,
		MinMax,
		AvgGainLoss,
		Probabilities
	}
	public partial class WebForm7 : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				ddlInterval.DataSource = Enum.GetNames(typeof(Interval));
				ddlInterval.DataBind();
				ddlInterval.SelectedIndex = 1;

				ddlOutputType.DataSource = Enum.GetNames(typeof(OutputType));
				ddlOutputType.DataBind();
			}
		}
		public Interval Interval => (Interval)ddlInterval.SelectedIndex;
		public OutputType OutputType => (OutputType)ddlOutputType.SelectedIndex;
		protected void btnSimulate_Click(object sender, EventArgs e)
		{
			chartStock.Series.Clear();
			double price = double.Parse(tbStock.Text);
			int days = int.Parse(tbDays.Text);
			double years = (double)days / 365;
			double callIV = double.Parse(tbCallIV.Text) / 100;
			double putIV = double.Parse(tbPutIV.Text) / 100;
			double interest = double.Parse(tbInterest.Text) / 100;

			int sims = int.Parse(tbSimulations.Text);
			int steps = ChartInterval.GetSteps(Interval, days);
			var paths = new Dictionary<DateTime, List<double>>();
			for (int s = 0; s < sims; s++)
			{
				var prices = GeometricBrownianMotion.AsymmetricGBM(price, interest, putIV, callIV, years, steps);
				if (OutputType == OutputType.Paths)
					chartStock.AddSeries(prices, s, true);
				foreach (var pr in prices)
				{
					if (!paths.ContainsKey(pr.Timestamp.Date))
					{
						paths.Add(pr.Timestamp.Date, new List<double>());
					}
					paths[pr.Timestamp.Date].Add(pr.Price);
				}
				
			}
			List<double> probs = new List<double>();
			
			var maxes = new List<StockPrice>();
			var mins = new List<StockPrice>();
			var avgs = new List<StockPrice>();
			var avgGains =new List<StockPrice>();
			var avgLosses = new List<StockPrice>();
			foreach (var p in paths)
			{
				double avg = p.Value.Average();
				maxes.Add(new StockPrice(p.Key, p.Value.Max()));
				mins.Add(new StockPrice(p.Key, p.Value.Min()));
				avgs.Add(new StockPrice(p.Key, avg));
				var gains = p.Value.Where(x => x > avg);
				var losses = p.Value.Where(x => x < avg);
				double avgG = gains.Count() == 0 ? avg : gains.Average();
				double avgL = losses.Count() == 0 ? avg : losses.Average();
				avgGains.Add(new StockPrice(p.Key, avgG));
				avgLosses.Add(new StockPrice(p.Key, avgL));	
			}
			
			switch (OutputType)
			{
				case OutputType.MinMax:
					PlotMinMax(maxes, mins, avgs); break;
				case OutputType.AvgGainLoss:
					PlotAvgGainLoss(avgGains, avgLosses, avgs); break;
				case OutputType.MinMaxGainLoss:
					PlotMinMaxGainLoss(maxes, mins, avgs, avgGains, avgLosses);
					break;
			}

			
			chartFreq.AddHistogram(paths.Last().Value, 30);
		}
		private void PlotMinMax(List<StockPrice> maxes, List<StockPrice> mins, List<StockPrice> avgs)
		{
			chartStock.AddSeries(maxes, "Maxes", true);
			chartStock.AddSeries(mins, "Mins", true);
			chartStock.AddSeries(avgs, "Averages", true);
		}
		private void PlotMinMaxGainLoss(List<StockPrice> maxes,
			List<StockPrice> mins, List<StockPrice> avgs, List<StockPrice> avgGains, 
			List<StockPrice> avgLosses)
		{
			chartStock.AddSeries(maxes, "Maxes", true);
			chartStock.AddSeries(mins, "Mins", true);
			chartStock.AddSeries(avgs, "Averages", true); 
			chartStock.AddSeries(avgLosses, "Losses", true);
			chartStock.AddSeries(avgGains, "Gains", true);
		}
		
		private void PlotAvgGainLoss(List<StockPrice> avgGains, List<StockPrice> avgLosses, List<StockPrice> avgs)
		{
			chartStock.AddSeries(avgGains, "Gains", true);
			chartStock.AddSeries(avgs, "Averages", true);
			chartStock.AddSeries(avgLosses, "Losses", true);
		}
	}
}