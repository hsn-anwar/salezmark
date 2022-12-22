using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using B2B.Models.Store;

namespace B2B.Models.Cart
{
    public class CartLine
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Qty should not be empty.")]
        public decimal Qty { get; set; }
        
        [ForeignKey("ProductProxy")]
        public int ProductId { get; set; }
        [ForeignKey("HeaderProxy")]
        public int HeaderId { get; set; }

        public Store_Products ProductProxy { get; set; }
        public CartHeader HeaderProxy { get; set; }

    }
}