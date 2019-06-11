using Garage25.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage25.Models
{
    public class CreatePVViewModel
    {
        public int Id { get; set; }

        [Unique]
        [Required(ErrorMessage = "The registration number is required.")]
        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9]*$", ErrorMessage = "The name must consist of alphanumeric characters.")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "The registration number must be 6 characters.")]
        public string RegNum { get; set; }

        [Required(ErrorMessage = "The color is required.")]
        [RegularExpression("^[a-zA-Z][a-z A-Z]*$", ErrorMessage = "The name must consist of letters and spaces.")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "The color must be between 1 and 20 letters.")]
        public string Color { get; set; }

        [Required(ErrorMessage = "The user name is required.")]
        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9]*$", ErrorMessage = "The name must consist of alphanumeric characters.")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "The name must be between 1 and 20 letters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "The name is required.")]
        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9]*$", ErrorMessage = "The name must consist of alphanumeric characters.")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "The name must be between 1 and 20 letters.")]
        public string TypeName { get; set; }
    }
}
