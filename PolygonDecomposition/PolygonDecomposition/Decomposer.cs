using System;
using System.Collections.Generic;

namespace PolygonDecomposition
{
	static public class Decomposer
	{
		//static IEnumerable<Node> DecomposePolygon(Node polygon)
		//{
		//	foreach (var node in polygon.Polygon)
		//	{
		//		PolygonGeometry.MovePolygon(polygon, new Point2D(-node.Point.X, -node.Point.Y));

		//		var cosA = node.NextNode.Point.X/node.NextNode.Point.GetDistanceTo(node.Point);
		//		var sinA = node.NextNode.Point.Y / node.NextNode.Point.GetDistanceTo(node.Point);

		//		var angle = sinA > 0 ? Angle.FromRad(Math.Acos(cosA)) : -Angle.FromRad(Math.Acos(cosA));
		//	}
		//}
	}
}
