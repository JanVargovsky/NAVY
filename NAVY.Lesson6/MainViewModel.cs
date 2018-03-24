namespace NAVY.Lesson6
{
    public class MainViewModel : ViewModelBase
    {
        private int lSystemIterations;
        public int LSystemIterations
        {
            get { return lSystemIterations; }
            set
            {
                lSystemIterations = value;
                NotifyPropertyChanged();
            }
        }

        private int ifsIterations;
        public int IFSIterations
        {
            get { return ifsIterations; }
            set
            {
                ifsIterations = value;
                NotifyPropertyChanged();
            }
        }

        private double elapsedTime;
        public double ElapsedTime
        {
            get { return elapsedTime; }
            set
            {
                elapsedTime = value;
                NotifyPropertyChanged();
            }
        }

        private bool keepRatio;
        public bool KeepRatio
        {
            get { return keepRatio; }
            set
            {
                keepRatio = value;
                NotifyPropertyChanged();
            }
        }

        private int affineTransformationCount;
        public int AffineTransformationCount
        {
            get { return affineTransformationCount; }
            set
            {
                affineTransformationCount = value;
                NotifyPropertyChanged();
            }
        }

        private bool filterPoints;
        public bool FilterPoints
        {
            get { return filterPoints; }
            set
            {
                filterPoints = value;
                NotifyPropertyChanged();
            }
        }

        private double filterAverageDistanceMultiplyConstant;
        public double FilterAverageDistanceMultiplyConstant
        {
            get { return filterAverageDistanceMultiplyConstant; }
            set
            {
                filterAverageDistanceMultiplyConstant = value;
                NotifyPropertyChanged();
            }
        }

        public MainViewModel()
        {
            LSystemIterations = 0;
            IFSIterations = 100000;
            ElapsedTime = 0;
            KeepRatio = true;
            AffineTransformationCount = 4;
            FilterPoints = true;
            FilterAverageDistanceMultiplyConstant = 2.5;
        }
    }
}
