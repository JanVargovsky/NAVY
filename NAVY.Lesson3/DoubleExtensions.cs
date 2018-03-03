using System;

namespace NAVY.Lesson3
{
    public static class DoubleExtensions
    {
        public static bool ApproxEquals(this double a, double b) => ApproxEquals(a,b, double.Epsilon);
        public static bool ApproxEquals(this double a, double b, double e) => Math.Abs(a - b) < e;
    }
}
