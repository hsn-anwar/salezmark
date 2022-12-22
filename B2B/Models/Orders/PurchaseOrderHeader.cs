using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;

namespace B2B.Models.Orders
{
    public class PurchaseOrderHeader : BaseModel
    {
        public string Description { get; set; }
        public string OrderNumber { get; set; }
        public string SupplierName { get; set; }
        public string PhoneNumber { get; set; }
        [ForeignKey("CreatedByUserProxy")]
        public string CreatedByUserId { get; set; }

        public virtual ApplicationUser CreatedByUserProxy { get; set; }
    }
}