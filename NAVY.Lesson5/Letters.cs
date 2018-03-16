using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace NAVY.Lesson5
{
    public class Letters : IEnumerable<Vector<float>>
    {
        readonly int N;

        public Vector<float> X => GetX(N);
        public Vector<float> O => GetO(N);
        public Vector<float> H => GetH(N);

        public Letters(int n) => N = n;

        public IEnumerator<Vector<float>> GetEnumerator()
        {
            yield return X;
            yield return O;
            yield return H;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static void Print(Vector<float> u, string title = null)
        {
            if (!string.IsNullOrEmpty(title))
                Console.WriteLine(title);

            int n = (int)Math.Sqrt(u.Count);
            Console.WriteLine(PrintToStringBuilder(u, n));
        }

        public static void Print(Vector<float> u, string uTitle, Vector<float> v, string vTitle)
        {
            int n = (int)Math.Sqrt(u.Count);
            Console.Write(uTitle.PadRight(n + 3));
            Console.WriteLine(vTitle);
            Console.WriteLine(PrintToStringBuilder(u, n, v));
        }

        const char VerticalBorderChar = '║';
        const char TopLeftBorderChar = '╔';
        const char TopRightBorderChar = '╗';
        const char BottomLeftBorderChar = '╚';
        const char BottomRightBorderChar = '╝';

        static StringBuilder PrintToStringBuilder(Vector<float> u, int n)
        {
            StringBuilder sb = new StringBuilder((n + 2) * (n + 2));

            var horizontalBorderLine = new string('═', n);

            sb.AppendLine($"{TopLeftBorderChar}{horizontalBorderLine}{TopRightBorderChar}");
            for (int y = 0; y < n; y++)
            {
                sb.Append(VerticalBorderChar);
                for (int x = 0; x < n; x++)
                    sb.Append($"{(u[y * n + x] == 1f ? '#' : ' ')}");
                sb.Append(VerticalBorderChar).AppendLine();
            }
            sb.AppendLine($"{BottomLeftBorderChar}{horizontalBorderLine}{BottomRightBorderChar}");

            return sb;
        }

        static StringBuilder PrintToStringBuilder(Vector<float> u, int n, Vector<float> v)
        {
            StringBuilder sb = new StringBuilder((n + 2) * (n + 3) * 2);

            void PrintLine(Vector<float> a, int y)
            {
                sb.Append(VerticalBorderChar);
                for (int x = 0; x < n; x++)
                    sb.Append($"{(a[y * n + x] == 1f ? '#' : ' ')}");
                sb.Append(VerticalBorderChar);
            }

            var horizontalBorderLine = new string('═', n);

            sb.Append($"{TopLeftBorderChar}{horizontalBorderLine}{TopRightBorderChar}")
                .Append(" ")
                .AppendLine($"{TopLeftBorderChar}{horizontalBorderLine}{TopRightBorderChar}");
            for (int y = 0; y < n; y++)
            {
                PrintLine(u, y);

                sb.Append(" ");

                PrintLine(v, y);
                sb.AppendLine();
            }
            sb.Append($"{BottomLeftBorderChar}{horizontalBorderLine}{BottomRightBorderChar}")
                .Append(" ")
                .Append($"{BottomLeftBorderChar}{horizontalBorderLine}{BottomRightBorderChar}");

            return sb;
        }

        static Random r = new Random(42);
        public Vector<float> Noise(Vector<float> u, int c)
        {
            var x = Vector<float>.Build.SameAs(u);
            u.CopyTo(x);
            HashSet<int> usedIndexes = new HashSet<int>();
            while (c-- > 0)
            {
                int i = -1;
                do
                {
                    i = r.Next(x.Count);
                } while (!usedIndexes.Add(i));

                var v = x[i];
                x[i] = v == 1 ? 0 : 1; // exchange 0 <-> 1
            }
            return x;
        }

        Vector<float> GetX(int n)
        {
            var u = Vector<float>.Build.Sparse(n * n);

            for (int i = 0; i < n; i++)
            {
                u[i * n + i] = 1;
                u[i * n + n - 1 - i] = 1;
            }
            return u;
        }

        Vector<float> GetO(int n)
        {
            var u = Vector<float>.Build.Sparse(n * n);

            for (int i = 1; i < n - 1; i++)
            {
                // top
                u[i] = 1;
                // bottom
                u[n * (n - 1) + i] = 1;
                // left
                u[i * n + 1] = 1;
                // right
                u[i * n + n - 2] = 1;
            }
            return u;
        }

        Vector<float> GetH(int n)
        {
            var u = Vector<float>.Build.Sparse(n * n);

            for (int i = 1; i < n; i++)
            {
                // left
                u[i * n + 1] = 1;
                // right
                u[i * n + n - 2] = 1;
            }

            for (int i = 1; i < n - 1; i++)
                // middle
                u[n / 2 * n + i] = 1;

            return u;
        }
    }
}
