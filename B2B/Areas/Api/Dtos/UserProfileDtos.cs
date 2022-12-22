using B2B.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2B.Areas.Api.Dtos
{
    public class UserProfileDtos
    {
        public string Id { get; set; }
        public string ProfileImageUrl { get; set; }
        public string PhoneNumber { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string DeviceNumber { get; set; }
        public string DeviceToken { get; set; }
        public string DeviceFCM { get; set; }
        public int? AssignBranchId { get; set; }
        public UserTypes UserType { get; set; }

        public bool IsEmailVerified { get; set; }
        public bool NotificationEnabled { get; set; }
        public bool OrderAuthenticationEnabled { get; set; }
    }
}