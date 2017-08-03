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

        private static ProductDemand[] _salesHistory = null;

        public static ProductDemand[] GetSalesHistory()
        {
            if (_salesHistory != null) return _salesHistory;

            var result = new List<ProductDemand>();
            var textFile = File.OpenText(@"files\\sales-jan-mar-2017.csv");
            textFile.ReadLine();
            var line = textFile.ReadLine();
            while (line != null)
            {
                var parts = line.Split(',');
                var pd = new ProductDemand();
                pd.Locationid = int.Parse(parts[1]);
                pd.RecipeName = parts[2];
                pd.Plu = int.Parse(parts[3]);
                pd.Salesdate = DateTime.Parse(parts[4]);
                pd.Quantity = float.Parse(parts[5]);
                pd.NetSalesPrice = float.Parse(parts[6]);
                pd.CostPrice = float.Parse(parts[7]);
                pd.Year = int.Parse(parts[8]);
                pd.Month = int.Parse(parts[9]);
                pd.Day = int.Parse(parts[10]);
                pd.WeekDay = int.Parse(parts[11]);
                pd.YearDay = int.Parse(parts[12]);
                result.Add(pd);

                line = textFile.ReadLine();
            }
            _salesHistory= result.ToArray();
            return _salesHistory;
        }

        public static void ClearSalesHistory()
        {
            _salesHistory = null;
            GC.Collect();
        }

        public static void SaveToCsv(ProductDemand[] sales, string fileName)
        {
            using (var f = File.CreateText(fileName))
            {
                var counter = 1;
                f.WriteLine(",Locationid,RecipeName,PLU,Salesdate,Quantity,NetSalesPrice,CostPrice,Year,Month,Day,WeekDay,YearDay");
                foreach (var sale in sales) {
                    f.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}"
                        , counter++
                        , sale.Locationid
                        , sale.RecipeName
                        , sale.Plu
                        , sale.Salesdate.ToShortDateString()
                        , sale.Quantity
                        , sale.NetSalesPrice
                        , sale.CostPrice
                        , sale.Year
                        , sale.Month
                        , sale.Day
                        , sale.WeekDay
                        , sale.YearDay);
                }
            }
        }

        public static void SaveToCsv2(ProductDemandEstimated[] sales, string fileName)
        {
            using (var f = File.CreateText(fileName))
            {
                var counter = 1;
                f.WriteLine(",Locationid,PLU,Quantity,Year,Month,Day");
                foreach (var sale in sales)
                {
                    f.WriteLine("{0},{1},{2},{3},{4},{5},{6}"
                        , counter++
                        , sale.Locationid
                        , sale.Plu
                        , sale.Quantity
                        , sale.Year
                        , sale.Month
                        , sale.Day);
                }
            }
        }
    }
}
