using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Comque.Validation
{
    public class DataAnnotationsValidator
    {
        public bool TryValidate(object instance, out ICollection<ValidationResult> validationResults)
        {
            var context = new ValidationContext(instance, serviceProvider: null, items: null);
            validationResults = new List<ValidationResult>();
            return Validator.TryValidateObject(
                instance, context, validationResults,
                validateAllProperties: true
            );
        }

        public bool TryValidateObject(object instance, ICollection<ValidationResult> validationResults)
        {
            return Validator.TryValidateObject(instance, new ValidationContext(instance, null, null), validationResults, true);
        }

        public bool TryValidateObjectRecursive<T>(T instance, List<ValidationResult> validationResults)
        {
            bool isValid = TryValidateObject(instance, validationResults);

            var properties = instance.GetType().GetProperties().Where(prop => prop.CanRead && !prop.GetCustomAttributes(typeof(SkipRecursiveValidation), false).Any()).ToList();

            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string) || property.PropertyType.IsValueType) continue;

                var value = instance.GetPropertyValue(property.Name);

                if (value == null) continue;

                var listValue = value as IEnumerable;
                if (listValue != null)
                {
                    foreach (var enumObj in listValue)
                    {
                        var nestedResults = new List<ValidationResult>();
                        if (!TryValidateObjectRecursive(enumObj, nestedResults))
                        {
                            isValid = false;
                            foreach (var validationResult in nestedResults)
                            {
                                var property1 = property;
                                validationResults.Add(new ValidationResult(validationResult.ErrorMessage, validationResult.MemberNames.Select(x => property1.Name + '.' + x)));
                            }
                        };
                    }
                }
                else
                {
                    var nestedResults = new List<ValidationResult>();
                    if (!TryValidateObjectRecursive(value, nestedResults))
                    {
                        isValid = false;
                        foreach (var validationResult in nestedResults)
                        {
                            var property1 = property;
                            validationResults.Add(new ValidationResult(validationResult.ErrorMessage, validationResult.MemberNames.Select(x => property1.Name + '.' + x)));
                        }
                    };
                }
            }

            return isValid;
        }

        /// <summary>
        /// Validates the instance
        /// </summary>
        /// <param name="instance">instance to validate</param>
        /// <returns>validation errors</returns>
        public string GetValidationErrors(object instance)
        {
            var validationResults = new List<ValidationResult>();
            if (!TryValidateObjectRecursive(instance, validationResults))
            {
                return validationResults.Select(r => r.ErrorMessage).Aggregate((current, next) => current + "\n " + next);
            }
            return null;

            //var validationResults = default(ICollection<ValidationResult>);
            //if (!TryValidate(instance, out validationResults))
            //{
            //    return validationResults.Select(r => r.ErrorMessage).Aggregate((current, next) => current + "\n " + next);
            //}
            //return null;
        }

        /// <summary>
        /// Validates the instance 
        /// throws a ValidationException on failed validation
        /// </summary>
        /// <param name="instance">instance to validate</param>
        public void Validate(object instance)
        {
            var validationErrors = GetValidationErrors(instance);
            if (!string.IsNullOrEmpty(validationErrors))
            {
                throw new ValidationException(validationErrors);
            }
        }
    }
}
