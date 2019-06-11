using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage25.Models
{
    public class CreatePVViewModel
    {
        [Display(Name = "Registration Number")]
        [Required(ErrorMessage = "Registration Number is required"), StringLength(6),]
        public string RegNum { get; set; }
        public string Color { get; set; }
        public string UserName { get; set; }
        public string TypeName { get; set; }
    }
}
