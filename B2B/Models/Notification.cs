using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2B.Models
{
    public class Notification : BaseModel
    {
        public string NotifyToUserId { get; set; }
        public string NotifyByUserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Isseen { get; set; }
    }
}