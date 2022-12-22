using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace B2B.Areas.Api.Dtos
{
    public class BranchDtos
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Branch name is required.")]
        [StringLength(25, MinimumLength = 4, ErrorMessage = "Name length must be in between 4-25 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Country name is required.")]
        [StringLength(25, MinimumLength = 4, ErrorMessage = "Country length must be in between 4-25 characters.")]    
        public string Country { get; set; }
        [StringLength(25, MinimumLength = 10,ErrorMessage = "Phone number length must be in between 10-25 characters.")]
        public string PhoneNumber { get; set; }
        public string ImageUrl { get; set; }
        [StringLength(25, MinimumLength = 4, ErrorMessage = "State must be in between 4-25 characters.")]
        public string State { get; set; }
        [StringLength(25, MinimumLength = 4, ErrorMessage = "City must be in between 4-25 characters.")]
        public string City { get; set; }
        [StringLength(400, MinimumLength = 4, ErrorMessage = "Adress must be in between 4-400 characters.")]
        public string AddressLine { get; set; }
        public bool LiveModelEnabled { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        
        public string AssignedTo { get; set; }
        public bool IsAssigned { get; set; }
        
    }
}