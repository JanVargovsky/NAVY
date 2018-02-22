using System;
using System.Collections.Generic;
using System.Linq;

namespace NAVY.Lesson2
{
    class NeuralNet
    {
        static Random r = new Random(42);

        //readonly Perceptron[] input;
        internal readonly Perceptron[][] hiddenLayer;
        internal readonly Perceptron output;

        public NeuralNet(params int[] sizes)
        {
            //input = Enumerable.Range(0, sizes[0]).Select(_ => Perceptron.GetRandomPerceptron(r, sizes[0])).ToArray();
            hiddenLayer = new Perceptron[sizes.Length][];
            for (int i = 0; i < hiddenLayer.Length; i++)
                hiddenLayer[i] = Enumerable.Range(0, sizes[i]).Select(_ => Perceptron.GetRandomPerceptron(r, sizes[i])).ToArray();
            output = Perceptron.GetRandomPerceptron(r, sizes[sizes.Length - 1]);
        }

        public bool Train((double[] Inputs, double Output)[] data)
        {
            void SetNewWeights(double[] w)
            {
                int offset = 0;
                foreach (var p in GetPerceptrons())
                {
                    Array.Copy(w, offset, p.w, 0, p.w.Length);
                    offset += p.w.Length;
                }
            }

            SimulatedAnnealing simulatedAnnealing = new SimulatedAnnealing();
            foreach (var (Inputs, Output) in data)
            {
                var parameters = GetPerceptrons().SelectMany(t => t.w).ToArray();
                var newWeights = simulatedAnnealing.Generate(r, parameters, weights =>
                {
                    SetNewWeights(weights);
                    return GetError(Inputs, Output);
                });
                SetNewWeights(newWeights);
            }

            bool result = true;
            Console.WriteLine("results:");
            for (int i = 0; i < data.Length; i++)
            {
                var expected = data[i].Output;
                var actual = Evaluate(data[i].Inputs);
                if (expected != actual)
                {
                    result = false;
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                    Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine($"{i}=({string.Join(",", data[i].Inputs)}), expected={expected}, actual={actual}");
                Console.ResetColor();
            }

            return result;
        }

        double[] EvaluateHiddenLayer(double[] x)
        {
            double[] previousResults = x;
            for (int i = 0; i < hiddenLayer.Length; i++)
            {
                var size = hiddenLayer[i].Length;
                double[] actualResults = new double[size];
                for (int j = 0; j < size; j++)
                    actualResults[j] = hiddenLayer[i][j].Evaluate(previousResults);
                previousResults = actualResults;
            }
            return previousResults;
        }

        double GetError(double[] x, double y)
        {
            var hiddenLayerResult = EvaluateHiddenLayer(x);
            return Math.Abs(output.GetError(hiddenLayerResult, y));
        }

        public double Evaluate(double[] x)
        {
            var hiddenLayerResult = EvaluateHiddenLayer(x);
            return output.Evaluate(hiddenLayerResult);
        }

        public IEnumerable<Perceptron> GetPerceptrons() => hiddenLayer.SelectMany(t => t).Append(output);

        //public IEnumerable<Perceptron> GetPerceptrons()
        //{
        //    foreach (var layer in hiddenLayer)
        //        foreach (var p in layer)
        //            yield return p;
        //    yield return output;
        //}

        public override string ToString() =>
            string.Join(Environment.NewLine, GetPerceptrons().Select(p => $"w=({string.Join(",", p.w)})"));
    }
}
