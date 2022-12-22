using B2B.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace B2B.Areas.Api.Dtos
{
    public class RegisterUserDtos
    {
        public RegisterUserDtos()
        {
            IsBranchAssigned = false;
        }
        public string Id { get; set; }
        public string ProfileImageUrl { get; set; }
        [StringLength(25, MinimumLength = 10, ErrorMessage = "Phone number must be in between 10-25 characters.")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "Name must be in between 4-30 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Country is required.")]
        [StringLength(25, MinimumLength = 4, ErrorMessage = "Country length must be in between 4-25 characters.")]
        public string Country { get; set; }
        [StringLength(25, MinimumLength = 4, ErrorMessage = "State must be in between 4-25 characters.")]
        public string State { get; set; }
        [StringLength(25, MinimumLength = 4, ErrorMessage = "City must be in between 4-25 characters.")]
        public string City { get; set; }
        [StringLength(400, MinimumLength = 4, ErrorMessage = "Adress must be in between 4-400 characters.")]
        public string Address { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public bool EnableLiveMode { get; set; }
        public int? BranchId { get; set; }
        public string BranchName { get; set; }
        public bool IsBranchAssigned { get; set; }
        public Genders Gender { get; set; }
        public UserStatus Status { get; set; }
    }
}