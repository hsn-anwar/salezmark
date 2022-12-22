using B2B.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace B2B.Areas.Admin.ViewModel
{
    public class AdminUser
    {
        public AdminUser()
        {
            EnableLiveLocation = false;
        }

        public bool EnableLiveLocation { get; set; }

        public string Id { get; set; }
        public string UserId { get; set; }
        public UserTypes UserType { get; set; }
        public UserStatus Status { get; set; }
        public string OutputDate { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(25,MinimumLength = 4,ErrorMessage ="Name must be 4-25 characters long.")]
        public string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email is badly formatted")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Phone number is badly formatted.")]
        [StringLength(25, MinimumLength = 10, ErrorMessage = "Phone number must be 10-25 characters long.")]
        public string PhoneNumber { get; set; }

        public string ImageUrl { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }

        [StringLength(25,MinimumLength =4 ,ErrorMessage = "City must be 4-25 characters long.")]
        public string City { get; set; }
        [Required(ErrorMessage = "Country field is required.")]
        [StringLength(25,MinimumLength =4 ,ErrorMessage = "Country must be 4-25 characters long.")]
        public string Country { get; set; }
        [StringLength(20, MinimumLength = 4, ErrorMessage = "State must be 4-20 characters long.")]
        public string State { get; set; }
        [StringLength(400, MinimumLength = 4, ErrorMessage = "Adress must be 4-400 characters long.")]
        public string AddressLine { get; set; }

        [Required(ErrorMessage ="Password is required.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Password and confirm password do not match.")]
        public string ConfirmPassword { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }

    }
}