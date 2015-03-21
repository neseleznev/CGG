using System;

namespace Task5
{
	public struct Point3D
	{
		public readonly double X;
		public readonly double Y;
		public readonly double Z;

		public Point3D(double x, double y, double z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		#region Arithmetical operations

		public static Point3D operator +(Point3D p1, Point3D p2)
		{
			return new Point3D(p1.X + p2.X, p1.Y + p2.Y, p1.Z +p2.Z);
		}

		public static Point3D operator -(Point3D p1, Point3D p2)
		{
			return new Point3D(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
		}

		public static Point3D operator -(Point3D p)
		{
			return new Point3D(-p.X, -p.Y, -p.Z);
		}

		public static Point3D operator *(Point3D p, double l)
		{
			return new Point3D(p.X * l, p.Y * l, p.Z * l);
		}

		public static Point3D operator *(double l, Point3D p)
		{
			return p * l;
		}

		public static Point3D operator /(Point3D p, double l)
		{
			return p * (1 / l);
		}

		public double MultiplyScalar(Point3D p)
		{
			return X * p.X + Y * p.Y;
		}

		#endregion

		#region Equals and GetHashCode implementation

		public static bool operator ==(Point3D a, Point3D b)
		{
			const double epsilon = 0.000001;
			return GetDistance(a, b) < epsilon;
		}

		public static bool operator !=(Point3D a, Point3D b)
		{
			return !(a == b);
		}

		public override bool Equals(object obj)
		{
			return (obj is Point3D) && Equals((Point3D)obj);
		}

		public bool Equals(Point3D other)
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
				hashCode += 1000000007*(int) Math.Round(Z, 3);
			}
			return hashCode;
		}

		#endregion

		public static double GetDistance(Point3D a, Point3D b)
		{
			return Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y) + (a.Z - b.Z) * (a.Z - b.Z));
		}

		public double GetDistanceTo(Point3D otherPoint)
		{
			return GetDistance(this, otherPoint);
		}

		public override string ToString()
		{
			return String.Format("(X={0:0.000};Y={1:0.000};Z={2:0.000}", X, Y, Z);
		}
	}
}