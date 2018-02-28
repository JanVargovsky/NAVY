using System;
using System.Diagnostics;
using System.Linq;

namespace NAVY.Lesson2
{
    class Perceptron
    {
        internal readonly double[] w;

        public Perceptron(double[] w)
        {
            this.w = w;
        }

        public void AdjustWeights(double[] x, double y)
        {
            Debug.Assert(w.Length - 1 == x.Length);

            const double LearningConstant = 0.01d;

            double error = GetError(x, y);
            for (int i = 0; i < w.Length; i++)
            {
                var value = i < x.Length ? x[i] : 1;
                var newW = w[i] + error * value * LearningConstant;
                Console.WriteLine($"w[{i}] = {w[i]} => {newW}");
                w[i] = newW;
            }
        }

        public double GetError(double[] x, double y)
        {
            double guess = Evaluate(x);
            double error = y - guess;
            return Math.Abs(error);
        }

        public double Evaluate(double[] x)
        {
            Debug.Assert(w.Length - 1 == x.Length);

            double sum = 0;
            for (int i = 0; i < x.Length; i++)
                sum += w[i] * x[i];
            return sum >= w[w.Length - 1] ? 1 : 0;
            //double sum = w[w.Length - 1];
            //for (int i = 0; i < x.Length; i++)
            //    sum += w[i] * x[i];
            //return Math.Sign(sum) / 2d + 0.5;
        }

        internal static Perceptron GetRandomPerceptron(Random random, int inputLength) => new Perceptron(
            Enumerable.Range(0, inputLength + 1)
            .Select(_ => random.NextDouble())
            .ToArray());

        public override string ToString() => $"w=[{string.Join(", ", w)}]";
    }
}
