namespace _3DFunction
{
	public struct ScreenPoint
	{
		public readonly int X;
		public readonly int Y;

		public ScreenPoint(int x, int y)
		{
			X = x;
			Y = y;
		}

		#region Arithmetical operations

		public static ScreenPoint operator +(ScreenPoint p1, ScreenPoint p2)
		{
			return new ScreenPoint(p1.X + p2.X, p1.Y + p2.Y);
		}

		public static ScreenPoint operator -(ScreenPoint p1, ScreenPoint p2)
		{
			return new ScreenPoint(p1.X - p2.X, p1.Y - p2.Y);
		}

		public static ScreenPoint operator -(ScreenPoint p)
		{
			return new ScreenPoint(-p.X, -p.Y);
		}

		#endregion

		#region Equals and GetHashCode implementation

		public static bool operator ==(ScreenPoint a, ScreenPoint b)
		{
			return a.X == b.X && a.Y == b.Y;
		}

		public static bool operator !=(ScreenPoint a, ScreenPoint b)
		{
			return !(a == b);
		}

		public override bool Equals(object obj)
		{
			return (obj is ScreenPoint) && Equals((ScreenPoint)obj);
		}

		public bool Equals(ScreenPoint other)
		{
			return this == other;
		}

		public override int GetHashCode()
		{
			int hashCode = 0;
			unchecked
			{
				hashCode += 1000000007 * X;
				hashCode += 1000000009 * Y;
			}
			return hashCode;
		}

		#endregion


	}
}