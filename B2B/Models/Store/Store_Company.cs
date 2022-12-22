using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace B2B.Models.Store
{
    public class Store_Company
    {
        public Store_Company()
        {
            IsDeleted = false;
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Company name is required")]
        [StringLength(25,MinimumLength =4,ErrorMessage =" Name must be in between 4-25.")]
        public string Name { get; set; }

        public string ImageUrl { get; set; }
        public bool IsDeleted { get; set; }
    }
}