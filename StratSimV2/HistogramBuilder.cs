using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StratSimV2
{
	public static class HistogramBuilder
	{
		public static Dictionary<string, int> BuildHistogram(List<double> payouts, int binCount = 30)
		{
			if (payouts == null || payouts.Count == 0)
				return new Dictionary<string, int>();

			double min = payouts.Min(),
				max = payouts.Max(),
				binSize = (max - min) / binCount;

			var bins = new Dictionary<string, int>();
			for (int i = 0; i < binCount; i++)
			{
				double lower = min + i * binSize,
					upper = i == binCount - 1 ? max : lower + binSize;
				string binLabel = $"[{lower:C0} - {upper:C0})";
				bins[binLabel] = 0;
			}
			foreach (var value in payouts)
			{
				int binIndex = Math.Min((int)((value - min) / binSize), binCount - 1);
				double lower = min + binIndex * binSize;
				double upper = binIndex == binCount - 1 ? max : lower + binSize;
				string binLabel = $"[{lower:C0} - {upper:C0})";
				bins[binLabel]++;
			}

			return bins;
		}
	}
}