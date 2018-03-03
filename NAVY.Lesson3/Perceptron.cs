using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace NAVY.Lesson3
{
    public interface IPerceptron
    {
        double[] W { get; }
        double B { get; set; }

        double Evaluate(double[] input);
    }

    class Perceptron : IPerceptron
    {
        public double[] W { get; }
        public double B { get; set; }

        public Perceptron(double[] w, double b)
        {
            W = w;
            B = b;
        }

        public double Evaluate(double[] input)
        {
            Debug.Assert(W.Length == input.Length);

            double sum = 0;
            for (int i = 0; i < W.Length; i++)
                sum += W[i] * input[i];
            sum += B;

            double result = 1d / (1 + Math.Exp(-sum));
            return result;
        }

        public static double Derivative(double x) =>
            x * (1 - x);
    }

    class OutputPerceptron : IPerceptron
    {
        public double[] W { get; }
        public double B { get; set; }

        public OutputPerceptron(double[] w, double b)
        {
            W = w;
            B = b;
        }

        public double Evaluate(double[] input)
        {
            Debug.Assert(W.Length == input.Length);

            double sum = 0;
            for (int i = 0; i < W.Length; i++)
                sum += W[i] * input[i];
            sum += B;
            return sum;
        }
    }
}