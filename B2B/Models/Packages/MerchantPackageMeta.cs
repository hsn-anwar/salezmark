using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;

namespace B2B.Models.Packages
{
    public class MerchantPackageMeta
    {
        public int Id { get; set; }
        public DateTime StartingFrom { get; set; }
        public DateTime ValidTill { get; set; }
        
        [ForeignKey("PackageProxy")]
        public int PackageId { get; set; }
        [ForeignKey("UserProxy")]
        public string UserId { get; set; }

        public decimal Amount { get; set; }
        public bool IsAmountPaid { get; set; }

        public packageStatus Status { get; set; }

        public ApplicationUser UserProxy { get; set; }
        public Package PackageProxy { get; set; }
    }
}