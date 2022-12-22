using B2B.Models;
using B2B.Models.Packages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace B2B.Areas.Admin.ViewModel
{
    public class PackageViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Package name is required.")]
        [StringLength(25, MinimumLength = 4, ErrorMessage = "Name must be 4-25 characters long.")]
        public string Name { get; set; }
        [StringLength(250,MinimumLength =6,ErrorMessage = "Description must be 6-250 characters long.")]
        public string Description { get; set; }
        [StringLength(250,MinimumLength =4 ,ErrorMessage = "Note must be 4-250 characters long.")]
        public string Note { get; set; }
        [Required(ErrorMessage = "Duration is required.")]
        [Range(1, 50,ErrorMessage ="Duration must be in between 1-50")]
        public int Duration { get; set; }
        [Required(ErrorMessage = "Package amount is required.")]
        [Range(1, 99999, ErrorMessage = "Amount must be 5 characters long")]
        public decimal Amount { get; set; }
        public PackageDuration DurationType { get; set; }
        public string date { get; set; }
        public string ImageUrl { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        public List<FeatureViewModel> Features { get; set; }
    }
}