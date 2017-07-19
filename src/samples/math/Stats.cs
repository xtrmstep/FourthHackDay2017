using System;
using System.Linq;

namespace stats.math
{
    /// <summary>
    ///     Statistic functions to evaluate sequences
    /// </summary>
    /// <remarks>
    ///     Some information is taken from
    ///     http://machinelearningmastery.com/metrics-evaluate-machine-learning-algorithms-python/
    /// </remarks>
    public class Stats
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
        private static double Correlation(double[] actuals, double[] estimated)
        {
            var ae = actuals.Select((t, i) => t*estimated[i]).ToArray();
            var a2 = actuals.Select(t => t*t).ToArray();
            var e2 = estimated.Select(t => t*t).ToArray();

            var sumActuals = actuals.Sum();
            var sumEstimated = actuals.Sum();
            var sumAe= ae.Sum();
            var sumA2 = a2.Sum();
            var sumE2 = e2.Sum();

            var n = actuals.Length;
            var r = (n*sumAe - sumActuals*sumEstimated)/Math.Sqrt((n*sumA2 - sumA2*sumA2)*(n*sumE2 - sumE2*sumE2));
            return r;
        }
    }
}