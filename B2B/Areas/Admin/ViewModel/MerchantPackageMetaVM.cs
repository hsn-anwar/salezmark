using B2B.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2B.Areas.Admin.ViewModel
{
    public class MerchantPackageMetaVM
    {
        public int Id { get; set; }
        public DateTime StartingFrom { get; set; }
        public DateTime ValidTill { get; set; }

        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public string PackageImage { get; set; }
        public string PackageNumber { get; set; }
        
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserImage { get; set; }

        public decimal TotalBill { get; set; }
        public bool IsAmountPaid { get; set; }

        public packageStatus Status { get; set; }
        
    }
}