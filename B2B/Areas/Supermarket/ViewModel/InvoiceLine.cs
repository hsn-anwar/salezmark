using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2B.Areas.Supermarket.ViewModel
{
    public class InvoiceLine
    {
        public int LineId { get; set; }
        public decimal Tax { get; set; }
        public decimal ActuallPrice { get; set; }
        public string ProductName { get; set; }
        public decimal Discount { get; set; }
        public string ProductImageUrl { get; set; }
        public string CategoryName { get; set; }
        public decimal Qty { get; set; }
        public decimal Price { get; set; }
        public decimal LineTotal { get; set; }
    }
}