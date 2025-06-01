using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFin
{
	/// <summary>
	/// Compound Annual Growth Rate
	/// </summary>
	public class CAGR
	{
		public double Years => (EndAmount.Timestamp - StartAmount.Timestamp).TotalDays / 365.0;
		public double GrowthDiff => EndAmount.Amount - StartAmount.Amount;
		public AmountPoint StartAmount { get; set; }
		public AmountPoint EndAmount { get; set; }
		public double GrowthRate => GetGrowthRate(StartAmount, EndAmount);
		/// <summary>
		/// Growth Rate
		/// </summary>
		/// <param name="startFlow">Starting Free Cash Flow</param>
		/// <param name="endFlow">Ending Free Cash Flow</param>
		/// <returns>The compound annual growth rate of free cash flow</returns>
		public static double GetGrowthRate(AmountPoint startFlow, AmountPoint endFlow)
		{
			double years = (endFlow.Timestamp - startFlow.Timestamp).TotalDays / 365.0 ;
			return Math.Pow(endFlow.Amount / startFlow.Amount, (1 / years)) - 1;
		}
		public CAGR(AmountPoint fcf1, AmountPoint fcf2)
		{
			StartAmount = fcf1;
			EndAmount = fcf2;
		}
	}
	public struct AmountPoint
	{
		public DateTime Timestamp { get; set; }
		public double Amount { get; set; }
		public AmountPoint(DateTime timestamp, double cash)
		{
			Timestamp = timestamp;
			Amount = cash;
		}
	}
}
