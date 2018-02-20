using System;
using System.Diagnostics;

namespace NAVY.Lesson1
{
    class Perceptron
    {
        readonly double[] w;
        double b;

        public Perceptron(double[] w, double b)
        {
            this.w = w;
            this.b = b;
        }

        public void AdjustWeights(int[] x, int y)
        {
            Debug.Assert(w.Length == x.Length);

            const double LearningConstant = 0.01d;

            double guess = Evaluate(x);
            double error = y - guess;
            for (int i = 0; i < w.Length; i++)
            {
                var newW = w[i] + error * x[i] * LearningConstant;
                Console.WriteLine($"w[{i}] = {w[i]} => {newW}");
                w[i] = newW;
            }
            var newB = b + error * LearningConstant;
            Console.WriteLine($"b = {b} => {newB}");
            b = newB;
        }

        public int Evaluate(int[] x)
        {
            Debug.Assert(w.Length == x.Length);

            double sum = b;
            for (int i = 0; i < w.Length; i++)
                sum += w[i] * x[i];
            return Math.Sign(sum);
        }

        public override string ToString() => $"w=[{string.Join(", ", w)}], bias={b}";
    }
}
