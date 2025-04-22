using QFin.Securities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QFin.Models.OptionPricing;
using Microsoft.Win32;
using System.Text;
using System.Runtime.InteropServices;

namespace StratSimV2
{
	public partial class WebForm4 : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (IsPostBack)
				Calculate(sender, e);
		}

		protected void btnAddOption_Click(object sender, EventArgs e)
		{
		}

		protected void btnCalculate_Click(object sender, EventArgs e)
		{
			
		}
		protected void Calculate(object sender, EventArgs e)
		{
			contracts = oiOption.Option.GetContracts(oiOptionSim.QuantityType, oiOptionSim.Quantity, 500);
			percLength = int.Parse(tbScale.Text);
			SetChartPL();
			SetChartPrices();
		}
		private int percLength = 10;
		private int contracts = 1;
		private void SetChartPL()
		{
			var allPls = oiOption.Option.GetPLMap(oiOptionSim.Direction, 1, oiOptionSim.TakeProfit, oiOptionSim.StopLoss, contracts);
			var allEPLS = oiOption.Option.GetExpiredPLMap(oiOptionSim.Direction, 1, contracts);
			var sb = new StringBuilder();
			sb.Append(Format.LabelMoney("Breakeven", oiOption.Option.Breakeven));
			double maxGain = allPls.Max(x => x.Value);
			double maxLoss = allPls.Min(x => x.Value);
			if (oiOptionSim.TakeProfit == -1 && oiOptionSim.Direction == TradeDirection.Long)
				maxGain = double.PositiveInfinity;
			if (oiOptionSim.StopLoss == -1 && oiOptionSim.Direction == TradeDirection.Short)
				maxLoss = double.NegativeInfinity;
			sb.Append(Format.LabelMoney("Max Gain", maxGain, "#22ab94"));
			sb.Append(Format.LabelMoney("Max Loss", maxLoss, "#f23645"));
			if (oiOptionSim.TakeProfit != -1)
			{
				double tpStock = allPls.First(x => x.Value == maxGain).Key;
				if (oiOption.Type == OptionType.Put && oiOptionSim.Direction == TradeDirection.Long 
					|| oiOption.Type == OptionType.Call && oiOptionSim.Direction == TradeDirection.Short)
				{
					tpStock = allPls.Last(x => x.Value == maxGain).Key;
				}
				sb.Append(Format.LabelMoney("Stock Price @ Take Profit", tpStock, "#22ab94"));
			}
			if (oiOptionSim.StopLoss != -1)
			{
				double slStock = allPls.Last(x => x.Value == maxLoss).Key;
				if (oiOption.Type == OptionType.Put && oiOptionSim.Direction == TradeDirection.Long || 
					oiOption.Type == OptionType.Call && oiOptionSim.Direction == TradeDirection.Short)
				{
					slStock = allPls.First(x => x.Value == maxLoss).Key;
				}
				sb.Append(Format.LabelMoney("Stock Price @ Stop Loss", slStock, "#f23645"));
			}
			
			
			lblResPL.Text = sb.ToString();


			chartPL.Series.Clear();
			var pls = oiOption.Option.GetPLMap(oiOptionSim.Direction, percLength, oiOptionSim.TakeProfit, oiOptionSim.StopLoss, contracts);
			var ePls = oiOption.Option.GetExpiredPLMap(oiOptionSim.Direction, percLength, contracts);
			chartPL.AddSeries(ePls, ChartUtil.UpColor, "Expired");
			chartPL.AddSeries(pls, Color.PowderBlue, "Current DTE");
			if (oiOptionSim.CloseDTE != -1)
			{
				var cdtePrices = oiOption.Option.GetDtePlMap(oiOptionSim.CloseDTE, oiOptionSim.Direction, percLength, contracts);
				chartPL.AddSeries(cdtePrices, Color.LavenderBlush, "Close DTE");
			}
			chartPL.AddIndicator("X", oiOption.Strike, Color.Gold);
			chartPL.AddIndicator("X", oiOption.StockPrice, Color.DarkGray);
			chartPL.AddIndicator("Y", 0, Color.DarkGray);
			chartPL.YLabel("Profit / Loss");
			chartPL.XLabel("Stock Price");

			
		}
		private void SetChartPrices()
		{
			
			chartPrices.Series.Clear();
			var prices = oiOption.Option.GetPriceMap(percLength);
			var ePrices = oiOption.Option.GetExpiredPriceMap(percLength);
			
			chartPrices.AddSeries(prices, Color.PowderBlue, "Current DTE");
			
			chartPrices.AddSeries(ePrices, ChartUtil.UpColor, "Expired");
			chartPrices.AddIndicator("X", oiOption.Strike, Color.Gold);
			chartPrices.AddIndicator("Y", oiOption.Option.Price, Color.DarkGray);
			chartPrices.AddIndicator("X", oiOption.StockPrice, Color.DarkGray);
			var sb = new StringBuilder();
			if (oiOptionSim.TakeProfit != -1)
			{
				double tp = (oiOptionSim.TakeProfit + 1);
				if (oiOptionSim.Direction == TradeDirection.Short)
				{
					tp = (2 - tp);
				}
				double tpPrice = tp * oiOption.Option.Price;

				chartPrices.AddIndicator("Y", tpPrice, Color.MediumSeaGreen);
				sb.Append(Format.LabelMoney("Option Price @ Take Profit", tpPrice, "#22ab94"));
			}
			if (oiOptionSim.StopLoss != -1)
			{
				double sl = (oiOptionSim.StopLoss);
				if (oiOptionSim.Direction == TradeDirection.Short)
				{
					sl += 1;
				}
				double slPrice = sl * oiOption.Option.Price;
				chartPrices.AddIndicator("Y", slPrice, Color.Pink);
				sb.Append(Format.LabelMoney("Option Price @ Stop Loss", slPrice, "#f23645"));
			}
			if (oiOptionSim.CloseDTE != -1)
			{
				var cdtePrices = oiOption.Option.GetDtePriceMap(oiOptionSim.CloseDTE, percLength);
				chartPrices.AddSeries(cdtePrices, Color.LavenderBlush, "Close DTE");
			}
			if (cbFindProbabilities.Checked)
			{
				double winRate = oiOption.Option.FindWinProbability(
				out double tpProb, out double slProb, out double dteProb, out double expProb,
				oiOptionSim.TakeProfit, oiOptionSim.StopLoss, oiOptionSim.CloseDTE);
				sb.Append(Format.LabelPercent("Probability of Profit", winRate));
				if (oiOptionSim.TakeProfit != -1)
					sb.Append(Format.LabelPercent("Probability of Take Profit", tpProb));
				if (oiOptionSim.StopLoss != -1)
					sb.Append(Format.LabelPercent("Probability of Stop Loss", slProb));
				if (oiOptionSim.CloseDTE != -1)
					sb.Append(Format.LabelPercent("Probability of Close DTE", dteProb));
				sb.Append(Format.LabelPercent("Probability of Expiring", expProb));
			}
			
			lblResPrices.Text = sb.ToString();
			chartPrices.YLabel("Option Price");
			chartPrices.XLabel("Stock Price");
		}
	}
}