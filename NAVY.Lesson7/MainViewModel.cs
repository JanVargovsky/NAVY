using System.ComponentModel;
using System.Runtime.CompilerServices;

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

        private double elapsedTime;
        public double RenderTime
        {
            get { return elapsedTime; }
            set
            {
                elapsedTime = value;
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

        public MainViewModel()
        {
            elapsedTime = 0d;
            MaxIteration = 50;
        }
    }
}
