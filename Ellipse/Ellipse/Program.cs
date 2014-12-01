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
		private static int xCoef = 250;
		private static int yCoef = 100;
		private static int middle = WindowSize / 2;

		private static void DrawPixel(Bitmap image, int screenX, int screenY)
		{
			image.SetPixel(middle + screenX, middle - screenY, Color.Blue);
		}

		private static void CreateImage(Bitmap image)
		{

			Point nextPoint, oldPoint;
			
			double pixelSize = Math.Max(xCoef, yCoef) / (double)middle;
			int screenX, screenY;
			int graphicSizeX, graphicSizeY;

			if(Math.Max(xCoef, yCoef) == xCoef)
			{
				graphicSizeX = middle;
				graphicSizeY = middle - (int)((xCoef - yCoef) / pixelSize);
			}
			else
			{
				graphicSizeX = middle - (int)((yCoef - xCoef) / pixelSize);
				graphicSizeY = middle;
			}


			screenY = graphicSizeY;
			for (screenX = 0; screenX < graphicSizeX;)
			{
				int deltaX = screenX;
				double realX = (deltaX + 1)*pixelSize;
				double realY = yCoef*Math.Sin(Math.Acos(realX/xCoef));
				double delta = ((screenX + 1) * (screenX + 1) + (screenY - 1) * (screenY - 1)) - (realX * realX + realY * realY);
				if(delta < 0)//point inside
				{
					if(2*delta + 2* screenY -1 > 0)
					{
						screenY--;
						screenX++;
						DrawPixel(image, screenX, screenY);
					}
					else
					{
						screenX++;
						DrawPixel(image, screenX, screenY);
					}
				}
				else//outside
				{
					if(2*delta - 2*screenX - 1 < 0)
					{
						screenX++;
						screenY--;
						DrawPixel(image, screenX, screenY);
					}
					else
					{
						screenY--;
						DrawPixel(image, screenX, screenY);
					}
				}

			}
			//screenY = (int)((F(XFrom) - minY) * WindowSize / (maxY - minY));
			//oldPoint = new Point(0, screenY);

			//for (screenX = 0; screenX < WindowSize; screenX++)
			//{
			//	x = (XFrom + screenX * (xTo - XFrom) / WindowSize);
			//	y = F(x);
			//	if (double.IsNaN(y))
			//		continue;
			//	screenY = (int)((maxY - y) * WindowSize / (maxY - minY));
			//	nextPoint = new Point(screenX, screenY);
			//	g.DrawLine(pen, oldPoint, nextPoint);
			//	//image.SetPixel(nextPoint.X, nextPoint.Y, Color.Blue);
			//	oldPoint = nextPoint;
			//}
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
