using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAVY.Lesson6
{
    public class AffineTransformation
    {
        readonly double a;
        readonly double b;
        readonly double c;
        readonly double d;
        readonly double e;
        readonly double f;

        public AffineTransformation(double a, double b, double c, double d, double e, double f)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
            this.e = e;
            this.f = f;
        }

        public (double X, double Y) Apply(double x, double y)
        {
            var newX = a * x + b * y + e;
            var newY = c * x + d * y + f;
            return (newX, newY);
        }

        public static AffineTransformation GenerateRandom(Random r) =>
            new AffineTransformation(r.NextDouble(), r.NextDouble(), r.NextDouble(), r.NextDouble(), r.NextDouble(), r.NextDouble());
    }

    public class AffineTransformationGroup
    {
        static readonly Random r = new Random();
        readonly (AffineTransformation AffineTransformation, double Probability)[] f;
        readonly double probabilitySum;

        public AffineTransformationGroup(params (AffineTransformation AffineTransformation, double Probability)[] f)
        {
            this.f = f;
            probabilitySum = f.Sum(t => t.Probability);
        }

        public AffineTransformation GetNext()
        {
            double value = r.NextDouble() * probabilitySum;
            for (int i = 0; i < f.Length; i++)
            {
                value -= f[i].Probability;
                if (value < 0)
                    return f[i].AffineTransformation;
            }
            return f[f.Length - 1].AffineTransformation;
        }

        public static AffineTransformationGroup GenerateRandom(int count) =>
            new AffineTransformationGroup(Enumerable
                .Range(0, count)
                .Select(_ => (AffineTransformation.GenerateRandom(r), r.NextDouble()))
                .ToArray());
    }

    public class IteratedFunctionSystem
    {
        readonly AffineTransformationGroup affineTransformations;

        public IteratedFunctionSystem(AffineTransformationGroup affineTransformationGroup)
        {
            affineTransformations = affineTransformationGroup;
        }

        public static IteratedFunctionSystem Presentation1 => new IteratedFunctionSystem(new AffineTransformationGroup(
                (new AffineTransformation(0, 0, 0, 0.16, 0, 0), 0.1),
                (new AffineTransformation(0.2, -0.26, 0.23, 0.22, 0, 1.6), 0.08),
                (new AffineTransformation(-0.15, 0.28, 0.26, 0.24, 0, 0.44), 0.08),
                (new AffineTransformation(0.75, 0.04, -0.04, 0.85, 0, 1.6), 0.74)));

        public static IteratedFunctionSystem Presentation2 => new IteratedFunctionSystem(new AffineTransformationGroup(
            (new AffineTransformation(0, 0, 0, 0.16, 0, 0), 0.01),
            (new AffineTransformation(0.2, -0.26, 0.23, 0.22, 0, 1.6), 0.07),
            (new AffineTransformation(-0.15, 0.28, 0.26, 0.24, 0, 0.44), 0.07),
            (new AffineTransformation(0.85, 0.04, -0.04, 0.85, 0, 1.6), 0.85)));

        public static IteratedFunctionSystem Wikipedia => new IteratedFunctionSystem(new AffineTransformationGroup(
            (new AffineTransformation(0, 0, 0, 0.25, 0, -0.4), 0.02),
            (new AffineTransformation(0.95, 0.005, -0.005, 0.93, -0.002, 0.5), 0.84),
            (new AffineTransformation(0.035, -0.2, 0.16, 0.04, -0.09, 0.02), 0.07),
            (new AffineTransformation(-0.04, 0.2, 0.16, 0.04, 0.083, 0.12), 0.07)));

        public IEnumerable<(double X, double Y)> GetPoints(double sx, double sy, int iterations)
        {
            var p = (X: sx, Y: sy);
            yield return p;
            while (iterations-- > 0)
            {
                var affineTransformation = affineTransformations.GetNext();
                var next = affineTransformation.Apply(p.X, p.Y);
                yield return next;
                p = next;
            }
        }
    }
}
