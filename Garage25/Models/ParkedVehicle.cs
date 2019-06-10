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

      //  [Required(), StringLength(6)]
      //  [Range(3, 10, ErrorMessage = "The User Name must be between 3 and 20 letters.")]
        //   [StringLength(6, ErrorMessage = "Test", MinimumLength = 6)]
        public string RegNum { get; set; }

    //    [Range(1,20)]
        public string Color { get; set; }
        public DateTime CheckInTime { get; set; }

        //Navigation properties, not in EF-database
        public int MemberId { get; set; }
        public Member Member { get; set; }
        public int VehicleTypeId { get; set; }
        public VehicleType VehicleType { get; set; }
    }
}
