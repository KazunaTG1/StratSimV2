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
		private void PlotDelta()
		{
			var mapDelta = oiOption.Option.GetDeltaMap(out var prices);
			chartGreek.AddSeries(mapDelta, ChartUtil.DownColor, "Delta",  AxisType.Secondary);
			chartGreek.AddIndicator("X", oiOption.StockPrice, Color.DarkGray);
			chartGreek.AddIndicator("X", oiOption.Strike, Color.Gold);
			chartGreek.XLabel("Stock Price ($)");
			chartGreek.Y2Label("Delta", ChartUtil.DownColor);
			PlotPrice(prices);
			if (mapDelta.Count == 0) return;
			lblResult.Text = Format.Label("Delta", $"{mapDelta[oiOption.StockPrice]:C4}");
		}
		private void PlotGamma()
		{
			var mapGamma = oiOption.Option.GetGammaMap(out var prices);
			chartGreek.AddSeries(mapGamma, Color.LightGreen, "Gamma", AxisType.Secondary);
			chartGreek.AddIndicator("X", oiOption.StockPrice, Color.DarkGray);
			chartGreek.AddIndicator("X", oiOption.Strike, Color.Gold);
			chartGreek.XLabel("Stock Price ($)");
			chartGreek.Y2Label("Gamma", Color.LimeGreen);
			PlotPrice(prices);
			if (mapGamma.Count == 0) return;
			lblResult.Text = Format.Label("Gamma", $"{mapGamma[oiOption.StockPrice]:C4}" );
		}
		private void PlotRho()
		{
			var mapRho = oiOption.Option.GetRhoMap(out var prices);
			chartGreek.AddSeries(mapRho, Color.Azure, "Rho", AxisType.Secondary);
			chartGreek.AddIndicator("X", oiOption.InterestRate * 100, Color.DarkGray);
			chartGreek.XLabel("Interest Rate (%)");
			chartGreek.Y2Label("Rho", Color.Azure);
			PlotPrice(prices);
			int rate = (int)(oiOption.InterestRate * 100);
			if (mapRho.Count == 0) return;
			lblResult.Text = Format.Label("Rho", $"{mapRho[rate]:C4}");
		}
		private void PlotTheta()
		{
			var mapTheta = oiOption.Option.GetThetaMap(out var prices);
			chartGreek.AddSeries(mapTheta, Color.Orange, "Theta", AxisType.Secondary);
			chartGreek.AddIndicator("X", oiOption.Option.DaysToExpiry, Color.DarkGray);
			chartGreek.XLabel("Days to Expiration (DTE)");
			chartGreek.Y2Label("Theta (Daily Decay)", Color.Orange);
			PlotPrice(prices);
			if (mapTheta.Count == 0) return;
			lblResult.Text = Format.Label("Theta", $"{mapTheta[(int)oiOption.Option.DaysToExpiry]:C4}");
		}
		private void PlotVega()
		{
			double vol = oiOption.ImpliedVolatility * 100;
			var mapVega = oiOption.Option.GetVegaMap(out var prices);
			chartGreek.AddSeries(mapVega, Color.MediumPurple, "Vega",AxisType.Secondary);
			chartGreek.AddIndicator("X", vol, Color.DarkGray);
			chartGreek.XLabel("Implied Volatility (%)");
			chartGreek.Y2Label("Vega", Color.MediumPurple);
			PlotPrice(prices);
			if (mapVega.Count == 0) return;
			lblResult.Text = Format.Label("Vega", $"{mapVega[(int)vol]:C4}");

		}
		private void PlotPrice(Dictionary<double, double> prices)
		{
			chartGreek.AddSeries(prices, ChartUtil.UpColor, "Option Price", AxisType.Primary);
			
			chartGreek.YLabel("Option Price", ChartUtil.UpColor);
		}
		protected void ddlGreeks_SelectedIndexChanged(object sender, EventArgs e)
		{
			var greek = (Greek)ddlGreeks.SelectedIndex;
			lblTitle.Text = greek.ToString();
		}
	}
}