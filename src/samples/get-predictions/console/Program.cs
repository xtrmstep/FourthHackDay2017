using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace reader.console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            InvokeRequestResponseService().Wait();
        }

        private static async Task InvokeRequestResponseService()
        {
            using (var client = new HttpClient())
            {
                var scoreRequest = new
                {
                    Inputs = new Dictionary<string, List<Dictionary<string, string>>>
                    {
                        {
                            "input1",
                            new List<Dictionary<string, string>>
                            {
                                new Dictionary<string, string>
                                {
                                    {
                                        "CustomerId", "11217"
                                    },
                                    {
                                        "OrderDate", "2014-01-03T00:00:00Z"
                                    },
                                    {
                                        "SalesOrderNumber", "SO76869"
                                    },
                                    {
                                        "TerritoryID", "1"
                                    },
                                    {
                                        "ProductID", "771"
                                    },
                                    {
                                        "UnitPrice", "2039.994"
                                    },
                                    {
                                        "OrderQty", "1"
                                    }
                                }
                            }
                        }
                    },
                    GlobalParameters = new Dictionary<string, string>()
                };

                const string apiKey = "abc123"; // Replace this with the API key for the web service
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                client.BaseAddress = new Uri("https://ussouthcentral.services.azureml.net/workspaces/d17f979b70964d47bd15bfbae94c5a66/services/b0b8aaaa51ef4078988538a8a0effe23/execute?api-version=2.0&format=swagger");

                // WARNING: The 'await' statement below can result in a deadlock
                // if you are calling this code from the UI thread of an ASP.Net application.
                // One way to address this would be to call ConfigureAwait(false)
                // so that the execution does not attempt to resume on the original context.
                // For instance, replace code such as:
                //      result = await DoSomeTask()
                // with the following:
                //      result = await DoSomeTask().ConfigureAwait(false)

                var response = await client.PostAsJsonAsync("", scoreRequest);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Result: {0}", result);
                }
                else
                {
                    Console.WriteLine("The request failed with status code: {0}", response.StatusCode);

                    // Print the headers - they include the requert ID and the timestamp,
                    // which are useful for debugging the failure
                    Console.WriteLine(response.Headers.ToString());

                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseContent);
                }
            }
        }
    }
}