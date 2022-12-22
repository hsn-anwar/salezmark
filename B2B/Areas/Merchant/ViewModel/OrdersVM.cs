using B2B.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace B2B.Areas.Merchant.ViewModel
{
    public class OrdersVM
    {
        public int Id { get; set; }

        public DateTime? Date { get; set; }
        public String StringDate { get; set; }

        public string OrderNumber { get; set; }
        public string Email { get; set; }
        public string RejectionComment { get; set; }
        public string CanceledComment { get; set; }
        public string DeliveryNote { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsPaid { get; set; }
        public string CustomerName { get; set; }
        public string CustomerImage { get; set; }

        public paymentMethodEnum PaymentStatus { get; set; }
        public deliveryType DeliveryOption { get; set; }
        public orderStatusEnum Status { get; set; }
        public orderType Type { get; set; }
        public string DeliveryById { get; set; }
        public string BranchName { get; set; }

        public decimal? ShippingCharges { get; set; }
        public decimal? TotalBill { get; set; }
        public string UserNumber { get; set; }
        public List<OrderLineVM> Lines { get; set; }
    }
}