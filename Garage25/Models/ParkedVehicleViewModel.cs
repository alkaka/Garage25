using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage25.Models
{
    public class ParkedVehicleViewModel
    {
        public int  Id { get; set; }
        public string Owner { get; set; }
        public string VehicleType { get; set; }
        [Display(Name = "Registration Number")]
        public string RegNum { get; set; }

        [DisplayFormat(DataFormatString = "{0:%d} days {0:%h} hours {0:%m} minutes")]
        public TimeSpan ParkingTime { get; set; }
    }
}
