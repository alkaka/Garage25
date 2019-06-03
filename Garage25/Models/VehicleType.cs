using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage25.Models
{
    public enum VType
    {
        AIRPLANE,
        BOAT,  //TODO: #100 Change TÅ
        BUS,
        CAR,
        MOTORCYCLE,
        TRAIN,
        TRUCK  //TODO: #99 Change TÅ
    }
    public class VehicleType
    {
        public VType Type { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int NumWheels { get; set; }
    }
}
