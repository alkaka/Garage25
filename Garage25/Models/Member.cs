using Garage25.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage25.Models
{
    public class Member
    {
        public int Id { get; set; }

        [Unique]
        [Required(ErrorMessage = "The user name is required.")]
        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9]*$", ErrorMessage = "The name must consist of alphanumeric characters.")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "The name must be between 1 and 20 letters.")]
        public string UserName { get; set; }

        [Unique]
        [Required(ErrorMessage = "The e-mail address is required.")]
        [EmailAddress(ErrorMessage = "The e-mail address is not a valid e-mail address.")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "The e-mail address must be between 1 and 30 letters.")]
        public string Email { get; set; }
        public ICollection<ParkedVehicle> ParkedVehicles { get; set; }
    }
}
