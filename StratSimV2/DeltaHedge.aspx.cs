using QFin.Securities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QFin.Models.OptionPricing;
using QFin.MarketData.Transformations;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;
using QFin;
using MathNet.Numerics;
using System.Text;

namespace StratSimV2
{
	public partial class WebForm16 : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}
		private void Simulate()
		{
			oiOption.Option.TradeOption(TradeDirection.Short, -1, -1, -1,
				out var stockPath, out var optionPath, out var exitStatus);
			double finalOptionPrice = oiOption.Option.GetExpiredOptionPrice(stockPath.Last().Price);
			optionPath.Add(new StockPrice(oiOption.Expiration, finalOptionPrice));
			chartOption.AddSeries(optionPath, "Option", true);
			chartOption.AddIndicator("Y", oiOption.Option.MarketPrice, Color.DarkGray);

			double ratio = double.Parse(tbRatio.Text);

			var deltaPath = new List<AmountPoint>();
			var positionSizes = new List<AmountPoint>();
			var equityCurve = new List<StockPrice>();
			var optionPl = new List<AmountPoint>();
			var stockVals = new List<AmountPoint>();
			var optionVals = new List<AmountPoint>();
			var stockChanges = new List<AmountPoint>();
			var cashPath = new List<AmountPoint>();

			double initDelta = BlackScholes.OptionDelta(
				oiOption.Type, oiOption.StockPrice, oiOption.Strike, oiOption.Option.YearsToExpiry,
				oiOption.InterestRate, oiOption.ImpliedVolatility);
			double stockPosition = initDelta * ratio;
			positionSizes.Add(new AmountPoint(stockPath[0].Timestamp, stockPosition));

			double initStockValue = stockPosition * oiOption.StockPrice;
			double cash = oiOption.Option.MarketValue - initStockValue;
			cashPath.Add(new AmountPoint(DateTime.Now, cash));
			chartStock.AddSeries(stockPath, "Stock", true);
			chartStock.AddIndicator("Y", oiOption.StockPrice, Color.DarkGray);
			chartStock.AddIndicator("Y", oiOption.Strike, Color.Gold);
			chartStock.XLabel("Date");
			chartStock.YLabel("Stock Price");

			int count = Math.Min(optionPath.Count, stockPath.Count);

			for (int i = 0; i < count; i++)
			{
				var optionPricePoint = optionPath[i];
				var stockPricePoint = stockPath[i];

				double newT = (oiOption.Option.Expiration - optionPricePoint.Timestamp).TotalDays / 365.0;
				double stockPrice = stockPricePoint.Price;

				double delta = BlackScholes.OptionDelta(
					oiOption.Type, stockPrice, oiOption.Strike, newT,
					oiOption.InterestRate, oiOption.ImpliedVolatility);

				deltaPath.Add(new AmountPoint(optionPricePoint.Timestamp, delta));

				double targetPosition = delta * ratio;
				double diff = targetPosition - stockPosition;
				double change = diff * stockPrice;
				stockChanges.Add(new AmountPoint(optionPricePoint.Timestamp, change));
				cash -= change;
				cashPath.Add(new AmountPoint(optionPricePoint.Timestamp, cash));
				stockPosition = targetPosition;

				positionSizes.Add(new AmountPoint(optionPricePoint.Timestamp, stockPosition));

				double stockValue = stockPosition * stockPrice;
				double optionValue = optionPricePoint.Price * 100.0;
				optionVals.Add(new AmountPoint(optionPricePoint.Timestamp, optionValue));
				double portfolioPL =  (oiOption.Option.MarketValue-optionValue) - initStockValue;

				optionPl.Add(new AmountPoint(optionPricePoint.Timestamp, oiOption.Option.MarketValue - optionValue));
				stockVals.Add(new AmountPoint(optionPricePoint.Timestamp, stockValue));
				equityCurve.Add(new StockPrice(optionPricePoint.Timestamp, portfolioPL));
			}

			
			// Final hedge unwind at expiry
			double finalPrice = stockPath.Last().Price;
			double finalStockValue = stockPosition * finalPrice;
			cash += finalStockValue;
			cashPath.Add(new AmountPoint(oiOption.Expiration, cash));
			stockPosition = 0;
			positionSizes.Add(new AmountPoint(oiOption.Expiration, stockPosition));

			// ===== DISPLAY P/L =====

			var sbPL = new StringBuilder();
			string color = equityCurve.Last().Price > 0 ? "green" : "red";
			double finalEquity = cash - finalOptionPrice * 100 + (finalStockValue - initStockValue);
			equityCurve.Add(new StockPrice(oiOption.Expiration, finalEquity));
			double totalTradeValue = (oiOption.MarketPrice * 100) + initStockValue;
			double tPL = equityCurve.Last().Price + totalTradeValue;
			double plPerc = (equityCurve.Last().Price / totalTradeValue) ;
			sbPL.Append(Format.LabelMoney("Trade Value", totalTradeValue));
			sbPL.Append(Format.LabelMoney("Market Value", tPL));
			sbPL.Append(Format.LabelMoney("Total P/L", equityCurve.Last().Price, plPerc, color));
			lblPL.Text = sbPL.ToString();

			if (cbTotalPL.Checked)
				chartPL.AddSeries(equityCurve, "Total P/L", true);
			if (cbStockVal.Checked)
				chartPL.AddSeries(stockVals, Color.Orange, "Stock Value", AxisType.Primary, true);
			if (cbOptionPL.Checked)
				chartPL.AddSeries(optionPl, Color.MediumPurple, "Option P/L", AxisType.Primary, true);
			if (cbCash.Checked)
				chartPL.AddSeries(cashPath, Color.Gold, "Cash", AxisType.Primary, true);
			if (cbOptionVal.Checked)
				chartPL.AddSeries(optionVals, Color.DodgerBlue, "Option Values", AxisType.Primary, true);
			chartPL.AddLegend();
			chartPL.AddIndicator("Y", 0, Color.DarkGray);

			chartOption.AddSeries(deltaPath, Color.CadetBlue, "Delta Path", AxisType.Secondary, true);
			chartOption.XLabel("Date");
			chartOption.YLabel("Option Price");
			chartOption.Y2Label("Delta", Color.CadetBlue);

			chartStock.AddSeries(positionSizes, Color.CadetBlue, "Position Size", AxisType.Secondary, true);
			chartStock.Y2Label("Position Size", Color.CadetBlue);

			var sbOption = new StringBuilder();
			double optionPL = (finalOptionPrice - optionPath.First().Price) * 100.0;
			double changePerc = (finalOptionPrice / optionPath.First().Price) - 1.0;
			sbOption.Append(Format.LabelMoney("Entry Price", oiOption.Option.MarketPrice));
			sbOption.Append(Format.LabelMoney("Exit Price", finalOptionPrice));
			sbOption.Append(Format.LabelMoney("Change", finalOptionPrice - oiOption.Option.MarketPrice, changePerc));
			sbOption.Append(Format.LabelMoney("P/L", -optionPL, -changePerc));
			lblOptionResult.Text = sbOption.ToString();

			var sbStock = new StringBuilder();
			sbStock.Append(Format.LabelMoney("Entry Price", oiOption.StockPrice));
			sbStock.Append(Format.LabelMoney("Exit Price", finalPrice));
			sbStock.Append(Format.LabelMoney("Change", finalPrice - oiOption.StockPrice, (finalPrice / oiOption.StockPrice) - 1.0));
			lblStockRes.Text = sbStock.ToString();
		}


		protected void btnSimulate_Click(object sender, EventArgs e)
		{
			Simulate();
		}
	}
}