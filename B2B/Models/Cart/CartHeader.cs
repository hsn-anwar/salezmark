using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace B2B.Models.Cart
{
    public class CartHeader
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        //Properties
        public string Number { get; set; }

        [ForeignKey("CreatedForUserProxy")]
        public string CreatedForUserId { get; set; }

        [ForeignKey("CreatedByUserProxy")]
        public string CreatedByUserId { get; set; }

        //Proxy
        public virtual ApplicationUser CreatedByUserProxy { get; set; }
        public virtual ApplicationUser CreatedForUserProxy { get; set; }
    }
}