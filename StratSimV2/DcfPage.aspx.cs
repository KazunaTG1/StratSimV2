using MathNet.Numerics.LinearRegression;
using QFin;
using QFin.Models;
using QFin.Securities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StratSimV2
{
	public enum Multiplier
	{
		None,
		Thousand,
		Million,
		Billion,
		Trillion
	}
	public partial class WebForm8 : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				var mults = Enum.GetNames(typeof(Multiplier));
				SetDDL(ddlEFcfMultiplier, mults);
				SetDDL(ddlSfcfMultiplier, mults);
				SetDDL(ddlCapMult, mults);
				SetDDL(ddlCashMult, mults);
				SetDDL(ddlDebtMult, mults);
				SetDDL(ddlSharesMult, mults);
			}
		}
		private void SetDDL(DropDownList ddl, string[] mults)
		{
			ddl.DataSource = mults;
			ddl.DataBind();
			ddl.SelectedIndex = (int)Multiplier.Million;
		}
		
        protected void btnCalculate_Click(object sender, EventArgs e)
        {
			var cagr = GetGrowth();
			var stock = GetStock();
			stock.FreeCashFlows.Add(cagr.StartAmount);
			stock.FreeCashFlows.Add(cagr.EndAmount);
			SetGrowth(stock);
			SetProjectedCashFlows(stock);
			SetStockValues(stock);
			SetCapStructure(stock);
		}
		private void SetStockValues(Stock stock)
		{
			// Display result formatted
			var sb = new StringBuilder();
			sb.Append(Format.LabelMoney("Fair Value per Share", stock.GetFairValueDCF()));
			lblResult.Text = sb.ToString();
			var sbVals = new StringBuilder();
			sbVals.Append(Format.LabelNumber("Enterprise Value", stock.EnterpriseValue));
			sbVals.Append(Format.LabelNumber("Equity Value", stock.EquityValue));
			sbVals.Append(Format.LabelNumber("Terminal Value", stock.TerminalValue));
			sbVals.Append(Format.LabelNumber("Present Terminal Value", stock.PresentTerminalValue));
			lblValues.Text = sbVals.ToString();
		}
		private void SetProjectedCashFlows(Stock stock)
		{
			var pfcf = new Dictionary<DateTime, double>();
			var presentPfcf = new Dictionary<DateTime, double>();
			foreach (var pf in stock.ProjectedFCF)
			{
				DateTime newDate = stock.GrowthFCF.EndAmount.Timestamp.AddYears(pf.Key);
				pfcf.Add(newDate, pf.Value);
			}
			for (int i = 0; i < stock.ProjectedPVs.Count; i++)
			{
				presentPfcf.Add(pfcf.ElementAt(i).Key, stock.ProjectedPVs[i]);
			}
			barPFCF.AddBar(pfcf, "Projected Free Cash Flow");
			barPFCF.AddBar(presentPfcf, "Projected Present Values");
			barPFCF.XLabel("Date");
			barPFCF.YLabel("Projected Free Cash Flow ($ Millions)");
		}
		private void SetCapStructure(Stock stock)
		{
			var capstruct = new Dictionary<string, double>()
			{
				{ "Equity", stock.CapitalStructure.Equity },
				{ "Debt", stock.CapitalStructure.Debt }
			};
			barCapStructure.AddBar(capstruct);
			barCapStructure.XLabel("Capital Type");
			barCapStructure.YLabel("Amount ($ Millions)");
			var sbCapStruct = new StringBuilder();
			sbCapStruct.Append(Format.LabelPercent("Equity Weight", stock.CapitalStructure.EquityWeight));
			sbCapStruct.Append(Format.LabelPercent("Debt Weight", stock.CapitalStructure.DebtWeight));
			sbCapStruct.Append(Format.LabelPercent("Cost of Equity", stock.CostOfEquity));
			sbCapStruct.Append(Format.LabelPercent("Cost of Debt", stock.CostOfDebt));
			sbCapStruct.Append(Format.LabelPercent("Weighted Average Cost of Capital (WACC)", stock.Wacc));
			lblCapStruct.Text = sbCapStruct.ToString();
		}
		private void SetGrowth(Stock stock)
		{
			var dict = new Dictionary<DateTime, double>();
			foreach (var fcf in stock.FreeCashFlows)
			{
				dict.Add(fcf.Timestamp, fcf.Amount);
			}
			barFCF.AddBar(dict, "Free Cash Flow");
			barFCF.XLabel("Date");
			barFCF.YLabel("Free Cash Flow ($ Millions)");

			var sbFCF = new StringBuilder();

			sbFCF.Append(Format.LabelPercent("Growth Rate (%)", stock.GrowthFCF.GrowthRate));
			sbFCF.Append(Format.LabelNumber("Growth ($)", stock.GrowthFCF.GrowthDiff));
			sbFCF.Append(Format.Label("Timespan", $"{stock.GrowthFCF.Years:N1} years"));
			sbFCF.Append(Format.LabelPercent("Perpetual Growth Rate (%)", stock.PerpetualGrowthRate));
			lblFCF.Text = sbFCF.ToString();
		}
		private double GetRealValue(string text, Multiplier mult)
		{
			switch (mult)
			{
				case Multiplier.Thousand:
					return double.Parse(text) * 1_000;
				case Multiplier.Million:
					return double.Parse(text) * 1_000_000;
				case Multiplier.Billion:
					return double.Parse(text) * 1_000_000_000;
				case Multiplier.Trillion:
					return double.Parse(text) * 1_000_000_000_000;
				default:
					return double.NaN;
			}
		}
		private Stock GetStock()
		{
			double perpGrowth = double.Parse(tbPerpGrowth.Text) / 100;
			double sharesOutstanding = GetRealValue(tbShares.Text, (Multiplier)ddlSharesMult.SelectedIndex);
			double debt = GetRealValue(tbDebtOutstanding.Text, (Multiplier)ddlDebtMult.SelectedIndex);
			double cash = GetRealValue(tbCash.Text, (Multiplier)ddlCashMult.SelectedIndex);
			double s0 = 100;
			double stockBeta = double.Parse(tbStockBeta.Text);
			double expReturn = double.Parse(tbExpectedReturn.Text) / 100;
			double debtYields = double.Parse(tbDebtYields.Text) / 100;
			double taxRate = double.Parse(tbTaxRate.Text) / 100;
			double marketCap = GetRealValue(tbMarketCap.Text, (Multiplier)ddlCapMult.SelectedIndex);
			double interestRate = double.Parse(tbInterestRate.Text) / 100;
			return new Stock()
			{
				Cash = cash,
				StockPrice = s0,
				DebtOutstanding = debt,
				StockBeta = stockBeta,
				MarketCap = marketCap,
				DebtYields = debtYields,
				TaxRate = taxRate,
				RiskFreeRate = interestRate,
				ExpectedReturn = expReturn,
				SharesOutstanding = sharesOutstanding,
				PerpetualGrowthRate = perpGrowth
			};
		}
		private CAGR GetGrowth()
		{
			var startDate = DateTime.Parse(tbFCFDate1.Text);
			var endDate = DateTime.Parse(tbFCFDate2.Text);
			double startFCF = GetRealValue(tbFCF1.Text, (Multiplier)ddlSfcfMultiplier.SelectedIndex);
			double endFCF = GetRealValue(tbFCF2.Text, (Multiplier)ddlEFcfMultiplier.SelectedIndex);

			var startCashFlow = new AmountPoint(startDate, startFCF);
			var endCashFlow = new AmountPoint(endDate, endFCF);

			return new CAGR(startCashFlow, endCashFlow);
		}
    }
}