using MathNet.Numerics.LinearAlgebra;

namespace NAVY.Lesson5
{
    class Program
    {

        static void Test()
        {
            Vector<float> u = Vector<float>.Build.DenseOfArray(new float[] { 1, 0, 1, 0 });
            Letters.Print(u);
            Hopfield hopfield = new Hopfield(4);
            hopfield.Train(u);

            Vector<float> u2 = Vector<float>.Build.DenseOfArray(new float[] { 1, 1, 1, 0 });
            Letters.Print(u2);
            var u2fixed = hopfield.Recognise(u2);
            Letters.Print(u2fixed);
        }

        static void Main(string[] args)
        {
            Test();

            const int N = 8;
            var hopfield = new Hopfield(N);
            var letters = new Letters(N);

            foreach (var letter in letters)
                Letters.Print(letter);

            var noiseLevels = new[] { 0, 5, 10, 20, 30, 31, 32, 33, 35, 40, 45, 50 };

            foreach (var letter in letters)
            {
                Letters.Print(letter, "Original=");
                hopfield.Train(letter);

                foreach (var noise in noiseLevels)
                {
                    var noised = letters.Noise(letter, noise);
                    //Letters.Print(noised, $"Noised ({noise})");
                    var recovered = hopfield.Recognise(noised);
                    //Letters.Print(recovered, $"Recovered ({noise})");

                    Letters.Print(noised, $"Noised ({noise})", recovered, $"Recovered ({noise})");
                }
            }
        }
    }
}
