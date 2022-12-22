using B2B.Models.Orders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace B2B.Models
{
    public class Complains : BaseModel
    {
        [ForeignKey("UserProxy")]
        public string UserId { get; set; }
        [ForeignKey("OrderProxy")]
        public int OrderId { get; set; }
        public string Title { get; set; }
        public string AttachFileUrl { get; set; }
        public string Complain { get; set; }
        public complainStatus Status { get; set; }

        public virtual ApplicationUser UserProxy { get; set; }
        public virtual OrderHeader OrderProxy { get; set; }
    }
}