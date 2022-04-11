using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Restaurantbookingpage.Models
{
    public class Login
    {
        [Required]
        [EmailAddress]
        [StringLength(150)]
        [Display(Name = "Email ")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(150, MinimumLength = 6)]
        [Display(Name = "Password ")]
        public string Password { get; set; }
    }
}