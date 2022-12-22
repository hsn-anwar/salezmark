using B2B.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2B.Areas.Merchant.ViewModel
{
    public class ShopkeeperVM
    {

        public string Id { get; set; }
        public string UserId { get; set; }
        public string OutputDate { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string ImageUrl { get; set; }
      
        public string City { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string AddressLine { get; set; }

        public string Status { get; set; }
    }
}