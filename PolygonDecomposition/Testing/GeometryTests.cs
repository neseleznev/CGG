using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolygonDecomposition;

namespace Testing
{
	[TestClass]
	public class GeometryTests
	{
		[TestMethod]
		public void CreationalTest()
		{
			var point1 = new Point2D(1, 1);
			var point2 = new Point2D(3, 1);
			var point3 = new Point2D(3, 0);

			var triangle = PolygonGeometry.CreatePolygon(new[] { point1, point2, point3 });


			var node1 = new Node(point1);
			var node2 = new Node(point2);
			var node3 = new Node(point3);

			node1.NextNode = node2;
			node2.NextNode = node3;
			node3.NextNode = node1;

			var expected = node1.Polygon.ToList();

			var actual = triangle.Polygon.ToList();

			CollectionAssert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void MoveTest()
		{
			var point1 = new Point2D(1, 1);
			var point2 = new Point2D(3, 1);
			var point3 = new Point2D(3, 0);

			var triangle = PolygonGeometry.CreatePolygon(new [] {point1, point2, point3});

			var expected = PolygonGeometry.CreatePolygon(new[]
			{
				new Point2D(2, 1),
				new Point2D(4, 1),
				new Point2D(4, 0)
			});

			PolygonGeometry.MovePolygon(triangle, new Point2D(1, 0));

			var actual = triangle;

			CollectionAssert.AreEqual(expected.Polygon.ToList(), actual.Polygon.ToList());
		}

		[TestMethod]
		public void RotateTest()
		{
			var point1 = new Point2D(1, 1);
			var point2 = new Point2D(3, 1);
			var point3 = new Point2D(3, 0);

			var triangle = PolygonGeometry.CreatePolygon(new[] { point1, point2, point3 });

			PolygonGeometry.RotatePolygon(triangle, Angle.FromGrad(-90));

			var expected = PolygonGeometry.CreatePolygon(new[]
			{
				new Point2D(1, -1),
				new Point2D(1, -3),
				new Point2D(0, -3)
			}).Polygon.ToList();

			var actual = triangle.Polygon.ToList();

			CollectionAssert.AreEqual(expected, actual);
		}
	}
}
