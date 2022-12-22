using B2B.Models.Store;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace B2B.Areas.Merchant.ViewModel
{
    public class OrderLineVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product is required.")]
        public int ProductId { get; set; }

        public string ProductName { get; set; }
        public string ProductImage { get; set; }

        [Required(ErrorMessage = "Line qty is required")]
        public decimal Qty { get; set; }
        [Required(ErrorMessage = "Line price is required")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Line total is required")]
        public decimal LineTotal { get; set; }
        
    }
}