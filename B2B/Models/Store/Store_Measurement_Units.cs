using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace B2B.Models.Store
{
    public class Store_Measurement_Units : BaseModel
    {
        public Store_Measurement_Units()
        {
            IsDeleted = false;
        }
        [Required(ErrorMessage = "Unit name is required.")]
        public string Name { get; set; }

        [ForeignKey("CreatedByUserProxy")]
        public string CreatedByUserId { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ApplicationUser CreatedByUserProxy { get; set; }
    }
}