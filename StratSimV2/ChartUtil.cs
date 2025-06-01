
using QFin;
using QFin.Securities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.UI.DataVisualization.Charting;
using System.Xml.Linq;

namespace StratSimV2
{
	public static class ChartUtil
	{
		public static void SyncAxesWithPercent(this Chart chart, double initialCap)
		{ 
			var ySeries = chart.Series.Where(x => x.YAxisType == AxisType.Primary);
			Series y2Series = GetDefaultSeries("Percent", true);
			y2Series.YAxisType = AxisType.Secondary;
			foreach (var point in ySeries.First().Points)
			{
				double dollar = point.YValues[0];
				double percent = (dollar / initialCap);
				y2Series.Points.AddXY(point.XValue, percent);
			}
			double yMin = ySeries.Min(x => x.Points.Min(k => k.YValues[0]));
			double yMax = ySeries.Max(x => x.Points.Max(k => k.YValues[0]));
			double padding = (yMax - yMin) * 0.1;
			yMin -= padding;
			yMax += padding;

			chart.ChartAreas[0].AxisY.Minimum = yMin;
			chart.ChartAreas[0].AxisY.Maximum = yMax;
			chart.Series.Add(y2Series);
		}
		public static void ClearIndicators(this Chart chart)
		{
			chart.ChartAreas[0].AxisX.StripLines.Clear();
			chart.ChartAreas[0].AxisY.StripLines.Clear();
			chart.ChartAreas[0].AxisY2.StripLines.Clear();
		}
		public static Color UpColor = Color.FromArgb(34, 171, 148);
		public static Color DownColor = Color.FromArgb(242, 54, 69);
		public static void AddLegend(this Chart chart)
		{
			var legend = new Legend()
			{
				Docking = Docking.Bottom,
				Alignment = StringAlignment.Center,
				BackColor = Color.Transparent,
				ForeColor = Color.White
			};
			legend.Font = new Font(legend.Font, FontStyle.Bold);
			chart.Legends.Add(legend);
		}
		public static void AddBar(this Chart chart, Dictionary<DateTime, double> values, string name)
		{
			var series = new Series()
			{
				ChartType = SeriesChartType.Column,
				XValueType = ChartValueType.DateTime,
				ToolTip = "X = #VALX, Y = #VALY",
				LegendText = name
			};
			foreach (var value in values)
			{
				series.Points.AddXY(value.Key, value.Value);
			}
			chart.Series.Add(series);

			if (!chart.Legends.Any())
			{
				var legend = new Legend()
				{
					Docking = Docking.Bottom,
					Alignment = StringAlignment.Center,
					Font = new Font("Arial", 10f),
					IsDockedInsideChartArea = false,
					BackColor = Color.Transparent,
					ForeColor = Color.White
				};
				chart.Legends.Add(legend);
			}
		}
		public static void AddBar(this Chart chart, Dictionary<string, double> values)
		{
			var series = new Series()
			{
				ChartType = SeriesChartType.Column,
				ToolTip = "X = #VALX, Y = #VALY"
			};
			foreach (var value in values)
			{
				series.Points.AddXY(value.Key, value.Value);
			}
			chart.Series.Add(series);
		}
		public static void AddHistogram(this Chart chart, List<double> values, int bins)
		{
			var histo = HistogramBuilder.BuildHistogram(values, bins);
			var series = new Series()
			{
				ChartType = SeriesChartType.Column,
				ToolTip = "X = #VALX, Y = #VALY"
			};
			series.Points.DataBind(histo, "KEY", "VALUE", "");
			chart.ChartAreas[0].AxisY.LabelStyle.Format = "N0";
			chart.Series.Add(series);
			
		}
		public static void XFormat(this Chart chart, string format) => chart.ChartAreas[0].AxisX.LabelStyle.Format = format;
		public static void Y2Format(this Chart chart, string format) => chart.ChartAreas[0].AxisY2.LabelStyle.Format = format;
		private static Series GetDefaultSeries(string name, bool useMarker)
		{
			var series =  new Series(name)
			{
				ChartType = SeriesChartType.Line,
				BorderWidth = 2,
				XValueType = ChartValueType.DateTime,
				ToolTip = "X = #VALX, Y = #VALY"
			};
			if (useMarker)
			{
				series.MarkerStyle = MarkerStyle.Circle;
				series.MarkerSize = 7;
			}
			return series;
		}
		public static void AddSeries(this Chart chart, List<StockPrice> prices, string name = "Path", bool useMarkers = false)
		{
			var series = GetDefaultSeries(name, useMarkers);
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
		public static void AddSeries(this Chart chart, List<AmountPoint> points, string name = "Path", bool useMarkers = false)
		{
			var series = GetDefaultSeries(name, useMarkers);
			if (points.First().Amount < points.Last().Amount)
				series.Color = UpColor;
			else
				series.Color = DownColor;
			foreach (var item in points)
			{
				series.Points.AddXY(item.Timestamp.ToOADate(), item.Amount);
			}
			chart.Series.Add(series);

			chart.ChartAreas[0].RecalculateAxesScale();

		}
		public static void AddSeries(this Chart chart, List<double> vals, string name = "Path", bool useMarkers = false)
		{
			var series = GetDefaultSeries(name, useMarkers);
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
				StripWidth = 0,                  // Small width to simulate a line
				BorderColor = color,
				BorderWidth = 2,
				BackColor = Color.Transparent // No fill
			};
			if (axis == "X")
				chart.ChartAreas[0].AxisX.StripLines.Add(line);
			else if (axis == "Y")
				chart.ChartAreas[0].AxisY.StripLines.Add(line);
			else if (axis == "Y2")
				chart.ChartAreas[0].AxisY2.StripLines.Add(line);
			chart.ChartAreas[0].RecalculateAxesScale();
		}
		public static void AddSeries(this Chart chart, List<StockPrice> prices, int index = 1, bool useMarkers = false)
		{
			var series = GetDefaultSeries($"Path {index}", useMarkers);
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
		public static void AddSeries(this Chart chart, List<AmountPoint> prices, int index = 1, bool useMarkers = false)
		{
			var series = GetDefaultSeries($"Path {index}", useMarkers);
			if (prices.First().Amount < prices.Last().Amount)
				series.Color = UpColor;
			else
				series.Color = DownColor;
			foreach (var item in prices)
			{
				series.Points.AddXY(item.Timestamp.ToOADate(), item.Amount);
			}
			chart.Series.Add(series);
			chart.ChartAreas[0].RecalculateAxesScale();
		}
		public static void AddSeries(this Chart chart, Dictionary<double, double> dict, Color color,
			string name = "Path", AxisType axisType = AxisType.Primary, bool useMarkers = false)
		{
			var series = GetDefaultSeries(name, useMarkers);
			series.XValueType = ChartValueType.Int64;
			series.Color = color;
			series.YAxisType = axisType;
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
		public static void AddSeries(this Chart chart, List<AmountPoint> points, Color color, string name = "Path",
			AxisType axisType = AxisType.Primary, bool useMarkers = false)
		{
			var series = GetDefaultSeries(name, useMarkers);
			series.XValueType = ChartValueType.DateTime;
			series.Color = color;
			series.YAxisType = axisType;
			foreach (var item in points)
			{
				series.Points.AddXY(item.Timestamp.ToOADate(), item.Amount);
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