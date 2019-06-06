using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage25.Models
{
    public class DetailsMViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public ICollection<ParkedVehicle> ParkedVehicles { get; set; }
    }
}
