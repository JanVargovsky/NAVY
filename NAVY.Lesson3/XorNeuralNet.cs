using System;

namespace NAVY.Lesson3
{
    class XorNeuralNet
    {
        IPerceptron net0, net1, output;

        public XorNeuralNet(bool predefined)
        {
            if (predefined)
            {
                const double
                    w1 = 0.5d,
                    w2 = 0.3d,
                    w3 = 0.4d,
                    w4 = 0.2d,
                    b0 = 0.5d,
                    b1 = 0.3d,
                    w5 = 0.8d,
                    w6 = 0.9d,
                    b2 = 0.8d;

                net0 = new Perceptron(new[] { w1, w3 }, b0);
                net1 = new Perceptron(new[] { w2, w4 }, b1);
                output = new OutputPerceptron(new[] { w5, w6 }, b2);
            }
            else
            {
                var r = new Random(42);
                net0 = new Perceptron(new[] { r.NextDouble(), r.NextDouble() }, r.NextDouble());
                net1 = new Perceptron(new[] { r.NextDouble(), r.NextDouble() }, r.NextDouble());
                output = new OutputPerceptron(new[] { r.NextDouble(), r.NextDouble() }, r.NextDouble());
            }
        }

        public double Evaluate(double[] input)
        {
            var o0 = net0.Evaluate(input);
            var o1 = net1.Evaluate(input);
            var y = output.Evaluate(new[] { o0, o1 });
            return y;
        }

        public double EvaluateWithBackpropagation(double[] x, double y)
        {
            const double L = 0.1d;

            // Classic eval
            var o0 = net0.Evaluate(x);
            var o1 = net1.Evaluate(x);
            var z = output.Evaluate(new[] { o0, o1 });

            // Backpropagation
            // output layer
            var E = y - z;
            var dZ = L * E;
            // hidden layer
            var E1 = Perceptron.Derivative(o0) * dZ * output.W[0];
            var E2 = Perceptron.Derivative(o1) * dZ * output.W[1];

            // adjust weights
            // output layer
            output.W[0] += dZ * o0; // w5'
            output.W[1] += dZ * o1; // w6'
            output.B += dZ; // b2'

            // hidden layer
            net0.W[0] += x[0] * E1; // w1'
            net0.W[1] += x[1] * E1; // w3'
            net0.B += E1; // b0'
            net1.W[0] += x[0] * E2; // w2'
            net1.W[1] += x[1] * E2; // w4'
            net1.B += E2; // b1'

            return z;
        }
    }
}
