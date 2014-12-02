using System;
using System.Windows.Forms;
using System.Drawing;

namespace FuncGraphic
{
	class Program
	{
		const int WindowSize = 500;
		private static Pen pen = new Pen(Color.Blue, 1);
		private static Graphics g;
		private static int xShift = 0;
		private static int yShift = 0;
		private static int xCoef = 200;
		private static int yCoef = 100;
		private static int middle = WindowSize / 2;

		private static bool InDiapason(int val, int min, int max)
		{
			return (val < max && val > min);
		}

		private static void DrawPixelSsshifted(Bitmap image, int screenX, int screenY)
		{
			if (InDiapason(xShift + middle + screenX, 0, WindowSize) &&
				InDiapason(yShift + middle - screenY, 0, WindowSize))
				image.SetPixel(xShift + middle + screenX, yShift + middle - screenY, Color.Blue);

			if (InDiapason(xShift + middle + screenX, 0, WindowSize) &&
				InDiapason(yShift + WindowSize - middle + screenY, 0, WindowSize))
				image.SetPixel(xShift + middle + screenX, yShift + WindowSize - middle + screenY, Color.Blue);

			if (InDiapason(xShift + middle - screenX, 0, WindowSize) &&
				InDiapason(yShift + middle - screenY, 0, WindowSize))
				image.SetPixel(xShift + middle - screenX, yShift + middle - screenY, Color.Blue);

			if (InDiapason(xShift + middle - screenX, 0, WindowSize) &&
				InDiapason(yShift + WindowSize - middle + screenY, 0, WindowSize))
				image.SetPixel(xShift + middle - screenX, yShift + WindowSize - middle + screenY, Color.Blue);
		}

		private static void DrawPixel(Bitmap image, int screenX, int screenY)
		{
			image.SetPixel(middle + screenX, middle - screenY, Color.Blue);
			image.SetPixel(middle + screenX, WindowSize - middle + screenY, Color.Blue);
			image.SetPixel(middle - screenX, middle - screenY, Color.Blue);
			image.SetPixel(middle - screenX, WindowSize - middle + screenY, Color.Blue);
		}

		private static void DrawPixel1(Bitmap image, int screenX, int screenY)
		{
			image.SetPixel(screenX, screenY, Color.Blue);
		}

		private static void CreateImage(Bitmap image)
		{
			int screenX, screenY;
			//int graphicSizeX = 0;
			//int graphicSizeY = 0;

			//if (xCoef > yCoef)
			//{
			//    graphicSizeX = middle;
			//    graphicSizeY = ((yCoef * graphicSizeX) / xCoef);
			//}
			//else
			//{
			//    graphicSizeY = middle;
			//    graphicSizeX = (xCoef * graphicSizeY) / yCoef;
			//}
			//xShift = (graphicSizeX * xShift) / xCoef;
			//yShift = (graphicSizeY * yShift) / yCoef;

			screenY = yCoef;
			DrawPixelSsshifted(image, 0, yCoef);
			for (screenX = 0; screenX <= xCoef && screenY >= 0; )
			{
				double realX = screenX + 1;
				double t = Math.Acos(realX / middle);
				double realY = yCoef * Math.Sin(Math.Acos(realX / xCoef));
				double radiusSqr = (realX * realX + realY * realY);
				double delta = ((screenX + 1) * (screenX + 1) + (screenY - 1) * (screenY - 1)) - radiusSqr;
				if (delta < 0)//point inside
				{
					if (2 * delta + 2 * screenY - 1 > 0)
					{
						//screenY--;
						screenX++;
						DrawPixelSsshifted(image, screenX, screenY);
					}
					else
					{
						screenX++;
						DrawPixelSsshifted(image, screenX, screenY);
					}
				}
				else//outside
				{
					if (2 * delta - 2 * screenX - 1 < 0)
					{
						//screenX++;
						screenY--;
						DrawPixelSsshifted(image, screenX, screenY);
					}
					else
					{
						screenY--;
						DrawPixelSsshifted(image, screenX, screenY);
					}
				}
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

			CreateImage(image);

			// image.Save("img.png", ImageFormat.Png);

			ShowImageInWindow(image);

		}

		public static int rightBorder { get; set; }
	}
}