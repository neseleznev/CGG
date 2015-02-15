using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolygonDecomposition;

namespace Testing
{
	[TestClass]
	public class NodeTests
	{
		[TestMethod]
		public void PolygonTest()
		{
			var point = new Point2D(0, 0);

			var node1 = new Node(point);
			var node2 = new Node(point);

			node1.NextNode = node2;
			node2.NextNode = node1;

			var expected = new[] {node1, node2};
			var actual = node1.Polygon.ToList();

			CollectionAssert.AreEqual(expected, actual);
		}
	}
}
