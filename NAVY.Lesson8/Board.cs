using System.Collections.Generic;

namespace NAVY.Lesson8
{
    public class Board<CellType>
    {
        readonly CellType[] cells;
        public int Width { get; }
        public int Height { get; }

        public Board(int width, int height)
        {
            cells = new CellType[width * height];
            Width = width;
            Height = height;
        }

        public CellType this[int x, int y]
        {
            get => cells[y * Width + x];
            set => cells[y * Width + x] = value;
        }

        bool IsInRange(int x, int y) =>
            x >= 0 &&
            y >= 0 &&
            x < Width &&
            y < Height;

        public IEnumerable<CellType> GetNeighbors(int x, int y)
        {
            for (int i = x - 1; i <= x + 1; i++)
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (i == x && j == y)
                        continue;

                    if (IsInRange(i, j))
                        yield return this[i, j];
                }
        }
    }
}
