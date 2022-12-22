using System.Data.Entity;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using B2B.Models.Shopkeeper;

namespace B2B.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            IsEmailVerified = false;
            AccountSuspend = false;
            IsDeleted = false;
            OrderAuthenticationEnabled = false;
            NotificationEnabled = true;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public DateTime CreatedOn { get; set; }

        public bool AccountSuspend { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool NotificationEnabled { get; set; }
        public bool OrderAuthenticationEnabled { get; set; }

        public string ProfileName { get; set; }
        public string ProfileImageUrl { get; set; }
        public string UserID { get; set; }
        public string DeviceNumber { get; set; }
        public string DeviceToken { get; set; }
        public string DeviceFCM { get; set; }

        public Genders Gender { get; set; }
        public UserTypes UserType { get; set; }
        public UserStatus Status { get; set; }

        [ForeignKey("LocationProxy")]
        public int? LocationId { get; set; }
        [ForeignKey("ParentUserId")]
        public string CreatedByUserId { get; set; }
    
        public virtual Locations LocationProxy { get; set; }
        public virtual ApplicationUser ParentUserId { get; set; }
    }
}