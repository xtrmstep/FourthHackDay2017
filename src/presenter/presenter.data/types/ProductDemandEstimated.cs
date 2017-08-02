using System;

namespace presenter.data.types
{
    public class ProductDemandEstimated
    {
        public int Locationid { get; set; }
        public int Plu { get; set; }
        public float Quantity { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
    }
}
