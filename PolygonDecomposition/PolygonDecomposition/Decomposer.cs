using System;
using System.Collections.Generic;
using System.Linq;

namespace PolygonDecomposition
{
	static public class Decomposer
	{
		static public IEnumerable<Node> DecomposePolygon(Node polygon)
		{
			if (OrientedArea(polygon) < 0)
				polygon = RevertPolygon(polygon);
			return DecomposePolygonInternal(polygon);
		}

		static public IEnumerable<Node> DecomposePolygonInternal(Node polygon)
		{
			foreach (var node in polygon.Polygon)
			{
				var originalNodePoint = node.Point;
				PolygonGeometry.MovePolygon(node, new Point2D(-originalNodePoint.X, -originalNodePoint.Y));

				var angle = PolygonGeometry.VectorAngle(node);
				PolygonGeometry.RotatePolygon(polygon, -angle);

				if (node.NextNode.NextNode.Point.Y < -1e-3)// всё плохо и невыпукло
				{
					var firstNodeInPartitionableSegment = polygon.Polygon
						.Skip(1)
						.Where(SegmentCrossOX)
						.Where(someNode => someNode.Point.X > PolygonGeometry.Epsilon || someNode.NextNode.Point.X > PolygonGeometry.Epsilon)
						.Where(someNode => IntersectionOfOX(someNode) > 0)
						.OrderBy(IntersectionOfOX)
						.First();

					var newPolygons = Separate(node, firstNodeInPartitionableSegment);
					PolygonGeometry.RotatePolygon(newPolygons.Item1, angle);
					PolygonGeometry.RotatePolygon(newPolygons.Item2, angle);

					PolygonGeometry.MovePolygon(newPolygons.Item1, originalNodePoint);
					PolygonGeometry.MovePolygon(newPolygons.Item2, originalNodePoint);

					var partion = DecomposePolygon(newPolygons.Item1).ToList();
					partion.AddRange(DecomposePolygon(newPolygons.Item2));

					return partion;
				}

				PolygonGeometry.RotatePolygon(node, angle);
				PolygonGeometry.MovePolygon(node, originalNodePoint);
			}
			return new[] {polygon};
		}

		static public Tuple<Node, Node> Separate(Node separatingSegment, Node separatedSegment)
		{
			var a = separatingSegment.NextNode;
			var b = new Node(new Point2D(IntersectionOfOX(separatedSegment), 0));

			var aButtomCopy = new Node(a.Point);
			var bButtomCopy = new Node(b.Point);

			aButtomCopy.NextNode = a.NextNode;

			bButtomCopy.NextNode = aButtomCopy;
			a.NextNode = b;

			b.NextNode = separatedSegment.NextNode;
			separatedSegment.NextNode = bButtomCopy;

			a = RemoveRepeatingPoints(a);
			aButtomCopy = RemoveRepeatingPoints(aButtomCopy);
			return Tuple.Create(a, aButtomCopy);
		}

		static public bool SegmentCrossOX(Node node)
		{
			return //!SegmentLocatedOnOX(node) && 
				   node.Point.Y <= PolygonGeometry.Epsilon && 
				   node.NextNode.Point.Y >= -PolygonGeometry.Epsilon;
		}

		static public bool SegmentLocatedOnOX(Node node)
		{
			return Math.Abs(node.Point.Y) < 1e-10 && Math.Abs(node.NextNode.Point.Y) < 1e-10;
		}

		static public double IntersectionOfOX(Node node)
		{
			if (!SegmentCrossOX(node))
				throw new Exception("segment does no cross OX");

			return node.Point.X - 
				((node.NextNode.Point.X - node.Point.X) / (node.NextNode.Point.Y - node.Point.Y)) *
				node.Point.Y;

		}

		static public Node RevertPolygon(Node firstNode)
		{
			var pointList = firstNode.Polygon.Reverse().Select(node => node.Point);
			return PolygonGeometry.CreatePolygon(pointList);
		}

		static public double OrientedArea(Node firstNode)
		{
			var area = 0.0;
			foreach (var node in firstNode.Polygon)
			{
				area += node.Point.X*node.NextNode.Point.Y - node.Point.Y*node.NextNode.Point.X;
			}
			return area/2;
		}

		public static Node RemoveRepeatingPoints(Node firstNode)
		{
			var points = firstNode.Polygon.Select(node => node.Point).ToList();
			var distPoints = points.Distinct().ToList();
			return PolygonGeometry.CreatePolygon(distPoints);
		}
	}
}
