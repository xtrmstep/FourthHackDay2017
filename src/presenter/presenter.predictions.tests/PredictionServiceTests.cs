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
            var storedDemand = CsvReader.Load(@"testdata\\testdata.csv", true);
            var productDemand = storedDemand.Where(d => d.Plu == 3604);
            var data = productDemand.ToDictionary(d => d.Salesdate, d => d);
            var predictionService = new PredictionService(string.Empty, string.Empty);

            var actual = predictionService.GetMovingAveragePredictions(data);
        }
    }
}
