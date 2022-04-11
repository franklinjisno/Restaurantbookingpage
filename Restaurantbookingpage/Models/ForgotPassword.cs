using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Restaurantbookingpage.Models
{
    public class ForgotPassword
    {
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
    }
}