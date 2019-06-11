using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage25.Models
{
    public class ParkedVehicle
    {
        public int Id { get; set; }
        [Display(Name = "Registration Number")]
        public string RegNum { get; set; }
        public string Color { get; set; }
        public DateTime CheckInTime { get; set; }

        //Navigation properties, not in EF-database
        public int MemberId { get; set; }
        public Member Member { get; set; }
        public int VehicleTypeId { get; set; }
        public VehicleType VehicleType { get; set; }
    }
}
