using Garage25.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage25.Models
{
    public class ParkedVehicle : IValidatableObject
    {
        public int Id { get; set; }

        [Unique]
        [Required(ErrorMessage = "The registration number is required.")]
        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9]*$", ErrorMessage = "The name must consist of alphanumeric characters.")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "The registration number must be 6 characters.")]
        public string RegNum { get; set; }

        [Required(ErrorMessage = "The color is required.")]
        [RegularExpression("^[a-zA-Z][a-z A-Z]*$", ErrorMessage = "The name must consist of letters and spaces.")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "The color must be between 1 and 20 letters.")]
        public string Color { get; set; }

        [Required(ErrorMessage = "The check in time is required.")]
        public DateTime CheckInTime { get; set; }

        //Navigation properties, not in EF-database
        public int MemberId { get; set; }
        public Member Member { get; set; }
        public int VehicleTypeId { get; set; }
        public VehicleType VehicleType { get; set; }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            if (Color.Contains("blue", StringComparison.CurrentCultureIgnoreCase))
            {
                var context = (Garage25Context)validationContext.GetService(typeof(Garage25Context));
                if (context == null) yield return new ValidationResult("context is null");

                VehicleType vehicleType = context.VehicleType.First(v => v.Id == VehicleTypeId);
                if (vehicleType.Name.Contains("bus", StringComparison.CurrentCultureIgnoreCase))
                    yield return new ValidationResult("Buses are not allowed to be blue", new[] { "Color" });
            }

            yield return ValidationResult.Success;

            //var context = (Garage25Context)validationContext.GetService(typeof(Garage25Context));
            //if (context == null) yield return new ValidationResult("context is null");

            //if (context.ParkedVehicle.Any(p => p.Color.Contains("blue", StringComparison.CurrentCultureIgnoreCase))) {
            //    ParkedVehicle parkedVehicle = context.ParkedVehicle.First(p => p.Color.Contains("blue", StringComparison.CurrentCultureIgnoreCase));
            //    VehicleType vehicleType = context.VehicleType.First(v => v.Id == parkedVehicle.VehicleTypeId);
            //    if (vehicleType.Name.Contains("bus", StringComparison.CurrentCultureIgnoreCase))
            //        yield return new ValidationResult("Buses are not allowed to be blue", new[] { "Color" });
            //}

            //if (context.ParkedVehicle.Any(p => string.Equals(p.Color, Color.ToUpper())))
            //    yield return new ValidationResult("IV: Color entered already exists", new[] {"Color"});

            //if (context.ParkedVehicle.Any(p => string.Equals(p.Make, Make.ToUpper())))
            //    yield return new ValidationResult("IV: Make entered already exists", new[] { "Make" });

            //if (context.ParkedVehicle.Any(p => string.Equals(p.Model, Model.ToUpper())))
            //    yield return new ValidationResult("IV: Model entered already exists", new[] { "Model" });

            //if (Type == VehicleType.MOTORCYCLE && NumWheels != 2)
            //    yield return new ValidationResult("IV: Motorcycle must have two wheels", new[] { "Type", "NumWheels" });

            //if (Type == VehicleType.BOAT && NumWheels != 0)
            //    yield return new ValidationResult("IV: Boat must have zero wheels", new[] { "Type", "NumWheels" });
        }
    }
}
