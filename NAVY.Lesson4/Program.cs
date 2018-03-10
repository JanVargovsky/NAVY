using System;

namespace NAVY.Lesson4
{
    class Program
    {
        static void Main(string[] args)
        {
            const float Gamma = 0.8f;
            const int Size = 4;
            const int Episodes = 1000;
            const int Blocks = 5;

            Random r = new Random(42);
            FindTheCheese findTheCheese = FindTheCheese.Generate(r, Size, Blocks);
            QLearning qLearning = new QLearning(Gamma);
            findTheCheese.Set(qLearning);
            ConsoleExtensions.WriteMatrix(qLearning.R, "R");

            qLearning.Learn(findTheCheese.Start, findTheCheese.Cheese, Episodes, Size, r);
            ConsoleExtensions.WriteMatrix(qLearning.Q, "Final Q");
            qLearning.WritePath(findTheCheese.Start, findTheCheese.Cheese);
        }
    }
}
