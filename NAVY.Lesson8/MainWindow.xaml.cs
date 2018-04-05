using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NAVY.Lesson8
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
            Canvas.Children.Clear();

            var size = Canvas.RenderSize;
            int width = (int)size.Width;
            int height = (int)size.Height;

            WriteableBitmap writeableBitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Pbgra32, null);
            if (viewModel.CellularAutomaton is CellularAutomaton<GameOfLifeCellState> gof)
            {
                Color GetColor(GameOfLifeCellState state) => state == GameOfLifeCellState.Alive ? Colors.Black : Colors.White;

                int cellsWidth = gof.Cells.Width;
                int cellsHeight = gof.Cells.Height;
                int cellWidth = width / cellsWidth;
                var cellHeight = height / cellsHeight;
                for (int y = 0; y < cellsHeight; y++)
                {
                    var y1 = y * cellHeight;
                    var y2 = y1 + cellHeight;

                    for (int x = 0; x < cellsWidth; x++)
                    {
                        Color c = GetColor(gof.Cells[x, y]);
                        var x1 = x * cellWidth;
                        var x2 = x1 + cellWidth;
                        writeableBitmap.FillRectangle(x1, y1, x2, y2, c);
                    }
                }
                writeableBitmap.DrawRectangle(0, 0, cellWidth * cellsWidth, cellHeight * cellsHeight, Colors.Black);
            }
            else if(viewModel.CellularAutomaton is CellularAutomaton<ForestFireCellState> ff)
            {
                Color GetColor(ForestFireCellState state) =>
                    state == ForestFireCellState.Tree ? Colors.Green :
                    state == ForestFireCellState.Burning ? Colors.OrangeRed :
                    Colors.Black;

                int cellsWidth = ff.Cells.Width;
                int cellsHeight = ff.Cells.Height;
                for (int y = 0; y < cellsHeight; y++)
                    for (int x = 0; x < cellsWidth; x++)
                    {
                        Color c = GetColor(ff.Cells[x, y]);
                        writeableBitmap.SetPixel(x, y, c);
                    }
            }

            Image img = new Image
            {
                Source = writeableBitmap
            };

            Canvas.Children.Add(img);
        }

        private void Next(object sender, RoutedEventArgs e)
        {
            viewModel.Next();
            Render();
        }

        private void SetGameOfLife(object sender, RoutedEventArgs e)
        {
            viewModel.SetGameOfLife();
            Render();
        }

        private void SetForrestFire(object sender, RoutedEventArgs e)
        {
            int w = (int)Canvas.ActualWidth;
            int h = (int)Canvas.ActualHeight;
            viewModel.SetForestFire(w, h);
            Render();
        }

        private void Random(object sender, RoutedEventArgs e)
        {
            viewModel.SetRandom();
            Render();
        }
    }
}
