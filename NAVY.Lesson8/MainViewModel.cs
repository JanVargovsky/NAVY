using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NAVY.Lesson8
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Random r = new Random();

        private ICellularAutomaton cellularAutomaton;
        public ICellularAutomaton CellularAutomaton
        {
            get { return cellularAutomaton; }
            set
            {
                cellularAutomaton = value;
                NotifyPropertyChanged();
            }
        }

        public MainViewModel()
        {
            SetGameOfLife();
        }

        public void SetGameOfLife() => CellularAutomaton = new CellularAutomaton<GameOfLifeCellState>(100, 100, new GameOfLifeRules());

        public void SetForestFire(int w, int h)
        {
            const double P = 0.05;
            const double F = 0.001;
            CellularAutomaton = new CellularAutomaton<ForestFireCellState>(w, h, new ForestFireRules(P, F));
        }

        public void SetRandom()
        {
            if (CellularAutomaton is CellularAutomaton<GameOfLifeCellState> gof)
            {
                int w = gof.Cells.Width;
                int h = gof.Cells.Height;

                for (int i = 0; i < w * h; i++)
                {
                    int x = r.Next(w);
                    int y = r.Next(h);
                    if (r.NextDouble() <= 0.05d)
                    {
                        gof.Cells[x, y] = GameOfLifeCellState.Alive;
                    }
                }
            }
            else if(CellularAutomaton is CellularAutomaton<ForestFireCellState> ff)
            {
                int w = ff.Cells.Width;
                int h = ff.Cells.Height;

                for (int i = 0; i < w * h; i++)
                {
                    int x = r.Next(w);
                    int y = r.Next(h);
                    if (r.NextDouble() <= 0.1d)
                        ff.Cells[x, y] = ForestFireCellState.Tree;
                }
            }
        }

        public void Next()
        {
            CellularAutomaton.NextGeneration();
        }
    }
}
