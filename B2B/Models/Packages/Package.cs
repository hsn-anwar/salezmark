using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2B.Models.Packages
{
    public class Package : BaseModel
    {
        public Package()
        {
            IsActive = true;
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Note { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; }
        public PackageDuration DurationType { get; set; }
        public bool IsActive { get; set; }
    }
}