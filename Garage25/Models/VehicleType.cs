using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage25.Models
{
    public enum VType
    {
        AIRPLANE,
        BOAT,  
        BUS,
        CAR,
        MOTORCYCLE,
        TRAIN,
        TRUCK  
    }
    public class VehicleType
    {
        public int Id { get; set; }
        public VType Type { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int NumWheels { get; set; }
    }
}
