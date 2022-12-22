using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace B2B.Models.Shopkeeper
{
    public class Branch : BaseModel
    {
        public Branch()
        {
            IsDeleted = false;
        }
        [Required(ErrorMessage = "Branch name is required.")]
        public string Name { get; set; }

        public string PhoneNumber { get; set; }
        public string ImageUrl { get; set; }

        public bool IsDeleted { get; set; }
        [ForeignKey("UserProxy")]
        public string UserId { get; set; }

        [ForeignKey("AssignedToProxy")]
        public string AssignedToUserId { get; set; }

        [ForeignKey("LocationProxy")]
        public int? LocationId { get; set; }

        public virtual ApplicationUser UserProxy { get; set; }
        public virtual ApplicationUser AssignedToProxy { get; set; }
        public virtual Locations LocationProxy { get; set; }

    }
}