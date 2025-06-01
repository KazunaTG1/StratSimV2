using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QFin.Securities;
using System.Diagnostics;
using QFin.MarketData.Transformations;

namespace QFin
{
	namespace Models
	{
		namespace Stochastic
		{
			public class GeometricBrownianMotion
			{
				public static double GetGBM(double stockPrice, double interest, double timeToExpiry, double volatility)
				{
					double z = new Normal(0, 1).Sample();
					return stockPrice * Math.Exp((interest - 0.5 * volatility * volatility) * timeToExpiry + volatility * Math.Sqrt(timeToExpiry) * z);
				}
				public static double GetAsymmetricGBM(
					double stockPrice, double interest,
					double putIV, double callIV, double dt)
				{
					

					double z = new Normal(0, 1).Sample();
					double sigma = z >= 0 ? callIV : putIV;
					double drift = (interest - 0.5 * sigma * sigma) * dt;
					double diffusion = sigma * Math.Sqrt(dt) * z;
					return stockPrice * Math.Exp(drift + diffusion);
				}
				public static List<StockPrice> AsymmetricGBM(
					double stockPrice, double interest, 
					double putIV, double callIV, double years, int steps)
				{
					List<StockPrice> prices = new List<StockPrice>()
					{
						new StockPrice(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 31, 0), stockPrice)
					};
					double dt = years / steps;
					DateTime currentDate = prices.Last().Timestamp;
					for (int t = 1; t <= steps; t++)
					{
						var lastPrice = prices.Last();
						
						if ((currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday) ||
							(currentDate.Hour > 16.5 && currentDate.Hour < 9.5))
						{
							currentDate = currentDate.AddDays(dt * 365);
							prices.Add(new StockPrice(currentDate, GetAsymmetricGBM(lastPrice.Price, interest, putIV, callIV, dt)));

						}
						else
						{
							currentDate = currentDate.AddDays(1);
						}

					}
					return prices;
				}
				public static List<StockPrice> AsymmetricGBM(
					double s0, double r, double putIV, double callIV, double years, Interval interval)
				{
					int days = (int)(years * 365);
					int totalSteps = ChartInterval.GetSteps(interval, days);
					return AsymmetricGBM(s0, r, putIV, callIV, years, totalSteps);
				}
				public static List<StockPrice> GBM(double stockPrice, double interest, double timeToExpiry, double volatility, DateTime startDate,
					Interval interval)
				{
					int days = (int)(365 * timeToExpiry);
					int totalSteps = ChartInterval.GetSteps(interval, days);
					double dt = timeToExpiry / totalSteps;

					var path = new List<StockPrice>();

					double currentPrice = stockPrice;
					DateTime currentDate = startDate;
					while (currentDate < startDate.AddDays(days))
					{
						if ((currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday) ||
							(currentDate.Hour > 16.5 && currentDate.Hour < 9.5))
						{
							path.Add(new StockPrice(currentDate, currentPrice));
							currentPrice = GetGBM(currentPrice, interest, dt, volatility);
							currentDate = currentDate.AddDays(dt * 365);
						}
						else
						{
							currentDate = currentDate.AddDays(1);
						}

						
					}
					return path;
				}
			}
		}
	}
	
}
