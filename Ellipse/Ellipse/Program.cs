﻿using System;
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

		public static double SumDistToFocuses(int x, int y, double focus)
		{
			return Math.Sqrt(Math.Abs(y*y + (x - focus)*(x - focus))) + 
				   Math.Sqrt(Math.Abs(y*y + (x + focus)*(x + focus)));
		}

		private static void CreateImage(Bitmap image)
		{
			int screenX, screenY;
			double focus = Math.Sqrt(Math.Abs(XCoef*XCoef - YCoef*YCoef));
			screenY = YCoef;
			screenX = 0;
			double etalonSum = Math.Sqrt(YCoef*YCoef + focus*focus)*2;
			
			//DrawPixelSsshifted(image, 0, YCoef);
			for (screenX = 0; screenX <= XCoef && screenY > 0; )
			{
				var currentsum = SumDistToFocuses(screenX + 1, screenY - 1, focus);
				if (currentsum > etalonSum)//point inside
				{
					screenX++;
					DrawPixel1(image, screenX, screenY);
				}
				else//outside
				{
					screenY--;
					DrawPixel1(image, screenX, screenY);
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