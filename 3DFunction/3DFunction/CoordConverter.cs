using System;

namespace _3DFunction
{
	public class CoordConverter
	{
		public int CenterX { get; private set; }
		public int CenterY { get; private set; }

		public CoordConverter(int centerX, int centerY)
		{
			CenterX = centerX;
			CenterY = centerY;
		}

		public ScreenPoint ToPlaneCoord(double x, double y, double z)
		{
			return new ScreenPoint(PlaneX(x, y, z) + CenterX, PlaneY(x, y, z) + CenterY);
		}

		private int PlaneX(double x, double y, double z)
		{
			return (int) (-x/(2*Math.Sqrt(2)) + y);
		}

		private int PlaneY(double x, double y, double z)
		{
			return (int) (x/(2*Math.Sqrt(2)) - z);
		}
	}
}
