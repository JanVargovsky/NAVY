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
            SizeChanged += (o, e) => Render();
        }

        void Render()
        {
            Stopwatch sw = Stopwatch.StartNew();
            canvas.Children.Clear();


            int w = (int)canvas.ActualWidth;
            int h = (int)canvas.ActualHeight;
            var values = mandelbrotSet.Calculate(w, h);

            DrawingVisual dv = new DrawingVisual();
            var size = new Size(1, 1);
            using (DrawingContext dc = dv.RenderOpen())
            {
                for (int y = 0; y < h; y++)
                    for (int x = 0; x < w; x++)
                    {
                        
                        var brush = new SolidColorBrush(values[x,y]);
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
