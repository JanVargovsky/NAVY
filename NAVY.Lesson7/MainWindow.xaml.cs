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
        readonly MainViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = viewModel = new MainViewModel();
            Loaded += (o, e) => Render();
        }

        void Render()
        {
            canvas.Children.Clear();

            Stopwatch sw = Stopwatch.StartNew();
            var mandelbrotSet = new MandelbrotSet(viewModel.MaxIteration);

            int width = (int)canvas.ActualWidth;
            int height = (int)canvas.ActualHeight;

            WriteableBitmap writeableBitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Pbgra32, null);
            var values = mandelbrotSet.CalculateRaw(width, height);
            var colorizedValues = mandelbrotSet.HistogramColoring(values);
            sw.Stop();
            viewModel.CalculateTime = sw.ElapsedMilliseconds;

            sw = Stopwatch.StartNew();
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    writeableBitmap.SetPixel(x, y, colorizedValues[x, y]);
            sw.Stop();

            Image img = new Image
            {
                Source = writeableBitmap
            };

            canvas.Children.Add(img);
            viewModel.RenderTime = sw.ElapsedMilliseconds;
        }

        private void Render_Click(object sender, RoutedEventArgs e)
        {
            Render();
        }
    }
}
