using System;
using System.Collections.Generic;
using System.Linq;

namespace NAVY.Lesson4
{
    public class Point
    {
        public int Row { get; }
        public int Col { get; }

        public Point(int row, int col)
        {
            Row = row;
            Col = col;
        }



        public override string ToString() => $"({Row},{Col})";

        public override bool Equals(object obj)
        {
            var point = obj as Point;
            return point != null &&
                   Row == point.Row &&
                   Col == point.Col;
        }

        public override int GetHashCode()
        {
            var hashCode = 1084646500;
            hashCode = hashCode * -1521134295 + Row.GetHashCode();
            hashCode = hashCode * -1521134295 + Col.GetHashCode();
            return hashCode;
        }
    }

    public class FindTheCheese
    {
        public int Size { get; private set; }
        public Point Start { get; private set; }
        public List<Point> Blocks { get; private set; }
        public Point Cheese { get; private set; }

        public static FindTheCheese Generate(Random r, int size, int blocks) => new FindTheCheese
        {
            Size = size,
            Blocks = Enumerable.Range(0, blocks)
                .Select(_ => new Point(r.Next(size), r.Next(size)))
                .ToList(),
            Start = new Point(0, 0),
            Cheese = new Point(size - 1, size - 1)
        };

        internal bool CanMove(Point from, Point to)
        {
            var rowDiff = Math.Abs(from.Row - to.Row);
            var colDiff = Math.Abs(from.Col - to.Col);
            //return (rowDiff <= 1 && colDiff <= 1) && (rowDiff == 1f) != (colDiff == 1f);
            //return (rowDiff == 0f && colDiff == 1f) || (rowDiff == 1f && colDiff == 0f);
            return rowDiff + colDiff == 1;
        }

        public void Set(QLearning q)
        {
            var points = new Point[Size * Size];
            for (int row = 0; row < Size; row++)
                for (int col = 0; col < Size; col++)
                    points[row * Size + col] = new Point(row, col);

            State ToState(Point p) => new State(p.Row, p.Col);
            void Fill(Dictionary<State, Dictionary<State, float>> dictionary, float value)
            {
                foreach (var statePoint in points)
                {
                    var state = ToState(statePoint);
                    var actions = dictionary[state] = new Dictionary<State, float>();
                    foreach (var actionPoint in points)
                        actions.Add(ToState(actionPoint), value);
                }
            }

            Fill(q.Q, 0);
            Fill(q.R, -1);

            foreach (var from in points)
            {
                foreach (var to in points)
                {
                    if (CanMove(from, to))
                    {
                        float value = 0;
                        if (Blocks.Contains(to))
                            value = -10;
                        if (Start.Equals(to))
                            value = 0;
                        if (Cheese.Equals(to))
                            value = 10;
                        q.R[ToState(from)][ToState(to)] = value;
                    }
                }
            }
        }
    }
}
