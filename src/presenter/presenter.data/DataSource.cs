using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using presenter.data.types;

namespace presenter.data
{
    public static class DataSource
    {
        public static long[] GetLocations()
        {
            var lines = File.ReadAllLines(@"files\\locations.csv");
            return lines
                .Skip(1) // skip header
                .Select(long.Parse).ToArray();
        }

        public static Recipe[] GetRecipes()
        {
            var result = new List<Recipe>();
            var lines = File.ReadAllLines(@"files\\products.csv");
            var headerSkipped = false;
            foreach (var line in lines)
            {
                if (!headerSkipped)
                {
                    headerSkipped = true;
                    continue;
                }
                var parts = line.Split(',');
                var recipe = new Recipe
                {
                    Name = parts[0],
                    Plu = long.Parse(parts[1])
                };
                result.Add(recipe);
            }
            return result.ToArray();
        }

        public static ProductDemand[] GetSalesHistory(bool isTest = false)
        {
            var result = new List<ProductDemand>();
            var textFile = isTest
                ? File.OpenText(@"files\\testhistory.csv")
                : File.OpenText(@"files\\PE-TRG-Mar-2017.csv");
            textFile.ReadLine();
            var line = textFile.ReadLine();
            while (line!= null) {
                var parts = line.Split(',');
                var pd = new ProductDemand();
                pd.Locationid = int.Parse(parts[0]);
                pd.RecipeName = parts[1];
                pd.Plu = int.Parse(parts[2]);
                pd.Salesdate = DateTime.Parse(parts[3]);
                pd.Quantity = float.Parse(parts[4]);
                pd.NetSalesPrice = float.Parse(parts[5]);
                pd.CostPrice = float.Parse(parts[6]);
                pd.Year = int.Parse(parts[7]);
                pd.Month = int.Parse(parts[8]);
                pd.Day = int.Parse(parts[9]);
                pd.WeekDay = int.Parse(parts[10]);
                pd.YearDay = int.Parse(parts[11]);
                result.Add(pd);

                line = textFile.ReadLine();
            }
            return result.ToArray();
        }
    }
}
