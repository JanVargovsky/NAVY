using System;
using Xunit;

namespace NAVY.Lesson7.Tests
{
    public class MandelbrotSetTest
    {
        [Theory]
        [InlineData(0d, 0, 100, 0, 10, 0)]
        [InlineData(50, 0, 100, 0, 10, 5)]
        [InlineData(100, 0, 100, 0, 10, 10)]
        [InlineData(0d, 0, 1000, -2.5, 1, -2.5)]
        [InlineData(0d, 0, 1000, -1, 1, -1)]
        [InlineData(250, 0, 1000, -1, 1, -0.5)]
        [InlineData(500, 0, 1000, -1, 1, 0)]
        [InlineData(750, 0, 1000, -1, 1, 0.5)]
        [InlineData(1000, 0, 1000, -1, 1, 1)]
        public void ScaleTest(double value, double currentScaleMin, double currentScaleMax, double desiredScaleMin, double desiredScaleMax, double expected)
        {
            var actual = MandelbrotSet.Scale(value, currentScaleMin, currentScaleMax, desiredScaleMin, desiredScaleMax);

            Assert.Equal(expected, actual, 6);
        }
    }
}
