using Garage25.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Garage25.Attributes
{
    public class UniqueAttribute : ValidationAttribute, IClientModelValidator
    {
        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            if (value == null)
                // throw new ArgumentNullException(value.ToString());
                return new ValidationResult("value is null");

            if (validationContext == null)
                // throw new ArgumentNullException(validationContext.ToString());
                return new ValidationResult("validationContext is null");

            //ParkedVehicle parkedVehicle = (ParkedVehicle)validationContext.ObjectInstance;
            //if (parkedVehicle == null) return ValidationResult.Success;

            var context = (Garage25Context)validationContext.GetService(typeof(Garage25Context));
            if (context == null)
                  // throw new ArgumentNullException(context.ToString());
                  return new ValidationResult("context is null");

            switch (validationContext.DisplayName)
            {
                case "Name":
                    if (context.VehicleType.Any(v => v.Name == value.ToString()))
                        return new ValidationResult(string.Format("\'{0}\' is not unique.", value));
                    break;
                case "UserName":
                    Member member = (Member)validationContext.ObjectInstance;
                    if (member == null)
                        return new ValidationResult("member is null");
                    if (member.Id == 0) {
                        if (context.Member.Any(m => m.UserName == member.UserName))
                            return new ValidationResult(string.Format("\'{0}\' is not unique.", value));
                    } else if (context.Member.AsNoTracking().First(m => m.Id == member.Id).UserName != member.UserName &&
                               context.Member.AsNoTracking().Any(m => m.UserName == member.UserName))
                            return new ValidationResult(string.Format("\'{0}\' is not unique.", value));
                    break;
                case "Email":
                    member = (Member)validationContext.ObjectInstance;
                    if (member == null)
                        return new ValidationResult("member is null");
                    if (member.Id == 0) {
                        if (context.Member.Any(m => m.Email == member.Email))
                            return new ValidationResult(string.Format("\'{0}\' is not unique.", value));
                    } else if (context.Member.AsNoTracking().First(m => m.Id == member.Id).Email != member.Email &&
                               context.Member.AsNoTracking().Any(m => m.Email == member.Email))
                            return new ValidationResult(string.Format("\'{0}\' is not unique.", value));
                    break;
                case "RegNum":
                    ParkedVehicle vehicle = new ParkedVehicle
                    {
                        Id = (validationContext.ObjectInstance is CreatePVViewModel) ?
                               (validationContext.ObjectInstance as CreatePVViewModel).Id :
                               (validationContext.ObjectInstance as ParkedVehicle).Id,
                        RegNum = (validationContext.ObjectInstance is CreatePVViewModel) ?
                               (validationContext.ObjectInstance as CreatePVViewModel).RegNum :
                               (validationContext.ObjectInstance as ParkedVehicle).RegNum
                    };

                    if (vehicle.Id == 0) {
                        if (context.ParkedVehicle.Any(p => p.RegNum == vehicle.RegNum))
                            return new ValidationResult(string.Format("\'{0}\' is not unique.", value));
                    } else if (context.ParkedVehicle.AsNoTracking().First(p => p.Id == vehicle.Id).RegNum != vehicle.RegNum &&
                               context.ParkedVehicle.AsNoTracking().Any(p => p.RegNum == vehicle.RegNum))
                           return new ValidationResult(string.Format("\'{0}\' is not unique.", value));
                    break;
            }

            return ValidationResult.Success;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException();
            }

            if (context.ModelMetadata.ModelType == typeof(string))
            {
                MergeAttribute(context.Attributes, "data-val", "true");
                MergeAttribute(context.Attributes, "data-val-unique",
                                                   GetErrorMessage(context));
            }
        }

        private static bool MergeAttribute(IDictionary<string, string>
            attributes, string key, string value)
        {
            if (attributes.ContainsKey(key))
                return false;

            attributes.Add(key, value);

            return true;
        }

        private string GetErrorMessage(ClientModelValidationContext context)
        {
            string result = "";

            if (context.ModelMetadata.ModelType == typeof(string))
            {
                result = string.Format("\'{0}\' is not unique.", context.Attributes["value"]);
            }

            return result;
        }
    }
}
