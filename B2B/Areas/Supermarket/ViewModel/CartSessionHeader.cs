using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2B.Areas.Supermarket.ViewModel
{
    public class CartSessionHeader
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int Count { get; set; }
        public string Email { get; set; }
    }
}