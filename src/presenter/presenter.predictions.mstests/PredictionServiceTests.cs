using presenter.data;
using presenter.data.types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace presenter.predictions.mstests
{
    [TestClass]
    public class PredictionServiceTests
    {
        [TestMethod]
        public void Generate_Predictions_fromMovingAverage()
        {
            var storedDemand = DataSource.GetSalesHistory();
            var salesForMarch = storedDemand.Where(d => d.Month == 3).ToArray();

            var predictionService = new PredictionService(MlSettings.Endpoint, MlSettings.Key);

            var movingAverages = predictionService.GetMovingAveragePredictions(salesForMarch);

            DataSource.SaveToCsv2(movingAverages, "c:\\logs\\sales_march_moving_average.csv");
        }

        [TestMethod]
        public void Generate_Predictions_fromMl()
        {
            var storedDemand = DataSource.GetSalesHistory();
            var salesForMarch = storedDemand.Where(d => d.Month == 3).ToArray();

            var predictionService = new PredictionService(MlSettings.Endpoint, MlSettings.Key);

            var estimations = predictionService.GetAmlPredictions2(salesForMarch);

            DataSource.SaveToCsv2(estimations, "c:\\logs\\sales_march_ml.csv");
        }

        [TestMethod]
        public void Generate_RealSales()
        {
            var storedDemand = DataSource.GetSalesHistory();
            var salesForMarch = storedDemand.Where(d => d.Month == 3).ToArray();

            var asEstimatedDemand = salesForMarch.Select(d => new ProductDemandEstimated
            {
                Locationid = d.Locationid,
                Plu = d.Plu,
                Year = d.Year,
                Month = d.Month,
                Day = d.Day,
                Quantity = d.Quantity
            }).ToArray();
            DataSource.SaveToCsv2(asEstimatedDemand, "c:\\logs\\sales_march.csv");
        }
    }
}
