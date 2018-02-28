using System;

namespace NAVY.Lesson2
{
    class Program
    {
        static void Main(string[] args)
        {
            var xorData = new(double[] Input, double Output)[]
            {
                (new[] { 0d, 0d }, 0),
                (new[] { 1d, 0d }, 1d),
                (new[] { 0d, 1d }, 1d),
                (new[] { 1d, 1d }, 0),
            };
            var xorNeuralNet = new NeuralNet(2);

            int i = 0;
            while(!xorNeuralNet.Train(xorData)) i++;

            Console.WriteLine($"Training took {i} epochs");
            Console.WriteLine(xorNeuralNet);
        }
    }
}
