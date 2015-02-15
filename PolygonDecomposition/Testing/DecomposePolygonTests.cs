using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolygonDecomposition;

namespace Testing
{
	[TestClass]
	public class DecomposePolygonTests
	{
		[TestMethod]
		public void DecomposeTest1()
		{
			var point1 = new Point2D(1, 1);
			var point2 = new Point2D(2, 1);
			var point3 = new Point2D(2, 2);
			var point4 = new Point2D(1, 2);

			var polygon = PolygonGeometry.CreatePolygon(new[] { point1, point2, point3, point4 });
			var newPolygons = Decomposer.DecomposePolygon(polygon);

			var expected = polygon.Polygon.ToList();
			var actual = newPolygons.First().Polygon.ToList();

			CollectionAssert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void DecomposeTest2()
		{
			var point1 = new Point2D(1, 1);
			var point2 = new Point2D(2, 1);
			var point3 = new Point2D(2, 2);

			var polygon = PolygonGeometry.CreatePolygon(new[] { point1, point2, point3 });
			var newPolygons = Decomposer.DecomposePolygon(polygon);

			var expected = polygon.Polygon.ToList();
			var actual = newPolygons.First().Polygon.ToList();

			CollectionAssert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void DecomposeTest3()
		{
			var point1 = new Point2D(0, 0);
			var point2 = new Point2D(4, 0);
			var point3 = new Point2D(4, 4);
			var point4 = new Point2D(2, 2);
			var point5 = new Point2D(0, 4);

			var polygon = PolygonGeometry.CreatePolygon(new[] { point1, point2, point3, point4, point5 });
			var newPolygons = Decomposer.DecomposePolygon(polygon);

			var expected = new[]
			{
				PolygonGeometry.CreatePolygon(new[] {point4, point1, point2, point3}),
				PolygonGeometry.CreatePolygon(new[] {point4, point5, point1, point1})
			};
			var actual = newPolygons.ToList();

			CollectionAssert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Test()
		{
			var point1 = new Point2D(0, 0);
			var point2 = new Point2D(4, 0);
			var point3 = new Point2D(4, 4);
			var point4 = new Point2D(2, 2);
			var point5 = new Point2D(0, 4);

			var polygon = PolygonGeometry.CreatePolygon(new[] { point5, point1, point1, point4});
			PolygonGeometry.MovePolygon(polygon, new Point2D(0, -4));

			var expected = PolygonGeometry.CreatePolygon(new[]
			{
				point5 + new Point2D(0, -4), 
				point1 + new Point2D(0, -4),
				point1 + new Point2D(0, -4),
				point4 + new Point2D(0, -4), 
			}).Polygon.ToList();
			var actual = polygon.Polygon.ToList();

			CollectionAssert.AreEqual(expected, actual);
		}
	}
}
