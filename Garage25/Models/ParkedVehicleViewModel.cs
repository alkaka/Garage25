using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage25.Models
{
    public class ParkedVehicleViewModel
    {
        public IEnumerable<VehicleIndexViewModel> ParkedVehicles { get; set; }
        public string SearchTerm { get; set; }
    }

}
