using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Task5
{
	class Program
	{
		readonly static Bitmap Image = new Bitmap(Constants.WindowSize, Constants.WindowSize);
		static readonly Figure[] Figures = {DataProvider.GetPyramid(), DataProvider.GetCube()};

		private static void DrawPixel(int screenX, int screenY, Color color)
		{
			Image.SetPixel(screenX, screenY, color);
		}

		private static void CreateImage()
		{
			
			for (var screenY = 1; screenY < Constants.WindowSize; screenY++)
			{
				ProcessLine(screenY);
			}
		}

		private static void ProcessLine(int currentY)
		{
			var zBuf = new double[Constants.WindowSize];
			var colors = new Color[Constants.WindowSize];
			for (var i = 0; i < Constants.WindowSize; i++)
			{
				zBuf[i] = int.MaxValue;
				colors[i] = Color.Azure;
			}

			foreach (var figure in Figures)
			{
				ProcessFigureInLine(currentY, figure, zBuf, colors);
			}
		}

		private static void ProcessFigureInLine(int currentZ, Figure figure, double[] zBuf, Color[] colors)
		{
			foreach (var side in SidesIntersectsThePlane(figure, currentZ))
			{
				var intersectionPoints = IntersectionPoints(side, currentZ);

				if (intersectionPoints.Count <= 2)
				{
					var firstPoint = intersectionPoints.First();
					var secondPoint = intersectionPoints.Last();

					var firstScreenPoint = GetScreenPoint(firstPoint);
					var secondScreenPoint = GetScreenPoint(secondPoint);

					var vector = secondPoint - firstPoint;
					vector /= (Math.Abs(firstScreenPoint.X - secondScreenPoint.X));

					foreach (var i in Range(PutIntoScreen(firstScreenPoint.X), PutIntoScreen(secondScreenPoint.X)))
					{
						var distance = GetDistance(firstPoint);
						if (zBuf[i] > distance)
						{
							zBuf[i] = distance;
							colors[i] = side.Color;
						}
						firstPoint += vector;
					}
				}
				else
				{
					throw new Exception("Side is not convex");
				}
			}

			for (int i = 0; i < Constants.WindowSize; i++)
				DrawPixel(i, Constants.WindowSize - currentZ, colors[i]);
		}

		private static IEnumerable<int> Range(int from, int to)
		{
			if (from < to)
			{
				for (var i = from; i < to; i++)
					yield return i;
			}
			else
			{
				for (var i = from; i > to; i--)
					yield return i;
			}
			//yield return to;
		}

		private static int PutIntoScreen(int val)
		{
			return Math.Max(Math.Min(Constants.WindowSize - 1, val), 0);
		}

		private static double GetDistance(Point3D point3D)
		{
			if (point3D.X > Constants.DistToZero)
			{
				return int.MaxValue;
			}
			return Constants.DistToZero - point3D.X;
		}

		private static ScreenPoint GetScreenPoint(Point3D point3D)
		{
			return new ScreenPoint((int) point3D.Y, (int) (Constants.WindowSize - point3D.Z));
		}

		private static IEnumerable<Side> SidesIntersectsThePlane(Figure figure, double z)
		{
			return figure.Sides
				.Where(side => 
					side.Points.Any(point => point.Z >= z) && 
					side.Points.Any(point => point.Z <= z));

		}

		private static IList<Point3D> IntersectionPoints(Side side, double z)
		{
			var intersectionPoints = new List<Point3D>();

			var lastPoint = side.Points.Last();
			foreach (var point in side.Points)
			{
				if (lastPoint.Z >= z && point.Z <= z ||
					lastPoint.Z <= z && point.Z >= z)
				{
					if (Math.Abs(lastPoint.Z - point.Z) < Constants.Epsilon)
					{
						intersectionPoints.Add(lastPoint);
						intersectionPoints.Add(point);
					}
					else
					{
						var vector = point - lastPoint;
						vector *= (z - lastPoint.Z)/(point.Z - lastPoint.Z);
						intersectionPoints.Add(lastPoint + vector);
					}
				}
				lastPoint = point;
			}

			if (intersectionPoints.All(point => Math.Abs(point.Z - z) < Constants.Epsilon))
			{
				var orderedPoints = intersectionPoints.OrderBy(p => (int)p.Y);
				return new[] {orderedPoints.First(), orderedPoints.Last()};
			}

			return intersectionPoints.Distinct().ToList();
			
		}

		private static void ShowImageInWindow(Bitmap image)
		{
			var form = new Form
			{
				ClientSize = new Size(Constants.WindowSize, Constants.WindowSize)
			};
			form.Controls.Add(new PictureBox { Image = image, Dock = DockStyle.Fill, SizeMode = PictureBoxSizeMode.CenterImage });
			form.ShowDialog();
		}

		static void Main(string[] args)
		{
			var graphic = Graphics.FromImage(Image);
			graphic.FillRectangle(Brushes.Azure, 0, 0, Image.Width, Image.Height);

			CreateImage();

			// image.Save("img.png", ImageFormat.Png);

			ShowImageInWindow(Image);

		}
	}
}