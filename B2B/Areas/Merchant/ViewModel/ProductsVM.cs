using B2B.Models;
using B2B.Models.Store;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace B2B.Areas.Merchant.ViewModel
{
    public class ProductsVM
    {
        public int? Id { get; set; }
        public DateTime? Date { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(25, MinimumLength =4, ErrorMessage = "Name must be in between 4-25")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Product cost is required.")]
        [Range(1, 99999, ErrorMessage = "Cost only 5 chracters long")]
        public decimal Cost { get; set; }

        [Required(ErrorMessage = "Actual price is required.")]
        [Range(1, 99999, ErrorMessage = "Actual Price only 5 chracters long")]
        public decimal ActualPrice { get; set; }
        [Range(1, 100, ErrorMessage = "Tax must be in between 1-100")]
        public decimal? Tax { get; set; }
        [Range(1, 100, ErrorMessage = "Discount must be in between 1-100")]
        public decimal? Discount { get; set; }
        [Required(ErrorMessage = "Sale price required.")]
        [Range(1, 99999, ErrorMessage = "Sale price only 5 chracters long")]
        public decimal SalePrice { get; set; }
        [Required(ErrorMessage = "Stock Level required.")]
        [Range(1, 999, ErrorMessage = "Low stock only 3 chracters long")]
        public decimal LowStock { get; set; }
        public decimal? InStock { get; set; }
        [StringLength(500, MinimumLength = 4, ErrorMessage = "Description must be in between 4-500")]
        public string Description { get; set; }
        [StringLength(500, MinimumLength = 4, ErrorMessage = "Specification must be in between 4-500")]
        public string Specification { get; set; }
        public string FeatureImageUrl { get; set; }
        public string CategoryName { get; set; }
        public UserStatus userStatus { get; set; }
        [Required(ErrorMessage = "Measurement unit is required.")]
        public int StoreMeasurementUnitId { get; set; }
        [Required(ErrorMessage = "Category is required.")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Company is required.")]
        public int CompanyId { get; set; }

        public string StoreMeasurementUnitName { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        public System.Web.Mvc.SelectList StoreMeasurementUnitList { get; set; }
        public System.Web.Mvc.SelectList CategoryList { get; set; }
    }
}