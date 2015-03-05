using System.Drawing;

namespace Task5
{
	static class DataProvider
	{
		static public Figure GetPyramid()
		{
			var basePoint1 = new Point3D(10, 100, 20);
			var basePoint2 = new Point3D(10, 300, 20);
			var basePoint3 = new Point3D(-100, 320, 10);
			var basePoint4 = new Point3D(-100, 120, 10);
			var topPoint = new Point3D(-40, 200, 300);

			var frontSide = new Side(new []{basePoint1, basePoint2, topPoint}, Color.BlueViolet);
			var rightSide = new Side(new []{basePoint2, basePoint3, topPoint}, Color.Brown);
			var backSide = new Side(new []{basePoint3, basePoint4, topPoint}, Color.Chartreuse);
			var leftSide = new Side(new []{basePoint4, basePoint1, topPoint}, Color.DarkBlue);
			var bottomSide = new Side(new []{basePoint1, basePoint2, basePoint3, basePoint4}, Color.Chocolate);

			var pyramid = new Figure(new[] {frontSide, rightSide, backSide, leftSide, bottomSide});

			return pyramid;
		}

		static public Figure GetCube()
		{
			var bottomPoint1 = new Point3D(10, 100, 30);
			var bottomPoint2 = new Point3D(10, 300, 30);
			var bottomPoint3 = new Point3D(-100, 320, 50);
			var bottomPoint4 = new Point3D(-100, 120, 50);
			var topPoint1 = new Point3D(10, 100, 130);
			var topPoint2 = new Point3D(10, 300, 130);
			var topPoint3 = new Point3D(-100, 320, 150);
			var topPoint4 = new Point3D(-100, 120, 150);

			var frontSide = new Side(new[] {bottomPoint1, bottomPoint2, topPoint2, topPoint1}, Color.DarkRed);
			var rightSide = new Side(new[] {bottomPoint2, bottomPoint3, topPoint3, topPoint2}, Color.DarkBlue);
			var backSide = new Side(new[] {bottomPoint3, bottomPoint4, topPoint4, topPoint3}, Color.Cyan);
			var leftSide = new Side(new[] {bottomPoint1, bottomPoint4, topPoint4, topPoint1}, Color.DarkOliveGreen);
			var bottomSide = new Side(new[] {bottomPoint1, bottomPoint2, bottomPoint3, bottomPoint4}, Color.DarkSlateBlue); 
			var topSide = new Side(new[] {topPoint1, topPoint2, topPoint3, topPoint4}, Color.Fuchsia);

			var cube = new Figure(new[] {frontSide, rightSide, backSide, leftSide, bottomSide, topSide});
			return cube;
		}
	}
}
