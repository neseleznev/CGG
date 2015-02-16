using System;
using System.Collections.Generic;
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

		[TestMethod]
		public void AngleTest()
		{
			var point1 = new Point2D(1, 1);
			var point2 = new Point2D(3, 1);
			var point3 = new Point2D(3, 0);

			var triangle = PolygonGeometry.CreatePolygon(new[] { point1, point2, point3 });

			var node1 = triangle;
			var node2 = node1.NextNode;
			var node3 = node2.NextNode;

			var expected1 = Angle.FromGrad(0);
			var actual1 = PolygonGeometry.VectorAngle(node1);

			var expected2 = Angle.FromGrad(-90);
			var actual2 = PolygonGeometry.VectorAngle(node2);

			var expected3 = Angle.FromRad(Math.PI - Math.Atan(0.5));
			var actual3 = PolygonGeometry.VectorAngle(node3);

			Assert.AreEqual(expected1, actual1);
			Assert.AreEqual(expected2, actual2);
			Assert.AreEqual(expected3, actual3);
		}

		[TestMethod]
		public void RemoveRepeatingPointsTest()
		{
			var nodes = new List<Point2D>
			{
				new Point2D(1, 0),
				new Point2D(2, 0),
				new Point2D(1, 0),
				new Point2D(3, 1)
			};

			var firstNode = PolygonGeometry.CreatePolygon(nodes);

			var expectedNodes = new List<Point2D>
			{
				new Point2D(1, 0),
				new Point2D(2, 0),
				new Point2D(3, 1)
			};
			var expectedPolygon = PolygonGeometry.CreatePolygon(expectedNodes);
			var expected = Decomposer.RemoveRepeatingPoints(expectedPolygon).Polygon.ToList();

			var actual = Decomposer.RemoveRepeatingPoints(firstNode).Polygon.ToList();

			CollectionAssert.AreEqual(expected, actual);

		}
	}
}
