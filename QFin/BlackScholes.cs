using MathNet.Numerics;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.RootFinding;
using Microsoft.SqlServer.Server;
using QFin.Securities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFin
{
	namespace Models
	{
		namespace OptionPricing
		{
			public enum TradeDirection
			{
				Long,
				Short
			}
			public enum Greek
			{
				Rho,
				Theta,
				Delta,
				Gamma,
				Vega
			}
			public class BlackScholes
			{
				public static double OptionGamma(OptionType type, double s0, double k, double t, double r, double iv)
				{
					double term1 = OptionDelta(type, s0, k, t, r, iv);
					double term2 = OptionDelta(type, s0+1, k, t, r, iv);
					return (term2 - term1).Round(8);
				}
				
				public static double OptionGammaRate(OptionType type, double s0, double k, double t, double r, double iv)
				{
					double term1 = OptionDelta(type, s0, k, t, r, iv);
					double term2 = OptionDelta(type, s0 + 1, k, t, r, iv);
					if ((term2 - term1).Round(3) == 0)
						return 0;
					return ((term2 / term1) - 1).Round(10);
				}
				public static double OptionDelta(OptionType type, double s0, double k, double t, double r, double iv)
				{
					double term1 = OptionPrice(type, s0, k, t, r, iv);
					double term2 = OptionPrice(type, s0+1, k, t, r, iv);
					return (term2 - term1).Round(5);
				}
				public static double OptionDeltaRate(OptionType type, double s0, double k, double t, double r, double iv)
				{
					double term1 = OptionPrice(type, s0, k, t, r, iv);
					double term2 = OptionPrice(type, s0 + 1, k, t, r, iv);
					if ((term2 - term1).Round(3) == 0)
						return 0;
					return ((term2 / term1)-1).Round(10);	
				}
				public static double OptionRho(OptionType type, double s0, double k, double t, double r, double iv)
				{
					double term1 = OptionPrice(type, s0, k, t, r, iv);
					double term2 = OptionPrice(type, s0, k, t, r+0.01, iv);
					return (term2 - term1).Round(5);
				}
				public static double OptionRhoRate(OptionType type, double s0, double k, double t, double r, double iv)
				{
					double term1 = OptionPrice(type, s0, k, t, r, iv);
					double term2 = OptionPrice(type, s0, k, t, r + 0.01, iv);
					if ((term2 - term1).Round(3) == 0)
						return 0;
					return ((term2 / term1)-1).Round(10);
				}
				public static double OptionTheta(OptionType type, double s0, double k, double dte, double r, double iv)
				{
					double term1 = OptionPrice(type, s0, k, dte / 365, r, iv);
					double term2 = OptionPrice(type, s0, k, (dte -1) / 365, r, iv);

					return term2 - term1;
				}
				public static double OptionThetaRate(OptionType type, double s0, double k, double dte, double r, double iv)
				{
					double term1 = OptionPrice(type, s0, k, dte / 365, r, iv);
					double term2 = OptionPrice(type, s0, k, (dte - 1) / 365, r, iv);
					if ((term2 - term1).Round(3) == 0)
						return 0;
					return ((term2 / term1) - 1).Round(10);
				}
				public static double OptionVega(OptionType type, double s0, double k, double t, double r, double iv)
				{
					double term1 = OptionPrice(type, s0, k, t, r, iv);
					double term2 = OptionPrice(type, s0, k, t, r, iv + 0.01);
					return term2 - term1;
				}
				public static double OptionVegaRate(OptionType type, double s0, double k, double t, double r, double iv)
				{
					double term1 = OptionPrice(type, s0, k, t, r, iv);
					double term2 = OptionPrice(type, s0, k, t, r, iv + 0.01);
					if ((term2 - term1).Round(3) == 0)
						return 0;
					return ((term2 / term1) - 1).Round(10);
				}
				public static double MarketIV(OptionType type, double marketPrice, double s0, double k, double t, double r)
				{
					double priceDiff(double iv) => OptionPrice(type, s0, k, t, r, iv) - marketPrice;
					return Brent.FindRoot(priceDiff, 1e-5, 100.0);
				}
				public static double OptionPrice(OptionType type, double s0, double k, double t, double r, double iv)
				{
					if (t <= 0)
					{
						switch (type)
						{
							case OptionType.Call:
								return Math.Max(0, s0 - k);
							case OptionType.Put:
								return Math.Max(0, k - s0);
							default: return 0;
						}
					}
					double d1 = D1(s0, k, t, r, iv),
						d2 = D2(d1, t, iv),
						compRate = Math.Exp(-r * t);
					switch (type)
					{
						case OptionType.Call:
							return (s0 * CDF(d1)) - (k * compRate * CDF(d2)).Round(5);
						case OptionType.Put:
							return (k * compRate * CDF(-d2)) - (s0 * CDF(-d1)).Round(5);
						default:
							break;
					}
					return 0;
				}
				public static double PutPrice(double stockPrice, double strike, double timeToExpiration, double interestRate, double volatility)
				{
					return OptionPrice(OptionType.Put, stockPrice, strike, timeToExpiration, interestRate, volatility);
				}

				public static double CallPrice(double stockPrice, double strike, double timeToExpiration, double interestRate, double volatility)
				{
					return OptionPrice(OptionType.Call, stockPrice, strike, timeToExpiration, interestRate, volatility);
				}
				private static double CDF(double d)
				{
					return Normal.CDF(0, 1, d);
				}
				private static double D1(double stockPrice, double strike, double timeToExpiration, double interestRate, double volatility)
				{
					if (timeToExpiration <= 0) return 0;
					return (Math.Log(stockPrice / strike) + (interestRate + 0.5 * volatility * volatility)
						* timeToExpiration) / (volatility * Math.Sqrt(timeToExpiration));
				}
				private static double D2(double d1, double timeToExpiration, double volatility)
				{
					if (timeToExpiration <= 0) return 0;
					return d1 - volatility * Math.Sqrt(timeToExpiration);
				}
			}
		}
	}

}
