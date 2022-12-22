using B2B.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2B.Areas.Merchant.ViewModel
{
    public class QtyMovementReportVM
    {
        public DateTime Date { get; set; }
        public string OrderNumber { get; set; }
        public transactionType TransactionType { get; set; }
        public decimal? Cost { get; set; }
        public decimal? Price { get; set; }
        public decimal? Total { get; set; }
        public decimal? In { get; set; }
        public decimal? Out { get; set; }
    }
}