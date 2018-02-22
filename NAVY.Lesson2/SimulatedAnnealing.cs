using System;
using System.Linq;

namespace NAVY.Lesson2
{
    public class SimulatedAnnealing
    {
        public double[] Generate(Random random, double[] parameters, Func<double[], double> fitnessFunc)
        {
            double[] GeneratePoint() => 
                Enumerable.Range(0, parameters.Length)
                .Select(_ => random.NextDouble() * 4d - 2d).
                ToArray();

            const int T0 = 2000;
            const double Alpha = 0.99d;
            const double Tn = 1e-6d;

            double t = T0;
            var x0 = parameters;
            int it = 0;
            do
            {
                double[] x = GeneratePoint();
                double f = fitnessFunc(x0) - fitnessFunc(x);

                if (f > 0)
                    x0 = x;
                else
                {
                    var r = random.NextDouble();
                    var v = Math.Pow(Math.E, -Math.E / t);
                    if (r < v)
                        x0 = x;
                    t *= Alpha;
                }
                it++;
            } while (t >= Tn);

            return x0;
        }
    }
}
