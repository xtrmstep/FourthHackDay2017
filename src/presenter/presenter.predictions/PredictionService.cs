using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using presenter.data;
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

        public async Task<ProductDemandEstimated[]> GetAmlPredictionsAsync(ProductDemand[] data)
        {
            var predictions = new List<ProductDemandEstimated>();
            var groupsByLocPlu = data.GroupBy(d => new Tuple<int, int>(d.Locationid, d.Plu)).ToDictionary(d => d.Key, d => d.ToArray());
            foreach(var d in groupsByLocPlu)
            {
                var payload = d.Value.Select(CreatePayload).ToList();
                var response = await GetPrediction(payload);
                var result = JsonConvert.DeserializeObject<MlPredictionResponse>(response);
                var estimations = ExtractEstimatedValue(result.Results.Output1);
                predictions.AddRange(estimations);
            }
            return predictions.ToArray();
        }

        public async Task<ProductDemandEstimated[]> GetAmlPredictionsAsync2(ProductDemand[] data)
        {
            // split array to chunks
            var predictions = new List<ProductDemandEstimated>();
            var chunks = data.Split(10000).ToArray();

            foreach(var chunk in chunks)
            {
                var payload = chunk.Select(CreatePayload).ToList();
                var response = await GetPrediction(payload);
                var result = JsonConvert.DeserializeObject<MlPredictionResponse>(response);
                var estimations = ExtractEstimatedValue(result.Results.Output1);
                predictions.AddRange(estimations);
            }

            GC.Collect();
            return predictions.ToArray();
        }

        public ProductDemandEstimated[] GetAmlPredictions(ProductDemand[] data)
        {
            var predictions =new ConcurrentBag<ProductDemandEstimated>();
            var groupsByLocPlu = data. GroupBy(d => new Tuple<int, int>(d.Locationid, d.Plu)).ToDictionary(d => d.Key, d => d.ToArray());
            Parallel.ForEach(groupsByLocPlu, async d =>
            {
                var payload = d.Value.Select(CreatePayload).ToList();
                var response = await GetPrediction(payload);
                var result = JsonConvert.DeserializeObject<MlPredictionResponse>(response);
                var estimations = ExtractEstimatedValue(result.Results.Output1);
                foreach (var estimation in estimations)
                {
                    predictions.Add(estimation);
                }
            });
            GC.Collect();
            return predictions.ToArray();
        }

        public ProductDemandEstimated[] GetAmlPredictions2(ProductDemand[] data)
        {
            // split array to chunks
            var predictions = new ConcurrentBag<ProductDemandEstimated>();
            var chunks = data.Split(10000).ToArray();
            Parallel.ForEach(chunks,
                //new ParallelOptions {MaxDegreeOfParallelism = 4},
                chunk =>
                {
                    var payload = chunk.Select(CreatePayload).ToList();
                    var response = GetPrediction(payload).Result;
                    var result = JsonConvert.DeserializeObject<MlPredictionResponse>(response);
                    var estimations = ExtractEstimatedValue(result.Results.Output1);
                    foreach (var estimation in estimations) { predictions.Add(estimation); }
                });
            return predictions.ToArray();
        }

        public ProductDemandEstimated[] GetMovingAveragePredictions(ProductDemand[] data)
        {
            var result = new ConcurrentBag<ProductDemandEstimated>();
            var salesHistory = DataSource.GetSalesHistory();
            var groupsByLocPluInput = data.GroupBy(d => new Tuple<int,int>(d.Locationid, d.Plu)).ToDictionary(d => d.Key, d => d.ToArray());
            var groupsByLocPluSales = salesHistory.GroupBy(d => new Tuple<int, int>(d.Locationid, d.Plu)).ToDictionary(d => d.Key, d => d.ToArray());
            Parallel.ForEach(groupsByLocPluInput, demand =>
            {
                var demandLoc = demand.Key.Item1;
                var demandPlu = demand.Key.Item2;
                var sales = groupsByLocPluSales[demand.Key];

                foreach (var demandItem in demand.Value)
                {
                    // 4 weeks average
                    var productSales = sales.Where(s =>
                        s.Plu == demandPlu
                        && s.Locationid == demandLoc
                        && s.Salesdate < demandItem.Salesdate
                        && s.Salesdate >= demandItem.Salesdate.AddDays(-4*7)).ToArray();

                    var salesAverage = 0f;
                    if (productSales.Length > 0)
                    {
                        salesAverage = productSales
                            .Select(s => s.Quantity)
                            .Sum()/(4*7); // 28 sale days
                    }
                    var estimatedDemand = new ProductDemandEstimated
                    {
                        Locationid = demandLoc,
                        Plu = demandPlu,
                        Year = demandItem.Year,
                        Month = demandItem.Month,
                        Day = demandItem.Day,
                        Quantity = salesAverage // estimation
                    };
                    result.Add(estimatedDemand);
                }
            });
            GC.Collect();
            return result.ToArray();
        }

        private Dictionary<string, string> CreatePayload(ProductDemand productDemand)
        {
            return new Dictionary<string, string>()
            {
                {"Locationid", productDemand.Locationid.ToString()},
                {"RecipeName", productDemand.RecipeName},
                {"PLU", productDemand.Plu.ToString()},
                {"Salesdate", productDemand.Salesdate.ToString("s")},
                {"Quantity", productDemand.Quantity.ToString()},
                {"NetSalesPrice", productDemand.NetSalesPrice.ToString()},
                {"CostPrice", productDemand.CostPrice.ToString()},
                {"Year", productDemand.Year.ToString()},
                {"Month", productDemand.Month.ToString()},
                {"Day", productDemand.Day.ToString()},
                {"WeekDay", productDemand.WeekDay.ToString()},
                {"YearDay", productDemand.YearDay.ToString()}
            };
        }

        private ProductDemandEstimated[] ExtractEstimatedValue(Dictionary<string, string>[] output)
        {
            var result = new List<ProductDemandEstimated>();
            foreach (var dic in output)
            {
                var pd = new ProductDemandEstimated();

                try
                {
                    pd.Quantity = float.Parse(dic[MlSettings.ScoredValueFieldName]);

                    pd.Locationid = int.Parse(dic["Locationid"]);
                    pd.Plu = int.Parse(dic["PLU"]);
                    pd.Year = int.Parse(dic["Year"]);
                    pd.Month = int.Parse(dic["Month"]);
                    pd.Day = int.Parse(dic["Day"]);
                }
                catch (FormatException e)
                {
                    var message = JsonConvert.SerializeObject(dic);
                    throw new FormatException(message, e);
                }

                result.Add(pd);
            }
            return result.ToArray();
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