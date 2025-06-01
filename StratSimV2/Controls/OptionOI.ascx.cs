using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QFin.Securities;

namespace StratSimV2
{
	public partial class OptionOI : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				Symbol = "QQQ";
				Type = OptionType.Call;
				Strike = 115;
				StockPrice = 100;
				MarketPrice = .30;
				ImpliedVolatility = 30;
				Expiration = new DateTime(2025, 05, 23);
				InterestRate = 4.12;
			}
		}
		public string Symbol 
		{ 
			get 
			{ 
				return tbSymbol.Text; 
			} 
			set 
			{ 
				tbSymbol.Text = value; 
			} 
		}
		public OptionType Type
		{
			get
			{
				return ddlType.SelectedItem.Text == "Call" ? OptionType.Call : OptionType.Put;
			}
			set
			{
				ddlType.Text = value.ToString();
			}
		}
		public double Strike
		{
			get
			{
				return double.Parse(tbStrike.Text);
			}
			set
			{
				tbStrike.Text = value.ToString();
			}
		}
		public double Moneyness
		{
			get
			{
				return double.Parse(tbMoneyness.Text);
			}
			set
			{
				tbMoneyness.Text = value.ToString();
			}
		}
		public double StockPrice
		{
			get
			{
				return double.Parse(tbStockPrice.Text);
			}
			set
			{
				tbStockPrice.Text = value.ToString();
			}
		}
		public double MarketPrice
		{
			get
			{
				return double.Parse(tbMarketPrice.Text);
			}
			set
			{
				tbMarketPrice.Text = value.ToString();
			}
		}
		public DateTime Expiration
		{
			get
			{
				try
				{
					var date = DateTime.Parse(tbExpiration.Text);
					return date;
				}
				catch
				{
					return DateTime.Now ;
				}
			}
			set
			{
				tbExpiration.Text = value.ToString("yyyy-MM-dd");
			}
		}
		public double ImpliedVolatility
		{
			get
			{
				return double.Parse(tbIV.Text) / 100;
			}
			set
			{
				tbIV.Text = value.ToString();
			}
		}
		public double InterestRate
		{
			get
			{
				return double.Parse(tbInterest.Text) / 100;
			}
			set
			{
				tbInterest.Text = value.ToString();
			}
		}
		public int DaysToExpiration
		{
			get
			{
				return int.Parse(tbDTE.Text);
			}
			set
			{
				tbDTE.Text = value.ToString();
			}
		}
		protected void option_changed(object sender, EventArgs e)
		{
			if (Option != null)
			{
				var sb = new StringBuilder();
				sb.Append(Format.LabelMoney("Theor Price", Option.Price));
				if (UseMarketPrice)
				{
					sb.Append(Format.LabelMoney("Edge", Option.Edge));
					sb.Append(Format.LabelPercent("Edge Ratio", Option.EdgeRatio));
				}
				lblResult.Text = sb.ToString();

				if (!UseMarketPrice)
				{
					MarketPrice = Option.Price;
				}
			}
		}
		public bool UseMarketPrice
		{
			get
			{
				return rowMarketPrice.Visible;
			}
			set
			{
				rowMarketPrice.Visible = value;
				
			}
		}
		public Option Option
		{
			get
			{
				return new Option(Symbol, Type, Expiration, Strike, StockPrice, MarketPrice, ImpliedVolatility, InterestRate);
			}
			set
			{
				Symbol = Option.Symbol;
				Type = Option.Type;
				Expiration = Option.Expiration;
				Strike = Option.Strike;
				StockPrice = Option.StockPrice;
				ImpliedVolatility = Option.ImpliedVolatility;
				InterestRate = Option.InterestRate;
			}
		}

		protected void tbMoneyness_TextChanged(object sender, EventArgs e)
		{
			Strike = StockPrice * Moneyness;
			option_changed(sender, e);
		}

		protected void tbStrike_TextChanged(object sender, EventArgs e)
		{
			Moneyness = Strike / StockPrice; 
			option_changed(sender, e);
		}

		protected void tbExpiration_TextChanged(object sender, EventArgs e)
		{
			DaysToExpiration = (int)(Expiration - DateTime.Now).TotalDays;
			option_changed(sender, e);
		}

		protected void tbDTE_TextChanged(object sender, EventArgs e)
		{
			Expiration = DateTime.Now.AddDays(DaysToExpiration);
			option_changed(sender, e);
		}
	}
}