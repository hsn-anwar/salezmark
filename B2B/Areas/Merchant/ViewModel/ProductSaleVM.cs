using B2B.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2B.Areas.Merchant.ViewModel
{
    public class ProductSaleVM
    {

        public int Id { get; set; }
        public deliveryType DeliveryOption { get; set; }
        public orderType Type { get; set; }

        public string OrderNumber { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Note { get; set; }
        
        public decimal Qty { get; set; }
        public decimal Price { get; set; }
        public decimal LineTotal { get; set; }

        public DateTime Date { get; set; }
    }
}