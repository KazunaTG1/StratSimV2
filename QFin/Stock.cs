using MathNet.Numerics;
using QFin.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace QFin
{
	namespace Securities
	{
		[Serializable]
		public readonly struct StockPrice
		{
			public DateTime Timestamp { get; }
			public double Price { get; }

			public StockPrice(DateTime timestamp, double price)
			{
				Timestamp = timestamp;
				Price = price;
			}

			public override string ToString() => $"{Price:C} @ {Timestamp:d}";
		}
		public class Stock
		{
			public Stock() { }
			public string Exchange { get; set; }
			public string Symbol { get; set; }
			public string Name { get; set; }
			public double StockPrice { get; set; }
			/// <summary>
			/// Market value of equity
			/// </summary>
			public double MarketCap { get; set; }
			/// <summary>
			/// Market value of debt
			/// </summary>
			public double DebtOutstanding { get; set; }
			/// <summary>
			/// Company debt yields
			/// </summary>
			public double DebtYields { get; set; }
			/// <summary>
			/// How volatile a stock is vs. the market
			/// </summary>
			public double StockBeta { get; set; }
			/// <summary>
			/// Effective corporate tax rate
			/// </summary>
			public double TaxRate { get; set; }
			/// <summary>
			/// Expected market return (9-10% historically for US equities)
			/// </summary>
			public double ExpectedReturn { get; set; }
			/// <summary>
			/// Usually 10-year US Treasury yield
			/// </summary>
			public double RiskFreeRate { get; set; }
			public double Cash { get; set; }
			public double SharesOutstanding { get; set; }
			public CapitalStructure CapitalStructure => new CapitalStructure(MarketCap, DebtOutstanding);
			public double CostOfEquity => CAPM.GetCostOfEquity(RiskFreeRate, StockBeta, ExpectedReturn).Round(5);
			public double CostOfDebt => GetCostOfDebt(DebtYields, TaxRate).Round(5);
			public List<AmountPoint> FreeCashFlows { get; set; } = new List<AmountPoint>();
			public List<AmountPoint> DividendPayouts { get; set; } = new List<AmountPoint>();
			private static double GetCostOfDebt(double yields, double tax)
			{
				return yields * (1 - tax);
			}
			public double Wacc => WACC.GetResult(CapitalStructure, CostOfEquity, CostOfDebt);
			public double GetFairValueDCF()
			{
				if (FreeCashFlows.Count <= 1) return -1;
				return EquityValue / SharesOutstanding;
			}
			public double GetFairValueDDM()
			{
				if (DividendPayouts.Count <= 1) return double.NaN;
				double lastDividend = DividendPayouts.Last().Amount;
				double growthRate = GrowthDividend.GrowthRate;
				double requiredReturn = CostOfEquity;

				if (requiredReturn <= growthRate || requiredReturn <= 0)
				{
					requiredReturn = 7;
				}	
				double nextDividend = lastDividend * (1 + growthRate);
				return nextDividend / (requiredReturn - growthRate);
			}
			public double EquityValue
			{
				get
				{
					if (FreeCashFlows.Count <= 1) return -1;
					return EnterpriseValue + Cash - DebtOutstanding;
				}
			}
			/// <summary>
			/// Sum of present values 
			/// </summary>
			public double EnterpriseValue
			{
				get
				{
					if (FreeCashFlows.Count <= 1) return -1;
					return (ProjectedPVs.Sum() + PresentTerminalValue).Round(8);
				}
			}
			/// <summary>
			/// Discount each year's projected FCF back to present
			/// </summary>
			public List<double> ProjectedPVs
			{
				get
				{
					if (FreeCashFlows.Count <= 1) return null;
					return ProjectedFCF.Select(x => DCF.GetPresentValue(x.Value, Wacc, x.Key).Round(8)).ToList();
				}
			}
			/// <summary>
			/// Discounted terminal value back to present
			/// </summary>
			public double PresentTerminalValue
			{
				get
				{
					if (FreeCashFlows.Count <= 1) return -1;
					return DCF.GetPresentValue(TerminalValue, GrowthFCF.GrowthRate, ProjectedFCF.Last().Key).Round(8);
				}
			}
			/// <summary>
			/// Calculate the terminal value using Gordon Growth Model (GGM)
			/// </summary>
			public double TerminalValue
			{
				get
				{
					if (FreeCashFlows.Count <= 1) return -1;
					return GGM.GetTerminalValue(Wacc, PerpetualGrowthRate, ProjectedFCF.Last().Value).Round(8);
				}
			}
			public CAGR GrowthFCF
			{
				get
				{
					if (FreeCashFlows.Count <= 1) return null;
					return new CAGR(FreeCashFlows.First(), FreeCashFlows.Last());
				}
			}
			public CAGR GrowthDividend
			{
				get
				{
					if (DividendPayouts.Count <= 1) return null;
					return new CAGR(DividendPayouts.First(), DividendPayouts.Last());
				}
			}
			/// <summary>
			/// Project Free Cash Flows (FCF) for the next 5 years
			/// </summary>
			public Dictionary<int, double> ProjectedFCF
			{
				get
				{
					if (FreeCashFlows.Count <= 1) return null;
					var projectedFCF = new Dictionary<int, double>() { { 0, GrowthFCF.EndAmount.Amount } };
					for (int i = 1; i <= 5; i++)
					{
						double tFcf = projectedFCF[i - 1] * (1 + GrowthFCF.GrowthRate);
						projectedFCF.Add(i, tFcf);
					}
					projectedFCF.Remove(0); // Remove the initial value placeholder
					return projectedFCF;
				}
				
			}
			public double PerpetualGrowthRate { get; set; } = 0.025;
		}
		[Serializable]
		public class StockDTO
		{
			public StockDTO(string symbol, double price, double quantity)
			{
				Symbol = symbol;
				Price = price;
				Quantity = quantity;
			}

			public string Symbol { get; set; }
			public double Price { get; set; }
			public double Quantity { get; set; }
		}
	}
	
}
