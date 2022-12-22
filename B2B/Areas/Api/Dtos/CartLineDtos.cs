using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2B.Areas.Api.Dtos
{
    public class CartLineDtos
    {
        public int LineId { get; set; }
        public string ProductName { get; set; }
        public string MeasurementUnit { get; set; }
        public string ProductImageUrl { get; set; }
        public decimal Qty { get; set; }
        public decimal Price { get; set; }
        public decimal LineTotal { get; set; }
    }
}