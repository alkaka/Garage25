using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage25.Models
{
    public class RecieptViewModel
    {
        public string RegNo { get; set; }
        public VehicleType Type { get; set; }
        public string Brand { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime CheckOutTime { get; set; }
        public string TotalTime { get; set; }
        public string Price { get; set; }

    }
}
