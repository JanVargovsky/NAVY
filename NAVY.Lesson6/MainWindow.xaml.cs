using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NAVY.Lesson6
{
    public partial class MainWindow : Window
    {
        public int Iterations { get; set; }

        public MainWindow()
        {
            Iterations = 1;
            InitializeComponent();
        }

        void NumbersOnlyValidation(object sender, TextCompositionEventArgs e)
        {
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        void ScaleAndInvertY(Point[] points, Size size)
        {
            var minX = points.Min(t => t.X);
            var maxX = points.Max(t => t.X);
            var minY = points.Min(t => t.Y);
            var maxY = points.Max(t => t.Y);

            var sizeX = maxX - minX;
            var sizeY = maxY - minY;

            var scaleX = size.Width / sizeX;
            var scaleY = size.Height / sizeY;

            for (int i = 0; i < points.Length; i++)
            {
                points[i].X -= minX;
                points[i].X *= scaleX;
                points[i].Y -= minY;
                points[i].Y *= scaleY;
                points[i].Y = size.Height - points[i].Y;
            }
        }

        void AddPointsToCanvas(IEnumerable<(double X, double Y)> p)
        {
            canvas.Children.Clear();

            var points = p.Select(t => new Point(t.X, t.Y)).ToArray();
            ScaleAndInvertY(points, canvas.RenderSize);

            var pointCollection = new PointCollection(points);
            var lines = new Polyline
            {
                Points = pointCollection,
                Stroke = Brushes.Black,
            };

            canvas.Children.Add(lines);
        }

        void AddPointsToCanvas(LSystem lSystem)
        {
            AddPointsToCanvas(lSystem.GetPoints(canvas.ActualWidth / 2, canvas.ActualHeight / 2, 0, Iterations));
        }

        void KochSnowFlake(object sender, RoutedEventArgs e)
        {
            const string Axiom = "F--F--F";
            const string F = "F+F--F+F";
            var l = new LSystem(Axiom, new Dictionary<char, string>
            {
                ['F'] = F,
            }, 60);
            AddPointsToCanvas(l);
        }

        void KochCurve(object sender, RoutedEventArgs e)
        {
            const string Axiom = "F";
            const string F = "F+F-F-F+F";
            var l = new LSystem(Axiom, new Dictionary<char, string>
            {
                ['F'] = F,
            }, 90);
            AddPointsToCanvas(l);
        }

        void QuadraticFlake(object sender, RoutedEventArgs e)
        {
            const string Axiom = "F+F+F+F";
            const string F = "F+F-F-FF+F+F-F";
            var l = new LSystem(Axiom, new Dictionary<char, string>
            {
                ['F'] = F,
            }, 90);
            AddPointsToCanvas(l);
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
            AddPointsToCanvas(l);
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
            AddPointsToCanvas(l);
        }
    }
}
