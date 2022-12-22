using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace B2B.Areas.Supermarket.ViewModel
{
    public class MarketProductDtos
    {
        public int? Id { get; set; }
        public DateTime? Date { get; set; }
        [Required(ErrorMessage = "Product name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Product cost is required.")]
        public decimal Cost { get; set; }
        [Required(ErrorMessage = "Actual price is required.")]
        public decimal ActualPrice { get; set; }
        public decimal? Tax { get; set; }
        public decimal? Discount { get; set; }
        [Required(ErrorMessage = "Sale price required.")]
        public decimal SalePrice { get; set; }
        [Required(ErrorMessage = "Stock Level required.")]
        public decimal LowStock { get; set; }
        public decimal? InStock { get; set; }
        public decimal? TotalStock { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public string Specification { get; set; }
        public string FeatureImageUrl { get; set; }
        public string CategoryName { get; set; }
        public bool IsNew { get; set; }
        public bool OnSale { get; set; }
        public string StoreMeasurementUnitName { get; set; }

    }
}