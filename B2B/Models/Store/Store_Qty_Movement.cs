using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using B2B.Models.Orders;

namespace B2B.Models.Store
{
    public class Store_Qty_Movement : BaseModel
    {

        public transactionType Type { get; set; }
        public decimal? Cost { get; set; }
        public decimal? Price { get; set; }
        public decimal? Total { get; set; }
        public decimal? InQty { get; set; }
        public decimal? OutQty { get; set; }
        [ForeignKey("PurchaseOrderHeaderProxy")]
        public int? PurchaseOrderHeaderId { get; set; }
        [ForeignKey("OrderHeaderProxy")]
        public int? OrderHeaderId { get; set; }
        [ForeignKey("ProductsProxy")]
        public int ProductId { get; set; }

        public virtual Store_Products ProductsProxy { get; set; }
        public virtual OrderHeader OrderHeaderProxy { get; set; }
        public virtual PurchaseOrderHeader PurchaseOrderHeaderProxy { get; set; }
    }
}