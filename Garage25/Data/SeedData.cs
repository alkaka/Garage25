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
                    var userName = Faker.Name.First();
                    var member = new Member
                    {
                        UserName = userName,
                        Email = Faker.Internet.Email(userName)
                    };
                    members.Add(member);
                }
                context.Member.AddRange(members);

                string[] vtypes = { "Airplane",
                                    "Bicycle",
                                    "Boat",
                                    "Bus",
                                    "Car",
                                    "Lorry",
                                    "Moped",
                                    "Motorcycle" ,
                                    "Train",
                                    "Truck" };

                Random random = new Random();
                var vehicletypes = new List<VehicleType>();
                for (int i = 0; i < 10; i++)
                {
                    var vehicletype = new VehicleType
                    {
                        Name = vtypes[i]
                    };

                    vehicletypes.Add(vehicletype);
                }
                context.VehicleType.AddRange(vehicletypes);
                context.SaveChanges();
                var vh = new Bogus.DataSets.Vehicle();
                var color = new Bogus.DataSets.Commerce(locale: "en");
                TextInfo textInfo = new CultureInfo("en", false).TextInfo;
                var memberIds = new List<int>();
                foreach (var member in members)
                {
                    memberIds.Add(member.Id);
                }
                var vehicleTypeIds = new List<int>();
                foreach (var vehicletype in vehicletypes)
                {
                    vehicleTypeIds.Add(vehicletype.Id);
                }
                var parkedVehicles = new List<ParkedVehicle>();
                foreach (var member in members)
                {
                    foreach (var vehicletype in vehicletypes)
                    {
                        var vhcol = color.Color();
                        var parkedVehicle = new ParkedVehicle
                        {
                            RegNum = vh.Vin().Substring(0, 6),
                            Color = textInfo.ToTitleCase(vhcol),
                            CheckInTime = DateTime.Now.AddDays(random.Next(-10, 0))
                                                      .AddHours(random.Next(0, 24))
                                                      .AddMinutes(random.Next(0, 60))
                                                      .AddSeconds(random.Next(0, 60)),
                            MemberId = memberIds[random.Next(0, 9)],
                            VehicleTypeId = vehicleTypeIds[random.Next(0, 9)],
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
