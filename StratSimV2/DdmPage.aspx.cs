using QFin;
using QFin.Securities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StratSimV2
{
	public partial class WebForm10 : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

        protected void btnCalculate_Click(object sender, EventArgs e)
        {
			double sDividend = double.Parse(tbStartDividend.Text);
			double eDividend = double.Parse(tbEndDividend.Text);
			var sDate = DateTime.Parse(tbStartDividendDate.Text);
			var eDate = DateTime.Parse(tbEndDividendDate.Text);
			var sPoint = new AmountPoint(sDate, sDividend);
			var ePoint = new AmountPoint(eDate, eDividend);
			double stockBeta = double.Parse(tbStockBeta.Text);
			double riskFreeRate = double.Parse(tbRiskFreeRate.Text) / 100;
			double expectedMarketReturn = double.Parse(tbExpectedReturn.Text) / 100;
			var stock = new Stock()
			{
				StockBeta = stockBeta,
				RiskFreeRate = riskFreeRate,
				ExpectedReturn = expectedMarketReturn
			};
			stock.DividendPayouts.Add(sPoint);
			stock.DividendPayouts.Add(ePoint);
			lblResult.Text = Format.LabelMoney("Fair Value", stock.GetFairValueDDM());
        }
    }
}