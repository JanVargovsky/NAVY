using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NAVY.Lesson7
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly MandelbrotSet mandelbrotSet;

        public MainWindow()
        {
            InitializeComponent();
            mandelbrotSet = new MandelbrotSet();
            Loaded += (o, e) => Render();
        }

        void Render()
        {
            Stopwatch sw = Stopwatch.StartNew();
            canvas.Children.Clear();

            DrawingVisual dv = new DrawingVisual();
            var size = new Size(1, 1);
            int h = (int)canvas.ActualHeight;
            int w = (int)canvas.ActualWidth;
            using (DrawingContext dc = dv.RenderOpen())
            {
                for (int y = 0; y < h; y++)
                    for (int x = 0; x < w; x++)
                    {
                        var color = mandelbrotSet.Calculate(x, y, canvas.RenderSize);
                        var brush = new SolidColorBrush(color);
                        dc.DrawRectangle(brush, null, new Rect(new Point(x, y), size));
                    }
                dc.Close();
            }

            RenderTargetBitmap rtb = new RenderTargetBitmap(w, h, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(dv);
            Image img = new Image
            {
                Source = rtb
            };

            canvas.Children.Add(img);
            sw.Stop();
            Debug.WriteLine($"Render time: {sw.ElapsedMilliseconds} ms");
        }
    }
}
