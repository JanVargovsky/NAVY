using System;

namespace NAVY.Lesson3
{
    class Program
    {
        static void Main(string[] args)
        {
            var xorTrainingData = new(double[] Input, double Output)[]
            {
                (new[] { 0d, 0d }, 0),
                (new[] { 1d, 0d }, 1d),
                (new[] { 0d, 1d }, 1d),
                (new[] { 1d, 1d }, 0),
            };
            var xorNN = new XorNeuralNet(false);
            double AcceptedError = 1e-6;

            int epochCount = 0;
            bool allCorrect;
            do
            {
                Console.WriteLine($"results ({epochCount}):");
                allCorrect = true;
                foreach (var (input, expected) in xorTrainingData)
                {
                    var actual = xorNN.EvaluateWithBackpropagation(input, expected);
                    if (!actual.ApproxEquals(expected, AcceptedError))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        allCorrect = false;
                    }
                    else
                        Console.ForegroundColor = ConsoleColor.Green;

                    Console.WriteLine($"({string.Join(",", input)}), expected={expected}, actual={actual:n5}");
                    Console.ResetColor();
                }
                epochCount++;
            } while (!allCorrect);
            Console.WriteLine($"Learned after {epochCount} epoch(s)");
        }
    }
}
