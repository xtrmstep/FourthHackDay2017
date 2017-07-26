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
            var singleProductHistory = storedDemand.Where(d => d.Plu == 3604);
            var data = singleProductHistory.Select(d => new Tuple<DateTime, ProductDemand>(d.Salesdate, d)).ToArray();
            var predictionService = new PredictionService(string.Empty, string.Empty);

            var actual = predictionService.GetMovingAveragePredictions(data);
        }

        [Fact]
        public void GetAmlPredictions_case1()
        {
            var storedDemand = DataSource.GetSalesHistory();
            var singleProductHistory = storedDemand.Where(d => d.Plu == 3604);
            var data = singleProductHistory.Select(d => new Tuple<DateTime, ProductDemand>(d.Salesdate, d)).ToArray();
            var predictionService = new PredictionService(string.Empty, string.Empty);

            var actual = predictionService.GetAmlPredictions(data);
        }
    }
}
