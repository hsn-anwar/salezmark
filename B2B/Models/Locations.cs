using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2B.Models
{
    public class Locations : BaseModel
    {
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string AddressLine { get; set; }
        public bool LiveModeEnable { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}