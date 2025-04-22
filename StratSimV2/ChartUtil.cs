using QFin.Securities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI.DataVisualization.Charting;

namespace StratSimV2
{
	public static class ChartUtil
	{
		public static Color UpColor = Color.FromArgb(34, 171, 148);
		public static Color DownColor = Color.FromArgb(242, 54, 69);
		public static void AddSeries(this Chart chart, List<StockPrice> prices, string name = "Path")
		{
			var series = new Series(name)
			{
				ChartType = SeriesChartType.Line,
				BorderWidth = 2,
				XValueType = ChartValueType.Date
			};
			if (prices.First().Price < prices.Last().Price)
				series.Color = UpColor;
			else
				series.Color = DownColor;
			foreach (var item in prices)
			{
				series.Points.AddXY(item.Timestamp.ToOADate(), item.Price);
			}
			chart.Series.Add(series);

			chart.ChartAreas[0].RecalculateAxesScale();
		}
		public static void AddSeries(this Chart chart, List<double> vals, string name = "Path")
		{
			var series = new Series(name)
			{
				ChartType = SeriesChartType.Line,
				BorderWidth = 2,
				XValueType = ChartValueType.Date
			};
			if (vals.First() < vals.Last())
				series.Color = UpColor;
			else
				series.Color = DownColor;
			for (int i = 0; i < vals.Count; i++)
			{
				series.Points.AddXY(i + 1, vals[i]);
			}
			series.XValueType = ChartValueType.Int32;
			chart.Series.Add(series);
			chart.ChartAreas[0].RecalculateAxesScale();
		}
		public static void AddIndicator(this Chart chart, string axis, double x, Color color)
		{
			var line = new StripLine
			{
				IntervalOffset = x,           // Y-axis position
				StripWidth = 0.001,                  // Small width to simulate a line
				BorderColor = color,
				BorderWidth = 10,
				BackColor = Color.Transparent       // No fill
			};
			if (axis == "X")
				chart.ChartAreas[0].AxisX.StripLines.Add(line);
			else if (axis == "Y")
				chart.ChartAreas[0].AxisY.StripLines.Add(line);
			chart.ChartAreas[0].RecalculateAxesScale();
		}
		public static void AddSeries(this Chart chart, List<StockPrice> prices, int index = 1)
		{
			var series = new Series($"Path {index}")
			{
				ChartType = SeriesChartType.Line,
				BorderWidth = 2,
				XValueType = ChartValueType.Date
			};
			if (prices.First().Price < prices.Last().Price)
				series.Color = UpColor;
			else
				series.Color = DownColor;
			foreach (var item in prices)
			{
				series.Points.AddXY(item.Timestamp.ToOADate(), item.Price);
			}
			chart.Series.Add(series);
			chart.ChartAreas[0].RecalculateAxesScale();
		}
		public static void AddSeries(this Chart chart, Dictionary<double, double> dict, Color color,
			string name = "Path", AxisType axisType = AxisType.Primary)
		{
			var series = new Series(name)
			{
				ChartType = SeriesChartType.Line,
				BorderWidth = 2,
				XValueType = ChartValueType.Int64,
				Color = color,
				YAxisType = axisType
			};
			foreach (var item in dict)
			{
				series.Points.AddXY(item.Key, item.Value);
			}
			chart.Series.Add(series);
			chart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Black;
			chart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Black;
			chart.ChartAreas[0].AxisY2.MajorGrid.Enabled = true;
			chart.ChartAreas[0].RecalculateAxesScale();

		}
		public static void XLabel(this Chart chart, string name)
		{
			chart.ChartAreas[0].AxisX.Title = name;
		}
		public static void YLabel(this Chart chart, string name)
		{
			chart.ChartAreas[0].AxisY.Title = name;
		}
		public static void YLabel(this Chart chart, string name, Color color)
		{
			chart.ChartAreas[0].AxisY.Title = name;
			chart.ChartAreas[0].AxisY.TitleForeColor = color;
		}
		
		public static void Y2Label(this Chart chart, string name, Color color)
		{
			chart.ChartAreas[0].AxisY2.Title = name;
			chart.ChartAreas[0].AxisY2.TitleForeColor = color;
		}

	}
}