using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2B.Areas.Api.Dtos
{
    public class UserDtos
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string DeviceNumber { get; set; }
        public string DeviceToken { get; set; }
        public string DeviceFCM { get; set; }
    }
}