using QFin.Models.Stochastic;
using QFin.MarketData.Transformations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using QFin.Securities;
using System.IO;
using System.Web.UI.DataVisualization.Charting;
using System.Text;
using QFin;
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace StratSimV2
{
	public enum MarketModel
	{
		GBM,
		AsymGBM,
		Heston
	}
	public partial class WebForm17 : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				LoadMMDdl();
				LoadIntervalDdl();
			}
			else
			{
				if (ViewState["StockPaths"] != null)
				{
					StockPaths = (List<StockPath>)ViewState["StockPaths"];
					PlotStock(StockPaths);
				}
				
			}
		}
		public List<StockPath> StockPaths { get; set; }
		private void LoadIntervalDdl()
		{
			ddlInterval.DataSource = ChartInterval.IntervalStrings;
			ddlInterval.DataTextField = "VALUE";
			ddlInterval.DataValueField = "KEY";
			ddlInterval.DataBind();
			ddlInterval.SelectedIndex = 5;
		}
		private void LoadMMDdl()		
		{
			var dict = new Dictionary<MarketModel, string>()
			{
				{ MarketModel.GBM, "Geometric Brownian Motion (GBM)" },
				{ MarketModel.AsymGBM, "Asymmetric GBM" },
				{ MarketModel.Heston, "Heston Model" }
			};
			ddlMarketModel.DataSource = dict;
			ddlMarketModel.DataTextField = "Value";
			ddlMarketModel.DataValueField = "KEY";
			ddlMarketModel.DataBind();
			SetVisibleRow();
		}
		private void SetVisibleRow()
		{
			var model = (MarketModel)ddlMarketModel.SelectedIndex;
			HideRows();
			switch (model)
			{
				case MarketModel.GBM:
					rowGBM.Visible = true;
					break;
				case MarketModel.AsymGBM:
					rowAsymGBM.Visible = true;
					break;
				case MarketModel.Heston:
					rowHeston.Visible = true;
					break;
			}
		}
		protected void ddlMarketModel_SelectedIndexChanged(object sender, EventArgs e)
		{
			SetVisibleRow();
			string desc = "";
			var model = (MarketModel)ddlMarketModel.SelectedIndex;
			switch (model)
			{
				case MarketModel.Heston:
					desc = "<h3>This simulation models stock prices under the Heston stochastic volatility framework.</h3>" +
						"<p>The Heston model assumes the volatility itself follows a random, mean-reverting process. " +
						"Stock prices are simulated using two correlated sources of randomness: one for the price and one for the variance.</p>";
					break;
			}
			litDescription.Text = desc;
		}
		private void HideRows()
		{
			rowAsymGBM.Visible = false;
			rowGBM.Visible = false;
			rowHeston.Visible = false;
		}

		private void Simulate()
		{
			chartStock.Series.Clear();
			chartStock.ChartAreas[0].AxisY.StripLines.Clear();
			chartStock.ChartAreas[0].AxisX.StripLines.Clear();
			var model = (MarketModel)ddlMarketModel.SelectedIndex;
			double stockPrice = double.Parse(tbStockPrice.Text);
			int dte = int.Parse(tbDTE.Text);
			int sims = int.Parse(tbSimulations.Text);
			double years = dte / 365.0;
			double interestRate = double.Parse(tbInterestRate.Text) / 100;
			Interval interval = (Interval)ddlInterval.SelectedIndex;
			switch (model)
			{
				case MarketModel.GBM:
					PlotGBM(stockPrice, years, interestRate, sims, interval);
					break;
				case MarketModel.AsymGBM:
					PlotAsymGBM(stockPrice, years, interestRate, sims, interval);
					break;
				case MarketModel.Heston:
					PlotHeston(stockPrice, years, interestRate, sims, interval);
					break;
			}
			BindGrid();
		}
		private void PlotStock(StockPath path)
		{
			chartStock.Series.Clear();
			double startPrice = path.Path.First().Price;
			double endPrice = path.Path.Last().Price;
			chartStock.AddSeries(path.Path, chartStock.Series.Count() + 1,  true);
			chartStock.AddIndicator("Y", startPrice, Color.DarkGray);
			Color color = endPrice > startPrice ? Color.LightGreen : Color.Pink;
			chartStock.AddIndicator("Y", endPrice, color);
			chartStock.XLabel("Date");
			chartStock.YLabel("Stock Price");

			var sb = new StringBuilder();
			
			
			double change = endPrice - startPrice;
			double changePerc = (endPrice / startPrice) - 1;
			sb.Append(Format.LabelMoney("Start Price", startPrice));
			sb.Append(Format.LabelMoney("End Price", endPrice));
			sb.Append(Format.LabelMoney("Change", change, changePerc));
			lblResult.Text = sb.ToString();
		}
		
		private void PlotStock(List<StockPath> paths)
		{
			chartStock.ClearIndicators();
			chartStock.Series.Clear();
			int bins = int.Parse(tbBins.Text);
			double startPrice = paths[0].Path.First().Price;
			List<double> finalPrices = new List<double>();
			foreach (var path in paths)
			{
				double endPrice = path.Path.Last().Price;
				finalPrices.Add(endPrice);
				chartStock.AddSeries(path.Path, chartStock.Series.Count() + 1, true);
				
			}
			
			
			double avgEndPrice = finalPrices.Average();
			var sb = new StringBuilder();
			double change = avgEndPrice - startPrice;
			double changePerc = (avgEndPrice / startPrice) - 1;

			sb.Append(Format.LabelMoney("Start Price", startPrice));
			sb.Append(Format.LabelMoney("Avg End Price", avgEndPrice));
			sb.Append(Format.LabelMoney("Change", change, changePerc));
			lblResult.Text = sb.ToString();

			chartStock.AddIndicator("Y", startPrice, Color.DarkGray);
			Color color = avgEndPrice > startPrice ? Color.LightGreen : Color.Pink;
			chartStock.AddIndicator("Y", avgEndPrice, color);
			chartStock.XLabel("Date");
			chartStock.YLabel("Stock Price");

			chartFreq.Series.Clear();
			chartFreq.AddHistogram(finalPrices, bins);
			chartFreq.YLabel("Frequency");

			
		}
		private void PlotGBM(double stockPrice, double years, double r, int sims, Interval interval)
		{
			double iv = double.Parse(tbIV.Text) / 100;
			var paths = new List<StockPath>();
			for (int i = 0; i < sims; i++)
			{
				paths.Add(new StockPath(GeometricBrownianMotion.GBM(stockPrice, r, years, iv, DateTime.Now, interval), interval));
			}
			ViewState["StockPaths"] = paths;
			if (paths.Count() == 1)
				PlotStock(paths[0]);
			else PlotStock(paths);

			gvStockPaths.DataSource = paths;
			gvStockPaths.DataBind();
		}
		private void PlotAsymGBM(double stockPrice, double years, double r, int sims, Interval interval)
		{
			double callIV = double.Parse(tbCallIV.Text) / 100;
			double putIV = double.Parse(tbPutIV.Text) / 100;
			var paths = new List<StockPath>();
			for (int i = 0; i < sims; i++)
			{
				paths.Add(new StockPath(GeometricBrownianMotion.AsymmetricGBM(stockPrice, r, putIV, callIV, years, interval), interval));
			}
			ViewState["StockPaths"] = paths;
			if (paths.Count() == 1)
				PlotStock(paths[0]);
			else PlotStock(paths);
			gvStockPaths.DataSource = paths;
			gvStockPaths.DataBind();
		}
		private void PlotHeston(double stockPrice, double years, double r, int sims, Interval interval)
		{
			double v0 = Math.Pow(double.Parse(tbVolatility.Text) / 100, 2);
			double kappa = double.Parse(tbMeanReversion.Text);
			double theta = Math.Pow(double.Parse(tbLongTermVol.Text) / 100, 2);
			double volOfVol = Math.Pow(double.Parse(tbVolVol.Text) / 100, 2);
			double rho = double.Parse(tbVolCorrelation.Text);
			bool showVol = cbShowVol.Checked;
			var paths = new List<StockPath>();
			var volPaths = new List<List<AmountPoint>>();
			for (int i = 0; i < sims; i++)
			{
				paths.Add(new StockPath(Heston.GetPath(stockPrice, v0, years, kappa, theta, volOfVol, rho, r, interval, out var volPath), interval));
				volPaths.Add(volPath);
			}
			ViewState["StockPaths"] = paths;
			if (paths.Count() == 1)
				PlotStock(paths[0]);
			else PlotStock(paths);
			if (showVol)
			{
				for (int i = 0; i  < sims; i++)
				{
					chartStock.AddSeries(volPaths[i], Color.MediumPurple, $"Volatility {i+1}", AxisType.Secondary, true);
				}
				
				chartStock.Y2Label("Volatility", Color.MediumPurple);
				chartStock.Y2Format("P2");
				chartStock.AddIndicator("Y2", Math.Sqrt(theta), Color.Lavender);
			}
			gvStockPaths.DataSource = paths;
			gvStockPaths.DataBind();
		}
		protected void btnSimulate_Click(object sender, EventArgs e)
		{
			
			Simulate();
		}

		protected void gvStockPaths_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			gvStockPaths.PageIndex = e.NewPageIndex;
			BindGrid();
		}

		protected void gvStockPaths_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.Header)
			{
				string sortExpression = gvStockPaths.SortExpression;
				foreach (DataControlField field in gvStockPaths.Columns)
				{
					if (field.SortExpression == sortExpression)
					{
						int colIndex = gvStockPaths.Columns.IndexOf(field);
						e.Row.Cells[colIndex].CssClass = "lnk";
					}
				}
			}
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				List<string> headers = new List<string>();
				foreach (DataControlField col in gvStockPaths.Columns)
					headers.Add(col.HeaderText);

				int changeIndex = headers.IndexOf("Change") + 1;
				string changeText = e.Row.Cells[changeIndex].Text.Replace("(", "-").Replace("$", "").Replace(")", "");
				if (double.TryParse(changeText, out double change))
				{
					if (change > 0)
						e.Row.Cells[changeIndex].ForeColor = ChartUtil.UpColor;
					else if (change < 0)
						e.Row.Cells[changeIndex].ForeColor = ChartUtil.DownColor;

				}

				int percIndex = headers.IndexOf("Change Percent") + 1;
				string percentText = e.Row.Cells[percIndex].Text.Replace("%", "");
				if (double.TryParse(percentText, out double percent))
				{
					if (percent >0)
						e.Row.Cells[percIndex].ForeColor = ChartUtil.UpColor;
					else if (percent < 0)
						e.Row.Cells[percIndex].ForeColor = ChartUtil.DownColor;
				}

			}
		}
		private string SortExpression
		{
			get => ViewState["SortExpression"] as string ?? "StartDate";
			set => ViewState["SortExpression"] = value;
		}
		private SortDirection SortDirection
		{
			get => ViewState["SortDirection"] != null ? (SortDirection)ViewState["SortDirection"] : SortDirection.Descending;
			set => ViewState["SortDirection"] = value;
		}
		protected void gvStockPaths_Sorting(object sender, GridViewSortEventArgs e)
		{
			if (SortExpression == e.SortExpression)
				SortDirection = SortDirection == SortDirection.Descending ? SortDirection.Ascending : SortDirection.Descending;
			else
			{
				SortExpression = e.SortExpression;
				SortDirection = SortDirection.Descending;
			}
			BindGrid();
		}
		private List<StockPath> GetStockPaths()
		{
			return (List<StockPath>)ViewState["StockPaths"];
		}
		private void BindGrid()
		{
			StockPaths = SortedPaths();

			gvStockPaths.DataSource = StockPaths;
			gvStockPaths.DataBind();
		}
		private List<StockPath> SortedPaths()
		{
			var data = GetStockPaths(); // your method
			var prop = TypeDescriptor.GetProperties(typeof(StockPath))[SortExpression];

			if (SortDirection == SortDirection.Ascending)
				data = data.OrderBy(x => prop.GetValue(x)).ToList();
			else
				data = data.OrderByDescending(x => prop.GetValue(x)).ToList();

			return data;
		}

		protected void gvStockPaths_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (gvStockPaths.SelectedIndex == -1)
			{
				PlotStock(StockPaths);
			}
			var paths = SortedPaths();
			int rowIndex = gvStockPaths.SelectedIndex;
			int pageLength = gvStockPaths.PageSize;
			int pageNumber = gvStockPaths.PageIndex;
			int pathIndex = (rowIndex + (pageNumber * pageLength));
			var selectedPath = paths.ElementAt(pathIndex);
			PlotStock(selectedPath);
			BindGrid();
		}

		protected void btnAllSims_Click(object sender, EventArgs e)
		{
			gvStockPaths.SelectedIndex = -1;
		}
	}
}