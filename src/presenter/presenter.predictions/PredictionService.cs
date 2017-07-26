using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using presenter.data.types;

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

        public async Task<string> GetAmlPredictions(Tuple<DateTime, ProductDemand>[] data)
        {
            var payload = data.Select(d => CreatePayload(d.Item1, d.Item2)).ToList();
            var response = await GetPrediction(payload);
            return string.Empty;
        }

        public Dictionary<DateTime, double> GetMovingAveragePredictions(Tuple<DateTime, ProductDemand>[] data)
        {
            throw new NotImplementedException();
        }

        private Dictionary<string, string> CreatePayload(DateTime dateTime, ProductDemand data)
        {
            return new Dictionary<string, string>()
            {
                {"Locationid", data.Locationid.ToString()},
                {"RecipeName", data.RecipeName},
                {"PLU", data.Plu.ToString()},
                {"Salesdate", data.Salesdate.ToString("s")},
                {"Quantity", data.Quantity.ToString()},
                {"NetSalesPrice", data.NetSalesPrice.ToString()},
                {"CostPrice", data.CostPrice.ToString()},
                {"Year", data.Year.ToString()},
                {"Month", data.Month.ToString()},
                {"Day", data.Day.ToString()},
                {"WeekDay", data.WeekDay.ToString()},
                {"YearDay", data.YearDay.ToString()}
            };
        }

        private double ExtractEstimatedValue(string response)
        {
            var dic = JsonConvert.DeserializeObject(response);
            return double.NaN;
        }

        private async Task<string> GetPrediction(List<Dictionary<string, string>> payload)
        {
            using (var client = new HttpClient())
            {
                var scoreRequest = new
                {
                    Inputs = new Dictionary<string, List<Dictionary<string, string>>> { { "input1", payload } },
                    GlobalParameters = new Dictionary<string, string>() { }
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _key);
                client.BaseAddress = new Uri(_uri);

                var response = await client.PostAsJsonAsync(string.Empty, scoreRequest);

                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
        }
    }
}