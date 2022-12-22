using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;

namespace B2B.Models.Store
{
    public class Store_Product_Gallery : BaseModel
    {
        public string ImageUrl { get; set; }

        [ForeignKey("StoreProductProxy")]
        public int StoreProductId { get; set; }
        
        public virtual Store_Products StoreProductProxy { get; set; }

    }
}