using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace FuncGraphic
{
	class Program
	{
		const int WindowSize = 500;
		const int XFrom = -1;
		const int XTo = 4;
		private static Pen pen = new Pen(Color.Blue, 1);
		private static Pen axisPen = new Pen(Color.Black, 2);
		private static Graphics g;

		private delegate double FuncToShow(double x);

		private static FuncToShow myF = x => x*Math.Cos(x*x);

		private static double Cos1DivX(double x)
		{
			return Math.Cos(1 / x);
		}

		private static double XCube(double x)
		{
			return x * x * x;
		}

		private static double XIn5(double x)
		{
			return x * x * x * x * x;
		}

		private static Tuple<double, double> GetMinMaxValues(FuncToShow F, double xFrom, double xTo)
		{
			double maxY = F(XFrom);
			double minY = F(XFrom);
			int screenX;
			double x, y;
			for (screenX = 0; screenX < WindowSize; ++screenX)
			{
				x = (XFrom + screenX * (xTo - XFrom) / WindowSize);
				y = F(x);
				if (y < minY)
					minY = y;
				if (y > maxY)
					maxY = y;
			}
			return Tuple.Create(minY, maxY);
		}

		private static void DrowAxis(double minX, double maxX, double minY, double maxY)
		{
			if (minX < 0 && maxX > 0)
			{
				int screenX0 = (int)(-minX * WindowSize / (maxX - minX));
				g.DrawLine(axisPen, screenX0, WindowSize, screenX0, 0);
			}

			if (minY < 0 && maxY > 0)
			{
				int screenY0 = (int)(maxY * WindowSize / (maxY - minY));
				g.DrawLine(axisPen, 0, screenY0, WindowSize, screenY0);
			}
		}

		private static void CreateImage(Bitmap image, FuncToShow F, double xFrom, double xTo)
		{

			Tuple<double, double> minMax = GetMinMaxValues(F, XFrom, XTo);
			double minY = minMax.Item1;
			double maxY = minMax.Item2;

			DrowAxis(XFrom, XTo, minY, maxY);

			Point nextPoint, oldPoint;
			int screenX, screenY;
			double x, y;

			screenY = (int)((maxY - F(XFrom)) * WindowSize / (maxY - minY));
			oldPoint = new Point(0, screenY);

			for (screenX = 0; screenX < WindowSize; screenX++)
			{
				x = (XFrom + screenX * (xTo - XFrom) / WindowSize);
				y = F(x);
				if (double.IsNaN(y))
					continue;
				screenY = (int)((maxY - y) * WindowSize / (maxY - minY));
				nextPoint = new Point(screenX, screenY);
				g.DrawLine(pen, oldPoint, nextPoint);
				//image.SetPixel(nextPoint.X, nextPoint.Y, Color.Blue);
				oldPoint = nextPoint;
			}
		}

		private static void ShowImageInWindow(Bitmap image)
		{
			var form = new Form
			{
				ClientSize = new Size(WindowSize, WindowSize)
			};
			form.Controls.Add(new PictureBox { Image = image, Dock = DockStyle.Fill, SizeMode = PictureBoxSizeMode.CenterImage });
			form.ShowDialog();
		}

		static void Main(string[] args)
		{
			Bitmap image = new Bitmap(WindowSize + 1, WindowSize + 1);
			g = Graphics.FromImage(image);
			g.FillRectangle(Brushes.Azure, 0, 0, image.Width, image.Height);

			CreateImage(image, myF, XFrom, XTo);

			// image.Save("img.png", ImageFormat.Png);

			ShowImageInWindow(image);

		}
	}
}
