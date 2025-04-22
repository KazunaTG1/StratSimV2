
using MathNet.Numerics;
using QFin.Securities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.EnterpriseServices;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace StratSimV2
{
	public partial class WebForm1 : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (IsPostBack)
			{
				if (Session["Equity"] is List<double> equity)
					Equity = equity;
				if (Session["Payouts"] is List<double> payouts)
					Payouts = payouts;
			}
			else
			{
				Session["Equity"] = new List<double>();
				Session["Payouts"] = new List<double>();
				Equity = new List<double>();
				double sBal = double.Parse(tbBal.Text);
				Equity.Add(sBal);
				Session["Equity"] = Equity;
			}
		}

		public List<double> Equity { get; set; }
		public List<double> Payouts { get; set; } = new List<double>();
		private Color upColor = Color.FromArgb(34, 171, 148);
		private Color downColor = Color.FromArgb(242, 54, 69);
		private int GetContracts()
		{
			return oiOption.Option.GetContracts(oiOptionSim.QuantityType, oiOptionSim.Quantity, CurrBal);
		}
		protected void btnTrade_Click(object sender, EventArgs e)
		{
			if (GetContracts() * oiOption.Option.MarketPrice * 100 > CurrBal)
			{
				lblError.Text = "Not enough Equity";
				return;
			}
			lblError.Text = "";
			chartStock.Series.Clear();
			SetOption(out double pl, out double exit, out var prices, out var optionPrices);
			double equity = CurrBal + pl;
			
			Equity.Add(equity);
			
			Session["Equity"] = Equity;
			
			
			var sbStockRes = new StringBuilder();
			sbStockRes.Append(Format.LabelMoney("Entry Price", prices.First().Price));
			if (optionPrices.Any(x => x.Price == exit))
			{
				int exitIndex = optionPrices.IndexOf(optionPrices.First(x => x.Price == exit));
				sbStockRes.Append(Format.LabelMoney("Close Price", prices[exitIndex].Price));
			}
			lblStockResult.Text = sbStockRes.ToString();
			double totalPL = CurrBal - Equity.First();
			string color = totalPL > 0 ? "#22ab94" : "#f23645";
			var sbEquity = new StringBuilder(); 
			sbEquity.Append(Format.LabelMoney("Starting Balance", Equity.First()));
			sbEquity.Append(Format.LabelMoney("Equity", CurrBal));
			sbEquity.Append(Format.Label("Trades", Equity.Count - 1));
			sbEquity.Append(Format.LabelMoney("PL", totalPL, (CurrBal / Equity.First())-1, color));
			sbEquity.Append(Format.LabelPercent("Win Rate", (double)Payouts.Count(x => x > 0) / Payouts.Count()));
			sbEquity.Append(Format.LabelMoney("Avg PL", Payouts.Average()));
			lblEquity.Text = sbEquity.ToString();
			
			double tpPrice = (oiOption.Option.MarketPrice * (1 + oiOptionSim.TakeProfit));
			if (tpPrice > 0)
				chartOption.AddIndicator("Y", tpPrice, upColor);
			
			chartStock.AddIndicator("Y", oiOption.Option.Strike, Color.Gold);
			chartEquity.AddIndicator("Y", Equity.First(), Color.Gray);
			chartStock.AddIndicator("Y", oiOption.Option.StockPrice, Color.Gray);
			
			chartStock.AddSeries(prices, "Stock Price");
			chartStock.XLabel("Date (MM/yy)");
			chartStock.YLabel("Stock Price");
			chartEquity.Series.Clear();
			chartEquity.AddSeries(Equity);
			chartEquity.XLabel("Trades");
			chartEquity.YLabel("Equity");
			chartEquity.ChartAreas[0].AxisX.LabelStyle.Format = "n0";
		}
		private void SetOption(out double pl, out double exit, out List<StockPrice> prices, out List<StockPrice> optionPrices)
		{
			chartOption.Series.Clear();
			exit = oiOption.Option.TradeOption(oiOptionSim.TakeProfit, oiOptionSim.StopLoss, oiOptionSim.CloseDTE, 
				out prices, out optionPrices, out var status);
			int contracts = oiOption.Option.GetContracts(
				oiOptionSim.QuantityType, oiOptionSim.Quantity, CurrBal);
			pl = (exit - oiOption.Option.MarketPrice) * 100 * contracts;
			double plRatio = (exit / oiOption.Option.MarketPrice) - 1;
			if (oiOptionSim.Direction == QFin.Models.OptionPricing.TradeDirection.Short)
				pl *= -1;
			Payouts.Add(pl);
			lbPayouts.Items.Add(pl.ToString("C"));
			Session["Payouts"] = Payouts;
			lblResult.Text = Format.GetSimLabel( contracts,
				oiOption.MarketPrice, exit, pl, plRatio, 
				oiOption.Expiration, oiOptionSim.TakeProfit, oiOptionSim.CloseDTE);
			chartOption.AddSeries(optionPrices, "Option Price");
			chartOption.XLabel("Date (MM/yy)");
			chartOption.YLabel("Option Price");
			chartOption.AddIndicator("Y", oiOption.MarketPrice, Color.Gray);
			chartOption.AddIndicator("X", oiOption.Option.Expiration.AddDays(-oiOptionSim.CloseDTE).ToOADate(), Color.Gold);
		}

		protected void btnReset_Click(object sender, EventArgs e)
		{
			lblError.Text = "";
			lblAvgPL.Text = "";
			lblResult.Text = "";
			Payouts.Clear();
			lbPayouts.Items.Clear();
			Equity.Clear();
			double sBal = double.Parse(tbBal.Text);
			Equity.Add(sBal);
			lblEquity.Text = $"<b>Equity: </b>{CurrBal:C}";
			lblEquity.Text = Format.LabelMoney("Equity", CurrBal);
		}
		public double CurrBal
		{
			get
			{
				return Equity.Last();
			}
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