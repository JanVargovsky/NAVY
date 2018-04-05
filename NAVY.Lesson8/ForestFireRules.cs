using System;
using System.Linq;
using System.Threading;

namespace NAVY.Lesson8
{
    public enum ForestFireCellState
    {
        Empty,
        Tree,
        Burning,
    }

    public class ForestFireRules : IRules<ForestFireCellState>
    {
        static int seed = Environment.TickCount;
        static readonly ThreadLocal<Random> random = new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref seed)));
        Random Random => random.Value;

        readonly double p;
        readonly double f;

        public ForestFireRules(double p, double f)
        {
            this.p = p;
            this.f = f;
        }

        public ForestFireCellState NextState(int x, int y, Board<ForestFireCellState> board)
        {
            ForestFireCellState state = board[x, y];

            // 1. A burning cell turns into an empty cell
            bool IsRule1() => state == ForestFireCellState.Burning;

            // 2. A tree will burn if at least one neighbor is burning
            bool IsRule2() => state == ForestFireCellState.Tree && board.GetNeighbors(x, y).Any(t => t == ForestFireCellState.Burning);

            // 3. A tree ignites with probability f even if no neighbor is burning
            bool IsRule3() => state == ForestFireCellState.Tree && Random.NextDouble() <= f;

            // 4. An empty space fills with a tree with probability p
            bool IsRule4() => state == ForestFireCellState.Empty && Random.NextDouble() <= p;

            if (IsRule1()) return ForestFireCellState.Empty;
            if (IsRule2()) return ForestFireCellState.Burning;
            if (IsRule3()) return ForestFireCellState.Burning;
            if (IsRule4()) return ForestFireCellState.Tree;
            return state;
        }
    }
}
