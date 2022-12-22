using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2B.Areas.Admin.ViewModel
{
    public class FeatureViewModel
    {
        public FeatureViewModel()
        {
            IsAssign = false;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Other { get; set; }
        public bool IsAssign { get; set; }
    }
}