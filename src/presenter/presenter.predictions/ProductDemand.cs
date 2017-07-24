using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace presenter.predictions
{
    public class ProductDemand
    {
        public long Locationid { get; set; }
        public string RecipeName { get; set; }
        public long Plu { get; set; }
        public DateTime Salesdate { get; set; }
        public double Quantity { get; set; }
        public double NetSalesPrice { get; set; }
        public double CostPrice { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int WeekDay { get; set; }
        public int YearDay { get; set; }
    }
}
