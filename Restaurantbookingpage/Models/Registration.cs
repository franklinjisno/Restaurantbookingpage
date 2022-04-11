using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using BookingClass.Enum;

namespace Restaurantbookingpage.Models
{
    public class Registration
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name: ")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(150)]
        [Display(Name = "Email Address: ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "You must provide a phone number,Phone Number at least 10 digit")]
        [Display(Name = "ContactNo")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string ContactNo { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(150, MinimumLength = 6)]
        [Display(Name = "Password: ")]
        public string Password { get; set; }
        public string CreatedByName { get; set; }
        public string ModifiedByName { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedDate { get;  set; }
        public int Deleted { get; set; }
        public bool Isadmin { get; set; }
        public Actions Actions { get;  set; }
    }
}