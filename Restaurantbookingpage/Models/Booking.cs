using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using BookingClass.Enum;

namespace Restaurantbookingpage
{
    public class Booking
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Customer name is required.")]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "Name Should be min 5 and max 20 length")]
        [RegularExpression(@"^([a-zA-Z ]+)$", ErrorMessage = "Please Provide Valid Name")]
        public string Customer_Name { get; set; }

        [Required(ErrorMessage = "Date & Time is required.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        public DateTime Datetime { get; set; }

        //[Required(ErrorMessage = "Dinning Type is required.")]
        public string Dinning_Type { get; set; }

        [Required(ErrorMessage = "Number of guests is required.")]
        [Range(0, 100, ErrorMessage = "Not Valid")]
        public int NumberofGuest { get; set; }
        public Actions Actions { get; set; }
    }
}
