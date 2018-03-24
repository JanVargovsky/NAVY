using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NAVY.Lesson6
{
    public partial class MainWindow : Window
    {
        readonly MainViewModel viewModel;
        readonly Brush brush;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = viewModel = new MainViewModel();
            brush = Brushes.Green;
        }

        void ScaleAndInvertY(Point[] points, Size size, bool keepRatio)
        {
            (double MinX, double MaxX, double YinY, double maxY) GetMinMaxXY() =>
                (points.Min(t => t.X), points.Max(t => t.X), points.Min(t => t.Y), points.Max(t => t.Y));

            var (minX, maxX, minY, maxY) = GetMinMaxXY();

            if (minX < 0)
                for (int i = 0; i < points.Length; i++)
                    points[i].X += -minX;
            if (minY < 0)
                for (int i = 0; i < points.Length; i++)
                    points[i].Y += -minY;

            if (minX < 0 || minY < 0)
                (minX, maxX, minY, maxY) = GetMinMaxXY();


            var sizeX = maxX - minX;
            var sizeY = maxY - minY;

            var scaleX = size.Width / sizeX;
            var scaleY = size.Height / sizeY;

            if (keepRatio)
                if (scaleX < scaleY)
                    scaleY = scaleX;
                else
                    scaleX = scaleY;

            for (int i = 0; i < points.Length; i++)
            {
                points[i].X -= minX;
                points[i].X *= scaleX;
                points[i].Y -= minY;
                points[i].Y *= scaleY;
                points[i].Y = size.Height - points[i].Y;
            }

            if (sizeX == 0)
                for (int i = 0; i < points.Length; i++)
                    points[i].X = size.Width / 2;
            if (sizeY == 0)
                for (int i = 0; i < points.Length; i++)
                    points[i].Y = size.Height / 2;
        }

        (double X, double Y)[] FilterPoints((double X, double Y)[] points, double acceptedAverageDistanceMultiply)
        {
            points = points.Where(t => !double.IsInfinity(t.X) && !double.IsInfinity(t.Y)).ToArray();

            (double X, double Y) avg = (points.Average(t => t.X), points.Average(t => t.Y));

            double Distance((double X, double Y) a, (double X, double Y) b) => Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));

            var pointsWithDistance = points
                .Select(p => new
                {
                    Point = p,
                    DistanceFromAverage = Distance(avg, p),
                })
                .OrderByDescending(t => t.DistanceFromAverage)
                .ToArray();

            var avgDistance = pointsWithDistance.Average(t => t.DistanceFromAverage);
            var maxAcceptedDistance = acceptedAverageDistanceMultiply * avgDistance;
            var result = pointsWithDistance.Where(t => t.DistanceFromAverage <= maxAcceptedDistance)
                .Select(t => t.Point)
                .ToArray();
            return result;
        }

        void AddLinesToCanvas(IEnumerable<(double X, double Y)> p)
        {
            Stopwatch sw = Stopwatch.StartNew();
            canvas.Children.Clear();

            var points = p.Select(t => new Point(t.X, t.Y)).ToArray();
            ScaleAndInvertY(points, canvas.RenderSize, viewModel.KeepRatio);

            var pointCollection = new PointCollection(points);
            var lines = new Polyline
            {
                Points = pointCollection,
                Stroke = brush,
            };

            canvas.Children.Add(lines);
            sw.Stop();
            viewModel.ElapsedTime = sw.ElapsedMilliseconds;
        }

        void AddLinesToCanvas(LSystem lSystem)
        {
            AddLinesToCanvas(lSystem.GetPoints(canvas.ActualWidth / 2, canvas.ActualHeight / 2, 0, viewModel.LSystemIterations));
        }

        void KochSnowFlake(object sender, RoutedEventArgs e)
        {
            const string Axiom = "F--F--F";
            const string F = "F+F--F+F";
            var l = new LSystem(Axiom, new Dictionary<char, string>
            {
                ['F'] = F,
            }, 60);
            AddLinesToCanvas(l);
        }

        void KochCurve(object sender, RoutedEventArgs e)
        {
            const string Axiom = "F";
            const string F = "F+F-F-F+F";
            var l = new LSystem(Axiom, new Dictionary<char, string>
            {
                ['F'] = F,
            }, 90);
            AddLinesToCanvas(l);
        }

        void QuadraticFlake(object sender, RoutedEventArgs e)
        {
            const string Axiom = "F+F+F+F";
            const string F = "F+F-F-FF+F+F-F";
            var l = new LSystem(Axiom, new Dictionary<char, string>
            {
                ['F'] = F,
            }, 90);
            AddLinesToCanvas(l);
        }

        void SierpinskiTriangle(object sender, RoutedEventArgs e)
        {
            const string Axiom = "F-G-G";
            const string F = "F-G+F+G-F";
            const string G = "GG";
            var l = new LSystem(Axiom, new Dictionary<char, string>
            {
                ['F'] = F,
                ['G'] = G,
            }, 120);
            AddLinesToCanvas(l);
        }

        void SierpinskiArrowheadTriangle(object sender, RoutedEventArgs e)
        {
            const string Axiom = "A";
            const string A = "B-A-B";
            const string B = "A+B+A";
            var l = new LSystem(Axiom, new Dictionary<char, string>
            {
                ['A'] = A,
                ['B'] = B,
            }, 60);
            AddLinesToCanvas(l);
        }

        void AddPointsToCanvas(IEnumerable<(double X, double Y)> p, bool filter)
        {
            Stopwatch sw = Stopwatch.StartNew();
            canvas.Children.Clear();

            var inputPoints = p.ToArray();
            if(filter)
                inputPoints = FilterPoints(inputPoints, viewModel.FilterAverageDistanceMultiplyConstant);
            var points = inputPoints.Select(t => new Point(t.X, t.Y)).ToArray();
            ScaleAndInvertY(points, canvas.RenderSize, viewModel.KeepRatio);

            DrawingVisual dv = new DrawingVisual();
            var size = new Size(1, 1);
            using (DrawingContext dc = dv.RenderOpen())
            {
                foreach (var point in points)
                    dc.DrawRectangle(brush, null, new Rect(point, size));

                dc.Close();
            }
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)canvas.ActualWidth, (int)canvas.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(dv);
            Image img = new Image
            {
                Source = rtb
            };

            canvas.Children.Add(img);
            sw.Stop();
            viewModel.ElapsedTime = sw.ElapsedMilliseconds;
        }

        void AddPointsToCanvas(IteratedFunctionSystem ifs)
        {
            AddPointsToCanvas(ifs.GetPoints(canvas.ActualWidth / 4, canvas.ActualHeight / 2, viewModel.IFSIterations), viewModel.FilterPoints);
        }

        private void Fern_Presentation1(object sender, RoutedEventArgs e)
        {
            IteratedFunctionSystem f = IteratedFunctionSystem.Presentation1;
            AddPointsToCanvas(f);
        }

        private void Fern_Presentation2(object sender, RoutedEventArgs e)
        {
            IteratedFunctionSystem f = IteratedFunctionSystem.Presentation2;
            AddPointsToCanvas(f);
        }

        private void Fern_Wikipedia(object sender, RoutedEventArgs e)
        {
            IteratedFunctionSystem f = IteratedFunctionSystem.Wikipedia;
            AddPointsToCanvas(f);
        }

        private void Fern_Random(object sender, RoutedEventArgs e)
        {
            IteratedFunctionSystem f = new IteratedFunctionSystem(AffineTransformationGroup.GenerateRandom(viewModel.AffineTransformationCount));
            AddPointsToCanvas(f);
        }
    }
}
