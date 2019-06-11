using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage25.Models
{
    public class VehicleIndexViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Registration Number")]
        public string RegNo { get; set; }
        public DateTime CheckInTime { get; set; }
        public TimeSpan ParkingTime { get; set; }
        public int MemberId { get; set; }
        public Member Member { get; set; }
        public int VehicleTypeId { get; set; }
        public VehicleType VehicleType { get; set; }

    }
}
