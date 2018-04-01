using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace NAVY.Lesson7
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private double calculateTime;
        public double CalculateTime
        {
            get { return calculateTime; }
            set
            {
                calculateTime = value;
                NotifyPropertyChanged();
            }
        }

        private double renderTime;
        public double RenderTime
        {
            get { return renderTime; }
            set
            {
                renderTime = value;
                NotifyPropertyChanged();
            }
        }

        private int maxIteration;
        public int MaxIteration
        {
            get { return maxIteration; }
            set
            {
                maxIteration = value;
                NotifyPropertyChanged();
            }
        }

        private Point point;
        public Point Point
        {
            get { return point; }
            set
            {
                point = value;
                NotifyPropertyChanged();
            }
        }

        private Size size;
        public Size Size
        {
            get { return size; }
            set
            {
                size = value;
                NotifyPropertyChanged();
            }
        }

        private bool canRender;
        public bool CanRender
        {
            get { return canRender; }
            set
            {
                canRender = value;
                NotifyPropertyChanged();
            }
        }

        private double zoomFactor;
        public double ZoomFactor
        {
            get { return zoomFactor; }
            set
            {
                zoomFactor = value;
                NotifyPropertyChanged();
            }
        }
    }
}
