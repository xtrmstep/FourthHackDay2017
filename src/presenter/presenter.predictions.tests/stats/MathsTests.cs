using System;
using presenter.predictions.stats;
using Xunit;

namespace presenter.predictions.tests.stats
{
    public class MathsTests
    {
        [Fact]
        public void MeanAbsoluteError_test()
        {
            var real = new double[] {4, 5, 10, 5, 7, 8, 4, 8, 2, 1, 2, 3, 5, 7, 3};
            var estimated = new double[] {2, 10, 5, 7, 5, 4, 1, 2, 6, 3, 4, 7, 8, 9, 4};

            const double EXPECTED = 3.133333333d;

            var actual = Maths.MeanAbsoluteError(real, estimated);

            Assert.True(Math.Abs(EXPECTED - actual) <= double.Epsilon);
        }

        [Fact]
        public void MeanSquaredError_test()
        {
            var real = new double[] {4, 5, 10, 5, 7, 8, 4, 8, 2, 1, 2, 3, 5, 7, 3};
            var estimated = new double[] {2, 10, 5, 7, 5, 4, 1, 2, 6, 3, 4, 7, 8, 9, 4};

            const double EXPECTED = 11.8d;

            var actual = Maths.MeanSquaredError(real, estimated);

            Assert.True(Math.Abs(EXPECTED - actual) <= double.Epsilon);
        }

        [Fact]
        public void Correlation_test()
        {
            var real = new double[] {4, 5, 10, 5, 7, 8, 4, 8, 2, 1, 2, 3, 5, 7, 3};
            var estimated = new double[] {2, 10, 5, 7, 5, 4, 1, 2, 6, 3, 4, 7, 8, 9, 4};

            const double EXPECTED = 0.093864155d;

            var actual = Maths.Correlation(real, estimated);

            Assert.True(Math.Abs(EXPECTED - actual) <= double.Epsilon);
        }

        [Fact]
        public void CoefficientOfDetermination_test()
        {
            var real = new double[] {4, 5, 10, 5, 7, 8, 4, 8, 2, 1, 2, 3, 5, 7, 3};
            var estimated = new double[] {2, 10, 5, 7, 5, 4, 1, 2, 6, 3, 4, 7, 8, 9, 4};

            const double EXPECTED = 0.00881048d;

            var actual = Maths.CoefficientOfDetermination(real, estimated);

            Assert.True(Math.Abs(EXPECTED - actual) <= double.Epsilon);
        }
    }
}