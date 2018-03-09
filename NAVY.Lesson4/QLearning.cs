using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NAVY.Lesson4
{
    public class QLearning
    {
        readonly float Gamma;

        public Dictionary<State, Dictionary<State, float>> R { get; }
        public Dictionary<State, Dictionary<State, float>> Q { get; }

        public QLearning(float gamma)
        {
            Gamma = gamma;
            R = new Dictionary<State, Dictionary<State, float>>();
            Q = new Dictionary<State, Dictionary<State, float>>();
        }

        public void Learn(Point start, Point pointEnd,int episodes, int size, Random r)
        {
            State end = new State(pointEnd.Row, pointEnd.Col);

            State state = new State(start.Row, start.Col);
            for (int i = 0; i < episodes; i++)
            {
                bool rewardFound = false;
                while (!rewardFound)
                {
                    var possibleActions = R[state].Where(t => t.Value >= 0).ToList();
                    var actionPair = possibleActions[r.Next(possibleActions.Count)];
                    var action = actionPair.Key;

                    var max = Q[action].MaxBy(t => t.Value);
                    Q[state][action] = R[state][action] + Gamma * max.Value;
                    if (action.Equals(end))
                        rewardFound = true;
                    state = action;
                    //ConsoleExtensions.WriteMatrix(Q, "Q");
                }
                state = new State(r.Next(size), r.Next(size));
                //ConsoleExtensions.WriteMatrix(Q, "Q");
            }

            ConsoleExtensions.WriteMatrix(Q, "Final Q");
        }

        public void WritePath(Point pointStart, Point pointEnd)
        {
            State current = new State(pointStart.Row, pointStart.Col);
            State end = new State(pointEnd.Row, pointEnd.Col);

            List<State> path = new List<State>
            {
                current
            };

            while (!current.Equals(end))
            {
                var bestAction = Q[current].MaxBy(t => t.Value).Key;
                path.Add(bestAction);
                current = bestAction;
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Path: {string.Join(" -> ", path)}");
            Console.ResetColor();
        }
    }

    public class State
    {
        public State(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public int Row { get; }
        public int Col { get; }

        public override string ToString() => $"({Row},{Col})";

        public override bool Equals(object obj)
        {
            var state = obj as State;
            return state != null &&
                   Row == state.Row &&
                   Col == state.Col;
        }

        public override int GetHashCode()
        {
            var hashCode = 1084646500;
            hashCode = hashCode * -1521134295 + Row.GetHashCode();
            hashCode = hashCode * -1521134295 + Col.GetHashCode();
            return hashCode;
        }
    }
}
