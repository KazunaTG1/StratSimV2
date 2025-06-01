using QFin.MarketData.Transformations;
using QFin.Models.Stochastic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace StratSimV2
{
	public partial class WebForm11 : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				var intervals = Enum.GetNames(typeof(Interval));
				ddlInterval.DataSource = intervals;
				ddlInterval.DataBind();
			}
			else
			{
				ScriptManager.RegisterStartupScript(this, this.GetType(), "renderMath", "renderMath();", true);
			}
		}

		protected void btnSimulate_Click(object sender, EventArgs e)
		{
			double stockPrice = double.Parse(tbStockPrice.Text);
			double interestRate = double.Parse(tbExpectedReturn.Text) / 100;
			int days = int.Parse(tbDays.Text);
			double variance = Math.Pow(double.Parse(tbVolatility.Text) / 100, 2);
			double time = days / 365.0;
			double kappa = double.Parse(tbMeanReversion.Text);
			double theta = Math.Pow(double.Parse(tbLongTermVol.Text) / 100, 2);
			double sigma = double.Parse(tbVolVol.Text) / 100;
			double rho = double.Parse(tbVolCorrelation.Text);
			var interval = (Interval)ddlInterval.SelectedIndex;
			bool showVolatility = cbVol.Checked;
			int simulations = int.Parse(tbSimulations.Text);

			chartStock.Series.Clear();
			for (int s = 0; s < simulations; s++)
			{
				var stockPath = Heston.GetPath(stockPrice, variance, time, kappa, theta, sigma, rho, interestRate, interval,
				out var volPath);
				chartStock.AddSeries(stockPath, $"Stock Path {s}", true);
				chartStock.XLabel("Date");
				chartStock.YLabel("Stock Price");
				if (showVolatility)
				{
					chartStock.AddSeries(volPath, Color.CornflowerBlue, $"Volatility Path {s}", AxisType.Secondary, true);
					chartStock.Y2Label("Volatility", Color.CornflowerBlue);
					chartStock.Y2Format("P2");
				}
			}
			chartStock.AddIndicator("Y", stockPrice, Color.DarkGray);
			chartStock.AddIndicator("Y2", Math.Sqrt(theta), Color.Azure);
			chartStock.AddIndicator("Y2", Math.Sqrt(variance), Color.DarkGray);
			
		}

		protected void cbVol_CheckedChanged(object sender, EventArgs e)
		{

		}
	}
}