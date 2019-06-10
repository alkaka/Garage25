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

      //  [Range(3,20, ErrorMessage = "The User Name must be between 3 and 20 letters.")]
        public string UserName { get; set; }

        [EmailAddress(ErrorMessage = "The Email is not a valid e-mail address.")]
        public string Email { get; set; }
        public ICollection<ParkedVehicle> ParkedVehicles { get; set; }
    }
}
