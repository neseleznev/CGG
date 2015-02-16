using System;

namespace PolygonDecomposition
{
	public struct Point2D
	{
		public readonly double X;
		public readonly double Y;

		public Point2D(double x, double y)
		{
			X = x;
			Y = y;
		}

		#region Arithmetical operations

		public static Point2D operator +(Point2D p1, Point2D p2)
		{
			return new Point2D(p1.X + p2.X, p1.Y + p2.Y);
		}

		public static Point2D operator -(Point2D p1, Point2D p2)
		{
			return new Point2D(p1.X - p2.X, p1.Y - p2.Y);
		}

		public static Point2D operator -(Point2D p)
		{
			return new Point2D(-p.X, -p.Y);
		}

		public static Point2D operator *(Point2D p, double l)
		{
			return new Point2D(p.X * l, p.Y * l);
		}

		public static Point2D operator *(double l, Point2D p)
		{
			return p * l;
		}

		public static Point2D operator /(Point2D p, double l)
		{
			return p * (1 / l);
		}

		public double MultiplyScalar(Point2D p)
		{
			return X * p.X + Y * p.Y;
		}

		#endregion

		#region Equals and GetHashCode implementation

		public static bool operator ==(Point2D a, Point2D b)
		{
			const double epsilon = 0.000001;
			return GetDistance(a, b) < epsilon;
		}

		public static bool operator !=(Point2D a, Point2D b)
		{
			return !(a == b);
		}

		public override bool Equals(object obj)
		{
			return (obj is Point2D) && Equals((Point2D)obj);
		}

		public bool Equals(Point2D other)
		{
			return this == other;
		}

		public override int GetHashCode()
		{
			int hashCode = 0;
			unchecked
			{
				hashCode += 1000000007*(int) Math.Round(X, 3);
				hashCode += 1000000009*(int) Math.Round(Y, 3);
			}
			return hashCode;
		}

		#endregion

		public static double GetDistance(Point2D a, Point2D b)
		{
			return Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
		}

		public double GetLenghtVector()
		{
			return Math.Sqrt(X * X + Y * Y);
		}

		public double GetDistanceTo(Point2D otherPoint)
		{
			return GetDistance(this, otherPoint);
			//return Math.Sqrt((X-otherPoint.X)*(X-otherPoint.X)+(Y-otherPoint.Y)*(Y-otherPoint.Y));
		}

		public override string ToString()
		{
			return String.Format("(X={0:0.000};Y{1:0.000})", X, Y);
		}
	}
}