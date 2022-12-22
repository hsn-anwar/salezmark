using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2B.Models
{
    public enum Genders
    {
        Male,
        Female,
        Others
    }
    public enum UserTypes
    {
        Admin,
        Supermarket,
        Merchant,
        DeliveryTeam,
        SalesTeam,
        Supervisor
    }
    public enum UserStatus
    {
        Pending,
        Active,
        Blocked,
        Deleted,
    }
    public enum PackageDuration
    {
        Days,
        Weeks,
        Months,
        Years,
    }
    public enum orderStatusEnum
    {
        Pening,
        InProcess,
        Rejected,
        Enroute,
        Delivered,
        Canceled,
        NeedApproval,
    }
    public enum paymentMethodEnum
    {
        COD,
        MobilePay,
        Credit,
    }
    public enum deliveryType
    {
        MerchantDelivery,
        BuyerCollectFromPoint,
        ThirdParty,
    }
    public enum orderType
    {
        Online,
        Offline,
    }
    public enum transactionType
    {
        Purchase,
        Sale,
    }
    public enum packageStatus
    {
        Active,
        Expire,
        Blocked,
    }
    public enum connectionStatus
    {
        Pending,
        Apporved,
        Blocked,
        Removed,
    }
    public enum complainStatus
    {
        Pending,
        Opened,
        Closed,
        Completed,
    }
}