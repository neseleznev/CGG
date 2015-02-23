using System;

namespace _3DFunction
{
	static public class Constants
	{
		public static double XFrom = -3;
		public static double XTo = 3;

		public static double YFrom = -3;
		public static double YTo = 3;

		public static double Function(double x, double y)
		{
			return CosXY(x, y);
		}

		private static double CosXY(double x, double y)
		{
			return Math.Cos(x*y);
		}

		private static double Function2(double x, double y)
		{
			return x*x + y*y;
		}

		private static double Sphere(double x, double y)
		{
			var z =  Math.Sqrt(4 -(x * x) - (y * y));
			if (double.IsNaN(z))
				return 0;
			return z;
		}

		private static double Sum(double x, double y)
		{
			return x + y;
		}

		private static double X(double x, double y)
		{
			return x;
		}
	}
}
