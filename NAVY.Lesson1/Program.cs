using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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

    class Program
    {
        static Random random = new Random();

        Perceptron GetPerceptron(IEnumerable<(int[] Inputs, int Output)> data)
        {
            var inputLength = data.First().Inputs.Length;

            double RandomWeight() => random.NextDouble() * 2 - 1;

            Perceptron GetRandomPerceptron() => new Perceptron(
                Enumerable.Range(0, inputLength)
                .Select(t => RandomWeight())
                .ToArray(),
                RandomWeight());

            bool Check(Perceptron p) => data.All(t => p.Evaluate(t.Inputs) == t.Output);

            Perceptron perceptron = GetRandomPerceptron();

            bool correct;
            do
            {
                correct = true;
                foreach (var (input, output) in data)
                    if (perceptron.Evaluate(input) != output)
                    {
                        perceptron.AdjustWeights(input, output);
                        correct = false;
                    }
            } while (!correct);

            Debug.Assert(Check(perceptron));

            return perceptron;
        }

        static void Main(string[] args)
        {
            var p = new Program();

            // 2x + 1
            int GetLineValue(int x) => 2 * x + 1;
            int IsAbove(int x, int y) => GetLineValue(x) > y ? 1 : -1;

            var learningData = new List<(int[], int)>();
            for (int x = -2; x <= 2; x++)
                for (int y = -2; y <= 2; y++)
                    learningData.Add((new[] { x, y }, IsAbove(x, y)));
            var perceptron = p.GetPerceptron(learningData);

            Debug.Assert(perceptron.Evaluate(new[] { 5, 8 }) == 1);
            Debug.Assert(perceptron.Evaluate(new[] { -5, 8 }) == -1);
            Console.WriteLine(perceptron);
        }
    }
}
