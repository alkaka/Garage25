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
            // Blue buses are not allowed
            if (Color.Contains("blue", StringComparison.CurrentCultureIgnoreCase)) {
                if (Id == 0) {
                    if (VehicleType.Name.Contains("bus", StringComparison.CurrentCultureIgnoreCase))
                        yield return new ValidationResult("Blue buses are not allowed", new[] { "Color" });
                } else {
                    var context = (Garage25Context)validationContext.GetService(typeof(Garage25Context));
                    VehicleType vehicleType = context.VehicleType.First(v => v.Id == VehicleTypeId);
                    if (vehicleType.Name.Contains("bus", StringComparison.CurrentCultureIgnoreCase))
                        yield return new ValidationResult("Blue buses are not allowed", new[] { "Color" });
                }
            } else
                yield return ValidationResult.Success;
        }
    }
}
