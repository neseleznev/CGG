using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Security.Cryptography;

namespace FuncGraphic
{
    class Program
    {
        private const double Eps = 1e-9;
        private delegate double FuncToShow(double x);
        static readonly Pen DefaultPen = new Pen(Color.DarkRed, 1);
        static readonly Pen DefaultAxisPen = new Pen(Color.DarkBlue, 3);
        static readonly Point DefaultWindowSize = new Point(600, 600);

        private static double SafeValue(FuncToShow f, double x)
        {
            try
            {
                return f(x);
            }
            catch (OverflowException)
            {
                return double.NaN;
            }
        }


        // Вычислим минимальное и максимальное значения функции на заданном интервале [xFrom; xTo]
		private static Tuple<double, double> GetMinMaxValues(FuncToShow f, double xFrom, double xTo, int windowWidth)
		{
			double maxY = double.MinValue,
                   minY = double.MaxValue;
		    double x, y;
		    for (int screenX = 0; screenX < windowWidth; ++screenX)
			{
				x = xFrom + screenX * (xTo - xFrom) / windowWidth;
			    y = SafeValue(f, x);
			    if (double.IsNaN(y) || double.IsInfinity(y))
			        continue;
				minY = Math.Min(y, minY);
				maxY = Math.Max(y, maxY);
			}
			return Tuple.Create(minY, maxY);
		}

        // Нарисуем необходимые оси в соответствии с диапазоном значений функции
        private static void DrawAxis(Graphics g, Pen axisPen, Point windowSize,
                                     double minX, double maxX, double minY, double maxY)
        {
            int? screenX = null, screenY = null;
            
            // Необходимость оси ординат
            if (Math.Abs(maxX) < Eps && Math.Abs(minX) < Eps)
                screenX = windowSize.Y / 2;
			if (minX < 0 && maxX > 0 && Math.Abs(maxX - minX) > Eps)
                screenX = (int)(-minX * windowSize.X / (maxX - minX));
            // Необходимость оси абсцисс
            if (Math.Abs(maxY) < Eps && Math.Abs(maxY) < Eps)
                screenY = windowSize.X/2;
            if (minY < 0 && maxY > 0)
		        screenY = (int)(maxY*windowSize.Y/(maxY - minY));

            if (screenX != null)
                g.DrawLine(axisPen, screenX.Value, windowSize.Y, screenX.Value, 0);
            if (screenY != null)
                g.DrawLine(axisPen, 0, screenY.Value, windowSize.X, screenY.Value);
		}

        // Отрисовка самого простого окна
        private static void ShowImageInWindow(Image image)
        {
            var form = new Form { ClientSize = new Size(image.Width, image.Height) };
            form.Controls.Add(new PictureBox
            {
                Image = image,
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.CenterImage
            });
            form.ShowDialog();
        }


        // Нарисовать f на [xFrom; xTo] (с ручками и размером окна по умолчанию)
        private static void DrawFunction(FuncToShow f, double xFrom, double xTo)
        {
            DrawFunction(f, xFrom, xTo, DefaultPen, DefaultAxisPen, DefaultWindowSize);
        }

        // Нарисовать f на [xFrom; xTo] ручкой pen, оси координат ручкой axisPen (размером окна по умолчанию)
        private static void DrawFunction(FuncToShow f, double xFrom, double xTo, Pen pen, Pen axisPen)
        {
            DrawFunction(f, xFrom, xTo, pen, axisPen, DefaultWindowSize);
        }

        // Нарисовать f на [xFrom; xTo] в окне размером windowSize (с ручками по умолчанию) 
        private static void DrawFunction(FuncToShow f, double xFrom, double xTo, Point windowSize)
        {
            DrawFunction(f, xFrom, xTo, DefaultPen, DefaultAxisPen, windowSize);
        }

        // Нарисовать f на [xFrom; xTo] ручкой pen, оси координат ручкой axisPen, размером окна windowSize
		private static void DrawFunction(FuncToShow f, double xFrom, double xTo, Pen pen, Pen axisPen, Point windowSize)
		{
		    if (windowSize.X*windowSize.Y == 0 || xFrom > xTo)
		        throw new ArgumentException(
                    "Неверные параметры! xFrom не должно быть больше xTo," +
                    "измерения windowSize не должны быть нулевыми.");

            var image = new Bitmap(windowSize.X, windowSize.Y);
            Graphics g = Graphics.FromImage(image);
            g.FillRectangle(Brushes.BlanchedAlmond, 0, 0, windowSize.X, windowSize.Y);

            Tuple<double, double> minMax = GetMinMaxValues(f, xFrom, xTo, windowSize.Y);
            double minY = minMax.Item1,
                   maxY = minMax.Item2;

            DrawAxis(g, axisPen, windowSize, xFrom, xTo, minY, maxY);

            // Если нужно нарисовать 1 точку по x
		    if (Math.Abs(xFrom - xTo) < Eps)
		    {
                // Если значение в ней определено
		        var y = SafeValue(f, xFrom);
		        if (!double.IsNaN(y) && !double.IsInfinity(y)) {
		            var point = new Point(windowSize.X/2, windowSize.Y/2);
                    g.DrawLine(pen, point, point);
		        }
                ShowImageInWindow(image);
		        return;
		    }

            // Определим первую точку по x, которую необходимо отобразить
		    int screenX = 0, screenY;
		    Point? oldPoint = null, nextPoint = null;
		    bool maxYEqualsMinY = Math.Abs(maxY - minY) < Eps;

		    for (; screenX < windowSize.X; ++screenX)
		    {
                double x = xFrom + screenX * (xTo - xFrom) / windowSize.X,
                       y = SafeValue(f, x);
		        if (double.IsNaN(y) || double.IsInfinity(y))
		            continue;
                // Если функция вида y = C (minY=maxY), рисуем на середине экрана; иначе обычный перевод в экранные
                screenY = maxYEqualsMinY ? windowSize.Y/2
                                         : (int)((maxY - y) * windowSize.Y / (maxY - minY));
                oldPoint = new Point(screenX, screenY);
		        break;
		    }

            // Нашли первую определенную точку
		    if (oldPoint != null)
		    {
		        for (; screenX < windowSize.X; ++screenX)
		        {
		            double x = xFrom + screenX * (xTo - xFrom) / windowSize.X,
                           y = SafeValue(f, x);
                    if (double.IsNaN(y) || double.IsInfinity(y))
                    {
                        // Новая точка не определена, нужно нарисовать точку в текущем положении
                        if (oldPoint != null)
                            g.DrawLine(pen, oldPoint.Value, oldPoint.Value);
		                oldPoint = null;
                        continue;
		            }
                    screenY = maxYEqualsMinY ? windowSize.Y/2
                                             : (int)((maxY - y) * windowSize.Y / (maxY - minY));
		            nextPoint = new Point(screenX, screenY);

                    // Если есть откуда рисовать, проводим прямую до новой точки 
		            if (oldPoint != null)
		                g.DrawLine(pen, oldPoint.Value, nextPoint.Value);
		            oldPoint = nextPoint;
		        }
		    }
		    //image.Save("img.png", ImageFormat.Png);
            ShowImageInWindow(image);
		}


        private static readonly FuncToShow[] Functions =
        {
            x => 1/x,
            x => 1,
            x => 0,
            x => (1/x)*Math.Sin(1/x),
            x => x*Math.Cos(x*x),
            x => Math.Cos(1/x),
            x => Math.Sqrt(x),
            x => x*x*x,
            x => x*x*x*x*x,
        };

		static void Main(string[] args)
        {
		    foreach (var function in Functions)
                DrawFunction(function, -1, 10);
        }
	}
}
