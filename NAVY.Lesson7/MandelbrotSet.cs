using System;
using System.Linq;
using System.Threading.Tasks;
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

        public MandelbrotSet(int maxIteration)
        {
            this.maxIteration = maxIteration;
        }

        internal static double Scale(double value, double currentScaleMin, double currentScaleMax, double desiredScaleMin, double desiredScaleMax)
        {
            return desiredScaleMin + (desiredScaleMax - desiredScaleMin) * ((value - currentScaleMin) / (currentScaleMax - currentScaleMin));
        }

        int CalculateRaw(int px, int py, int width, int height)
        {
            double x0 = Scale(px, 0, width, MinX, MaxX);
            double y0 = Scale(py, 0, height, MinY, MaxY);
            double x = 0d;
            double y = 0d;

            int iteration = 0;
            const double Threashold = 2;
            const double ThreasholdSquared = Threashold * Threashold;
            while (x * x + y * y < ThreasholdSquared && iteration < maxIteration)
            {
                var xtemp = x * x - y * y + x0;
                y = 2 * x * y + y0;
                x = xtemp;
                iteration++;
            }

            return iteration;
        }

        public int[,] CalculateRaw(int width, int height)
        {
            var result = new int[width, height];
            Parallel.For(0, result.GetLength(1) / 2 + 1, y =>
            {
                for (int x = 0; x < result.GetLength(0); x++)
                    result[x, y] = result[x, height - y - 1] = CalculateRaw(x, y, width, height);
            });
            return result;
        }

        public Color[,] PaletteColoring(int[,] values)
        {
            var palette = new Color[maxIteration + 1];
            for (int i = 0; i < maxIteration; i++)
            {
                var value = (byte)Scale(255 - i, 0, maxIteration, 0, 255);
                palette[i] = Color.FromRgb(0, 0, value);
            }
            palette[maxIteration] = Color.FromRgb(0, 0, 0);

            var result = new Color[values.GetLength(0), values.GetLength(1)];
            for (int y = 0; y < values.GetLength(1); y++)
                for (int x = 0; x < result.GetLength(0); x++)
                    result[x, y] = palette[values[x, y]];
            return result;
        }

        public Color[,] HistogramColoring(int[,] values)
        {
            Color ColorFromHSV(double hue, double saturation, double value)
            {
                int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
                double f = hue / 60 - Math.Floor(hue / 60);

                value = value * 255;
                byte v = Convert.ToByte(value);
                byte p = Convert.ToByte(value * (1 - saturation));
                byte q = Convert.ToByte(value * (1 - f * saturation));
                byte t = Convert.ToByte(value * (1 - (1 - f) * saturation));

                if (hi == 0)
                    return Color.FromArgb(255, v, t, p);
                else if (hi == 1)
                    return Color.FromArgb(255, q, v, p);
                else if (hi == 2)
                    return Color.FromArgb(255, p, v, t);
                else if (hi == 3)
                    return Color.FromArgb(255, p, q, v);
                else if (hi == 4)
                    return Color.FromArgb(255, t, p, v);
                else
                    return Color.FromArgb(255, v, p, q);
            }

            int[] histogram = new int[maxIteration + 1];
            for (int y = 0; y < values.GetLength(1); y++)
                for (int x = 0; x < values.GetLength(0); x++)
                    histogram[values[x, y]]++;

            var hues = new double[histogram.Length];
            for (int i = 1; i < histogram.Length; i++)
                hues[i] = hues[i - 1] + histogram[i];
            int total = histogram.Sum();
            for (int i = 1; i < histogram.Length; i++)
            {
                hues[i] /= total;
                hues[i] = Scale(hues[i], 0, 1, 0, 360);
            }

            const double S = 1;
            const double L = 1;
            var colors = new Color[hues.Length];
            for (int i = 0; i < colors.Length; i++)
                colors[i] = ColorFromHSV(hues[i], S, L);
            colors[colors.Length - 1] = Color.FromRgb(0, 0, 0);

            var result = new Color[values.GetLength(0), values.GetLength(1)];
            var black = Color.FromRgb(0, 0, 0);
            for (int y = 0; y < values.GetLength(1); y++)
                for (int x = 0; x < values.GetLength(0); x++)
                    result[x, y] = colors[values[x, y]];
            return result;
        }
    }
}
