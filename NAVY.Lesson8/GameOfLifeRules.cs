using System.Linq;

namespace NAVY.Lesson8
{
    public enum GameOfLifeCellState
    {
        Dead,
        Alive,
    }

    public class GameOfLifeRules : IRules<GameOfLifeCellState>
    {
        public GameOfLifeCellState NextState(int x, int y, Board<GameOfLifeCellState> board)
        {
            int liveNeighboursCount = board
                .GetNeighbors(x, y)
                .Count(t => t == GameOfLifeCellState.Alive);
            GameOfLifeCellState state = board[x, y];

            // 1. Any live cell with fewer than two live neighbours dies, as if caused by underpopulation.
            bool IsRule1(int liveNeighbours) => state == GameOfLifeCellState.Alive && liveNeighbours < 2;

            // 2. Any live cell with two or three live neighbours lives on to the next generation.
            bool IsRule2(int liveNeighbours) => state == GameOfLifeCellState.Alive && (liveNeighbours == 2 || liveNeighbours == 3);

            // 3. Any live cell with more than three live neighbours dies, as if by overpopulation.
            bool IsRule3(int liveNeighbours) => state == GameOfLifeCellState.Alive && liveNeighbours > 3;

            // 4. Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
            bool IsRule4(int liveNeighbours) => state == GameOfLifeCellState.Dead && liveNeighbours == 3;

            if (IsRule1(liveNeighboursCount)) return GameOfLifeCellState.Dead;
            if (IsRule2(liveNeighboursCount)) return GameOfLifeCellState.Alive;
            if (IsRule3(liveNeighboursCount)) return GameOfLifeCellState.Dead;
            if (IsRule4(liveNeighboursCount)) return GameOfLifeCellState.Alive;
            return state;
        }
    }
}
