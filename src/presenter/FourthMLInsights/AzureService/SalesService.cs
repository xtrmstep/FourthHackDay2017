using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureService
{
    public class SalesService
    {
        public void GetPrediction(DateTime startDate, DateTime endDate)
        {
            if (startDate == null)
                throw new ArgumentNullException();

            if (endDate == null)
                throw new ArgumentNullException();

            if (endDate < startDate)
                throw new ArgumentOutOfRangeException();

            var uri = Properties.Settings.Default.Uri;
            var key = Properties.Settings.Default.Key;

            var client = new SalesDemandService(uri, key);

            while (startDate < endDate)
            {
                var value = NewMethod(startDate, client);
                startDate.AddDays(1);
            }

            if (startDate.Date.Equals(endDate.Date))
            {
                var date = GetData(startDate);
            }
        }

        private string NewMethod(DateTime startDate, SalesDemandService client)
        {
            var data = GetData(startDate);

            var task = client.GetPrediction1(data);
            task.Wait();
            var prediction = task.Result;
            return prediction;
        }

        private Dictionary<string, string> GetData(DateTime date)
        {
            var strDate = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

            var data = new Dictionary<string, string>
            {
                {
                    "CustomerId", "11217"
                },
                {
                    "OrderDate", strDate
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

            return data;
        }        
    }
}
