using QFin.Models.OptionPricing;
using QFin.Securities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StratSimV2.Controls
{
	public partial class OptionSimOI : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}
		public double Quantity
		{
			get
			{
				if (ddlQuantity.SelectedItem.Text == "% of Balance")
				{
					return double.Parse(tbQuantity.Text) / 100;
				}
				else
				{
					return double.Parse(tbQuantity.Text);
				}

			}
		}
		public string QuantityType
		{
			get
			{
				return ddlQuantity.SelectedItem.Text;
			}
		}
		public TradeDirection Direction
		{
			get
			{
				return ddlDirection.SelectedItem.Text == "Long" ? TradeDirection.Long : TradeDirection.Short;
			}
		}
		public double TakeProfit
		{
			get
			{
				if (!cbTakeProfit.Checked)
				{
					return -1;
				}
				return double.Parse(tbTakeProfit.Text)/100;
			}
		}
		public double StopLoss
		{
			get
			{
				if (!cbStopLoss.Checked)
				{
					return -1;
				}
				return double.Parse(tbStopLoss.Text) / 100;
			}
		}
		public int CloseDTE
		{
			get
			{
				if (!cbCloseDTE.Checked)
					return -1;
				return int.Parse(tbCloseDTE.Text);
			}
		}
	}
}