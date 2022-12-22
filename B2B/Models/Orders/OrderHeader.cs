using B2B.Models.Shopkeeper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace B2B.Models.Orders
{
    public class OrderHeader : BaseModel
    {
        public OrderHeader()
        {
            Status = orderStatusEnum.Pening;
            WithCustomDelivery = false;
            IsPaid = false;
        }
        public bool WithCustomDelivery { get; set; }
        public DateTime? CustomDeliveryTime { get; set; }
        public string OrderNumber { get; set; }
        public string Email { get; set; }
        public string RejectionComment { get; set; }
        public string CanceledComment { get; set; }
        public string DeliveryNote { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsPaid { get; set; }
        public paymentMethodEnum PaymentStatus { get; set; }
        public deliveryType DeliveryOption { get; set; }
        public orderStatusEnum Status { get; set; }
        public orderType Type { get; set; }

        public decimal? ShippingCharges { get; set; }

        [ForeignKey("OrderByUserProxy")]
        public string OrderByUserId { get; set; }
        [ForeignKey("OrderCreatedByUserProxy")]
        public string OrderCreatedByUserId { get; set; }
        [ForeignKey("OrderForUserProxy")]
        public string OrderForUserId { get; set; }
        [ForeignKey("OrderAssignedToUserProxy")]
        public string OrderAssignedToUserId { get; set; }
        [ForeignKey("BranchProxy")]
        public int? BranchId { get; set; }

        public virtual ApplicationUser OrderByUserProxy { get; set; }
        public virtual ApplicationUser OrderCreatedByUserProxy { get; set; }
        public virtual ApplicationUser OrderForUserProxy { get; set; }
        public virtual ApplicationUser OrderAssignedToUserProxy { get; set; }
        public virtual Branch BranchProxy { get; set; }
    }
}