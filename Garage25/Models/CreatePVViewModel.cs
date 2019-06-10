using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage25.Models
{
    public class CreatePVViewModel
    {
        [StringLength(6, MinimumLength = 6, ErrorMessage = "The Registration Number must be 6 characters.")]
        public string RegNum { get; set; }

     //   [Range(1,20)]
        public string Color { get; set; }
        public string UserName { get; set; }
        public string TypeName { get; set; }
    }
}
