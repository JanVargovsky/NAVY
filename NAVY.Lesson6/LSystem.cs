using System;
using System.Collections.Generic;

namespace NAVY.Lesson6
{
    public class LSystem
    {
        readonly string axiom;
        readonly IDictionary<char, string> rules;
        readonly double shift;
        readonly double angle;

        public LSystem(string axiom, IDictionary<char, string> rules, double shift, double angle)
        {
            this.axiom = axiom;
            this.rules = rules;
            this.shift = shift;
            this.angle = angle;
        }

        enum Direction
        {
            Left, Right
        }

        public IEnumerable<(double X, double Y)> GetPoints(double sx, double sy, double angle, int iterations)
        {
            var p = (X: sx, Y: sy);

            (double X, double Y) Next()
            {
                double Rad(double a)
                {
                    return (Math.PI / 180) * a;
                }

                double x = p.X + shift * Math.Cos(Rad(angle));
                double y = p.Y + shift * Math.Sin(Rad(angle));
                return (x, y);
            }

            void Rotate(Direction d)
            {
                if (d == Direction.Left)
                    angle += this.angle;
                else
                    angle -= this.angle;
            }

            var rewritingRule = new List<char>(axiom);

            for (int i = 0; i < iterations; i++)
            {
                var l = new List<char>();
                foreach (var c in rewritingRule)
                {
                    if (rules.TryGetValue(c, out var rule))
                        l.AddRange(rule);
                    else
                        l.Add(c);
                }
                rewritingRule = l;
            }

            yield return p;
            foreach (var c in rewritingRule)
            {
                if (c == '+')
                    Rotate(Direction.Left);
                else if (c == '-')
                    Rotate(Direction.Right);
                else
                {
                    var next = Next();
                    yield return next;
                    p = next;
                }
            }
        }
    }
}
