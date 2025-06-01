using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFin
{
	/// <summary>
	/// Capital Asset Pricing Model (CAPM)
	/// </summary>
	public class CAPM
	{
		/// <summary>
		/// Capital Asset Pricing Model Formula
		/// </summary>
		/// <param name="r">Risk-free rate</param>
		/// <param name="b">Stock beta</param>
		/// <param name="m">Expected market return</param>
		/// <returns>Cost of Equity (%)</returns>
		public static double GetCostOfEquity(double r, double b, double m)
		{
			return r + (b * (m - r));
		}
	}
	/// <summary>
	 /// Gordon Growth Model
	 /// </summary>
	public static class GGM
	{
		public static double GetTerminalValue(double r, double g, double fcf) => (fcf) / (r - g);
	}

	/// <summary>
	/// Weighted Average Cost of Capital (WACC)
	/// </summary>
	public class WACC
	{
		public static double GetResult(CapitalStructure cs, double coe, double cod)
		{
			return (cs.EquityWeight * coe) + (cs.DebtWeight * cod);
		}
	}
	public class CapitalStructure
	{
		public CapitalStructure(double equity, double debt)
		{
			Equity = equity;
			Debt = debt;
		}
		public double Equity { get; set; }
		public double Debt { get; set; }
		public double EquityWeight => Equity / (Debt + Equity);
		public double DebtWeight => Debt / (Debt + Equity);
	}
}
