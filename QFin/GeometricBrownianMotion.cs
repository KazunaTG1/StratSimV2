using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QFin.Securities;

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
				public static List<StockPrice> GBM(double stockPrice, double interest, double timeToExpiry, double volatility, DateTime startDate)
				{
					int totalSteps = (int)(365 * timeToExpiry);
					double dt = timeToExpiry / totalSteps;

					var path = new List<StockPrice>();

					double currentPrice = stockPrice;
					DateTime currentDate = startDate;
					while (currentDate < startDate.AddDays(totalSteps))
					{
						if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
						{
							path.Add(new StockPrice(currentDate, currentPrice));
							currentPrice = GetGBM(currentPrice, interest, dt, volatility);
						}

						currentDate = currentDate.AddDays(1);
					}
					return path;
				}
			}
		}
	}
	
}
