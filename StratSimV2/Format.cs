using MathNet.Numerics.Integration;
using MathNet.Numerics.LinearRegression;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using static System.Net.Mime.MediaTypeNames;

namespace StratSimV2
{
	public class Format
	{
		public static string GetSimLabel(int contracts, double marketPrice, double exit, double pl, double ratio, DateTime expiry, double tp = -1, double closeDTE = -1)
		{
			string color = pl > 0 ? "#22ab94" : "#f23645";
			var sbRes = new StringBuilder();
			sbRes.Append(LabelMoney("Entry Price", marketPrice));
			sbRes.Append(LabelMoney("Close Price", exit));
			if (tp != -1)
			{
				sbRes.Append(LabelMoney("Take Profit", (tp + 1) * marketPrice));
			}
			if (closeDTE != -1)
			{
				sbRes.Append(Label("Close DTE", expiry.AddDays(-closeDTE).ToShortDateString()));
			}
			sbRes.Append(Label("Contracts", contracts));
			sbRes.Append(LabelMoney("PL", pl, ratio, color));
			return sbRes.ToString();
		}
		public static string Label<T>(string label, T text)
		{
			return $"{LabelName(label)} {Content(text)}<br/>";
		}
		public static string LabelNumber(string label, double val, bool longMultiplier = true, string format = "C")
		{
			Dictionary<Multiplier, char> multDict = new Dictionary<Multiplier, char>()
			{
				{
					Multiplier.Thousand, 'K'
				},
				{
					Multiplier.Million, 'M'
				},
				{
					Multiplier.Billion, 'B'
				},
				{
					Multiplier.Trillion, 'T'
				}
			};
			var multiplier = Multiplier.None;
			const double T = 1_000_000_000_000;
			const double b = 1_000_000_000;
			const double m = 1_000_000;
			const double k = 1_000;
			if (val >= T)
			{
				multiplier = Multiplier.Trillion;
				val /= T;
			}
			else if (val >= b)
			{
				multiplier = Multiplier.Billion;
				val /= b;
			}
			else if (val >= m)
			{
				multiplier = Multiplier.Million;
				val /= m;
			}
			else if (val >= k)
			{
				multiplier = Multiplier.Thousand;
				val /= k;
			}
			string multText = multiplier.ToString();
			if (multiplier == Multiplier.None)
			{
				multText = "";
			}
			if (longMultiplier)
			{
				multText = multDict[multiplier].ToString();
			}
			return $"{LabelName(label)} {Content($"{val.ToString(format)} {multText}")}<br/>";
		}
		private static string Content<T>(T text, string color = "white")
		{
			return $"<span style='color:{color};font-size:1.1em'>{text}</span>";
		}
		public static string Label(string label, string text, string color)
		{ 
			return $"{LabelName(label)} {Content(text, color)}<br/>";
		}
		public static string LabelMoney(string label, double text, string color)
		{
			if (color == "red")
				color = "#f23645";
			else if (color == "green")
				color = "#22ab94";
			return $"{LabelName(label)} {Content($"{text:C}", color)}<br/>";
		}
		public static string LabelMoney(string label, double pl, double perc, string color = "White")
		{
			color = pl > 0 ? "#22ab94" : "#f23645";
			return $"{LabelName(label)} {Content($"{pl:C} ({perc:P2})", color)}<br/>";
		}
		public static string LabelMoney(string label, double text)
		{
			return $"{LabelName(label)} {Content($"{text:C}")}<br/>";
		}
		public static string LabelPercent(string label, double text)
		{
			return $"{LabelName(label)} {Content($"{text:P2}")}<br/>";
		}
		private static string LabelName(string label)
		{
			return $"<small style='color:#AAAAAA'><b>{label}:</b></small>";
		}
	}
}