using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLFormulas
{
    public class MLMathFormulas
    {
        /// <summary>
        ///     Mean Absolute Error (MAE) measures how far predicted values are away from observed values.
        /// </summary>
        /// <remarks>
        ///     The Mean Absolute Error (or MAE) is the sum of the absolute differences between predictions and actual values. It
        ///     gives an idea of how wrong the predictions were. The measure gives an idea of the magnitude of the error, but no
        ///     idea of the direction (e.g. over or under predicting).
        ///     The less the better.
        /// </remarks>
        /// <returns></returns>
        public static double MeanAbsoluteError(double[] actuals, double[] estimated)
        {
            var result = actuals.Select((t, i) => Math.Abs(t - estimated[i])).Sum();
            result /= actuals.Length;
            return result;
        }

        /// <summary>
        ///     The Mean Squared Error (or MSE) is much like the mean absolute error in that it provides a gross idea of the
        ///     magnitude of error.
        /// </summary>
        /// <remarks>
        ///     Taking the square root of the mean squared error converts the units back to the original units of the output
        ///     variable and can be meaningful for description and presentation.This is called the Root Mean Squared Error(or
        ///     RMSE).
        /// </remarks>
        /// <returns></returns>
        public static double MeanSquaredError(double[] actuals, double[] estimated)
        {
            var result = actuals.Select((t, i) => Math.Pow(estimated[i] - t, 2)).Sum();
            result /= actuals.Length;
            return result;
        }

        /// <summary>
        ///     The R^2 (or R Squared) metric provides an indication of the goodness of fit of a set of predictions to the actual
        ///     values. In statistical literature, this measure is called the coefficient of determination.
        /// </summary>
        /// <remarks>
        ///     This is a value between 0 and 1 for no-fit and perfect fit respectively. Less than 0.5 is the poor fit.
        ///     Calculation: http://www.statisticshowto.com/what-is-a-coefficient-of-determination/
        /// </remarks>
        /// <returns></returns>
        public static double CoefficientOfDetermination(double[] actuals, double[] estimated)
        {
            var correlationCoef = Correlation(actuals, estimated);
            correlationCoef *= correlationCoef;
            return correlationCoef;
        }

        /// <summary>
        ///     http://www.statisticshowto.com/how-to-compute-pearsons-correlation-coefficients/
        /// </summary>
        /// <param name="actuals"></param>
        /// <param name="estimated"></param>
        /// <returns></returns>
        public static double Correlation(double[] values1, double[] values2)
        {
            //if (values1.Length != values2.Length)
            //    throw new ArgumentException("values must be the same length");

            var avg1 = values1.Average();
            var avg2 = values2.Average();

            double res = default(double);

            for (int i = 0; i < values1.Length; i++)
            {
                var val1 = (values1[i] - avg1) * (values2[i] - avg2);
                res += val1;
            }

            var sum1 = values1.Zip(values2, (x1, y1) => (x1 - avg1) * (y1 - avg2)).Sum();

            var sumSqr1 = values1.Sum(x => Math.Pow((x - avg1), 2.0));
            var sumSqr2 = values2.Sum(y => Math.Pow((y - avg2), 2.0));

            var result = sum1 / Math.Sqrt(sumSqr1 * sumSqr2);

            return result;
        }
    }
}
