using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AzureService
{
    public class SalesDemandService
    {
        private readonly string _key;
        private readonly string _uri;

        public SalesDemandService()
        {

        }

        public SalesDemandService(string uri, string key)
        {
            _uri = uri;
            _key = key;
        }

        public Task<string> GetPrediction1(Dictionary<string, string> data)
        {
            Task<string> result;
            using (var client = new HttpClient())
            {
                var payload = new List<Dictionary<string, string>> { data };
                var scoreRequest = new
                {
                    Inputs = new Dictionary<string, List<Dictionary<string, string>>> { { "input1", payload } },
                    GlobalParameters = new Dictionary<string, string>()
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _key);
                client.BaseAddress = new Uri(_uri);

                var response = client.PostAsJsonAsync(string.Empty, scoreRequest).Result;

                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsStringAsync();
                    return result;
                }

                result = response.Content.ReadAsStringAsync();
                return result;
            }
        }

        public async Task<string> GetPrediction(Dictionary<string, string> data)
        {
            using (var client = new HttpClient())
            {
                var payload = new List<Dictionary<string, string>> { data };
                var scoreRequest = new
                {
                    Inputs = new Dictionary<string, List<Dictionary<string, string>>> { { "input1", payload } },
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
