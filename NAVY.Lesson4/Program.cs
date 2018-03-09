using System;

namespace NAVY.Lesson4
{
    class Program
    {
        static void Main(string[] args)
        {
            const float Gamma = 0.8f;
            const int Size = 3;
            const int Episodes = 1000;
            const int Blocks = 0;

            Random r = new Random(42);
            FindTheCheeseSettings findTheCheese = FindTheCheeseSettings.Generate(r, Size, Blocks);
            QLearning qLearning = new QLearning(Gamma);
            findTheCheese.Set(qLearning);
            ConsoleExtensions.WriteMatrix(qLearning.R, "R");

            qLearning.Learn(findTheCheese.Start, findTheCheese.Cheese, Episodes, Size, r);
            qLearning.WritePath(findTheCheese.Start, findTheCheese.Cheese);
        }
    }
}
