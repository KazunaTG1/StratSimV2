using QFin.Securities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace StratSimV2
{
	public partial class WebForm2 : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (IsPostBack)
			{
				if (Session["Equity"] is List<double> equity)
					Equity = equity;
				if (Session["Payouts"] is List<double> payouts)
					Payouts = payouts;
				if (Session["PayoutPercs"] is List<double> payoutPercs)
					PayoutPercs = payoutPercs;
			}
			else
			{
				NewSession();
				Equity = new List<double>();
				double sBal = double.Parse(tbBal.Text);
				Equity.Add(sBal);
				Session["Equity"] = Equity;
				oiPut.Type = OptionType.Put;
			}
		}
		private void NewSession()
		{
			Session["Equity"] = new List<double>();
			Session["Payouts"] = new List<double>();
			Session["PayoutPercs"] = new List<double>();
		}

		protected void btnTrade_Click(object sender, EventArgs e)
		{
			double totalCost = oiCall.MarketPrice + oiPut.MarketPrice;
			if (GetCallContracts()*totalCost*100 > CurrBal)
			{
				lblError.Text = $"Not enough equity";
				return;
			}
			lblError.Text = "";
			SetCallOption(out var prices, out double callPl, out double callExit);
			SetPutOption(prices, out double putPL, out double putExitPrice);
			SetStock(prices);
			
			double PL = putPL + callPl;
			double exit = putExitPrice + callExit;
			
			double equity = CurrBal + PL;
			PayoutPercs.Add(((double)exit / totalCost)-1);
			Payouts.Add(PL);
			Equity.Add(equity);
			lbPayouts.Items.Add(PL.ToString("C"));
			SaveToSession();
			SetEquity();
		}
		private void SaveToSession()
		{
			Session["Equity"] = Equity;
			Session["Payouts"] = Payouts;
			Session["PayoutPercs"] = PayoutPercs;
		}
		private void SetEquity()
		{
			chartEquity.Series.Clear();
			lblEquity.Text = GetEquityString();
			chartEquity.AddIndicator("Y", Equity.First(), Color.Gray);
			chartEquity.AddSeries(Equity);
			chartEquity.XLabel("Trades");
			chartEquity.YLabel("Equity ($)");
			chartEquity.ChartAreas[0].AxisX.LabelStyle.Format = "n0";
		}
		private void SetStock(List<StockPrice> prices)
		{
			var sbStockRes = new StringBuilder();
			sbStockRes.Append(Format.LabelMoney("Entry Price", prices.First().Price));
			lblStockResult.Text = sbStockRes.ToString();
			chartStock.Series.Clear();
			chartStock.AddSeries(prices, "Stock Price");
			chartStock.XLabel("Date");
			chartStock.YLabel("Price");
			chartStock.AddIndicator("Y", oiCall.Option.Strike, Color.Yellow);
			chartStock.AddIndicator("Y", oiCall.Option.Strike, Color.Yellow);
			chartStock.AddIndicator("Y", oiPut.Option.Strike, Color.Yellow);
			chartStock.AddIndicator("Y", oiCall.Option.StockPrice, Color.Gray);
		}
		private void SetCallOption(out List<StockPrice> prices, out double pl, out double exit)
		{
			chartCall.Series.Clear();
			exit = oiCall.Option.TradeOption(oiOptionSim.TakeProfit, oiOptionSim.StopLoss, oiOptionSim.CloseDTE,
				out prices, out var optionPrices, out var status);
			int contracts = GetCallContracts();
			pl = (exit - oiCall.Option.MarketPrice) * 100 * contracts;
			double callPlRatio = (exit / oiCall.Option.MarketPrice) - 1;
			if (oiOptionSim.Direction == QFin.Models.OptionPricing.TradeDirection.Short)
				pl *= -1;
			lblCallRes.Text = Format.GetSimLabel( contracts,
				oiCall.MarketPrice, exit, pl, callPlRatio, 
				oiCall.Expiration, oiOptionSim.TakeProfit, oiOptionSim.CloseDTE);
			double callTpPrice = (oiCall.Option.MarketPrice * (1 + oiOptionSim.TakeProfit));
			if (callTpPrice > 0)
				chartCall.AddIndicator("Y", callTpPrice, Color.MediumSeaGreen);
			chartCall.AddSeries(optionPrices, "Call Price");
			chartCall.XLabel("Date");
			chartCall.YLabel("Price");
			chartCall.AddIndicator("Y", oiCall.MarketPrice, Color.Gray);
			chartCall.AddIndicator("X", oiCall.Option.Expiration.AddDays(-oiOptionSim.CloseDTE).ToOADate(), Color.Yellow);
		}
		private int GetPutContracts()
		{
			return oiPut.Option.GetContracts(oiOptionSim.QuantityType, oiOptionSim.Quantity, CurrBal);
		}
		private int GetCallContracts()
		{
			return oiCall.Option.GetContracts(oiOptionSim.QuantityType, oiOptionSim.Quantity, CurrBal);
		}
		private void SetPutOption(List<StockPrice> prices, out double pl, out double exit)
		{
			chartPut.Series.Clear();
			exit = oiPut.Option.TradeOption(
				oiOptionSim.TakeProfit, oiOptionSim.StopLoss, oiOptionSim.CloseDTE, prices, out var putPrices, out var status);

			int contracts = GetPutContracts();
			pl = (exit - oiPut.Option.MarketPrice) * 100 * contracts;
			double plRatio = (exit / oiPut.Option.MarketPrice) - 1;
			if (oiOptionSim.Direction == QFin.Models.OptionPricing.TradeDirection.Short)
				pl *= -1;
			lblPutRes.Text = Format.GetSimLabel( contracts,
				oiPut.MarketPrice, exit, pl, plRatio, oiPut.Expiration, 
				oiOptionSim.TakeProfit, oiOptionSim.CloseDTE);
			chartPut.AddSeries(putPrices, "Put Price");
			double putTpPrice = (oiPut.Option.MarketPrice * (1 + oiOptionSim.TakeProfit));
			if (putTpPrice > 0)
				chartPut.AddIndicator("Y", putTpPrice, Color.MediumSeaGreen);
			chartPut.AddIndicator("X", oiPut.Option.Expiration.AddDays(-oiOptionSim.CloseDTE).ToOADate(), Color.Yellow);
			chartPut.AddIndicator("Y", oiPut.MarketPrice, Color.Gray);
			chartPut.XLabel("Date");
			chartPut.YLabel("Price");

		}
		private string GetEquityString()
		{
			var sbEquity = new StringBuilder();
			double totalPL = CurrBal - Equity.First();
			string color = totalPL > 0 ? "#22ab94" : "#f23645";
			sbEquity.Append(Format.LabelMoney("Starting Balance", Equity.First()));
			sbEquity.Append(Format.LabelMoney("Equity", CurrBal));
			sbEquity.Append(Format.Label("Trades", Equity.Count - 1));
			sbEquity.Append(Format.LabelPercent("Win Rate", (double)Payouts.Count(x => x > 0) / Payouts.Count()));
			sbEquity.Append(Format.LabelMoney("PL", totalPL, (CurrBal / Equity.First()) - 1, color));
			sbEquity.Append(Format.LabelMoney("Avg PL", Payouts.Average(), PayoutPercs.Average(), color));
			return sbEquity.ToString();
		}
		public List<double> Equity { get; set; } = new List<double>() { 100 };
		public List<double> Payouts { get; set; } = new List<double>();
		public List<double> PayoutPercs { get; set; } = new List<double>();
		private Color upColor = Color.FromArgb(34, 171, 148);
		private Color downColor = Color.FromArgb(242, 54, 69);
		public double CurrBal
		{
			get
			{
				return Equity.Last();
			}
		}

		protected void btnReset_Click(object sender, EventArgs e)
		{
			lblError.Text = "";
			lblAvgPL.Text = "";
			lblPutRes.Text = "";
			lblCallRes.Text = "";
			Payouts.Clear();
			PayoutPercs.Clear();
			lbPayouts.Items.Clear();
			Equity.Clear();
			double sBal = double.Parse(tbBal.Text);
			Equity.Add(sBal);
			lblEquity.Text = $"<b>Equity: </b>{CurrBal:C}";
		}

		protected void timerTrade_Tick(object sender, EventArgs e)
		{
			btnTrade_Click(sender, e);
		}

		protected void btnAutoTrade_CheckedChanged(object sender, EventArgs e)
		{
			timerTrade.Enabled = btnAutoTrade.Checked;
			btnTrade_Click(sender, e);
		}

		protected void tbTickSpeed_TextChanged(object sender, EventArgs e)
		{
			int speed = int.Parse(tbTickSpeed.Text);
			timerTrade.Interval = speed * 1000;
		}
	}
}