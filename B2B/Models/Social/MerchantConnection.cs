using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace B2B.Models.Social
{
    public class MerchantConnection : BaseModel
    {
        [ForeignKey("MerchantProxy")]
        public string MerchantId { get; set; }
        [ForeignKey("ShopkeeperProxy")]
        public string ShopkeeperId { get; set; }

        public connectionStatus Status { get; set; }
        public DateTime? SubscribeAt { get; set; }

        public ApplicationUser ShopkeeperProxy { get; set; }
        public ApplicationUser MerchantProxy { get; set; }
    }
}