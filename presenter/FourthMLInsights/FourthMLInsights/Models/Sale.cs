using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FourthMLInsights.Models
{
    public class Sale
    {
        public int Id { get; set; }

        public int LocationId { get; set; }

        public string RecepieName { get; set; }

        public int PLU { get; set; }

        public DateTime SalesDate { get; set; }

        public int Quantity { get; set; }

        public decimal NetSalesPrice { get; set; }

        public decimal CostPrice { get; set; }
    }
}