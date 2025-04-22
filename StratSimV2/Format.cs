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
				sbRes.Append(LabelMoney("Take Profit", (tp * 2) * marketPrice));
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
			return $"{LabelName(label)} <span style='font-size:1.1em'>{text}</span><br/>";
		}
		public static string Label(string label, string text, string color)
		{
			return $"{LabelName(label)} <span style='color:{color};font-size:1.1em'>{text}</span><br/>";
		}
		public static string LabelMoney(string label, double text, string color)
		{
			return $"{LabelName(label)} <span style='color:{color};font-size:1.1em'>{text:C}</span><br/>";
		}
		public static string LabelMoney(string label, double pl, double perc, string color)
		{
			return $"{LabelName(label)} <span style='color:{color};font-size:1.1em'>{pl:C} ({perc:P2})</span><br/>";
		}
		public static string LabelMoney(string label, double text)
		{
			return $"{LabelName(label)} <span style='font-size:1.1em'>{text:C}</span><br/>";
		}
		public static string LabelPercent(string label, double text)
		{
			return $"{LabelName(label)} <span style='font-size:1.1em'>{text:P2}</span><br/>";
		}
		private static string LabelName(string label)
		{
			return $"<small><b>{label}:</b></small>";
		}
	}
}