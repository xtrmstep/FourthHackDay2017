using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace reader.console
{
    class SalesDemandService
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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

        public async Task<string> GetPrediction(Dictionary<string, string> data)
        {
            using (var client = new HttpClient())
            {
                var payload = new List<Dictionary<string, string>> {data};
                var scoreRequest = new
                {
                    Inputs = new Dictionary<string, List<Dictionary<string, string>>> { { "input1", payload } },
                    GlobalParameters = new Dictionary<string, string>()
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _key);
                client.BaseAddress = new Uri(_uri);

                var response = await client.PostAsJsonAsync(string.Empty, scoreRequest);

                if (response.IsSuccessStatusCode) { return await response.Content.ReadAsStringAsync(); }

                // if error occurred

                _log.InfoFormat("The request failed with status code: {0}", response.StatusCode);

                // Print the headers - they include the request ID and the timestamp,
                // which are useful for debugging the failure
                _log.InfoFormat(response.Headers.ToString());

                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}
