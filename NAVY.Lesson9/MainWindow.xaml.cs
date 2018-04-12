using System.Windows;
using System.Windows.Controls;

namespace NAVY.Lesson9
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
            //Loaded += (o, e) => viewModel.SetInitial(Canvas.Width / 2, Canvas.Height / 2);
        }

        private void Start(object sender, RoutedEventArgs e)
        {
            viewModel.Start();
        }

        private void Stop(object sender, RoutedEventArgs e)
        {
            viewModel.Stop();
        }

        private void Next(object sender, RoutedEventArgs e)
        {
            viewModel.Next();
        }
    }
}
