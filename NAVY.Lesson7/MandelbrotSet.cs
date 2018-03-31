using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace NAVY.Lesson7
{
    public class MandelbrotSet
    {
        const double MinX = -2.5d;
        const double MaxX = 1d;
        const double MinY = -1d;
        const double MaxY = 1d;

        readonly int maxIteration;
        readonly Color[] palette;

        public MandelbrotSet()
        {
            maxIteration = 1000;
            palette = new Color[maxIteration];
            for (int i = 0; i < maxIteration; i++)
            {
                var value = (byte)Scale(255 - i, 0, maxIteration, 0, 255);
                palette[i] = Color.FromRgb(0, 0, value);
            }
        }

        internal static double Scale(double value, double currentScaleMin, double currentScaleMax, double desiredScaleMin, double desiredScaleMax)
        {
            return desiredScaleMin + (desiredScaleMax - desiredScaleMin) * ((value - currentScaleMin) / (currentScaleMax - currentScaleMin));
        }

        public Color Calculate(int px, int py, Size size)
        {
            double x0 = Scale(px, 0, size.Width, MinX, MaxX);
            double y0 = Scale(py, 0, size.Height, MinY, MaxY);
            double x = 0d;
            double y = 0d;

            int iteration = 0;
            while (x * x + y * y < 2 * 2 && iteration < maxIteration)
            {
                var xtemp = x * x - y * y + x0;
                y = 2 * x * y + y0;
                x = xtemp;
                iteration++;
            }

            return palette[iteration - 1];
        }
    }
}
