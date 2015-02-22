
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace _3DFunction
{
	class Program
	{
		const int WindowSize = 500;
		const double Accuracy = 0.05;
		private static CoordConverter Converter = new CoordConverter(250, 250);
		private static double ZoomCoef = 100;

		private static Graphics g;
		private static Pen pen = new Pen(Color.Black);

		static private void CreateImage(Bitmap image)
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
				foreach (var y in Range(Constants.YFrom, Constants.YTo, Accuracy))
				{
					z = Constants.Function(x, y);
					var zoomedX = x * ZoomCoef;
					var zoomedY = y * ZoomCoef;
					var zoomedZ = z * ZoomCoef;
					var planePoint = Converter.ToPlaneCoord(zoomedX, zoomedY, zoomedZ);

					if(planePoint.X < 0 || planePoint.X >= WindowSize)
						continue;

					if (planePoint.Y >= topHorizon[planePoint.X])
					{
						topHorizon[planePoint.X] = planePoint.Y;
						//DrawPixel(image, planePoint.X, planePoint.Y);
						g.DrawLine(pen, lastPoint.X, lastPoint.Y, planePoint.X, planePoint.Y);
					}

					if (planePoint.Y <= bottomHorizon[planePoint.X])
					{
						bottomHorizon[planePoint.X] = planePoint.Y;
						//DrawPixel(image, planePoint.X, planePoint.Y);
						g.DrawLine(pen, lastPoint.X, lastPoint.Y, planePoint.X, planePoint.Y);
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
			Bitmap image = new Bitmap(WindowSize + 1, WindowSize + 1);
			g = Graphics.FromImage(image);
			g.FillRectangle(Brushes.Azure, 0, 0, image.Width, image.Height);

			CreateImage(image);

			// image.Save("img.png", ImageFormat.Png);

			ShowImageInWindow(image);

		}
	}
}
