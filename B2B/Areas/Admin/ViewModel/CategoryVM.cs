using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace B2B.Areas.Admin.ViewModel
{
    public class CategoryVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Name is required")]
        [StringLength(25,MinimumLength =4,ErrorMessage ="Name should be in between 4-25")]
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        
        public int? ParentCategoryId { get; set; }
        public string ParentCategoryName { get; set; }
        public List<SelectListItem> categoryList { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
    }
}