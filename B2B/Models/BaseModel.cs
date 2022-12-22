using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2B.Models
{
    public abstract partial class BaseModel
    {
        [System.ComponentModel.DataAnnotations.Key]
        public virtual int Id { get; set; }

        public virtual DateTime CreatedOn { get; set; }
        public virtual DateTime? UpdatedAt { get; set; }
    }
}