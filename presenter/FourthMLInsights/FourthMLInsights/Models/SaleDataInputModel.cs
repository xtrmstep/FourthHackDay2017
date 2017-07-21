using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FourthMLInsights.Models
{
    public class SaleDataInputModel
    {
        public string LocationId { get; set; }
        public string RecipeName { get; set; }
        public string PLU { get; set; }
        public string Quantity { get; set; }
        public string NetSalesPrice { get; set; }
        public string CostPrice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}