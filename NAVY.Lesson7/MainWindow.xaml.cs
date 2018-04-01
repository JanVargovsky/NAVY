using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static NAVY.Lesson7.MandelbrotSet;

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
            DataContext = viewModel = new MainViewModel
            {
                RenderTime = 0d,
                CalculateTime = 0d,
                MaxIteration = 50,
                CanRender = false,
            };
            Loaded += (o, e) =>
            {
                viewModel.Size = Canvas.RenderSize;
                viewModel.CanRender = true;
                Render();
            };
        }

        void Render()
        {
            viewModel.CanRender = false;
            Canvas.Children.Clear();

            Stopwatch sw = Stopwatch.StartNew();
            var mandelbrotSet = new MandelbrotSet(viewModel.MaxIteration);

            var size = Canvas.RenderSize;
            int width = (int)size.Width;
            int height = (int)size.Height;

            WriteableBitmap writeableBitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Pbgra32, null);
            //var values = mandelbrotSet.CalculateRaw(width, height);
            var values = mandelbrotSet.CalculateRaw(width, height, viewModel.Point, viewModel.Size);
            //var colorizedValues = mandelbrotSet.PaletteColoring(values);
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

            Canvas.Children.Add(img);
            viewModel.RenderTime = sw.ElapsedMilliseconds;
            viewModel.CanRender = true;
        }

        private void Render_Click(object sender, RoutedEventArgs e)
        {
            Render();
        }

        Point initialShiftPoint = new Point();

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                //var p = e.GetPosition(Canvas);
                //viewModel.Point = p;
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                initialShiftPoint = e.GetPosition(Canvas);
            }
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                //var a = viewModel.Point;
                //var b = e.GetPosition(Canvas);
                //viewModel.Size = new Size(Math.Abs(a.X - b.X), Math.Abs(a.Y - b.Y));

                //Render();
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                var endShiftPoint = e.GetPosition(Canvas);

                var distanceX = endShiftPoint.X - initialShiftPoint.X;
                var scaledDistanceXToSize = Scale(distanceX, 0, Canvas.ActualWidth, 0, viewModel.Size.Width);

                var distanceY = endShiftPoint.Y - initialShiftPoint.Y;
                var scaledDistanceYToSize = Scale(distanceY, 0, Canvas.ActualHeight, 0, viewModel.Size.Height);
                var p = viewModel.Point;
                viewModel.Point = new Point(p.X + scaledDistanceXToSize, p.Y + scaledDistanceYToSize);

                Render();
            }
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Point = new Point();
            viewModel.Size = Canvas.RenderSize;
            Render();
        }

        void ZoomIn()
        {
            var s = viewModel.Size;
            var p = viewModel.Point;
            viewModel.Point = new Point(p.X + s.Width / 4, p.Y + s.Height / 4);
            viewModel.Size = new Size(s.Width / 2, s.Height / 2);
        }

        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            ZoomIn();
            Render();
        }

        void ZoomOut()
        {
            var s = viewModel.Size;
            var p = viewModel.Point;
            viewModel.Point = new Point(p.X - s.Width / 2, p.Y - s.Height / 2);
            viewModel.Size = new Size(s.Width * 2, s.Height * 2);
        }

        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            ZoomOut();
            Render();
        }

        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
                ZoomIn();
            else if (e.Delta < 0)
                ZoomOut();
            Render();
        }
    }
}
