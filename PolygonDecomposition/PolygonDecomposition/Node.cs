using System.Collections.Generic;

namespace PolygonDecomposition
{
	public class Node
	{
		public Point2D Point { get; set; }
		public Node NextNode { get; set; }

		public Node(Point2D point)
		{
			Point = point;
		}

		public IEnumerable<Node> Polygon
		{
			get
			{
				var currentNode = this;

				do
				{
					yield return currentNode;
					currentNode = currentNode.NextNode;
				} while (currentNode != this);
			}
		}

		#region Equals and GetHashCode implementation

		public static bool operator ==(Node a, Node b)
		{
			return a.Point == b.Point && a.NextNode.Point == b.NextNode.Point;
		}

		public static bool operator !=(Node a, Node b)
		{
			return !(a == b);
		}

		public override bool Equals(object obj)
		{
			return (obj is Node) && Equals((Node)obj);
		}

		public bool Equals(Node other)
		{
			return this == other;
		}

		public override int GetHashCode()
		{
			int hashCode = 0;
			unchecked
			{
				hashCode += 1000000007 * Point.X.GetHashCode();
				hashCode += 1000000009 * Point.Y.GetHashCode();
			}
			return hashCode;
		}

		#endregion
	}
}
