using B2B.Models.Store;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace B2B.Models.Orders
{
    public class OrderLine
    {
        public int Id { get; set; }

        [ForeignKey("OrderHeaderProxy")]
        public int OrderHeaderId { get; set; }
        [ForeignKey("ProductProxy")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Line qty is required")]
        public decimal Qty { get; set; }
        [Required(ErrorMessage = "Line price is required")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Line total is required")]
        public decimal LineTotal { get; set; }

        public virtual OrderHeader OrderHeaderProxy { get; set; }
        public virtual Store_Products ProductProxy { get; set; }
    }
}