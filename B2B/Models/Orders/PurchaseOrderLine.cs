using B2B.Models.Store;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace B2B.Models.Orders
{
    public class PurchaseOrderLine
    {
        public int Id { get; set; }

        [ForeignKey("ProductProxy")]
        public int ProductId { get; set; }
        [ForeignKey("PurchaseOrderHeaderProxy")]
        public int PurchaseOrderHeaderId { get; set; }

        [Required( ErrorMessage = "Qty should be empty.")]
        public decimal Qty { get; set; }
        [Required(ErrorMessage = "Price should be empty.")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Total should be empty.")]
        public decimal Total { get; set; }

        public virtual Store_Products ProductProxy { get; set; }
        public virtual PurchaseOrderHeader PurchaseOrderHeaderProxy { get; set; }
    }
}