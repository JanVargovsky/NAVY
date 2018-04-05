using System.Threading.Tasks;

namespace NAVY.Lesson8
{
    public interface ICellularAutomaton
    {
        void NextGeneration();
    }

    public class CellularAutomaton<CellType> : ICellularAutomaton
        where CellType: new()
    {
        internal Board<CellType> Cells { get; private set; }
        private protected IRules<CellType> rules;

        public CellularAutomaton(int weight, int height, IRules<CellType> rules)
        {
            Cells = new Board<CellType>(weight, height);
            this.rules = rules;
        }

        public void NextGeneration()
        {
            var c = new Board<CellType>(Cells.Width, Cells.Height);

            Parallel.For(0, Cells.Height, y =>
            {
                Parallel.For(0, Cells.Width, x =>
                {
                    c[x, y] = rules.NextState(x, y, Cells);
                });
            });

            Cells = c;
        }
    }
}
