﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
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

        public Dictionary<DateTime, double> GetAmlPredictions(Tuple<DateTime, ProductDemand>[] data)
        {
            var estimations = new ConcurrentDictionary<DateTime, double>();
            Parallel.ForEach(data, async tuple =>
            {
                var payload = CreatePayload(tuple.Item1, tuple.Item2);
                var response = await GetPrediction(payload);
                var estimatedValue = ExtractEstimatedValue(response);
                estimations.TryUpdate(tuple.Item1, estimatedValue, double.NaN);
            });
            return estimations.ToDictionary(e => e.Key, e => e.Value);
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
                {"Plu", data.Plu.ToString()},
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