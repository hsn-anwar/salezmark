using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2B.Areas.Supermarket.ViewModel
{
    public class EditProfileVM
    {
        public string Id { get; set; }
        public bool EnableOrderAuth { get; set; }
        public bool NotificationEnabled { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}