using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2B.Areas.Api.Dtos
{
    public class ProductDtos
    {
        public ProductDtos()
        {
            IsNew = false;
        }
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public string Name { get; set; }

        public decimal Cost { get; set; }
        public decimal ActualPrice { get; set; }
        public decimal? Tax { get; set; }
        public decimal? Discount { get; set; }
        public decimal SalePrice { get; set; }
        public decimal InStock { get; set; }
        public decimal? TotalPurchaseQty { get; set; }

        public string Description { get; set; }
        public string Specification { get; set; }
        public string FeatureImageUrl { get; set; }

        public string StoreMeasurementUnitName { get; set; }
        public string MerchantName { get; set; }
        public string CategoryName { get; set; }
        public string MerchantId { get; set; }
        public List<string> Images { get; set; }

        public bool IsNew { get; set; }
    }
}