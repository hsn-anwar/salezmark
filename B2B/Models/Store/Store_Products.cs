using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace B2B.Models.Store
{
    public class Store_Products : BaseModel
    {
        [Required(ErrorMessage = "Product name is required.")]
        public string Name { get; set; }
        public string Code { get; set; }
        [Required(ErrorMessage = "Product cost is required.")]
        public decimal Cost { get; set; }
        [Required(ErrorMessage = "Product actual price without tax is required.")]
        public decimal ActualPrice { get; set; }
        public decimal? Tax { get; set; }
        public decimal? Discount { get; set; }
        public decimal SalePrice { get; set; }
        [Required(ErrorMessage = "Stock is required.")]
        public decimal InStock { get; set; }
        public decimal LowStock { get; set; }

        public string Description { get; set; }
        public string Specification { get; set; }
        public string FeatureImageUrl { get; set; }

        [ForeignKey("StoreMeasurementUnitProxy")]
        public int StoreMeasurementUnitId { get; set; }
        [ForeignKey("UserProxy")]
        public string UserId { get; set; }
        [ForeignKey("CategoryProxy")]
        public int CategoryId { get; set; }
        [ForeignKey("CompanyProxy")]
        public int CompanyId { get; set; }

        public virtual Store_Measurement_Units StoreMeasurementUnitProxy { get; set; }
        public virtual ApplicationUser UserProxy { get; set; }
        public virtual Store_Category CategoryProxy { get; set; }
        public virtual Store_Company CompanyProxy { get; set; }
        public UserStatus Status { get; set; }
    }
}