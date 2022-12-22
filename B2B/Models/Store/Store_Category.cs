using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace B2B.Models.Store
{
    public class Store_Category : BaseModel
    {
        public Store_Category()
        {
            IsDeleted = false;
        }

        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public bool IsDeleted { get; set; }
        [ForeignKey("ParentCategoryProxy")]
        public int? ParentCategoryId { get; set; }

        public Store_Category ParentCategoryProxy { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
    }
}