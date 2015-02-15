using System.Collections.Generic;
using System.Linq;

namespace PolygonDecomposition
{
	static public class PolygonGeometry
	{
		public static void MovePolygon(Node polygon, Point2D point)
		{
			foreach (var node in polygon.Polygon)
			{
				node.Point += point;
			}
		}

		public static void RotatePolygon(Node polygon, Angle angle)
		{
			foreach (var node in polygon.Polygon)
			{
				node.Point = new Point2D(
					angle.Cos * node.Point.X - angle.Sin * node.Point.Y,
					angle.Sin * node.Point.X + angle.Cos * node.Point.Y);
			}
		}

		public static Node CreatePolygon(IEnumerable<Point2D> points)
		{
			var firstNode = new Node(points.First());
			var previousNode = firstNode;
			var currentNode = firstNode;

			foreach (var point in points.Skip(1))
			{
				currentNode = new Node(point);
				previousNode.NextNode = currentNode;
				previousNode = currentNode;
			}

			currentNode.NextNode = firstNode;

			return firstNode;
		}
	}
}
