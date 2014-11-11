﻿using System;
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
        const int XTo = 1;

        private delegate double FuncToShow(double x);

        private static FuncToShow myF = Cos1DivX;

        private static double Cos1DivX(double x)
        {
            return Math.Cos(1 / x);
        }

        private static double XCube(double x)
        {
            return x*x*x;
        }

        private static double XIn5(double x)
        {
            return x*x*x*x*x;
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

        private static void CreateImage(Bitmap image, FuncToShow F, double xFrom, double xTo)
        {
            Pen pen = new Pen(Color.Yellow, 1);
            Graphics g = Graphics.FromImage(image);

            Tuple<double, double> minMax = GetMinMaxValues(F, XFrom, XTo);
            double minY = minMax.Item1;
            double maxY = minMax.Item2;

            Point nextPoint, oldPoint;
            int screenX, screenY;
            double x, y;

            screenY = (int)((F(XFrom) - minY) * WindowSize / (maxY - minY));
            oldPoint = new Point(0, screenY);

            for (screenX = 0; screenX < WindowSize; screenX++)
            {
                x = (XFrom + screenX * (xTo - XFrom) / WindowSize);
                y = F(x);
                if (double.IsNaN(y))
                    continue;
                screenY = (int)((y - minY) * WindowSize / (maxY - minY));
                nextPoint = new Point(screenX, screenY);
                g.DrawLine(pen, oldPoint, nextPoint);
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
            Bitmap image = new Bitmap(WindowSize, WindowSize);
            Graphics g = Graphics.FromImage(image);
            g.FillRectangle(Brushes.Blue, 0, 0, image.Width, image.Height);

            Tuple<double, double> minMax = GetMinMaxValues(myF, XFrom, XTo);

            CreateImage(image, myF, XFrom, XTo);

            // image.Save("img.png", ImageFormat.Png);

            ShowImageInWindow(image);

        }
    }
}
