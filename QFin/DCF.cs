using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFin
{
	namespace Models
	{
		public class DCF
		{
			public static double GetPresentValue(double cf, double r, double t) => cf / Math.Pow(1 + r, t);
		}
	}
	
}
