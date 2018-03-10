using System.Linq;
using Xunit;

namespace NAVY.Lesson4.Tests
{
    public class FindTheCheeseTest
    {
        readonly FindTheCheese findTheCheese;

        public FindTheCheeseTest()
        {
            findTheCheese = new FindTheCheese();
        }

        [Fact]
        public void CanMoveTest()
        {
            const int Size = 4;
            Point mid = new Point(1, 1);
            // grid (0,0) ... (4,4)
            var allNeighbors = new Point[Size * Size];
            for (int x = 0; x < Size; x++)
                for (int y = 0; y < Size; y++)
                    allNeighbors[x * Size + y] = new Point(x, y);

            var movableNeighbors = new[]
            {
                new Point(mid.Row + 1, mid.Col),
                new Point(mid.Row - 1, mid.Col),
                new Point(mid.Row, mid.Col + 1),
                new Point(mid.Row, mid.Col - 1),
            };

            foreach (var neighbor in allNeighbors)
            {
                bool canMoveExpected = movableNeighbors.Contains(neighbor);
                bool actual = findTheCheese.CanMove(mid, neighbor);
                Assert.Equal(canMoveExpected, actual);
            }
        }
    }
}
