using QFin.MarketData.Transformations;
using QFin.Models.Stochastic;
using QFin.Securities;
using QFin;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using QFin.Models.OptionPricing;
using System.IO;
using System.Runtime.Remoting.Messaging;

namespace StratSimV2
{
	public enum SecurityType
	{
		Stock,
		Option
	}
	public partial class WebForm18 : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				LoadMMDdl();
				LoadIntervalDdl();
				LoadSecurityDdl();
				LoadOptionDdl();
				LoadDirectionDdl();
			}
		}

		private List<OptionDTO> OptionPositions
		{
			get
			{
				var positions = (List<OptionDTO>)ViewState["OptionPositions"];
				if (positions == null)
					return new List<OptionDTO>();
				return positions;
			}
			set => ViewState["OptionPositions"] = value;
		}
		private List<StockDTO> StockPositions
		{
			get
			{
				var positions = (List<StockDTO>)ViewState["StockPositions"];
				if (positions == null) return new List<StockDTO>();
				return positions;
			}
			set => ViewState["StockPositions"] = value;
		}
		public TradeDirection Direction 
		{ 
			get
			{
				Enum.TryParse(ddlDirection.SelectedValue, out TradeDirection result);
				return result;
			}
		}
			
		private List<Trade> OptionTrades => OptionPaths.Select((StockPath x) => new Trade(x, true, 1, Direction)).ToList();
		public Portfolio Portfolio
		{
			get
			{
				List<Trade> trades = OptionTrades;
				if (StockTrade != null)
					trades.Add(StockTrade);
				var portfolio = new Portfolio(trades);
				return portfolio;
			}
		}
		private List<StockPath> OptionPaths
		{
			get
			{
				var paths = (List<StockPath>)ViewState["OptionPaths"];
				if (paths == null) return new List<StockPath>();
				return paths;
			}
			set => ViewState["OptionPaths"] = value;
		}
		private StockPath StockPath
		{
			get
			{
				var path = (StockPath)ViewState["StockPath"];
				return path;
			}
			set => ViewState["StockPath"] = value;
		}
		public Trade StockTrade
		{
			get
			{
				if (StockPositions.Count == 0) return null;
				return new Trade(StockPath, false, 1, Direction);
			}
		}
		private void LoadDirectionDdl()
		{
			var names = Enum.GetNames(typeof(TradeDirection));
			ddlDirection.DataSource = names;
			ddlDirection.DataBind();
		}
		private void LoadSecurityDdl()
		{
			var names = Enum.GetNames(typeof(SecurityType));
			ddlEquityType.DataSource = names;
			ddlEquityType.DataBind();
		}
		private void LoadIntervalDdl()
		{
			var names = Enum.GetNames(typeof(Interval));
			ddlInterval.DataSource = names;
			ddlInterval.DataBind();
			ddlInterval.SelectedIndex = 1;
		}
		private void LoadOptionDdl()
		{
			var names = Enum.GetNames(typeof(OptionType));
			ddlOptionType.DataSource = names;
			ddlOptionType.DataBind();
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
			var model = (MarketModel)ddlMarketModel.SelectedIndex;
			double stockPrice = double.Parse(tbStockPrice.Text);
			int dte = int.Parse(tbDTE.Text);
			double years = dte / 365.0;
			double interestRate = double.Parse(tbInterestRate.Text) / 100;
			Interval interval = (Interval)ddlInterval.SelectedIndex;
			switch (model)
			{
				case MarketModel.GBM:
					PlotGBM(stockPrice, years, interestRate,  interval);
					break;
				case MarketModel.AsymGBM:
					PlotAsymGBM(stockPrice, years, interestRate,interval);
					break;
				case MarketModel.Heston:
					PlotHeston(stockPrice, years, interestRate,  interval);
					break;
			}
			PlotPL();
		}
		private void PlotOption(Trade trade)
		{
			chartOption.AddSeries(trade.Path.Path, chartOption.Series.Count +1, true);
			chartOption.AddIndicator("Y", trade.Path.StartPrice, Color.DarkGray);
			
			chartOptionPL.AddSeries(trade.PathPL, chartOptionPL.Series.Count + 1, true);
			chartOptionPL.AddIndicator("Y", trade.PathPL.First().Price, Color.DarkGray);
			chartOptionPL.Y2Format("P2");
		}
		private void PlotStock(List<StockPrice> path)
		{
			double startPrice = path.First().Price;
			double endPrice = path.Last().Price;
			chartStock.AddSeries(path, chartStock.Series.Count() + 1, true);
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
		private void PlotPL()
		{
			chartPL.AddSeries(Portfolio.PathPL, "PL", true);
			chartPL.AddIndicator("Y", Portfolio.PathPL.First().Price, Color.DarkGray);

			var sb = new StringBuilder();
			sb.Append(Format.LabelMoney("Capital At Risk", Portfolio.CapitalAtRisk));
			sb.Append(Format.LabelMoney("PL ($)", Portfolio.PL, Portfolio.PercPL));
			litPL.Text = sb.ToString();
		}
		private void PlotStock(List<StockPath> paths)
		{
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

		}
		private void PlotGBM(double stockPrice, double years, double r,  Interval interval)
		{
			double iv = double.Parse(tbIV.Text) / 100;
			var optionPaths = new List<StockPath>();
			StockPath = new StockPath(GeometricBrownianMotion.GBM(stockPrice, r, years, iv, DateTime.Now, interval), interval);
			if (OptionPositions?.Count > 0)
			{
				for (int k = 0; k < OptionPositions.Count; k++)
				{
					var option = new Option(OptionPositions[k]);
					var trade = option.TradeOption(Direction, -1, -1, -1, StockPath.Path, out var opPath, out var status);
					optionPaths.Add(new StockPath(opPath, interval));
				}
			}
			PlotStock(StockPath.Path);
			OptionPaths = optionPaths;
			foreach (var trade in OptionTrades)
			{
				PlotOption(trade);
			}
			BindOptionPaths();
			chartOption.AddLegend();
			gvStockPath.DataSource = new List<StockPath>() { StockPath };
			gvStockPath.DataBind();

			var portfolio = new Portfolio(OptionTrades);
		}
		private void BindOptionPaths()
		{
			gvOptionPaths.DataSource = OptionPaths;
			gvOptionPaths.DataBind();

			gvOptionPL.DataSource = OptionTrades;
			gvOptionPL.DataBind();
		}
		private void PlotAsymGBM(double stockPrice, double years, double r,Interval interval)
		{
			double callIV = double.Parse(tbCallIV.Text) / 100;
			double putIV = double.Parse(tbPutIV.Text) / 100;
			var paths = new List<StockPath>();
			paths.Add(new StockPath(GeometricBrownianMotion.AsymmetricGBM(stockPrice, r, putIV, callIV, years, interval), interval));
			if (paths.Count() == 1)
				PlotStock(paths[0].Path);
			else PlotStock(paths);
		}
		private void PlotHeston(double stockPrice, double years, double r,Interval interval)
		{
			double v0 = Math.Pow(double.Parse(tbVolatility.Text) / 100, 2);
			double kappa = double.Parse(tbMeanReversion.Text);
			double theta = Math.Pow(double.Parse(tbLongTermVol.Text) / 100, 2);
			double volOfVol = Math.Pow(double.Parse(tbVolVol.Text) / 100, 2);
			double rho = double.Parse(tbVolCorrelation.Text);
			bool showVol = cbShowVol.Checked;
			var path = new StockPath(Heston.GetPath(stockPrice, v0, years, kappa, theta, volOfVol, rho, r, interval, out var volPath), interval);
			PlotStock(path.Path);
			if (showVol)
			{
				chartStock.AddSeries(volPath, Color.MediumPurple, $"Volatility", AxisType.Secondary, true);

				chartStock.Y2Label("Volatility", Color.MediumPurple);
				chartStock.Y2Format("P2");
				chartStock.AddIndicator("Y2", Math.Sqrt(theta), Color.Lavender);
			}

		}
		protected void btnSimulate_Click(object sender, EventArgs e)
		{

			Simulate();
		}
		
		protected void btnAddPosition_Click(object sender, EventArgs e)
		{
			double interest = double.Parse(tbInterestRate.Text) / 100;
			double stockPrice = double.Parse(tbStockPrice.Text);
			int quantity = int.Parse(tbQuantity.Text);
			var secType = (SecurityType)ddlEquityType.SelectedIndex;
			if (secType == SecurityType.Option)
			{
				
				
				double iv = double.Parse(tbOpIV.Text) / 100;
				int dte = int.Parse(tbOpDTE.Text);
				double strike = double.Parse(tbStrike.Text);
				var optionType = (OptionType)ddlOptionType.SelectedIndex;
				Option option = new Option("ASP", optionType, DateTime.Now.AddDays(dte), strike, stockPrice, 0, iv, interest);
				var tempList = OptionPositions;
				tempList.Add(new OptionDTO(option, quantity));
				OptionPositions = tempList;
				BindPositionGrid();
			}
			else if (secType == SecurityType.Stock)
			{
				var stock = new StockDTO("ASP", stockPrice, quantity);
				var tempList = StockPositions;
				tempList.Add(stock);
				StockPositions = tempList;
				BindPositionGrid();
			}

		}
		private void BindPositionGrid()
		{
			rowOptionPosition.Visible = OptionPositions.Count != 0;
			gvOpPositions.DataSource = OptionPositions;
			gvOpPositions.DataBind();

			rowStockPosition.Visible = StockPositions.Count != 0;
			gvStockPosition.DataSource = StockPositions;
			gvStockPosition.DataBind();
		}

		protected void ddlEquityType_SelectedIndexChanged(object sender, EventArgs e)
		{
			var secType = (SecurityType)ddlEquityType.SelectedIndex;
			bool isOption = secType == SecurityType.Option;
			rowStrike.Visible = isOption;
			rowDTE.Visible = isOption;
			rowOptionType.Visible = isOption;
			rowIV.Visible = isOption;
		}

		protected void btnClearPositions_Click(object sender, EventArgs e)
		{
			OptionPositions = new List<OptionDTO>();
			StockPositions = new List<StockDTO>();
			BindPositionGrid();
		}

		protected void gvStockPosition_RowDeleting(object sender, GridViewDeleteEventArgs e)
		{
			int index = e.RowIndex;
			var tempList = StockPositions;
			tempList.RemoveAt(index);
			StockPositions = tempList;
			BindPositionGrid();
		}

		protected void gvOpPositions_RowDeleting(object sender, GridViewDeleteEventArgs e)
		{
			int index = e.RowIndex;
			var tempList = OptionPositions;
			tempList.RemoveAt(index);
			OptionPositions = tempList;
			BindPositionGrid();
		}
	}
}