using QFin.Models.OptionPricing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace StratSimV2
{
	public partial class WebForm5 : System.Web.UI.Page
	{
		
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				ddlGreeks.Items.Clear();
				ddlGreeks.DataSource = Enum.GetNames(typeof(Greek));
				ddlGreeks.DataBind();
				ddlGreeks_SelectedIndexChanged(sender, e);
			}
			else
			{
				Calculate();
			}

		}
		private void Calculate()
		{
			chartGreek.Series.Clear();
			chartGreekRate.Series.Clear();
			var greek = (Greek)ddlGreeks.SelectedIndex;
			switch (greek)
			{
				case Greek.Delta:
					PlotDelta();
					break;
				case Greek.Gamma:
					PlotGamma();
					break;
				case Greek.Rho:
					PlotRho();
					break;
				case Greek.Theta:
					PlotTheta();
					break;
				case Greek.Vega:
					PlotVega();
					break;
			}
			
		}
		public int Scale
		{
			get
			{
				if (int.TryParse(tbScale.Text, out var scale))
				{
					return scale;
				}
				else
				{
					return 7;
				}
			}
		}
		private void PlotDelta()
		{
			var mapDelta = oiOption.Option.GetDeltaMap(Scale, out var prices, out var rates);
			chartGreekRate.AddSeries(rates, Color.Crimson, "Delta Rate (%)", AxisType.Secondary);
			chartGreekRate.AddIndicator("X", oiOption.Strike, Color.Gold);
			chartGreekRate.AddIndicator("X", oiOption.StockPrice, Color.DarkGray);
			chartGreekRate.Y2Format("P2");
			chartGreek.AddSeries(mapDelta, Color.Crimson, "Delta",  AxisType.Secondary);
			chartGreek.AddIndicator("X", oiOption.StockPrice, Color.DarkGray);
			chartGreek.AddIndicator("X", oiOption.Strike, Color.Gold);
			chartGreek.XLabel("Stock Price ($)");
			chartGreek.Y2Label("Delta", ChartUtil.DownColor);
			PlotPrice(prices);
			if (mapDelta.Count == 0) return;
			lblResult.Text = Format.Label("Delta", $"{mapDelta[oiOption.StockPrice]:C4}");
			lblRateResult.Text = Format.LabelPercent("Delta Rate", rates[oiOption.StockPrice]);
			lblDescription.Text = "<b>Delta</b> is the sensitivity of an option price to changes in the underlying price.";
		}
		private void PlotGamma()
		{
			var mapGamma = oiOption.Option.GetGammaMap(Scale, out var prices, out var rates);
			chartGreekRate.AddSeries(rates, Color.LightGreen, "Gamma Rate (%)", AxisType.Secondary);
			chartGreekRate.Y2Format("P2");
			chartGreek.AddSeries(mapGamma, Color.LightGreen, "Gamma", AxisType.Secondary);
			chartGreek.AddIndicator("X", oiOption.StockPrice, Color.DarkGray);
			chartGreek.AddIndicator("X", oiOption.Strike, Color.Gold);
			chartGreekRate.AddIndicator("X", oiOption.StockPrice, Color.DarkGray);
			chartGreekRate.AddIndicator("X", oiOption.Strike, Color.Gold);
			chartGreek.XLabel("Stock Price ($)");
			chartGreek.Y2Label("Gamma", Color.LimeGreen);
			PlotPrice(prices);
			if (mapGamma.Count == 0) return;
			lblResult.Text = Format.Label("Gamma", $"{mapGamma[oiOption.StockPrice]:C4}" );
			lblRateResult.Text = Format.LabelPercent("Gamma Rate", rates[oiOption.StockPrice]);
			lblDescription.Text = "<b>Gamma</b> is the sensitivity of <i>delta</i> to changes in the underlying price.";
		}
		private void PlotRho()
		{
			var mapRho = oiOption.Option.GetRhoMap(Scale, out var prices, out var rates);
			chartGreekRate.AddSeries(rates, Color.Azure, "Rho Rate (%)", AxisType.Secondary);
			chartGreekRate.Y2Format("P2");
			chartGreek.AddSeries(mapRho, Color.Azure, "Rho", AxisType.Secondary);
			chartGreek.AddIndicator("X", oiOption.InterestRate * 100, Color.DarkGray);
			chartGreekRate.AddIndicator("X", oiOption.InterestRate * 100, Color.DarkGray);
			chartGreek.XLabel("Interest Rate (%)");
			chartGreek.Y2Label("Rho", Color.Azure);
			PlotPrice(prices);
			int rate = (int)(oiOption.InterestRate * 100);
			if (mapRho.Count == 0) return;
			lblResult.Text = Format.Label("Rho", $"{mapRho[rate]:C4}");
			lblRateResult.Text = Format.LabelPercent("Rho Rate", rates[rate]);
			lblDescription.Text = "<b>Rho</b> is the sensitivity of an option price to changes in interest rates.";
		}
		private void PlotTheta()
		{
			var mapTheta = oiOption.Option.GetThetaMap(out var prices, out var rates);
			chartGreek.AddSeries(mapTheta, Color.Orange, "Theta", AxisType.Secondary);
			chartGreekRate.AddSeries(rates, Color.Orange, "Theta Rate (%)", AxisType.Secondary);
			chartGreekRate.Y2Format("P2");
			chartGreek.AddIndicator("X", oiOption.Option.DaysToExpiry, Color.DarkGray);
			chartGreekRate.AddIndicator("X", oiOption.Option.DaysToExpiry, Color.DarkGray);
			chartGreek.XLabel("Days to Expiration (DTE)");
			chartGreek.Y2Label("Theta (Daily Decay)", Color.Orange);
			PlotPrice(prices);
			if (mapTheta.Count == 0) return;
			int days = (int)oiOption.Option.DaysToExpiry;
			lblResult.Text = Format.Label("Theta", $"{mapTheta[days]:C4}");
			lblRateResult.Text = Format.LabelPercent("Theta Rate", rates[days]);
			lblDescription.Text = "<b>Theta</b> is the sensitivity of an option price to time decay, or simply how much value the option loses each day.";
		}
		private void PlotVega()
		{
			double vol = oiOption.ImpliedVolatility * 100;
			var mapVega = oiOption.Option.GetVegaMap(Scale, out var prices, out var rates);
			chartGreekRate.AddSeries(rates, Color.MediumPurple, "Vega", AxisType.Secondary);
			chartGreekRate.Y2Format("P2");
			chartGreek.AddSeries(mapVega, Color.MediumPurple, "Vega", AxisType.Secondary);
			chartGreek.AddIndicator("X", vol, Color.DarkGray);
			chartGreekRate.AddIndicator("X", vol, Color.DarkGray);
			chartGreek.XLabel("Implied Volatility (%)");
			chartGreek.Y2Label("Vega", Color.MediumPurple);
			PlotPrice(prices);
			if (mapVega.Count == 0) return;
			lblResult.Text = Format.Label("Vega", $"{mapVega[(int)vol]:C4}");
			lblRateResult.Text = Format.LabelPercent("Vega Rate", rates[(int)vol]);
			lblDescription.Text = "<b>Vega</b> is the sensitivity of an option price to changes in implied volatility.";

		}
		
		private void PlotPrice(Dictionary<double, double> prices)
		{
			chartGreek.AddSeries(prices, ChartUtil.UpColor, "Option Price", AxisType.Primary);
			chartGreekRate.AddSeries(prices, ChartUtil.UpColor, "Option Price", AxisType.Primary);
			chartGreek.YLabel("Option Price", ChartUtil.UpColor);
			chartGreekRate.YLabel("Option Price", ChartUtil.UpColor);
		}
		protected void ddlGreeks_SelectedIndexChanged(object sender, EventArgs e)
		{
			var greek = (Greek)ddlGreeks.SelectedIndex;
			lblTitle.Text = greek.ToString();
		}

		protected void tbScale_TextChanged(object sender, EventArgs e)
		{
			Calculate();
		}
	}
}