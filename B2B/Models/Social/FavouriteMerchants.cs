using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace B2B.Models.Social
{
    public class FavouriteMerchants : BaseModel
    {
        [ForeignKey("MerchantProxy")]
        public string MerchantId { get; set; }
        [ForeignKey("UserProxy")]
        public string UserID { get; set; }
        public ApplicationUser MerchantProxy { get; set; }
        public ApplicationUser UserProxy { get; set; }
    }
}