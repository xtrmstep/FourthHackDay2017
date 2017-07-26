using presenter.data;
using presenter.data.types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace presenter.predictions.tests
{
    public class PredictionServiceTests
    {
        [Fact]
        public void GetMovingAveragePredictions_case1()
        {
            var storedDemand = DataSource.GetSalesHistory();
            var singleProductHistory = storedDemand.Where(d => d.Plu == 2480).Take(10).ToArray();
            var predictionService = new PredictionService(MlSettings.Endpoint, MlSettings.Key);

            var actual = predictionService.GetMovingAveragePredictions(singleProductHistory);
        }

        [Fact]
        public void GetAmlPredictions_case1()
        {
            var storedDemand = DataSource.GetSalesHistory();
            var singleProductHistory = storedDemand.Where(d => d.Plu == 2480).Take(10).ToArray();
            var predictionService = new PredictionService(MlSettings.Endpoint, MlSettings.Key);

            var actual = predictionService.GetAmlPredictions(singleProductHistory).Result;
        }

        [Fact]
        public void GeneratePredictions()
        {
            var storedDemand = DataSource.GetSalesHistory();
            var salesForMarch = storedDemand.Where(d => d.Month == 3).ToArray();

            var predictionService = new PredictionService(MlSettings.Endpoint, MlSettings.Key);

            var movingAverages = predictionService.GetMovingAveragePredictions(salesForMarch);
            var estimations = predictionService.GetAmlPredictions(salesForMarch).Result;

            DataSource.SaveToCsv(salesForMarch, "c:\\logs\\sales_march.csv");
            DataSource.SaveToCsv(movingAverages, "c:\\logs\\sales_march_moving_average.csv");
            DataSource.SaveToCsv(estimations, "c:\\logs\\sales_march_ml.csv");
        }
    }
}
