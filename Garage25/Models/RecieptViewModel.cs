using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage25.Models
{
    public class RecieptViewModel
    {
        public string RegNum { get; set; }
        public string Color { get; set; }
        public string VehicleType { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime CheckOutTime { get; set; }

        [DisplayFormat(DataFormatString = "{0:%d} days {0:%h} hours {0:%m} minutes")]
        public TimeSpan TotalTime { get; set; }
        public string Price { get; set; }
    }
}
