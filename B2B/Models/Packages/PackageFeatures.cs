using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace B2B.Models.Packages
{
    public class PackageFeatures
    {
        public int Id { get; set; }
        [ForeignKey("FeatureProxy")]
        public int FeatureId { get; set; }
        [ForeignKey("PackageProxy")]
        public int PackageId { get; set; }

        public virtual Feature FeatureProxy { get; set; }
        public virtual Package PackageProxy { get; set; }
    }
}