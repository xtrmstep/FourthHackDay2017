using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace presenter.predictions
{
    public class PredictionService
    {
        private readonly string _key;
        private readonly string _uri;

        public PredictionService(string uri, string key)
        {
            _uri = uri;
            _key = key;
        }

        public double[] GetAmlPredictions(Dictionary<DateTime, ProductDemand> data)
        {
            var estimations = new ConcurrentDictionary<DateTime, double>();
            Parallel.ForEach(data, async pair =>
            {
                var payload = CreatePayload(pair.Key, pair.Value);
                var response = await GetPrediction(payload);
                var estimatedValue = ExtractEstimatedValue(response);
                estimations.TryUpdate(pair.Key, estimatedValue, double.NaN);
            });
            return estimations.Values.ToArray();
        }

        public double[] GetMovingAveragePredictions(Dictionary<DateTime, ProductDemand> data)
        {
            throw new NotImplementedException();
        }

        private Dictionary<string, string> CreatePayload(DateTime dateTime, ProductDemand data)
        {
            throw new NotImplementedException();
        }

        private double ExtractEstimatedValue(string response)
        {
            throw new NotImplementedException();
        }

        private async Task<string> GetPrediction(Dictionary<string, string> data)
        {
            using (var client = new HttpClient())
            {
                var payload = new List<Dictionary<string, string>> {data};
                var scoreRequest = new
                {
                    Inputs = new Dictionary<string, List<Dictionary<string, string>>> {{"input1", payload}},
                    GlobalParameters = new Dictionary<string, string>()
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _key);
                client.BaseAddress = new Uri(_uri);

                var response = await client.PostAsJsonAsync(string.Empty, scoreRequest);

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsStringAsync();

                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}