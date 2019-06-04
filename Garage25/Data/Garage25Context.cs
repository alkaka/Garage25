using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Garage25.Models;

namespace Garage25.Models
{
    public class Garage25Context : DbContext
    {
        public Garage25Context (DbContextOptions<Garage25Context> options)
            : base(options)
        {
        }

        public DbSet<Garage25.Models.ParkedVehicle> ParkedVehicle { get; set; }

        public DbSet<Garage25.Models.Member> Member { get; set; }

        public DbSet<Garage25.Models.VehicleType> VehicleType { get; set; }
    }
}
