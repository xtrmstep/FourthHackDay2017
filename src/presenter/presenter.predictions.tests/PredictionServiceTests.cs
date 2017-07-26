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
            var storedDemand = DataSource.GetSalesHistory(true);
            var singleProductHistory = storedDemand.Where(d => d.Plu == 3252).ToArray();
            var predictionService = new PredictionService(MlSettings.Endpoint, MlSettings.Key);

            var actual = predictionService.GetMovingAveragePredictions(singleProductHistory);
        }

        [Fact]
        public void GetAmlPredictions_case1()
        {
            var storedDemand = DataSource.GetSalesHistory(true);
            var singleProductHistory = storedDemand.Where(d => d.Plu == 3252).ToArray();
            var predictionService = new PredictionService(MlSettings.Endpoint, MlSettings.Key);

            var actual = predictionService.GetAmlPredictions(singleProductHistory).Result;
        }
    }
}
