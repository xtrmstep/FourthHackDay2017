using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FourthMLInsights.Models
{
    public class PredictionViewModel
    {
        public double CoefficientOfDetermination { get; set; }

        public double MeanAbsoluteError { get; set; }

        public double MeanSquaredError { get; set; }

        public double FourthCoefficientOfDetermination { get; set; }

        public double FourthMeanAbsoluteError { get; set; }

        public double FourthMeanSquaredError { get; set; }

        public double AccuracyML
        {
            get
            {
                double result = default(double);

                if (CoefficientOfDetermination != 0)
                    result = (CoefficientOfDetermination * 100);

                return Math.Round(result, 2);
            }
        }

        public double AccuracyFourth
        {
            get
            {
                double result = default(double);

                if (FourthCoefficientOfDetermination != 0)
                    result = (FourthCoefficientOfDetermination * 100);

                return Math.Round(result, 2);
            }
        }
    }
}