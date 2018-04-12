using LiveCharts;
using LiveCharts.Definitions.Series;
using LiveCharts.Wpf;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Windows.Threading;

namespace NAVY.Lesson9
{
    class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        readonly DispatcherTimer timer;

        private double x0;
        public double X0
        {
            get { return x0; }
            set
            {
                x0 = value;
                NotifyPropertyChanged();
            }
        }

        private double y0;
        public double Y0
        {
            get { return y0; }
            set
            {
                y0 = value;
                NotifyPropertyChanged();
            }
        }

        private double x1;
        public double X1
        {
            get { return x1; }
            set
            {
                x1 = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(CX1));
            }
        }

        private double y1;
        public double Y1
        {
            get { return y1; }
            set
            {
                y1 = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(CY1));
            }
        }

        private double x2;
        public double X2
        {
            get { return x2; }
            set
            {
                x2 = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(CX2));
            }
        }

        private double y2;
        public double Y2
        {
            get { return y2; }
            set
            {
                y2 = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(CY2));
            }
        }

        public double R0 => 10;
        public double R1 => M1;
        public double R2 => M2;

        public double CX0 => X0 - R0 / 2;
        public double CY0 => Y0 - R0 / 2;

        public double CX1 => X1 - R1 / 2;
        public double CY1 => Y1 - R1 / 2;

        public double CX2 => X2 - R2 / 2;
        public double CY2 => Y2 - R2 / 2;

        public double RequiredSize => 2 * (L1 + L2) + 10;

        readonly IChartValues phi1Series;
        readonly IChartValues phi2Series;
        public SeriesCollection Series { get; }

        public MainViewModel()
        {
            SetInitial(RequiredSize / 2, RequiredSize / 2);

            const double FPS60 = 1000d / 60d;
            var interval = TimeSpan.FromMilliseconds(FPS60);
            //var interval = TimeSpan.FromMilliseconds(200);
            timer = new DispatcherTimer
            {
                Interval = interval,
                IsEnabled = false,
            };
            timer.Tick += (o, e) => Next();

            phi1Series = new ChartValues<double>();
            phi2Series = new ChartValues<double>();

            ISeriesView CreateView(IChartValues v) => new LineSeries
            {
                Values = v,
                Fill = Brushes.Transparent,
                StrokeThickness = 0.5d,
                PointGeometry = null,
            };
            Series = new SeriesCollection
            {
                CreateView(phi1Series),
                CreateView(phi2Series),
            };
        }

        public void SetInitial(double x, double y)
        {
            X0 = x;
            Y0 = y;
            X1 = x + L1 * Math.Sin(Phi1);
            Y1 = y - L1 * Math.Cos(Phi1);
            X2 = x + L1 * Math.Sin(Phi1) + L2 * Math.Sin(Phi2);
            Y2 = y - L1 * Math.Cos(Phi1) - L2 * Math.Cos(Phi2);
        }

        public void Start() => timer.IsEnabled = true;
        public void Stop() => timer.IsEnabled = false;

        const double M1 = 15d;
        const double M2 = 10d;
        const double G = 9.81d;
        public const double L1 = 100d;
        public const double L2 = 60d;
        const double time = 0.2d;
        const double Alpha1 = 160;
        const double Alpha2 = 80;

        double Phi1 = Alpha1 * Math.PI / 180;
        double Phi2 = Alpha2 * Math.PI / 180;
        double d1Phi1 = 0d;
        double d1Phi2 = 0d;

        public void Next()
        {
            var mu = 1 + M1 / M2;
            var d2Phi1 = (G * (Math.Sin(Phi2) * Math.Cos(Phi1 - Phi2) - mu * Math.Sin(Phi1)) -
                (L2 * d1Phi2 * d1Phi2 + L1 * d1Phi1 * d1Phi1 * Math.Cos(Phi1 - Phi2)) * Math.Sin(Phi1 - Phi2)) /
                (L1 * (mu - Math.Cos(Phi1 - Phi2) * Math.Cos(Phi1 - Phi2)));

            var d2Phi2 = (mu * G * (Math.Sin(Phi1) * Math.Cos(Phi1 - Phi2) - Math.Sin(Phi2)) +
                (mu * L1 * d1Phi1 * d1Phi1 + L2 * d1Phi2 * d1Phi2 * Math.Cos(Phi1 - Phi2)) * Math.Sin(Phi1 - Phi2)) /
                (L2 * (mu - Math.Cos(Phi1 - Phi2) * Math.Cos(Phi1 - Phi2)));

            d1Phi1 += d2Phi1 * time;
            d1Phi2 += d2Phi2 * time;
            Phi1 += d1Phi1 * time;
            Phi2 += d1Phi2 * time;

            //Phi1 %= 2 * Math.PI;
            //Phi2 %= 2 * Math.PI;

            //if (Phi1 < 0) Phi1 += 2 * Math.PI;
            //if (Phi2 < 0) Phi2 += 2 * Math.PI;

            X1 = X0 + L1 * Math.Sin(Phi1);
            Y1 = Y0 - L1 * Math.Cos(Phi1);
            X2 = X0 + L1 * Math.Sin(Phi1) + L2 * Math.Sin(Phi2);
            Y2 = Y0 - L1 * Math.Cos(Phi1) - L2 * Math.Cos(Phi2);

            const int MaxSeriesCount = 100;
            if (phi1Series.Count > MaxSeriesCount) phi1Series.RemoveAt(0);
            phi1Series.Add(Phi1 / Math.PI * 180);
            if (phi2Series.Count > MaxSeriesCount) phi2Series.RemoveAt(0);
            phi2Series.Add(Phi2 / Math.PI * 180);
        }
    }
}
