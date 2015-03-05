using System.Collections.Generic;
using System.Drawing;

namespace Task5
{
	class Side
	{
		public IList<Point3D> Points { get; private set; }
		public Color Color { get; private set; }

		public Side(IList<Point3D> points, Color color)
		{
			Points = points;
			Color = color;
		}
	}
}
