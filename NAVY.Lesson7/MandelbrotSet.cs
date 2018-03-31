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
            maxIteration = 50;
            palette = new Color[maxIteration + 1];
            for (int i = 0; i < maxIteration; i++)
            {
                var value = (byte)Scale(255 - i, 0, maxIteration, 0, 255);
                palette[i] = Color.FromRgb(0, 0, value);
            }
            palette[maxIteration] = Color.FromRgb(0, 0, 0);
        }

        internal static double Scale(double value, double currentScaleMin, double currentScaleMax, double desiredScaleMin, double desiredScaleMax)
        {
            return desiredScaleMin + (desiredScaleMax - desiredScaleMin) * ((value - currentScaleMin) / (currentScaleMax - currentScaleMin));
        }

        public Color Calculate(int px, int py, Size size) => Calculate(px, py, (int)size.Width, (int)size.Height);

        public Color Calculate(int px, int py, int width, int height)
        {
            double x0 = Scale(px, 0, width, MinX, MaxX);
            double y0 = Scale(py, 0, height, MinY, MaxY);
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

            return palette[iteration];
        }

        public Color[,] Calculate(Size size) => Calculate((int)size.Width, (int)size.Height);

        public Color[,] Calculate(int width, int height)
        {
            var result = new Color[width, height];
            Parallel.For(0, result.GetLength(1), y =>
            {
                for (int x = 0; x < result.GetLength(0); x++)
                    result[x, y] = Calculate(x, y, width, height);
            });
            return result;
        }
    }
}
