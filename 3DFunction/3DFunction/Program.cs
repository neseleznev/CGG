using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace _3DFunction
{
	class Program
	{
		const int WindowSize = 500;
		const double Accuracy = 0.1;
		private static CoordConverter Converter = new CoordConverter(250, 250);
		private static double ZoomCoef = 50;

		private static Bitmap Image;
		private static Graphics g;
		private static Pen penTop = new Pen(Color.DeepPink);
		private static Pen penBottom = new Pen(Color.Blue);

		static private void CreateImage1()
		{
			var topHorizon = new int[WindowSize];
			var bottomHorizon = new int[WindowSize];

			for (var i = 0; i < WindowSize; i++)
			{
				topHorizon[i] = int.MinValue;
				bottomHorizon[i] = int.MaxValue;
			}

			foreach (var x in Range(Constants.XTo, Constants.XFrom, Accuracy))
			{
				var z = Constants.Function(x, Constants.YFrom);
				var lastPoint = Converter.ToPlaneCoord(x * ZoomCoef, Constants.YFrom * ZoomCoef, z * ZoomCoef);
				foreach (var y in Range(Constants.YFrom, Constants.YTo, Accuracy / 100))
				{
					z = Constants.Function(x, y);
					var zoomedX = x * ZoomCoef;
					var zoomedY = y * ZoomCoef;
					var zoomedZ = z * ZoomCoef;
					var planePoint = Converter.ToPlaneCoord(zoomedX, zoomedY, zoomedZ);

					if (planePoint.X < 0 || planePoint.X >= WindowSize)
						continue;

					if (lastPoint.X == planePoint.X)
						continue;

					if (planePoint.Y >= topHorizon[planePoint.X])
					{
						topHorizon[planePoint.X] = planePoint.Y;
						//DrawPixel(image, planePoint.X, planePoint.Y);
						g.DrawLine(penTop, lastPoint.X, lastPoint.Y, planePoint.X, planePoint.Y);
					}

					if (planePoint.Y <= bottomHorizon[planePoint.X])
					{
						bottomHorizon[planePoint.X] = planePoint.Y;
						//DrawPixel(image, planePoint.X, planePoint.Y);
						g.DrawLine(penBottom, lastPoint.X, lastPoint.Y, planePoint.X, planePoint.Y);
					}
					lastPoint = planePoint;
				}
			}
		}

		static private void CreateImage2()
		{
			var topHorizon = new int[WindowSize];
			var bottomHorizon = new int[WindowSize];

			for (var i = 0; i < WindowSize; i++)
			{
				topHorizon[i] = int.MinValue;
				bottomHorizon[i] = int.MaxValue;
			}

			foreach (var y in Range(Constants.YTo, Constants.YFrom, Accuracy))
			{
				var z = Constants.Function(Constants.XFrom, y);
				var lastPoint = Converter.ToPlaneCoord(Constants.XFrom * ZoomCoef, y * ZoomCoef, z * ZoomCoef);
				foreach (var x in Range(Constants.XFrom, Constants.XTo, Accuracy/100))
				{
					z = Constants.Function(x, y);
					var zoomedX = x * ZoomCoef;
					var zoomedY = y * ZoomCoef;
					var zoomedZ = z * ZoomCoef;
					var planePoint = Converter.ToPlaneCoord(zoomedX, zoomedY, zoomedZ);

					if(planePoint.X < 0 || planePoint.X >= WindowSize)
						continue;

					if(lastPoint.X == planePoint.X)
						continue;

					if (planePoint.Y >= topHorizon[planePoint.X])
					{
						topHorizon[planePoint.X] = planePoint.Y;
						//DrawPixel(image, planePoint.X, planePoint.Y);
						g.DrawLine(penTop, lastPoint.X, lastPoint.Y, planePoint.X, planePoint.Y);
					}

					if (planePoint.Y <= bottomHorizon[planePoint.X])
					{
						bottomHorizon[planePoint.X] = planePoint.Y;
						//DrawPixel(image, planePoint.X, planePoint.Y);
						g.DrawLine(penBottom, lastPoint.X, lastPoint.Y, planePoint.X, planePoint.Y);
					}
					lastPoint = planePoint;
				}
			}
		}

		private static bool InDiapason(int val, int min, int max)
		{
			return (val < max && val > min);
		}

		private static void DrawPixel(Bitmap image, int screenX, int screenY)
		{
			if(InDiapason(screenY, 0, WindowSize))
				image.SetPixel(screenX, screenY, Color.Blue);
		}

		private static IEnumerable<double> Range(double from, double to, double accyracy)
		{
			if (from < to)
			{
				for (var i = from; i < to; i += accyracy)
					yield return i;
			}
			else
			{
				for (var i = from; i > to; i -= accyracy)
					yield return i;
			}
			yield return to;
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
			Image = new Bitmap(WindowSize + 1, WindowSize + 1);
			g = Graphics.FromImage(Image);
			g.FillRectangle(Brushes.Azure, 0, 0, Image.Width, Image.Height);

			CreateImage1();
			CreateImage2();

			// image.Save("img.png", ImageFormat.Png);

			ShowImageInWindow(Image);

		}
	}
}
