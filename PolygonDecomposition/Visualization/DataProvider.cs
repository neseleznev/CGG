using System;
using System.Collections.Generic;
using System.Linq;
using ILNumerics;
using PolygonDecomposition;

namespace Visualization
{
	public static class DataProvider
	{
		public static List<Point2D> Nodes = new List<Point2D>
		{
			
			new Point2D(2, 1),
			new Point2D(1, 1),
			new Point2D(1, 2),
			new Point2D(0, 2),
			new Point2D(0, 0),
			new Point2D(5, 0),
			new Point2D(5, 2),
			new Point2D(4, 2),
			new Point2D(4, 1),
			new Point2D(3, 1),
			new Point2D(3, 2),
			new Point2D(2, 2)
		};

		public static ILArray<ILArray<float>> GetData()
		{
			var firstNode = PolygonGeometry.CreatePolygon(Nodes);
			var partion = Decomposer.DecomposePolygon(firstNode);

			var arr = new List<ILArray<float>>();
			foreach (var node in partion)
			{
				
				arr.Add(PolygonToILArray(node.Polygon));
			}
			return arr.ToArray();

		}

		private static ILArray<float> PolygonToILArray(IEnumerable<Node> polygon)
		{
			ILArray<float> arr1 =
				polygon.Select(node => (float) node.Point.X)
					.Concat(new[] {polygon.Select(node => (float) node.Point.X).First()})
					.ToArray();

			ILArray<float> arr2 =
				polygon.Select(node => (float) node.Point.Y)
					.Concat(new[] {polygon.Select(node => (float) node.Point.Y).First()})
					.ToArray();

			ILArray<float> arr = ILMath.zeros<float>(2, polygon.Count() + 1);
			arr["0;:"] = arr1;
			arr["1;:"] = arr2;
			return arr;
		}
	}
}
