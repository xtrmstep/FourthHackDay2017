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

        public async Task<ProductDemand[]> GetAmlPredictions(ProductDemand[] data)
        {
            var payload = data.Select(CreatePayload).ToList();
            var response = await GetPrediction(payload);
            var result = JsonConvert.DeserializeObject<MlPredictionResponse>(response);
            var estimations = ExtractEstimatedValue(result.Results.Output1);
            return estimations;
        }

        public ProductDemand[] GetMovingAveragePredictions(ProductDemand[] data)
        {
            var result = new List<ProductDemand>();
            var salesHistory = DataSource.GetSalesHistory();
            foreach (var demand in data)
            {
                // 4 weeks average
                var productSales = salesHistory.Where(s =>
                    s.Plu == demand.Plu
                    && s.Locationid == demand.Locationid
                    && s.Salesdate < demand.Salesdate
                    && s.Salesdate >= demand.Salesdate.AddDays(-4*7)).ToArray();

                var salesAverage = 0f;
                if (productSales.Length > 0)
                {
                    salesAverage = productSales
                        .Select(s => s.Quantity)
                        .Sum() / (4*7); // 28 sale days
                }
                var estimatedDemand = demand.Clone();
                estimatedDemand.Quantity = salesAverage;
                result.Add(estimatedDemand);
            }
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

        private ProductDemand[] ExtractEstimatedValue(Dictionary<string, string>[] output)
        {
            var result = new List<ProductDemand>();
            foreach (var dic in output)
            {
                var pd = new ProductDemand();

                try
                {
                    pd.Quantity = float.Parse(dic["Scored Labels"]);

                    pd.Locationid = int.Parse(dic["Locationid"]);
                    pd.RecipeName = dic["RecipeName"];
                    pd.Plu = int.Parse(dic["PLU"]);
                    pd.Salesdate = dic["Salesdate"].ToDateTime();
                    pd.NetSalesPrice = float.Parse(dic["NetSalesPrice"]);
                    pd.CostPrice = float.Parse(dic["CostPrice"]);
                    pd.Year = int.Parse(dic["Year"]);
                    pd.Month = int.Parse(dic["Month"]);
                    pd.Day = int.Parse(dic["Day"]);
                    pd.WeekDay = int.Parse(dic["WeekDay"]);
                    pd.YearDay = int.Parse(dic["YearDay"]);
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