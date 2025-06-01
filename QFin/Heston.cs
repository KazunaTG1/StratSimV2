using MathNet.Numerics;
using MathNet.Numerics.Distributions;
using QFin.MarketData.Transformations;
using QFin.Securities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QFin
{
	namespace Models
	{
		namespace Stochastic
		{
			public class Heston
			{
				/// <summary>
				/// Simulates a theoretical path of stock prices based on the Heston model.
				/// </summary>
				/// <param name="s0">Initial stock price</param>
				/// <param name="v0">Initial variance</param>
				/// <param name="t">Time in years</param>
				/// <param name="kappa">Speed of return of variance to the mean</param>
				/// <param name="theta">Long-term average variance</param>
				/// <param name="sigma">Volatility of variance</param>
				/// <param name="rho">Correlation between stock and volatility</param>
				/// <param name="mu">Expected Return / Risk-free rate</param>
				/// <returns>A list of stock prices</returns>
				public static List<StockPrice> GetPath(double s0, double v0, double t, 
					double kappa, double theta, double sigma , double rho, double mu, Interval interval,
					out List<AmountPoint> volPath)
				{
					var stockPath = new List<StockPrice>()
					{
						new StockPrice(DateTime.Now, s0)
					};
					var varPath = new List<AmountPoint>()
					{
						new AmountPoint(DateTime.Now, v0)
					};
					volPath = new List<AmountPoint>
					{
						new AmountPoint(DateTime.Now, Math.Sqrt(v0))
					};
		
					int days = (int)(365 * t);
					int totalSteps = ChartInterval.GetSteps(interval, days);
					double dt = t / totalSteps;
					var currDate = DateTime.Now;
					for (int i = 1; i < totalSteps; i++)
					{
						currDate = currDate.AddDays(dt * 365);
						double z1 = Normal.Sample(0, 1);
						double vt = GetVarianceBM(varPath[i - 1].Amount, kappa, theta, sigma, rho, z1, dt);
						double st = GetStockBM(stockPath[i -1].Price, vt, dt, mu, z1);
						stockPath.Add(new StockPrice(currDate, st));
						varPath.Add(new AmountPoint(currDate, vt));
						volPath.Add(new AmountPoint(currDate, Math.Sqrt(vt)));
					}
					return stockPath;
				}
				/// <summary>
				/// Simulates the next price in a geometric brownian motion
				/// </summary>
				/// <param name="s0">Stock price</param>
				/// <param name="v0">Variance</param>
				/// <param name="dt">Time in years</param>
				/// <param name="mu">Expected Return / Risk-free Rate</param>
				/// <param name="z">Brownian motion</param>
				/// <returns>A simulated price at the end of a time span</returns>
				private static double GetStockBM(double s0, double v0, double dt, double mu, double z)
				{
					return (s0 * Math.Exp((mu - 0.5 * v0) * dt + Math.Sqrt(v0 * dt) * z));
				}
				private static double GetVarianceBM(double v0, double kappa, double theta, double sigma, double rho, double z1, double dt )
				{
					double z = Normal.Sample(0, 1);
					double w2 = rho * z1 + Math.Sqrt(1 - Math.Pow(rho, 2)) * z;
					double newVar = v0 + kappa * (theta - v0) * dt + sigma * Math.Sqrt(Math.Max(v0, 0)) * Math.Sqrt(dt) * w2;
					double minVol = 0.05;
					double minVar = Math.Pow(minVol, 2);
					return Math.Max(minVar, newVar);
				}
			}
		}
	}
	
}
