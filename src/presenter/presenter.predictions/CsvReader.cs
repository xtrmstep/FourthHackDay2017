using System;
using System.Collections.Generic;
using System.IO;

namespace presenter.predictions
{
    public static class CsvReader
    {
        public static ProductDemand[] Load(string fileName, bool hasHeader = false)
        {
            var headerSkipped = !hasHeader;

            var result = new List<ProductDemand>();
            var lines = File.ReadAllLines(fileName);
            foreach (var line in lines)
            {
                if (!headerSkipped)
                {
                    headerSkipped = true;
                    continue;
                }

                var parts = line.Split(',');
                var pd = new ProductDemand();
                pd.Locationid = long.Parse(parts[0]);
                pd.RecipeName = parts[1];
                pd.Plu = long.Parse(parts[2]);
                pd.Salesdate = DateTime.Parse(parts[3]);
                pd.Quantity = double.Parse(parts[4]);
                pd.NetSalesPrice = double.Parse(parts[5]);
                pd.CostPrice = double.Parse(parts[6]);
                pd.Year = int.Parse(parts[7]);
                pd.Month = int.Parse(parts[8]);
                pd.Day = int.Parse(parts[9]);
                pd.WeekDay = int.Parse(parts[10]);
                pd.YearDay = int.Parse(parts[11]);
                result.Add(pd);
            }
            return result.ToArray();
        }
    }
}