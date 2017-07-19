using System;
using System.Collections.Generic;
using System.Management.Instrumentation;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using log4net;

namespace reader.console
{
    internal class Program
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static void Main(string[] args)
        {
            var client = new SalesDemandService(Properties.Settings.Default.Uri, Properties.Settings.Default.Key);
            var data = new Dictionary<string, string>
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
            };

            var task = client.GetPrediction(data);
            task.Wait();
            var prediction = task.Result;

            _log.InfoFormat("Result: {0}", prediction);
        }


    }
}