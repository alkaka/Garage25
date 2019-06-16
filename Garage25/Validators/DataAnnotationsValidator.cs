using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Garage25.Validators
{
    public static class DataAnnotationsValidator
    {
        public static void TryValidateModel(object value, ControllerBase controller, IServiceProvider serviceProvider)
        {
            if (!TryValidate(value, serviceProvider, out ICollection<ValidationResult> validationResults))
            {
                foreach (var validationResult in validationResults)
                {
                    if (validationResult.MemberNames.Count() > 0)
                        controller.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
                }
            }
        }

        public static bool TryValidate(object value, IServiceProvider serviceProvider, out ICollection<ValidationResult> results)
        {
            var items = new Dictionary<object, object>();
            var context = new ValidationContext(value, serviceProvider: serviceProvider, items: items);
            results = new List<ValidationResult>();
            return Validator.TryValidateObject(value, context, results, validateAllProperties: true );
        }
    }
}