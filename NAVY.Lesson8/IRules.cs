namespace NAVY.Lesson8
{
    public interface IRules<CellState>
    {
        CellState NextState(int x, int y, Board<CellState> board);
    }
}
