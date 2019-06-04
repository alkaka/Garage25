using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Garage25.Models;

namespace Garage25.Data
{
    public static class SeedData
    {
        internal static void Initialize(IServiceProvider services)
        {
            var options = services.GetRequiredService<DbContextOptions<Garage25Context>>();
            using (var context = new Garage25Context(options))
            {
                if (context.Member.Any())
                {
                    context.Member.RemoveRange(context.Member);
                    context.ParkedVehicle.RemoveRange(context.ParkedVehicle);
                    context.VehicleType.RemoveRange(context.VehicleType);
                }

                var members = new List<Member>();
                for (int i = 0; i < 10; i++)
                {
                    var userName = Faker.Name.FullName();
                    var member = new Member
                    {
                        UserName = userName,
                        Email = Faker.Internet.Email(userName)
                    };
                    members.Add(member);
                }
                context.Member.AddRange(members);

                //var textInfo = new CultureInfo("en-us", false).TextInfo;
                var vehicletypes = new List<VehicleType>();
                for (int i = 0; i < 10; i++)
                {
                    var vehicletype = new VehicleType
                    {
                        //Type = textInfo.ToTitleCase(Faker.Company.CatchPhrase())
                        Type = VType.CAR, // TODO: Fixes with name generator
                        Brand = "Volvo",
                        Model = "V60",
                        NumWheels = 4
                    };

                    vehicletypes.Add(vehicletype);
                }
                context.VehicleType.AddRange(vehicletypes);
                context.SaveChanges();

                var parkedVehicles = new List<ParkedVehicle>();
                foreach (var member in members)
                {
                    foreach (var vehicletype in vehicletypes)
                    {
                        DateTime now = DateTime.Now;
                        var parkedVehicle = new ParkedVehicle
                        {
                            RegNo = Faker.Name.First(), // TODO: fix unique reg num
                            CheckInTime = now,
                            ParkingTime = DateTime.Now - now,
                            Owner = member,
                            Type = vehicletype
                        };
                        parkedVehicles.Add(parkedVehicle);
                    }
                }
                context.ParkedVehicle.AddRange(parkedVehicles);
                context.SaveChanges();

            }
        }
    }
}
