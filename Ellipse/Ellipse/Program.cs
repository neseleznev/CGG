using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ellipse
{
	class Program
	{
		const int WindowSize = 500;
		private static Graphics Graphic;
		private const int XShift = 0;
		private const int YShift = 0;
		private const int XCoef = 200;
		private const int YCoef = 100;
		private const int Middle = WindowSize/2;

		private static bool InDiapason(int val, int min, int max)
		{
			return (val < max && val > min);
		}

		private static void DrawPixelSsshifted(Bitmap image, int screenX, int screenY)
		{
			if (InDiapason(XShift + Middle + screenX, 0, WindowSize) &&
				InDiapason(YShift + Middle - screenY, 0, WindowSize))
				image.SetPixel(XShift + Middle + screenX, YShift + Middle - screenY, Color.Blue);

			if (InDiapason(XShift + Middle + screenX, 0, WindowSize) &&
				InDiapason(YShift + WindowSize - Middle + screenY, 0, WindowSize))
				image.SetPixel(XShift + Middle + screenX, YShift + WindowSize - Middle + screenY, Color.Blue);

			if (InDiapason(XShift + Middle - screenX, 0, WindowSize) &&
				InDiapason(YShift + Middle - screenY, 0, WindowSize))
				image.SetPixel(XShift + Middle - screenX, YShift + Middle - screenY, Color.Blue);

			if (InDiapason(XShift + Middle - screenX, 0, WindowSize) &&
				InDiapason(YShift + WindowSize - Middle + screenY, 0, WindowSize))
				image.SetPixel(XShift + Middle - screenX, YShift + WindowSize - Middle + screenY, Color.Blue);
		}

		private static void DrawPixel(Bitmap image, int screenX, int screenY)
		{
			image.SetPixel(Middle + screenX, Middle - screenY, Color.Blue);
			image.SetPixel(Middle + screenX, WindowSize - Middle + screenY, Color.Blue);
			image.SetPixel(Middle - screenX, Middle - screenY, Color.Blue);
			image.SetPixel(Middle - screenX, WindowSize - Middle + screenY, Color.Blue);
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

			screenY = YCoef;
			DrawPixelSsshifted(image, 0, YCoef);
			for (screenX = 0; screenX <= XCoef && screenY >= 0; )
			{
				double realX = screenX + 1;
				double realY = YCoef * Math.Sin(Math.Acos(realX / XCoef));
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
			Graphic = Graphics.FromImage(image);
			Graphic.FillRectangle(Brushes.Azure, 0, 0, image.Width, image.Height);

			CreateImage(image);

			// image.Save("img.png", ImageFormat.Png);

			ShowImageInWindow(image);

		}
	}
}