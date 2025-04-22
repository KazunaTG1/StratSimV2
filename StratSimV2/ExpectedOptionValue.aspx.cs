using MathNet.Numerics;
using QFin.Securities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace StratSimV2
{
	public partial class WebForm3 : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

        protected void btnSimulate_Click(object sender, EventArgs e)
        {
			
			chartOption.ChartAreas[0].AxisY.IsLogarithmic = cbLog.Checked;
			int trades = int.Parse(tbTrades.Text);
			double sBal = double.Parse(tbSBal.Text);
			if (oiOption.Option.GetContracts(
				oiOptionSim.QuantityType, oiOptionSim.Quantity, sBal) * oiOption.Option.MarketValue > sBal)
			{
				lblError.Text = "Not enough equity.";
				return;
			}
			double currBal = sBal;
			List<double> equity = new List<double>() { sBal };
			List<double> payouts = new List<double>();
			List<double> payRatios = new List<double>();
			List<double> exits = new List<double>();
			for (int i = 0; i < trades; i++)
			{
				double exitPrice = oiOption.Option.TradeOption(
					oiOptionSim.TakeProfit, oiOptionSim.StopLoss, oiOptionSim.CloseDTE,
					out var prices, out var optionPrices, out var status);
				int contracts = oiOption.Option.GetContracts(oiOptionSim.QuantityType, oiOptionSim.Quantity, currBal);
				double cost = contracts * oiOption.Option.MarketValue;
				chartOption.AddSeries(optionPrices, i+1);
				chartStock.AddSeries(prices, i + 1);
				double pl = (exitPrice - oiOption.Option.MarketPrice) * 100 * contracts,
					plRatio = (exitPrice / oiOption.Option.MarketPrice) - 1;
				if (cost > currBal)
				{
					break;
				}
				exits.Add(exitPrice);
				payouts.Add(pl);
				payRatios.Add(plRatio);
				if (oiOptionSim.Direction == QFin.Models.OptionPricing.TradeDirection.Short)
					pl *= -1;
				currBal += pl;
				equity.Add(currBal);
			}
			chartEquity.AddSeries(equity);
			double tpPrice = (oiOption.Option.MarketPrice * (1 + oiOptionSim.TakeProfit));
			if (tpPrice > 0 && !cbLog.Checked)
				chartOption.AddIndicator("Y", tpPrice, upColor);
			chartStock.AddIndicator("Y", oiOption.Option.Strike, Color.Gold);
			chartEquity.AddIndicator("Y", sBal, Color.Gray);
			chartStock.AddIndicator("Y", oiOption.Option.StockPrice, Color.Gray);
			if (!cbLog.Checked)
				chartOption.AddIndicator("Y", oiOption.Option.MarketPrice, Color.Gray);
			SetResult(payouts, payRatios);
			SetEquity(equity, sBal);
		}
		private void SetResult(List<double> payouts, List<double> payRatios)
		{
			var sbRes = new StringBuilder();
			double avgpl = payouts.Average();
			string color = avgpl > 0 ? "#22ab94" : "#f23645";
			double avgplRatio = payRatios.Average();
			double winrate = (double)payouts.Count(x => x > 0) / payouts.Count();
			sbRes.Append(Format.LabelMoney("Avg PL", avgpl, avgplRatio, color));
			sbRes.Append(Format.LabelPercent("Win Rate", winrate));
			lblResult.Text = sbRes.ToString();
		}
		private void SetEquity(List<double> equity, double sBal)
		{
			var sbEquity = new StringBuilder();
			double totalPL = (equity.Last() - sBal);
			double totalPLRatio = (equity.Last() / sBal) - 1;
			string color = totalPL > 0 ? "#22ab94" : "#f23645";
			sbEquity.Append(Format.LabelMoney("Starting Balance", sBal));
			sbEquity.Append(Format.LabelMoney("Final Bal", equity.Last()));
			sbEquity.Append(Format.LabelMoney("Total PL", totalPL, totalPLRatio, color));
			lblEquity.Text = sbEquity.ToString();
			chartEquity.ChartAreas[0].AxisX.LabelStyle.Format = "n0";
		}

		public List<double> Equity { get; set; }
		public List<double> Payouts { get; set; } = new List<double>();
		private Color upColor = Color.FromArgb(34, 171, 148);
		private Color downColor = Color.FromArgb(242, 54, 69);
		
		protected void cbLog_CheckedChanged(object sender, EventArgs e)
		{
			chartOption.ChartAreas[0].AxisY.IsLogarithmic = cbLog.Checked;
		}
	}
}