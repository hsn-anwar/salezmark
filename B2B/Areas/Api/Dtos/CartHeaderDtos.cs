using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2B.Areas.Api.Dtos
{
    public class CartHeaderDtos
    {
        public int CartId { get; set; }
        public string Date { get; set; }
        public string Number { get; set; }
        public string MerchantName { get; set; }
        public string MerchantEmail { get; set; }
        public string MerchantProfileImageUrl { get; set; }
        
        public List<CartLineDtos> LineDetails { get; set; }
    }
}