using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NAVY.Lesson1
{

    class Program
    {
        static Random random = new Random(42);

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

        async static Task Main(string[] args)
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
            var nonImplication = new(int[], int)[]
            {
                (new[] { 0, 0 }, 1),
                (new[] { 1, 0 }, -1),
                (new[] { 0, 1 }, 1),
                (new[] { 1, 1 }, 1),
            };
            var perceptron1 = p.GetPerceptron(nonImplication);

            var xorData = new(int[], int)[]
            {
                (new[] { 0, 0 }, 0),
                (new[] { 1, 0 }, 1),
                (new[] { 0, 1 }, 1),
                (new[] { 1, 1 }, 0),
            };
            CancellationToken cancellationToken = default;
            var perceptron2Task = Task.Run(() => p.GetPerceptron(xorData), cancellationToken);
            var task = await Task.WhenAny(perceptron2Task, Task.Delay(TimeSpan.FromSeconds(5)));
            if (task == perceptron2Task)
                throw new Exception("Impossible");
        }
    }
}
