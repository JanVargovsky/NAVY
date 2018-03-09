using System;
using System.Collections.Generic;
using System.Linq;

namespace NAVY.Lesson4
{
    public static class ConsoleExtensions
    {
        public static void WriteMatrix<T>(this T[,] matrix, string name)
        {
            Console.WriteLine($"{name}=");
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                    Console.Write($"{matrix[i, j]}".PadLeft(5));

                Console.WriteLine();
            }
        }

        public static void WriteMatrix<TKey>(this Dictionary<TKey, Dictionary<TKey, float>> matrix, string name)
        {
            const int Pad = 6;

            Console.WriteLine($"{name}=");
            Console.Write($"{"".PadLeft(Pad)} ");
            foreach (var header in matrix.First().Value.Select(t => t.Key))
                Console.Write($"{header}".PadLeft(Pad));
            Console.WriteLine();
            Console.WriteLine(new string('-', matrix.Count * Pad + Pad + 1));

            foreach (var row in matrix)
            {
                Console.Write($"{row.Key}|".PadLeft(Pad));
                foreach (var col in row.Value)
                {
                    if (col.Value < 0f)
                        Console.ForegroundColor = ConsoleColor.Red;
                    else if (col.Value > 0f)
                        Console.ForegroundColor = ConsoleColor.Green;
                    else
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"{col.Value:f1}".PadLeft(Pad));
                    Console.ResetColor();

                }
                Console.WriteLine();
            }
        }
    }
}
