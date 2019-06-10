using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage25.Models
{
    public class VehicleType
    {
        public int Id { get; set; }

        [StringLength(20, MinimumLength = 1, ErrorMessage = "The Name must be between 1 and 20 letters.")] // TODO: Alphanumeric check
        public string Name { get; set; }
    }
}
