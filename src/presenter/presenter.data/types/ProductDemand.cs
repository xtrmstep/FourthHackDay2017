using System;

namespace presenter.data.types
{
    public class ProductDemand
    {
        public int Locationid { get; set; }
        public string RecipeName { get; set; }
        public int Plu { get; set; }
        public DateTime Salesdate { get; set; }
        public float Quantity { get; set; }
        public float NetSalesPrice { get; set; }
        public float CostPrice { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int WeekDay { get; set; }
        public int YearDay { get; set; }
    }
}
